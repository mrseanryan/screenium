//Copyright (c) 2015 Sean Ryan
//
//See the file license.txt for copying permission.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace screenium
{
    class HtmlReportCreator : IReportCreator
    {
        //TODO make a HTML report creator (and run both of them)
        //it can be just 1 html page, with the diff image for each test run.
        //we can open it via explorer (ref WeeWebWatcher ?)

        public Report CreateReport(TestDescription test, CompareResultDescription compareResult)
        {
            //TODO implement me - HTML
            //throw new NotImplementedException();
            return new Report();
        }

        public bool HasSaveCapability()
        {
            return true;
        }

        public void SaveReport(Report report, string filePath)
        {
            //TODO implement me - HTML
            //throw new NotImplementedException();
        }

        public void ShowReport(Report report)
        {
            //TODO implement me - HTML
            //throw new NotImplementedException();
        }
    }
}
