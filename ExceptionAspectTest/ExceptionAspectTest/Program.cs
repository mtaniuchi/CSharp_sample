/*
 * The MIT License (MIT)
 * 
 * Copyright (c) 2013 Masahiro Taniuchi
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 * 
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using PostSharp.Aspects;

namespace ExceptionAspectTest
{
    /// <summary>
    /// エントリーポイント
    /// </summary>
    class Program
    {
        static void Main(string[] args)
        {
            TestClass t = new TestClass();

            //t.doExceptionTest1();
            //t.doExceptionTest2();
            t.doExceptionTest3();
        }
    }
    
    /// <summary>
    /// ExceptionのMessagesを受け取るためのインタフェース
    /// </summary>
    interface IMessages
    {
        List<String> messages { get; set; }
    }

    /// <summary>
    /// AspectでTry-Catchさせて、Exceptionを溜める。
    /// テスト用クラス
    /// </summary>
    public class TestClass : IMessages
    {
        public List<String> messages
        {
            get { return this._messages; }
            set { this._messages = value; }
        }
        private List<String> _messages;

        #region 多段なしタイプの実装例

        /// <summary>
        /// Exceptionを2発投げる
        /// </summary>
        public void doExceptionTest1()
        {
            ThrowSampleExecption();

            ThrowSampleExecption();

            //ThrowDummyException(); //AspectではCatchされない

            System.Diagnostics.Debug.WriteLine("doExceptionTest1==========");
            foreach (var msg in this.messages)
            {
                System.Diagnostics.Debug.WriteLine(msg);
            }
            System.Diagnostics.Debug.WriteLine("doExceptionTest1==========");
        }
        
        [ExceptionAspect(ExceptionType = typeof(ApplicationException), Behavior = FlowBehavior.Continue)]
        private void ThrowSampleExecption()
        {
            throw new ApplicationException("Sample Exception");
        }

        private void ThrowDummyException()
        {
            throw new NotImplementedException();
        }

        #endregion 多段なしタイプの実装例



        #region 多段ありタイプの実装例

        /// <summary>
        /// Exceptionを2発投げる
        /// </summary>
        public void doExceptionTest2()
        {
            ThrowSampleExceptionOnChildren();

            //ThrowDummyException(); //AspectではCatchされない

            System.Diagnostics.Debug.WriteLine("doExceptionTest2==========");
            foreach (var msg in this.messages)
            {
                System.Diagnostics.Debug.WriteLine(msg);
            }
            System.Diagnostics.Debug.WriteLine("doExceptionTest2==========");
        }

        //ここにアスペクトを適用してもダメ。子メソッドのFlowBehavior.Continueは無視される。
        private void ThrowSampleExceptionOnChildren()
        {
            this.ChildMethod1(); //このメソッドにアスペクトを適用していれば、ChildMethod2も実行される

            this.ChildMethod2();
        }

        //ここにアスペクトを適用するのが正しい
        [ExceptionAspect(ExceptionType = typeof(ApplicationException), Behavior = FlowBehavior.Continue)]
        private void ChildMethod1()
        {
            throw new ApplicationException("ChildMethod1 Exception");

            // FlowBehavior.Continueにしても、ここは実行されない
            //System.Diagnostics.Debug.WriteLine("ChildMethod1 continue...");
        }

        //ここにアスペクトを適用するのが正しい
        [ExceptionAspect(ExceptionType = typeof(ApplicationException), Behavior = FlowBehavior.Continue)]
        private void ChildMethod2()
        {
            throw new ApplicationException("ChildMethod1 Exception");

            // FlowBehavior.Continueにしても、ここは実行されない
            //System.Diagnostics.Debug.WriteLine("ChildMethod2 continue...");
        }

        #endregion 多段ありタイプの実装例


        #region 入れ子タイプの実装例

        /// <summary>
        /// Exceptionを3発投げる
        /// </summary>
        public void doExceptionTest3()
        {
            ThrowAnotherSampleExceptionOnChildren();

            //ThrowDummyException(); //AspectではCatchされない

            System.Diagnostics.Debug.WriteLine("doExceptionTest2==========");
            foreach (var msg in this.messages)
            {
                System.Diagnostics.Debug.WriteLine(msg);
            }
            System.Diagnostics.Debug.WriteLine("doExceptionTest2==========");
        }

        private void ThrowAnotherSampleExceptionOnChildren()
        {
            this.AnotherChildMethod1();

            this.AnotherChildMethod1(false);

            this.AnotherChildMethod2();
        }

        //ここにアスペクトを適用するのが正しい
        [ExceptionAspect(ExceptionType = typeof(ApplicationException), Behavior = FlowBehavior.Continue)]
        private void AnotherChildMethod1(bool processApplicationException = true)
        {
            if (processApplicationException)
            {
                //processApplicationException = true の場合はthrowされたらこのメソッドを抜ける
                throw new ApplicationException("ChildMethod1 Exception");
            }

            //
            //processApplicationException = false の場合はこれより下のコードも実行される
            //

            this.AnotherMethod();

            System.Diagnostics.Debug.WriteLine("AnotherChildMethod1 continue...");
        }

        //ここにアスペクトを適用するのが正しい
        [ExceptionAspect(ExceptionType = typeof(ApplicationException), Behavior = FlowBehavior.Continue)]
        private void AnotherChildMethod2()
        {
            throw new ApplicationException("AnotherChildMethod2 Exception");

            // FlowBehavior.Continueにしても、ここは実行されない
            //System.Diagnostics.Debug.WriteLine("ChildMethod2 continue...");
        }

        //ここにアスペクトを適用するのが正しい 
        [AnotherExceptionAspect(ExceptionType = typeof(NotImplementedException), Behavior = FlowBehavior.Continue)]
        private void AnotherMethod()
        {
            throw new NotImplementedException("つくってまへん");
        }

        #endregion 入れ子タイプの実装例
    }



    #region アスペクトその１

    /// <summary>
    /// Exception用のアスペクト
    /// </summary>
    [Serializable]
    public class ExceptionAspect : OnExceptionAspect
    {
        public Type ExceptionType { get; set; }
 
        public FlowBehavior Behavior { get; set; }
 
        public override void OnException(MethodExecutionArgs args)
        {
            var testClass = args.Instance as TestClass;

            string msg = string.Format("{0}: Error running {1}. {2}{3}{4}", DateTime.Now, args.Method.Name, args.Exception.Message, Environment.NewLine, args.Exception.StackTrace);

            System.Diagnostics.Debug.WriteLine("OnException==========");
            Debug.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine("OnException==========");

            if (testClass.messages == null) { testClass.messages = new List<string>(); }

            testClass.messages.Add(msg);

            args.FlowBehavior = FlowBehavior.Continue;
        }
 
        public override Type GetExceptionType(System.Reflection.MethodBase targetMethod)
        {
            return ExceptionType;
        }
    }

    #endregion アスペクトその１



    #region アスペクトその２

    /// <summary>
    /// Exception用のアスペクト
    /// </summary>
    [Serializable]
    public class AnotherExceptionAspect : OnExceptionAspect
    {
        public Type ExceptionType { get; set; }

        public FlowBehavior Behavior { get; set; }

        public override void OnException(MethodExecutionArgs args)
        {
            var testClass = args.Instance as TestClass;

            string msg = string.Format("{0}: Error running {1}. {2}{3}{4}", DateTime.Now, args.Method.Name, args.Exception.Message, Environment.NewLine, args.Exception.StackTrace);

            System.Diagnostics.Debug.WriteLine("OnException==========");
            Debug.WriteLine(msg);
            System.Diagnostics.Debug.WriteLine("OnException==========");

            if (testClass.messages == null) { testClass.messages = new List<string>(); }

            testClass.messages.Add(msg);

            args.FlowBehavior = FlowBehavior.Continue;
        }

        public override Type GetExceptionType(System.Reflection.MethodBase targetMethod)
        {
            return ExceptionType;
        }
    }

    #endregion アスペクトその２

}