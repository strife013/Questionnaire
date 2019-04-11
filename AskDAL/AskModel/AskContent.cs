



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
    /// <para>摘要：AskContentModel 类，业务模型。</para>
    /// <para>说明：</para>
    /// <para>Programmer： Sean</para>
    /// <para>Email： </para>
    /// <remarks>
    /// 对应数据库表：AskContent
    /// <table class="dtTABLE" cellspacing="0">
    /// <tr valign="top"><th>序号</th><th>列名</th><th>数据类型</th><th>长度</th><th>小数位</th><th>标识</th><th>主键</th><th>允许空</th><th>默认值</th><th>字段说明</th></tr>
    /// <tr valign="top"><td>1</td><td>id</td><td>int</td><td>4</td><td></td><td>√</td><td>√</td><td></td><td></td><td></td></tr>
    /// <tr valign="top"><td>2</td><td>Desc</td><td>varchar</td><td>850</td><td></td><td></td><td></td><td>√</td><td></td><td>描述</td></tr>
    /// <tr valign="top"><td>3</td><td>Type</td><td>varchar</td><td>50</td><td></td><td></td><td></td><td>√</td><td></td><td>类型</td></tr>
    /// <tr valign="top"><td>4</td><td>Title</td><td>varchar</td><td>350</td><td></td><td></td><td></td><td>√</td><td></td><td>标题</td></tr>
    /// <tr valign="top"><td>5</td><td>Weight</td><td>varchar</td><td>50</td><td></td><td></td><td></td><td>√</td><td></td><td>权重</td></tr>
    /// <tr valign="top"><td>6</td><td>State</td><td>varchar</td><td>50</td><td></td><td></td><td></td><td>√</td><td></td><td>状态</td></tr>
    /// <tr valign="top"><td>7</td><td>CreateTime</td><td>datetime</td><td>8</td><td></td><td></td><td></td><td>√</td><td></td><td>创建时间</td></tr>
    /// <tr valign="top"><td>8</td><td>UpdateTime</td><td>datetime</td><td>8</td><td></td><td></td><td></td><td>√</td><td></td><td></td></tr>
    /// <tr valign="top"><td>9</td><td>TopicId</td><td>int</td><td>4</td><td></td><td></td><td></td><td>√</td><td></td><td>createhidden_updatehidden</td></tr>
    /// <tr valign="top"><td>10</td><td>IndexNum</td><td>int</td><td>4</td><td></td><td></td><td></td><td>√</td><td></td><td>排序值</td></tr>
    /// </table>
    /// </remarks>
    /// </summary>
    ///################################################################################################
    [Table("a_AskContent")]
    [Serializable]
    public partial class AskContent
    {
        /// <summary>
        /// ABCD
        /// </summary>
        [DisplayName("选择描述")]
        public string IndexDesc { get; set; }
        public static string LogClass = "问卷题目选项";
        #region -  公共属性  ------------------------------------------------------------

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
        /// 描述
        /// </summary>
        private string _Desc = "";
        /// <summary>
        /// 描述
        /// </summary>
        [DisplayName("描述")]
        public string Desc
        {
            set { _Desc = value; }
            get { return _Desc; }
        }





        /// <summary>
        /// 类型
        /// </summary>
        private string _Type = "";
        /// <summary>
        /// 类型
        /// </summary>
        [DisplayName("类型")]
        public string Type
        {
            set { _Type = value; }
            get { return _Type; }
        }





        /// <summary>
        /// 标题
        /// </summary>
        private string _Title = "";
        /// <summary>
        /// 标题
        /// </summary>
        [DisplayName("标题")]
        public string Title
        {
            set { _Title = value; }
            get { return _Title; }
        }





        /// <summary>
        /// 权重
        /// </summary>
        private string _Weight = "";
        /// <summary>
        /// 权重
        /// </summary>
        [DisplayName("权重")]
        public string Weight
        {
            set { _Weight = value; }
            get { return _Weight; }
        }





        /// <summary>
        /// 状态
        /// </summary>
        private string _State = "";
        /// <summary>
        /// 状态
        /// </summary>
        [DisplayName("状态")]
        public string State
        {
            set { _State = value; }
            get { return _State; }
        }





        /// <summary>
        /// 创建时间
        /// </summary>
        private DateTime? _CreateTime = SqlDateTime.MinValue.Value;
        /// <summary>
        /// 创建时间
        /// </summary>
        [DisplayName("创建时间")]
        public DateTime? CreateTime
        {
            set { _CreateTime = value; }
            get { return _CreateTime; }
        }

        private DateTime _CreateTimeStart = SqlDateTime.MinValue.Value;
        [NotMapped]
        public DateTime CreateTimeStart
        {
            set { _CreateTimeStart = value; }
            get { return _CreateTimeStart; }
        }
        private DateTime _CreateTimeEnd = SqlDateTime.MinValue.Value;
        [NotMapped]
        public DateTime CreateTimeEnd
        {
            set { _CreateTimeEnd = value; }
            get { return _CreateTimeEnd; }
        }




        /// <summary>
        /// 
        /// </summary>
        private DateTime? _UpdateTime = SqlDateTime.MinValue.Value;
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("UpdateTime")]
        public DateTime? UpdateTime
        {
            set { _UpdateTime = value; }
            get { return _UpdateTime; }
        }

        private DateTime _UpdateTimeStart = SqlDateTime.MinValue.Value;
        [NotMapped]
        public DateTime UpdateTimeStart
        {
            set { _UpdateTimeStart = value; }
            get { return _UpdateTimeStart; }
        }
        private DateTime _UpdateTimeEnd = SqlDateTime.MinValue.Value;
        [NotMapped]
        public DateTime UpdateTimeEnd
        {
            set { _UpdateTimeEnd = value; }
            get { return _UpdateTimeEnd; }
        }




        /// <summary>
        /// createhidden_updatehidden
        /// </summary>
        private int? _TopicId;
        /// <summary>
        /// createhidden_updatehidden
        /// </summary>
        [DisplayName("createhidden")]
        public int? TopicId
        {
            set { _TopicId = value; }
            get { return _TopicId; }
        }





        /// <summary>
        /// 排序值
        /// </summary>
        private int? _IndexNum;
        /// <summary>
        /// 排序值
        /// </summary>
        [DisplayName("排序值")]
        public int? IndexNum
        {
            set { _IndexNum = value; }
            get { return _IndexNum; }
        }








        #endregion ----------------------------------------------------------------------
    }
}
