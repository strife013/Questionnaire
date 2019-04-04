



using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using System.Data.SqlTypes;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace OUDAL
{
    ///################################################################################################
    /// <summary>
    /// <para>摘要：AskPageTopicModel 类，业务模型。</para>
    /// <para>说明：</para>
    /// <para>Programmer： Sean</para>
    /// <para>Email： </para>
	/// <remarks>
    /// 对应数据库表：AskPageTopic
    /// <table class="dtTABLE" cellspacing="0">
    /// <tr valign="top"><th>序号</th><th>列名</th><th>数据类型</th><th>长度</th><th>小数位</th><th>标识</th><th>主键</th><th>允许空</th><th>默认值</th><th>字段说明</th></tr>
    /// <tr valign="top"><td>1</td><td>id</td><td>int</td><td>4</td><td></td><td>√</td><td>√</td><td></td><td></td><td></td></tr>
    /// <tr valign="top"><td>2</td><td>AskPageId</td><td>int</td><td>4</td><td></td><td></td><td></td><td>√</td><td></td><td></td></tr>
    /// <tr valign="top"><td>3</td><td>AskTopicId</td><td>int</td><td>4</td><td></td><td></td><td></td><td>√</td><td></td><td></td></tr>
    /// <tr valign="top"><td>4</td><td>State</td><td>varchar</td><td>50</td><td></td><td></td><td></td><td>√</td><td></td><td></td></tr>
    /// </table>
    /// </remarks>
    /// </summary>
    ///################################################################################################
    [Serializable]
    public partial class AskPageTopic 
    {
    
        public static string LogClass = "AskPageTopic";
        #region -  公共属性  ------------------------------------------------------------

        /// <summary>
        /// 
        /// </summary>
        private int _id ;
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
        private int _AskPageId ;
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("AskPageId")]
        public int AskPageId
        {
            set { _AskPageId = value; }
            get { return _AskPageId; }
        }
        
        
          
        
        
        /// <summary>
        /// 
        /// </summary>
        private int _AskTopicId ;
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("AskTopicId")]
        public int AskTopicId
        {
            set { _AskTopicId = value; }
            get { return _AskTopicId; }
        }
        
        
          
        
        
        /// <summary>
        /// 
        /// </summary>
        private string _State  = "";
        /// <summary>
        /// 
        /// </summary>
        [DisplayName("State")]
        public string State
        {
            set { _State = value; }
            get { return _State; }
        }
        
        
          
        
        
		
        
        
        #endregion ----------------------------------------------------------------------
    } 
}
