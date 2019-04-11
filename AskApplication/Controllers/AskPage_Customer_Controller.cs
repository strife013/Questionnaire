
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

namespace HealthErp.Web.Controllers
{
    public partial class AskPageController : Controller
    {
        private AskDBContent db = new AskDBContent();
        private static List<SelectListItem> AllAskPageSelect = new List<SelectListItem>();
        private static List<AskPage> AllAskPageModel = null;



        public static List<SelectListItem> GetAllAskPage()
        {
            if (AllAskPageSelect.Count != 0) return AllAskPageSelect;
            AllAskPageSelect.Clear(); 
            using (AskDBContent con = new AskDBContent())
            {
                AllAskPageModel = con.AskPage.ToList();
            }
            AllAskPageModel.ForEach(m =>
            {
                SelectListItem item = new SelectListItem();
                item.Text = m.Name + "";
                item.Value = m.id + "";
                AllAskPageSelect.Add(item);
            });
            return AllAskPageSelect;
        }


        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
        /// <summary>
        /// 选项上移
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public JsonResult UpAskTopic(int tid)
        {
            return ChangeTopicItemIndex(tid, -1);
        }

        public JsonResult DownAskTopic(int tid)
        {
            return ChangeTopicItemIndex(tid, 1);
        }

        public JsonResult EditAskTopic(int tid, string desc, string type)
        {

            AskTopic at = db.AskTopic.Find(tid);
            if (at != null)
            {
                at.Type = type;
                at.Title = desc;
                db.SaveChanges();
            }
            return Json(at);
        }


        [HttpPost]
        public JsonResult SelectAskTopics(int pageId)
        {
            List<AskTopic> acList = db.AskTopic.Where(a => a.PageId == pageId).OrderBy(a => a.IndexNum).ToList();

            return Json(acList);
        }

        /// <summary>
        /// 添加topic下题目项并返回tid对应的所有content
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAskTopic(string desc, int pid, string type)
        {
            int next = 0;
            var query = (from o in db.AskTopic where o.PageId == pid orderby o.IndexNum select o).ToList();
            if (query.Count > 0) next = query[query.Count - 1].IndexNum.Value + 1;

            AskTopic ac = new AskTopic();
            ac.PageId = pid;
            ac.Title = desc;
            ac.Type = type;
            ac.CreateTime = DateTime.Now;
            ac.IndexNum = next;
            db.AskTopic.Add(ac);



            db.SaveChanges();

            List<AskTopic> acList = db.AskTopic.Where(a => a.PageId == pid).OrderByDescending(a => a.IndexNum).ToList();
            return Json(acList);
        }

        /// <summary>
        /// 查找问卷
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AskTopicQuery2(int pageindex, int pagesize)
        {
            //List<AskContent> cList = db.AskContent.OrderByDescending(c => c.id).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();

            List<AskPage> atList = db.AskPage.OrderBy(t => t.id).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();

            int pagenum = db.AskPage.Count();

            var jsonData = new
            {
                total = pagenum,
                pageindex = pageindex,
                pagesize = pagesize,
                rows = atList
            };
            return Json(jsonData);
        }
        /// <summary>
        /// 增加问卷
        /// </summary>
        /// <param name="title"></param>
        /// <param name="tid"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAskPage(string name)
        {
            AskPage ap = new AskPage();
            ap.Name = name;
            ap.CreateTime = DateTime.Now;
            db.AskPage.Add(ap);
            db.SaveChanges();

            return Json(ap);
        }


        /// <summary>
        /// 删除问题的选项
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="tids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteAskTopic2(int tid)
        {
            //需要查询不属于这个题目的所有题目条目
            //List<AskTopic> acList = new List<AskTopic>();
            //acList = (from o in db.AskTopic join p in db.AskTopicContent.AsNoTracking() on o.id equals p.AskTopicId select o).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            AskTopic ac = db.AskTopic.Find(tid);
            Result result = new Result();
            if (ac != null)
            {
                db.AskTopic.Remove(ac);
                db.SaveChanges();
                result.success = true;
                List<AskTopic> acList = db.AskTopic.Where(a => a.PageId == ac.PageId).OrderBy(a => a.IndexNum).ToList();
                result.obj = acList;
            }
            else
            {
                result.obj = "数据表中没有选中项";
                result.success = false;
            }


            return Json(result);
        }

        public ActionResult AskPageSingleEdit(int pid)
        {
            AskPage ap = db.AskPage.Find(pid);
            return View(ap);
        }
        [HttpPost]
        public ActionResult AskPageSingleEdit(int pageId, string spanTopic)
        {
            AskPage ap = db.AskPage.Find(pageId);
            ap.Name = spanTopic;
            db.SaveChanges();
            return View(ap);
        }
         
        [HttpPost]
        public ActionResult AskTopicEdit(int id, int? projectid, FormCollection collection)
        {
            AskTopic r = db.AskTopic.Find(id);
            if (r == null)
            {
                int pid = 0;
                if (projectid != null) pid = projectid.Value;
                r = new AskTopic();
                db.AskTopic.Add(r);
            }
            TryUpdateModel(r, collection);
            if (ModelState.IsValid)
            {
                db.SaveChanges();
                  
                return Redirect("../AskTopicView/" + r.id);
            }
            return View(r);
        }
        [HttpPost]
        public JsonResult DeleteAskTopic(int id, FormCollection collection)
        {
            Result result = new Result();
            var r = db.AskTopic.Find(id);
            if (r == null)
            {
                result.obj = "找不到AskTopic";
            }
            else
            { 
                db.AskTopic.Remove(r);
                db.SaveChanges();
                result.success = true;
                result.obj = "已删除";
            }
            return Json(result);
        }

