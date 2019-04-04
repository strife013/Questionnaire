using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
 
 

namespace BaseErp.Web.Models
{
 

    public class Result
    {
        public bool success { get; set; }
        public object obj { get; set; }
        public string msg { get; set; }
    }

    public class SubmitResult
    {
        public bool success { get; set; }
        public string RedirectUrl { get; set; }
        //如果此值为true，则关闭自己，刷新父页面
        public bool IsCloseSelf { get; set; }
        public List<SubmitError> Errors { get; set; }

        public  SubmitResult ()
        {
            Errors = new List<SubmitError>();
        }
    }

    public class SubmitError
    {
        public string Name { get; set; }
        public string Message { get; set; }
    }
    public class OrderBy
    {
        public string sidx;
        public string sord;
    }

    public class FullCalendarJson
    {
        public DateTime DateStart { get; set; }
        public string start {
            get
            {
                return DateStart.ToString("yyyy-MM-dd");
            }
        }
        public int id { get; set; }
        public string url { get; set; }
        public string title
        {
            get;
            set;
        }
    }
}