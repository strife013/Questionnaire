



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
        /// <para>摘要：AskPageModel 类，业务模型。</para>
        /// <para>说明：</para>
        /// <para>Programmer： Sean</para>
        /// <para>Email： </para>
        /// <remarks>
        /// 对应数据库表：AskPage
        /// <table class="dtTABLE" cellspacing="0">
        /// <tr valign="top"><th>序号</th><th>列名</th><th>数据类型</th><th>长度</th><th>小数位</th><th>标识</th><th>主键</th><th>允许空</th><th>默认值</th><th>字段说明</th></tr>
        /// <tr valign="top"><td>1</td><td>id</td><td>int</td><td>4</td><td></td><td>√</td><td>√</td><td></td><td></td><td></td></tr>
        /// <tr valign="top"><td>2</td><td>CreateTime</td><td>datetime</td><td>8</td><td></td><td></td><td></td><td>√</td><td></td><td></td></tr>
        /// <tr valign="top"><td>3</td><td>Type</td><td>varchar</td><td>50</td><td></td><td></td><td></td><td>√</td><td></td><td>类型</td></tr>
        /// <tr valign="top"><td>4</td><td>Name</td><td>varchar</td><td>350</td><td></td><td></td><td></td><td>√</td><td></td><td>问卷名称</td></tr>
        /// <tr valign="top"><td>5</td><td>State</td><td>varchar</td><td>50</td><td></td><td></td><td></td><td>√</td><td></td><td>状态</td></tr>
        /// <tr valign="top"><td>6</td><td>Weight</td><td>varchar</td><td>50</td><td></td><td></td><td></td><td>√</td><td></td><td>权重</td></tr>
        /// </table>
        /// </remarks>
        /// </summary>
        ///################################################################################################
        [Table("a_AskPage")]
    [Serializable]
    public partial class AskPage
    {

        public static string LogClass = "调查问卷";
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
        /// 
        /// </summary>
        private DateTime? _CreateTime = SqlDateTime.MinValue.Value;
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("CreateTime")]
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
        /// 问卷名称
        /// </summary>
        private string _Name = "";
        /// <summary>
        /// 问卷名称
        /// </summary>
        [DisplayName("问卷名称")]
        public string Name
        {
            set { _Name = value; }
            get { return _Name; }
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








        #endregion ----------------------------------------------------------------------
    }
}
