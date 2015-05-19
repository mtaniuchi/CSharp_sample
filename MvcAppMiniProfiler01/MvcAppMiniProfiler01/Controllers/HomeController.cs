﻿using System.Threading;
using System.Web.Mvc;
using StackExchange.Profiling;

namespace MvcAppMiniProfiler01.Controllers
{
    public class HomeController : Controller
    {
        /// <summary>
        /// GET: /Home/
        /// </summary>
        /// <see cref="http://miniprofiler.com/"/>
        public ActionResult Index()
        {
            var profiler = MiniProfiler.Current; // it's ok if this is null
            using (profiler.Step("Set page title"))
            {
                ViewBag.Title = "Home Page with profiler";
            }
            using (profiler.Step("Doing complex stuff"))
            {
                using (profiler.Step("Step A"))
                {
                    // something more interesting here
                    Thread.Sleep(100);
                }
                using (profiler.Step("Step B"))
                {
                    // and here
                    Thread.Sleep(250);
                }
            }

            return View();
        }
    }
}
