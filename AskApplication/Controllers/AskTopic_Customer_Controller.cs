




using BaseErp.Web.Models;
using Dapper;
using HealthErpDAL;
using OUDAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web.Mvc;
namespace Matrix.Web.Controllers
{
    public partial class AskTopicController : Controller
    {
        private AskDBContent db = new AskDBContent();
        protected override void Dispose(bool disposing)
        {
            
            db.Dispose();
            base.Dispose(disposing);
        }

        /*---------------------- AskTopic------------------------------*/
        // [MyAuthorize("AskTopic-AskTopic查看")]
        public ActionResult AskTopicList()
        {
            return View();
        }
        //[MyAuthorize(MyAuthorizeResultEnum.JsonResultType, "AskTopic-AskTopic查看")]
        //int projectid,
        [HttpPost]
        public JsonResult AskTopicQuery(string sidx, string sord, int page, int rows, FormCollection collection)
        { 
            List<object> parameters = new List<object>();
            ///search projectid
            ///id is pkname
            /// 0 condition,1 pagesize,2 startindex,3 pid
            string searchSql = "select top {1} * from  [AskTopic] where {0} and id not in (select top {2} id from [AskTopic] where {0}  order by id desc) order by id desc ";
            string totalCountSql = " select count(*) from  [AskTopic] where {0}";
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
            var query = db.Database.Connection.Query<AskTopic>(searchSQL, param: dynamicParams).ToList();
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


        /// <summary>
        /// 获取所有没分配Page的题目
        /// </summary>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AskTopicQuery2(int pageindex, int pagesize)
        {
            //List<AskContent> cList = db.AskContent.OrderByDescending(c => c.id).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();

            List<AskTopic> atList = db.AskTopic.OrderBy(t => t.id).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();

            int pagenum = db.AskTopic.Count();

            var jsonData = new
            {
                total = pagenum,
                pageindex = pageindex,
                pagesize = pagesize,
                rows = atList
            };
            return Json(jsonData);
        }

        // [MyAuthorize("AskTopic-AskTopic查看")]
        public ActionResult AskTopicView(int id)
        {
            AskTopic r = db.AskTopic.Find(id);
            return View(r);
        }
         
        // [MyAuthorize("AskTopic-AskTopic编辑")]
        public ActionResult AskTopicEdit(int id)
        {

            AskTopic r = db.AskTopic.Find(id);
            return View(r);
        }
        public ActionResult AskTopicEdit2(int id)
        {

            AskTopic r = db.AskTopic.Find(id);
            return View(r);
        }
         
        /// <summary>
        /// 选项上移
        /// </summary>
        /// <param name="cid"></param>
        /// <returns></returns>
        public JsonResult UpAskContent(int cid)
        {
            return ChangeContentItemIndex(cid, 1);
        }

        public JsonResult DownAskContent(int cid)
        {
            return ChangeContentItemIndex(cid, -1);
        }

        public JsonResult EditAskContent(int cid, string desc)
        {

            AskContent ac = db.AskContent.Find(cid);
            if (ac != null)
            {
                ac.Desc = desc;
                db.SaveChanges();
            }
            return Json(ac);
        }
        /// <summary>
        /// 交换 题目项位置
        /// </summary>
        /// <param name="contentId"></param>
        /// <param name="increase"></param>
        /// <returns></returns>
        public JsonResult ChangeContentItemIndex(int contentId, int increase)
        {
            //if (!UserInfo.CurUser.HasRight("系统管理-数据字典")) return Json(new Result { obj = "没有权限" });
            Result result = new Result();
            AskContent item = db.AskContent.Find(contentId);

            if (item == null)
            {
                result.obj = "找不到题目项";
            }
            else
            {
                AskContent another = null;
                if (increase > 0)
                {
                    another =
                        (from o in db.AskContent
                         where o.TopicId == item.TopicId && o.IndexNum > item.IndexNum
                         orderby o.IndexNum
                         select o).FirstOrDefault();
                }
                else
                {
                    another =
                        (from o in db.AskContent
                         where o.TopicId == item.TopicId && o.IndexNum < item.IndexNum
                         orderby o.IndexNum descending
                         select o).FirstOrDefault();
                }
                if (another == null)
                {
                    result.obj = "找不到可以交换位置的题目项";
                }
                else
                {
                    int? temp = item.IndexNum;
                    item.IndexNum = another.IndexNum;
                    item.IndexDesc = getAlphByIndex(item.IndexNum.Value, item.TopicId.Value);

                    another.IndexNum = temp;
                    another.IndexDesc = getAlphByIndex(another.IndexNum.Value, another.TopicId.Value);

                    db.SaveChanges();
                    result.success = true; 

                }
            }
            if (result.success == true)
            {
                var query = (from o in db.AskContent where o.TopicId == item.TopicId orderby o.IndexNum descending select o).ToList();
                result.obj = query;
            }
            return Json(result);
        }

        /// <summary>
        ///   查询属于这个题目的所有题目条目
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="pageindex"></param>
        /// <param name="pagesize"></param>
        /// <returns></returns>
        //[HttpPost]
        //public JsonResult QueryAskContent(int? cid, int pageindex, int pagesize)
        //{

        //    List<AskContent> acList = new List<AskContent>();
        //    acList = (from o in db.AskContent join p in db.AskTopicContent.AsNoTracking() on o.id equals p.AskContentId select o).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();

        //    var jsonData = new
        //    {
        //        curindex = pageindex,
        //        pagesize = pagesize,
        //        records = acList.Count,
        //        rows = acList
        //    };
        //    return Json(jsonData);

        //}

        /// <summary>
        /// 给一个题目分配明细选项
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="tids">topic ids</param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult SaveAskTopic(int? cid, string tids)
        {
            //需要查询不属于这个题目的所有题目条目
            //List<AskContent> acList = new List<AskContent>();
            //acList = (from o in db.AskContent join p in db.AskTopicContent.AsNoTracking() on o.id equals p.AskContentId select o).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            if (cid == null) return null;

            int[] topicids = tids.Split(","[0]).Select(s => Convert.ToInt32(s)).ToArray();
            foreach (int tid in topicids)
            {
                //int findCount = db.AskTopicContent.Select(a => a.AskContentId == cid.Value && a.AskTopicId == tid).Count();
                //if (findCount <= 0)
                //{
                //    db.AskTopicContent.Add(new AskTopicContent() { AskContentId = cid.Value, AskTopicId = tid });
                //}
            }

            db.SaveChanges();

            return Json(true);
        }

        [HttpPost]
        public JsonResult SelectAskContents(int topicId)
        {
            List<AskContent> acList = db.AskContent.Where(a => a.TopicId == topicId).OrderByDescending(a => a.IndexNum).ToList();

            return Json(acList);
        }


        public static string getAlphByIndex(int index,int tid)
        {
            using(AskDBContent ddb = new AskDBContent())
            {
                var askContents = (from o in ddb.AskContent where o.IndexNum < index && o.TopicId == tid select o).ToList();
                return AskTopic.AlphaList[askContents.Count];
            }
          
        }
        /// <summary>
        /// 添加topic下题目项并返回tid对应的所有content
        /// </summary>
        /// <param name="desc"></param>
        /// <param name="tid"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAskContent(string desc, int tid)
        {
            int next = 0;
            var query = (from o in db.AskContent where o.TopicId == tid orderby o.IndexNum select o).ToList();
            if (query.Count > 0) next = query[query.Count - 1].IndexNum.Value + 1;

           

            AskContent ac = new AskContent();
            ac.TopicId = tid;
            ac.Desc = desc;
            ac.CreateTime = DateTime.Now;
            ac.IndexNum = next;

            ac.IndexDesc = getAlphByIndex(next,tid);
            db.AskContent.Add(ac);
              
            db.SaveChanges();

            List<AskContent> acList = db.AskContent.Where(a => a.TopicId == tid).OrderBy(a => a.IndexNum).ToList();
            return Json(acList);
        }

        /// <summary>
        /// 增加题目
        /// </summary>
        /// <param name="title"></param>
        /// <param name="tid"></param>
        /// <param name="order"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddAskTopic(string title,int pageId, int order)
        {
            AskTopic at = new AskTopic();
            at.Title = title;
            at.Order = order;
            at.PageId = pageId;
            at.CreateTime = DateTime.Now;
            db.AskTopic.Add(at);
            db.SaveChanges();

            return Json(at);
        }


        /// <summary>
        /// 删除问题的选项
        /// </summary>
        /// <param name="cid"></param>
        /// <param name="tids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult DeleteAskContent(int cid)
        {
            //需要查询不属于这个题目的所有题目条目
            //List<AskContent> acList = new List<AskContent>();
            //acList = (from o in db.AskContent join p in db.AskTopicContent.AsNoTracking() on o.id equals p.AskContentId select o).Skip((pageindex - 1) * pagesize).Take(pagesize).ToList();
            AskContent ac = db.AskContent.Find(cid);
            Result result = new Result();
            if (ac != null)
            {
                db.AskContent.Remove(ac);
                db.SaveChanges();
                result.success = true;
                List<AskContent> acList = db.AskContent.Where(a => a.TopicId == ac.TopicId).OrderByDescending(a => a.IndexNum).ToList();
                result.obj = acList;
            }
            else
            {
                result.obj = "数据表中没有选中项";
                result.success = false;
            }
           
          
            return Json(result);
        }
        // [MyAuthorize("AskTopic-AskTopic编辑")]
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
        // [MyAuthorize(MyAuthorizeResultEnum.JsonResultType, "AskTopic-AskTopic编辑")]
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

    }
}