        /// <summary>
        /// 交换 题目项位置
        /// </summary>
        /// <param name="topicId"></param>
        /// <param name="increase"></param>
        /// <returns></returns>
        public JsonResult ChangeTopicItemIndex(int topicId, int increase)
        {
            //if (!UserInfo.CurUser.HasRight("系统管理-数据字典")) return Json(new Result { obj = "没有权限" });
            Result result = new Result();
            AskTopic item = db.AskTopic.Find(topicId);

            if (item == null)
            {
                result.obj = "找不到题目";
            }
            else
            {
                AskTopic another = null;
                if (increase > 0)
                {
                    another =
                        (from o in db.AskTopic
                         where o.PageId == item.PageId && o.IndexNum > item.IndexNum
                         orderby o.IndexNum
                         select o).FirstOrDefault();
                }
                else
                {
                    another =
                        (from o in db.AskTopic
                         where o.PageId == item.PageId && o.IndexNum < item.IndexNum
                         orderby o.IndexNum descending
                         select o).FirstOrDefault();
                }
                if (another == null)
                {
                    result.obj = "找不到可以交换位置的题目";
                }
                else
                {
                    int? temp = item.IndexNum;
                    item.IndexNum = another.IndexNum;
                    another.IndexNum = temp;
                    db.SaveChanges();
                    result.success = true;
                }
            }
            if (result.success == true)
            {
                var query = (from o in db.AskTopic where o.PageId == item.PageId orderby o.IndexNum ascending select o).ToList();
                result.obj = query;
            }
            return Json(result);
        }

        /*---------------------- AskPage------------------------------*/
        public ActionResult AskPageList()
        {
            return View();
        }
        public ActionResult AskPageList2()
        {
            return View();
        }
        [HttpPost]
        public JsonResult AskPageQuery(string sidx, string sord, int page, int rows, FormCollection collection)
        {
        
            List<object> parameters = new List<object>();
            ///search projectid
            ///id is pkname
            /// 0 condition,1 pagesize,2 startindex,3 pid
            string searchSql = "select  * from  a_AskPage where {0} and id not in (select t.id from (select  id from a_AskPage where {0}  order by id desc limit {2} ) as t ) order by id desc limit {1}";
            string totalCountSql = " select count(*) from  a_AskPage where {0}";
            int startindex = page * rows - rows;

            string condition = " 1=1 ";

           
            string searchSQL = string.Format(searchSql, condition, rows, startindex);
            totalCountSql = string.Format(totalCountSql, condition);

            StringBuilder sbSearch = new StringBuilder();

            string order = "order by id desc";
            if (!string.IsNullOrEmpty(sidx))
            {
                order = " order by " + sidx + " " + sord;
            }
            condition += order;

            db.Database.Connection.Open();
            var dynamicParams = new DynamicParameters();
            parameters.ForEach(o => { var p = o as SqlParameter; dynamicParams.Add(p.ParameterName, p.Value, p.DbType); });
            var query = db.Database.Connection.Query<AskPage>(searchSQL, param: dynamicParams).ToList();
            var totalQuery = db.Database.Connection.Query<int>(totalCountSql, param: dynamicParams).ToList()[0];

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


        public ActionResult AskPageView(int id)
        {
            AskPage r = db.AskPage.Find(id);
            return View(r);
        }
        public ActionResult AskPageView2(int id)
        {
            AskPage r = db.AskPage.Find(id);
            return View(r);
        }
        public ActionResult AskPageEdit(int id)
        {
            AskPage r = db.AskPage.Find(id);
            return View(r);
        }
        public ActionResult AskPageEdit2(int id)
        {
            AskPage r = db.AskPage.Find(id);
            return View(r);
        }
        [HttpPost]
        public ActionResult AskPageEdit(int id, int? projectid, FormCollection collection)
        {
            AskPage r = db.AskPage.Find(id);
            if (r == null)
            {
                int pid = 0;
                if (projectid != null) pid = projectid.Value;
                r = new AskPage();
                db.AskPage.Add(r);
            }
            TryUpdateModel(r, collection);
            if (ModelState.IsValid)
            {
                db.SaveChanges(); 
                return Redirect("../AskPageView/" + r.id);
            }
            return View(r);
        }

        [HttpPost]
        public JsonResult DeleteAskPage(int id, FormCollection collection)
        {
            Result result = new Result();
            var r = db.AskPage.Find(id);
            if (r == null)
            {
                result.obj = "找不到AskPage";
            }
            else
            { 
                db.AskPage.Remove(r);
                db.SaveChanges();
                result.success = true;
                result.obj = "已删除";
            }
            return Json(result);
        }



        /// <summary>
        /// 获取问卷信息
        /// </summary>
        /// <param name="pageid"></param>
        /// <returns></returns>
        public JsonResult GetPageTopic(int pageid)
        {
            PageFull pFull = new PageFull();

            AskPage page = db.AskPage.Find(pageid);
            pFull.Page = page;

            List<AskTopic> topics = db.AskTopic.Where(a => a.PageId == pageid).OrderBy(a => a.IndexNum).ToList();

            foreach (AskTopic topic in topics)
            {
                List<AskContent> contents = db.AskContent.Where(a => a.TopicId == topic.id).OrderBy(a => a.IndexNum).ToList();
                pFull.Topics.Add(new TopicFull() { Contents = contents, Topic = topic });
            }

            return Json(pFull); ;
        }

    }
}
