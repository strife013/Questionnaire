using System;
using System.Text;
using System.Collections.Generic;
using System.Web;
namespace BaseErp.Web
{
    /// <summary>
    /// JqGridHelper 的摘要说明
    /// </summary>
    public class JqGridHelper
    {        
//        static string includeFile = string.Format(@"<link rel='stylesheet' type='text/css' media='screen' href='{0}/jqgrid/themes/basic/grid.css' />
//<link rel='stylesheet' type='text/css' media='screen' href='{0}/jqgrid/themes/jqmodal.css' />
//<script src='{0}/jqgrid/js/jquery.jqGrid.js' type='text/javascript'></script>
//<script src='{0}/jqgrid/js/include.ashx' type='text/javascript'></script>
//<script src='{0}/jqgrid/js/jqModal.js' type='text/javascript'></script>
//<script src='{0}/jqgrid/js/jqDnR.js' type='text/javascript'></script>
//", HttpRuntime.AppDomainAppVirtualPath+"/scripts");

        public static string path = HttpRuntime.AppDomainAppVirtualPath;
        
        string name;
        string url;//
        public bool isMultiSelect;
        public bool isPager;
        public HttpContextBase context;
        public JqGridHelper(HttpContextBase _context, string gridName, string fileName)
        {
            this.context = _context;
            this.name = gridName;
            this.url = fileName;
            this.isMultiSelect = false;
            this.isPager = true;

        }
        /// <summary>
        /// 要求在script中定义colmodel的变量名字为 colModel+name
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="gridName"></param>
        /// <param name="fileName"></param>
        public JqGridHelper( string gridName, string fileName):this( null,  gridName,  fileName)
        {
            
        }
        public JqGridHelper(HttpContextBase _context,string path, string gridName, string fileName, bool _isMultiSelect, bool _isPager)
        {
            this.context = _context;
            this.name = gridName;
            this.url = fileName;
            this.isMultiSelect = _isMultiSelect;
            this.isPager = _isPager;

        }
        /// <summary>
        /// 要求在script中定义colmodel的变量名字为 colModel+name
        /// </summary>
        /// <param name="_page"></param>
        /// <param name="gridName"></param>
        /// <param name="fileName"></param>
        /// <param name="_isMultiSelect"></param>
        /// <param name="_isPager"></param>
        public JqGridHelper(string path, string gridName, string fileName, bool _isMultiSelect, bool _isPager):this(null, path,  gridName,  fileName,  _isMultiSelect,  _isPager)
        {
        }
        /// <summary>
        /// 输出 var grid的名字，供js使用.  如 var grid=<%=GridVar%>
        /// </summary>
        /// <returns></returns>
        public string GridVar
        {
            get { return string.Format("grid{0}", name); }
        }
        /// <summary>
        /// 输出 grid的名字，供js使用 如 var grid=$('#<%=GridName%>')
        /// </summary>
        /// <returns></returns>
        public string GridName
        {
            get { return string.Format("list{0}", name); }
        }
        /// <summary>
        /// 输出 table & pager
        /// </summary>
        /// <returns></returns>
        public string OutTable()
        {
            string str = @"<table id='list{0}' style='padding-right:10px;' ></table>
<div id='pager{0}' ></div><input type='hidden' id='hidData' runat='server' />";
            return string.Format(str, name);
        }

