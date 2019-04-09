



using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace OUDAL
{
    ///################################################################################################
    /// <summary>
    /// <para>摘要：AskResultModel 类，业务模型。</para>
    /// <para>说明：</para>
    /// <para>Programmer： Sean</para>
    /// <para>Email： </para>
    /// <remarks>
    /// 对应数据库表：AskResult
    /// <table class="dtTABLE" cellspacing="0">
    /// <tr valign="top"><th>序号</th><th>列名</th><th>数据类型</th><th>长度</th><th>小数位</th><th>标识</th><th>主键</th><th>允许空</th><th>默认值</th><th>字段说明</th></tr>
    /// <tr valign="top"><td>1</td><td>id</td><td>int</td><td>4</td><td></td><td>√</td><td>√</td><td></td><td></td><td></td></tr>
    /// <tr valign="top"><td>2</td><td>score</td><td>decimal</td><td>9</td><td>18,0</td><td></td><td></td><td>√</td><td></td><td>分数</td></tr>
    /// <tr valign="top"><td>3</td><td>result</td><td>varchar</td><td>850</td><td></td><td></td><td></td><td>√</td><td></td><td>结果说明</td></tr>
    /// <tr valign="top"><td>4</td><td>uid</td><td>int</td><td>4</td><td></td><td></td><td></td><td>√</td><td></td><td></td></tr>
    /// <tr valign="top"><td>5</td><td>uname</td><td>varchar</td><td>350</td><td></td><td></td><td></td><td>√</td><td></td><td>客户名</td></tr>
    /// <tr valign="top"><td>6</td><td>createdate</td><td>datetime</td><td>8</td><td></td><td></td><td></td><td>√</td><td></td><td>createtime</td></tr>
    /// <tr valign="top"><td>7</td><td>updatetime</td><td>datetime</td><td>8</td><td></td><td></td><td></td><td>√</td><td></td><td>updatetime</td></tr>
    /// <tr valign="top"><td>8</td><td>pageid</td><td>int</td><td>4</td><td></td><td></td><td></td><td>√</td><td></td><td>问卷id</td></tr>
    /// <tr valign="top"><td>9</td><td>pagetitle</td><td>varchar</td><td>450</td><td></td><td></td><td></td><td>√</td><td></td><td>问卷名称</td></tr>
    /// <tr valign="top"><td>10</td><td>answer</td><td>varchar</td><td>850</td><td></td><td></td><td></td><td>√</td><td></td><td></td></tr>
    /// <tr valign="top"><td>11</td><td>state</td><td>varchar</td><td>50</td><td></td><td></td><td></td><td>√</td><td></td><td></td></tr>
    /// </table>
    /// </remarks>
    /// </summary>
    ///################################################################################################
    [Table("a_AskResult")]
    [Serializable]
    public partial class AskResult
    {
        [NotMapped]
        /// <summary>
        /// 皮肤类型
        /// </summary>
        public string PiFuType
        {
            get
            {
                return msg10;
            }
        }

        /// <summary>
        /// 色素型
        /// </summary>
        [NotMapped]
        public string SeSuType
        {
            get
            {
                if (msg11 == null) return "";
                string[] sss = msg12.Split(",".ToCharArray());
                int selcount = 0;
                foreach (string s in sss)
                {
                    if (!string.IsNullOrEmpty(s))
                        selcount++;
                    //选6
                    if(s== "以上都没有") return "非色素型";
                }
                if (selcount == 1) return "轻度色素型";
                if (selcount <= 3) return "中度色素型";
                if (selcount <= 5) return "重度色素型"; 
                return "";
            }
        }

        /// <summary>
        /// 敏感性
        /// </summary>
        [NotMapped]
        public string MinGanType
        {
            get
            {
                if (msg11 == null) return "不敏感";
                string[] sss = msg11.Split(",".ToCharArray());
                int selcount = 0;
                foreach(string s in sss)
                {
                    if (!string.IsNullOrEmpty(s))
                        selcount++;
                }
                if (selcount <= 2) return "轻度敏感型";
                if (selcount <= 4) return "中度敏感型";
                if (selcount <= 7) return "严重敏感型";
                if (selcount >= 8) return "耐受型";
                return "";
            }
        }
        [NotMapped]
       public List<AskAnswer> AskAnswers
        {
            get
            {
                if (!string.IsNullOrEmpty(AnswerJson))
                    return Newtonsoft.Json.JsonConvert.DeserializeObject<List<AskAnswer>>(AnswerJson);
                return new List<AskAnswer>();
            }
        }
        public static string LogClass = "问卷结果表";

        public string msg1 { get; set; }
        public string msg2 { get; set; }

         public void SetData()
        {
            if(msg33!=null) msg3 = string.Join(",", msg33);
            if (msg44 != null) msg4 = string.Join(",", msg44);
            if (msg55 != null) msg5 = string.Join(",", msg55);
            if (msg66 != null) msg6 = string.Join(",", msg66);
            if (msg77 != null) msg7 = string.Join(",", msg77);
            if (msg88 != null) msg8 = string.Join(",", msg88);
            if (msg99 != null) msg9 = string.Join(",", msg99);

            if (msg110 != null) msg10 = string.Join(",", msg110);
            if (msg111 != null) msg11 = string.Join(",", msg111);
            if (msg112 != null) msg12 = string.Join(",", msg112);
            if (msg113 != null) msg13 = string.Join(",", msg113);
            if (msg114 != null) msg14 = string.Join(",", msg114);
            if (msg115 != null) msg15 = string.Join(",", msg115);
            if (msg116 != null) msg16 = string.Join(",", msg116);
            if (msg117 != null) msg17 = string.Join(",", msg117);
        }
        public string msg3 { get; set; }

        [NotMapped]
        public IEnumerable<string> msg33 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg114 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg115{ get; set; }
        [NotMapped]
        public IEnumerable<string> msg116 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg117 { get; set; }

        [NotMapped]
        public IEnumerable<string> msg110 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg111 { get; set; }


        [NotMapped]
        public IEnumerable<string> msg88 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg99 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg112 { get; set; }

        [NotMapped]
        public IEnumerable<string> msg113 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg44 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg55 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg66 { get; set; }
        [NotMapped]
        public IEnumerable<string> msg77 { get; set; }
        public string msg100 { get; set; }
        public string msg101 { get; set; }
        public string msg4 { get; set; }
        public string msg5 { get; set; }
        public string msg6 { get; set; }
        public string msg7 { get; set; }
        public string msg8 { get; set; }
        public string msg9 { get; set; }
        public string msg10 { get; set; }
        public string msg11 { get; set; }
        public string msg12 { get; set; }
        public string msg13 { get; set; }
        public string msg14 { get; set; }
        public string msg15 { get; set; }
        public string msg16 { get; set; }
        public string msg17 { get; set; }
        public string msg18 { get; set; }
        public string msg19 { get; set; }
        

        #region -  公共属性  ------------------------------------------------------------
        [DisplayName("所属项目")]
        public int? projectid { get; set; }

        [DisplayName("Json结果")]
        public string AnswerJson { get; set; }
        public int AppointmentId { get; set; }
        /// <summary>
        /// 
        /// </summary>
        private int _id;
        /// <summary>
        /// 
        /// </summary>
        [Key]
        public int id
        {
            set { _id = value; }
            get { return _id; }
        }





        /// <summary>
        /// 分数
        /// </summary>
        private decimal _score;
        /// <summary>
        /// 分数
        /// </summary>
        [DisplayName("分数")]
        public decimal score
        {
            set { _score = value; }
            get { return _score; }
        }





        /// <summary>
        /// 结果说明
        /// </summary>
        private string _result = "";
        /// <summary>
        /// 结果说明
        /// </summary>
        [DisplayName("结果说明")]
        public string result
        {
            set { _result = value; }
            get { return _result; }
        }


        private string _answerHtml = "";
        [DisplayName("回答明细")]
        public string answerHtml
        {
            set { _answerHtml = value; }
            get { return _answerHtml; }
        }




        /// <summary>
        /// 
        /// </summary>
        private int _uid;
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("uid")]
        public int uid
        {
            set { _uid = value; }
            get { return _uid; }
        }





        /// <summary>
        /// 客户名
        /// </summary>
        private string _uname = "";
        /// <summary>
        /// 客户名
        /// </summary>
        [DisplayName("客户名")]
        public string uname
        {
            set { _uname = value; }
            get { return _uname; }
        }





        /// <summary>
        /// createtime
        /// </summary>
        private DateTime? _createdate = SqlDateTime.MinValue.Value;
        /// <summary>
        /// createtime
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime? createdate
        {
            set { _createdate = value; }
            get { return _createdate; }
        }

        private DateTime _createdateStart = SqlDateTime.MinValue.Value;
        [NotMapped]
        public DateTime createdateStart
        {
            set { _createdateStart = value; }
            get { return _createdateStart; }
        }
        private DateTime _createdateEnd = SqlDateTime.MinValue.Value;
        [NotMapped]
        public DateTime createdateEnd
        {
            set { _createdateEnd = value; }
            get { return _createdateEnd; }
        }




        /// <summary>
        /// updatetime
        /// </summary>
        private DateTime? _updatetime = SqlDateTime.MinValue.Value;
        /// <summary>
        /// updatetime
        /// </summary>
        [DisplayName("updatetime")]
        public DateTime? updatetime
        {
            set { _updatetime = value; }
            get { return _updatetime; }
        }

        private DateTime _updatetimeStart = SqlDateTime.MinValue.Value;
        [NotMapped]
        public DateTime updatetimeStart
        {
            set { _updatetimeStart = value; }
            get { return _updatetimeStart; }
        }
        private DateTime _updatetimeEnd = SqlDateTime.MinValue.Value;
        [NotMapped]
        public DateTime updatetimeEnd
        {
            set { _updatetimeEnd = value; }
            get { return _updatetimeEnd; }
        }




        /// <summary>
        /// 问卷id
        /// </summary>
        private int _pageid;
        /// <summary>
        /// 问卷id
        /// </summary>
        [DisplayName("问卷id")]
        public int pageid
        {
            set { _pageid = value; }
            get { return _pageid; }
        }





        /// <summary>
        /// 问卷名称
        /// </summary>
        private string _pagetitle = "";
        /// <summary>
        /// 问卷名称
        /// </summary>
        [DisplayName("问卷名称")]
        public string pagetitle
        {
            set { _pagetitle = value; }
            get { return _pagetitle; }
        }





        /// <summary>
        /// 
        /// </summary>
        private string _answer = "";
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("回答结果")]
        public string answer
        {
            set { _answer = value; }
            get { return _answer; }
        }





        /// <summary>
        /// 
        /// </summary>
        private string _state = "";
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("state")]
        public string state
        {
            set { _state = value; }
            get { return _state; }
        }








        #endregion ----------------------------------------------------------------------
    }
}
