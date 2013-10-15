using System.Collections.Generic;
using System.Linq;

namespace CustomSQLSample.Database1DataSetTableAdapters
{
    public partial class StatesTableAdapter : global::System.ComponentModel.Component
    {
        //[global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Data.Design.TypedDataSetGenerator", "4.0.0.0")]
        [global::System.ComponentModel.Design.HelpKeywordAttribute("vs.data.TableAdapter")]
        [global::System.ComponentModel.DataObjectMethodAttribute(global::System.ComponentModel.DataObjectMethodType.Fill, false)]
        public virtual int FillByInOperator(Database1DataSet.StatesDataTable dataTable, IEnumerable<string> names)
        {
            var cmdText = this.CommandCollection[1].CommandText;

            if (names.Any())
            {
                var InOperator = names.Aggregate(@"WHERE Name IN(", (current, name) => current + ("'" + name + "', "));

                // trim last comma
                InOperator = InOperator.Substring(0, InOperator.Count() - 2);

                cmdText = cmdText.Replace(@"/*@IN*/", InOperator + @")");
            }

            System.Diagnostics.Debug.WriteLine(cmdText);

            this.CommandCollection[1].CommandText = cmdText;

            this.Adapter.SelectCommand = this.CommandCollection[1];
            if ((this.ClearBeforeFill))
            {
                dataTable.Clear();
            }
            int returnValue = this.Adapter.Fill(dataTable);
            return returnValue;
        }
    }
}