        /// <summary>
        /// 输出 grid=jquery('xx').jqgrid(. 这部分
        /// </summary>
        /// <returns></returns>
        public string OutScript()
        {
            string fullurl = url;
            if (fullurl.IndexOf('~') >= 0)
            {
                fullurl = fullurl.Replace("~", path);
            }
            
            string str = @"
function SysAfterInsertRow(rowid,aData,rowelem) {{
    if(rowid == '-1' || rowid == '-2'){{
        var Grid = $('#list{0}{1}');
        $('#jqg_' + rowid).hide();

        var colName = '';
        for(var i=0;i<colModel{0}.length;i++) {{
            if(colModel{0}[i]){{
                if(colModel{0}[i].hidden!=true) {{
                    colName=colModel{0}[i].name;
                    break;
                }}
            }}
        }}
        /*
        if(rowid == '-1') {{
            var colModel1 = Grid.getGridParam('colModel');
            for(var i=0;i<colModel1.length;i++) {{
                if(colModel1[i]){{
                    colModel1[i].formatter = '';
                }}
            }}
            Grid.setGridParam({{colModel: colModel1}});
        }}*/

        if (colName != '' && Grid.getCell(rowid, colName) != '') {{
            Grid.setCell(rowid, colName, rowid=='-1'?'<b>本页小计</b>':'<b>合计</b>');
        }}

        return;
    }}
    AfterInsertRow(rowid,aData,rowelem);
}}

var WIDTH=800;var HEIGHT=435;
var PAGEROW=14;
if(document.body.clientHeight>700){{HEIGHT=640;PAGEROW=25;}}
if(document.body.clientWidth>1024){{WIDTH=900;}}

if(typeof(setWidth)!='undefined')WIDTH=setWidth();
if(typeof(setHeight)!='undefined')HEIGHT=setHeight();
var SORTORDER='';var SORTNAME='';
if(typeof(SortOrder)!='undefined')SORTORDER=SortOrder();
if(typeof(SortName)!='undefined')SORTNAME=SortName();
if(typeof(AfterInsertRow)=='undefined'){{AfterInsertRow=function(){{}};}}
if(typeof(GridComplete)=='undefined'){{GridComplete=function(){{}};}}
if(typeof(OnSelectRow)=='undefined'){{OnSelectRow=function(){{}};}}
if(typeof(OnSelectAll)=='undefined'){{OnSelectAll=function(){{}};}}
var PostData=SearchParamter();
var grid{0}{1} = jQuery('#list{0}{1}').jqGrid({{
    url: '{3}',
    datatype: 'json',    
    colModel: colModel{0},    
   
    {4}
    {5}
    rownumbers: true,
    height:'100%',
autowidth:true,
shrinkToFit :true,
    sortname: SORTNAME,
    viewrecords: true,
    jsonReader: {{ repeatitems: false }},
    sortorder: SORTORDER,
    caption: '',
    mtype: 'POST',
    postData:PostData,
    afterInsertRow:SysAfterInsertRow,
    gridComplete:GridComplete
}}){6};";
            string multiSelect = "";
            string pager = "";
            string pageNav = "";
            if (isMultiSelect) multiSelect = "multiselect:true,";
            if (isPager)
            {
                pager = string.Format("rowNum: PAGEROW,rowList: [15, 25, 50,100,200],pager:jQuery('#pager{0}'),", name);
                pageNav = string.Format(".navGrid('#pager{0}',{{edit:false,add:false,del:false,search:false,refresh:false}})", name);
            }
            else
            {
                //pager = string.Format("rowNum: 50000,pager: jQuery('#pager{0}{1}'),", name, page.ClientID);
                // pageNav = string.Format(".navGrid('#pager{0}{1}',{{edit:false,add:false,del:false,search:false,refresh:false}})", name, page.ClientID);

                pager = string.Format("rowNum: 100,rowList: [100, 300, 500,1000],pager:jQuery('#pager{0}'),", name);
                pageNav = string.Format(".navGrid('#pager{0}',{{edit:false,add:false,del:false,search:false,refresh:false}})", name);
            }
            return string.Format(str, name, "", path, fullurl, multiSelect, pager, pageNav)+ OutToExcel();

        }

