using Dapper;
using OUDAL;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc; 
 
using HealthErpDAL;
using BaseErp.Web.Models;
using System.Web.Script.Serialization;

namespace HealthErp.Web.Controllers
{
    public partial class AskResultController : Controller
    {
        private AskDBContent asdb = new AskDBContent();
        protected override void Dispose(bool disposing)
        { 
            base.Dispose(disposing);
        }
        #region Custom
        public ActionResult AskResultPerson(int cid, int aid)
        { 
            List<AskResult> res = asdb.AskResult.Where(a => a.uid == cid && a.AppointmentId == aid).OrderByDescending(a => a.createdate).ToList();
            return View(res);
        }

        #endregion

        [ValidateInput(false)]
        [System.Web.Mvc.HttpPost]
        public ActionResult InsertQuestionPage(FormCollection collection)
        {
            StringBuilder sbResult = new StringBuilder();
            AskResult result = new AskResult();
            List<AskAnswer> answers = new List<AskAnswer>();

            string TopicName = collection["topicname"];

            foreach (string key in collection.Keys)
            {
                if (key.IndexOf("iptquestion") >= 0)
                {
                    if (sbResult.Length > 0) sbResult.Append(",");
                    sbResult.AppendFormat("{0}:{1}", key, collection[key]);
                    int aIndex = key.LastIndexOf("a");
                    string stringTopicId = "";
                    if (aIndex >= 0)
                    {
                        stringTopicId = key.Substring(aIndex + 1);

                    }  
                    int topicId = 0;
                    int.TryParse(stringTopicId, out topicId);

                    answers.Add(new AskAnswer() { SelectResult = collection[key], TopicId = topicId, Score = PageHelper.GetQuestionScore(collection[key]) });
                }
            }

            int pid = 0;
            int.TryParse(collection["pageid"], out pid);
            result.pageid = pid;
            result.createdate = DateTime.Now;

            int uid = 0;
            int.TryParse(collection["uid"], out uid);
            result.uid = uid;

            result.uname = PageHelper.ClientIP;
            result.pagetitle = TopicName;
            result.score = answers.Select(a => a.Score).Sum();
             
            result.answer = answers.Sum(a=>a.Score).ToString();
            result.AnswerJson = new JavaScriptSerializer().Serialize(answers);

            result.answerHtml = collection["divmaindata"]; 

           asdb.AskResult.Add(result);
            asdb.SaveChanges();
            //iptquestion
            ViewBag.Result = result;
            ViewBag.PostSuccess = "true";
            return View("../AskResult/AskResultList");
        }
        /*---------------------- AskResult------------------------------*/
        public ActionResult AskResultList()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AskResultQuery(int? projectid, string sidx, string sord, int page, int rows, FormCollection collection)
        {
            string searchSql = "select  * from  a_AskResult where {0} and id not in (select t.id from (select  id from a_AskResult where {0}  {3}  limit {2} ) as t ) {3} limit {1}";
            //string searchSql = "select top {1} * from  [AskResult] where {0} and id not in (select top {2} id from [AskResult] where {0}  {3}) {3} ";
            string totalCountSql = " select count(*) from  a_AskResult where {0}";
            int startindex = page * rows - rows;
            string condition = " 1=1 ";
 

            string order = "order by id desc ";
            if (!string.IsNullOrEmpty(sidx))
            {
                order = " order by " + sidx + " " + sord;
            }


            string searchSQL = string.Format(searchSql, condition, rows, startindex, order);
            totalCountSql = string.Format(totalCountSql, condition);

            StringBuilder sbSearch = new StringBuilder();


            asdb.Database.Connection.Open();

            var ccc = asdb.AskContent.ToList();
            var dynamicParams = new DynamicParameters();
          
            var query = asdb.Database.Connection.Query<AskResult>(searchSQL, param: dynamicParams).ToList();
            var totalQuery = asdb.Database.Connection.Query<int>(totalCountSql, param: dynamicParams).ToList()[0];

            int totalrow = totalQuery;
            int pagenum = (totalrow - totalrow % rows - 1) / rows + 1;
            var jsonData = new
            {
                total = pagenum,
                page = page,
                records = totalrow,
                rows = query
            };
            return Json(jsonData);
        } 
        public ActionResult AskResultView(int id)
        {
            AskResult r = asdb.AskResult.Find(id);
            return View(r);
        } 
        public ActionResult AskResultEdit(int id)
        {
            AskResult r = asdb.AskResult.Find(id);
            return View(r);
        }
        [ValidateInput(false)]
        [HttpPost]
        public JsonResult UpdateById(int id, AskResult data)
        {
            AskResult model = new AskResult();
            if (id == 0)
            { 
            }
            else
            {
                var aa = asdb.AskResult.ToList();
                model = asdb.AskResult.FirstOrDefault(a=>a.id==id); 
            }

            if (!string.IsNullOrEmpty(data.answerHtml)) model.answerHtml = data.answerHtml;

            asdb.SaveChanges();

            return Json(data);
        }
 
        [HttpPost]
        public ActionResult AskResultEdit(int id, int? projectid, FormCollection collection)
        {
            AskResult r = asdb.AskResult.Find(id);
            if (r == null)
            {
                int pid = 0;
                if (projectid != null) pid = projectid.Value;
                r = new AskResult();
                if (projectid != null)
                    r.projectid = projectid;
                asdb.AskResult.Add(r);
            }
            TryUpdateModel(r, collection);
            if (ModelState.IsValid)
            {
                asdb.SaveChanges();
                return Redirect("../AskResultView/" + r.id);
            }
            return View(r);
        }
        [HttpPost]
        public JsonResult DeleteAskResult(int id, FormCollection collection)
        {
            Result result = new Result();
            var r = asdb.AskResult.Find(id);
            if (r == null)
            {
                result.obj = "找不到问卷结果表";
            }
            else
            {
                asdb.AskResult.Remove(r);
                asdb.SaveChanges();
                result.success = true;
                result.obj = "已删除";
            }
            return Json(result);
        }

    }
}