        /*
        var colModelGrid = [
            { name: 'ccode', label: '合同号', width: 100},
            { name: 'clientname', label: '客户', width: 100,align:'center'},
            { name: 'rname', label: '房源', width: 100,align:'center'},
            { name: 'csalearea', label: '销售面积', width: 100, formatter:'number', align:'right'},
            { name: 'cunitprice', label: '单价', width: 100, formatter:'number', align:'right'},
            { name: 'cfactprice', label: '总价', width: 100, formatter:'number', align:'right'},
            { name: 'ccontractdate', label: '签约日期', formatter:'date', width: 100 ,align:'center'},
            { name: 'username', label: '业务员', width: 120 ,align:'center'}
        ];   	
    */
        public Query query;
        public string[] FieldList = null;
        public string OutField()
        {
            return OutField(false);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="appendEmptyColumn">是否在最后输出一空列</param>
        /// <returns></returns>
        public string OutField(bool appendEmptyColumn)
        {
            
            if (query == null) return "";
            if (FieldList == null)
            {
                if (context == null) return "//无法初始化FieldList";
                FieldList = query.GetFieldList(context).Split(',');
            }
            StringBuilder sb = new StringBuilder("var colModel");
            sb.Append(name);
            sb.Append("= [\n");
            bool isfirst = true;
            bool isDepend = false;
            List<string> containsField = new List<string>();
            //显示正常字段
            foreach (string s in FieldList)
            {
                Field f = query.GetFieldByFullName(s);
                if (f != null)
                {
                    containsField.Add(s);
                    if (f.DependOnField != null) isDepend = true;
                    if (isfirst)
                    {
                        isfirst = false;
                    }
                    else
                    {
                        sb.Append(",");
                    }
                    toField(sb, f);
                }
            }
            if (isDepend)
            {
                foreach (string s in FieldList)
                {
                    Field f = query.GetFieldByFullName(s);
                    if (f != null)
                    {
                        if (f.DependOnField != null)//计算字段，需要将关联的字段也添加到col中
                        {
                            foreach (string s2 in f.DependOnField)
                            {
                                if (!containsField.Contains(s2))
                                {
                                    containsField.Add(s2);
                                    Field f2 = query.GetFieldByFullName(s2);
                                    if (f2 != null)
                                    {
                                        sb.Append(",");
                                        toField(sb, f2, true);
                                    }
                                }
                            }
                        }
                    }
                }
            }
            if (appendEmptyColumn)
            {
                sb.AppendLine(",{ name: 'emptycolumn', label: '', width: 7}");
            }
            sb.AppendLine("\n];");
            return sb.ToString();

        }
        protected void toField(StringBuilder sb, Field f)
        {
            toField(sb, f, f.IsHidden);
        }
        protected void toField(StringBuilder sb, Field f, bool isHidden)
        {
            string s = " {{ name: '{0}', label: '{1}', formatter:'{2}', {3} align:'{4}'{5}{6}}}";
            if (f.Align == "" || f.Align == null)
            {
                switch (f.FieldType)
                {
                    case "integer":
                    case "number":
                    case "area":
                    case "rate":
                    case "exchangerate":
                    case "currency":
                        f.Align = "right";
                        break;
                }
            }
            string sortable = "";
            if (f.Sortable == false)
            {
                sortable = ",sortable:false";
            }
            sb.AppendLine(string.Format(s, f.Alias == "" ? f.FieldName : f.Alias, f.Name, f.FieldType,f.width>0?string.Format("width:{0},", f.width):"", f.Align, isHidden ? ",hidden:true" : "", sortable));
        }
        public string OutFieldSelect()
        {
            if (query != null)
            {
                string s = @"grid{0}{1}.navButtonAdd('#pager{0}{1}',{{caption:'字段选择',onClickButton:function(){{OpenLargeWindow(rootpath+'usercontrols/selectfieldform.aspx?type={2}','select');}}}});
";
                return string.Format(s, name, "", query.Name);
            }
            return "";
        }
        
        public string OutToExcel()
        {
//            string s = @"grid{0}{1}.navButtonAdd('#pager{0}{1}',{{caption:'导出EXCEL',onClickButton:function(e){{
//    try{{grid{0}{1}('excelExport', {{ url: 'grid.php' }});
//          }} catch (e) {{
//              window.location= 'grid.php?oper=excel';
//          }}
//      }}
//        }});";
//    return string.Format(s, name, "");
            string s = @"grid{0}{1}.navButtonAdd('#pager{0}{1}',{{caption:'导出EXCEL',onClickButton:function(){{
var tbl=document.getElementById('list{0}{1}');
if(tbl.rows.length<=1)
{{
    alert('没有数据');
    return;
}}
var row='<table border=1><tbody>';
row+='<tr><td>序号</td>';
for(var i=0;i<colModel{0}.length;i++)
{{
if(colModel{0}[i]){{
    if(colModel{0}[i].hidden!=true)
    {{
    row+='<td>';
    row+=colModel{0}[i].label;
    row+='</td>';
    }}
}}
}}
row+='</tr>';
for(var i={2};i<tbl.rows.length;i++)
{{
row+='<tr>';
for(var j={3};j<tbl.rows[i].cells.length;j++)
{{
    if(tbl.rows[i].cells[j].style.display!='none')
    {{
    row+='<td>';
    row+=tbl.rows[i].cells[j].innerText+'';
    row+='</td>';
    }}
}}
row+='</tr>';
}}
row+='</tboday></table>';
document.getElementById('hidData').value=row.replace(/<A.*?>(.*?)<\/A>/ig,'$1');
window.open(rootpath+'usercontrols/toExcel.aspx?cid=hidData&name=');
}}}});
";
            int cellId = 0;
            if (isMultiSelect)
            {
                cellId = 1;
            }
            return string.Format(s, name, "", 1, cellId);
 
        }
        public bool IsField(string fullname)
        {
            foreach (string s in FieldList)
            {
                if (s == fullname) return true;
            }
            return false;
        }
        
        public string OutGrid()
        {
            StringBuilder sb = new StringBuilder();
            
            sb.AppendLine(OutField());
            sb.AppendLine(OutScript());
            sb.AppendLine(OutFieldSelect());
            return sb.ToString();
        }
    }

}



