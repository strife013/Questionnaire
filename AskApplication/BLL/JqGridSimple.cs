using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace BaseErp.Web
{
    public class JqGridSimple
    {
        //0gridindex,1 dataurl,2 other property like multiselect:true, ,
        static private string script =
            @"var grid{0}=jQuery('#listGrid{0}').jqGrid({{
        url: '{1}',
        datatype: 'json',
        rownumbers: true,
        {2}
        height: '100%',
    width: '96%',
        autowidth: true,
        shrinkToFit: true,
        viewrecords: true,
        jsonReader: {{ repeatitems: false }},
        caption: '',
        mtype: 'POST',
        postData: PostData,
        afterInsertRow: AfterInsertRow,
    onSortCol:SortCol,
    onSelectRow:rowclick,
        colModel: colModelGrid      ,
        pager: jQuery('#pagerGrid{0}'),
        rowNum: 20,
        rowList: [20, 30, 50,100]            
    }});";
        static string tag = @"<table id='listGrid{0}' class='scroll' cellpadding='0' cellspacing='0' ></table>
    <div id='pagerGrid{0}' ></div>";
        public static string OutGrid(string path, int num)
        {
            return string.Format(script, num, path);
        }
        public static string OutGrid(string path, bool isMultiSelect = false)
        {
            string multiSelect = "";
            if (isMultiSelect) multiSelect = "multiselect:true,";
            return string.Format(script, "", path, multiSelect);
        }
        public static string OutTable(int num)
        {
            return string.Format(tag, num);
        }
        public static string OutTable()
        {
            return string.Format(tag, "");
        }


    }
    public static class JQExtensions
    {
        public static MvcHtmlString EnumNameFunction(this HtmlHelper helper, string name, Type enumType)
        {
            string temp = "function {0}Name(val){{switch(val){{{1}}}}}";
            StringBuilder sb = new StringBuilder("");

            foreach (var v in Enum.GetValues(enumType))
            {

                sb.Append(string.Format("case {0}:return '{1}';", (int)v, v));
            }
            return MvcHtmlString.Create(string.Format(temp, name, sb.ToString()));

        }
        public static MvcHtmlString JqFieldString(this HtmlHelper helper, string name, string title, int width, string index = "")
        {
            return MvcHtmlString.Create(string.Format(",{{ name: '{0}',label:'{1}',width:{2} {3}, align: 'left' }}\n", name, title, width, index == "" ? ",sortable:false" : ",index:'" + index + "'"));
        }
        public static MvcHtmlString JqFieldInt(this HtmlHelper helper, string name, string title, int width, string index = "")
        {
            return MvcHtmlString.Create(string.Format(",{{ name: '{0}',label:'{1}',width:{2} {3}, align: 'right',formatter:'integer' }}\n", name, title, width, index == "" ? "" : ",index:'" + index + "'"));
        }
        public static MvcHtmlString JqFieldNumber(this HtmlHelper helper, string name, string title, int width, string index = "", int precision = 2)
        {
            return MvcHtmlString.Create(string.Format(",{{ name: '{0}',label:'{1}',width:{2} {3}, align: 'right',formatter:'number' {4}}}\n", name, title, width
                , index == "" ? "" : ",index:'" + index + "'"
                , precision == 2 ? "" : string.Format(",formatoptions:{{decimalPlaces: {0}}}", precision)));
        }
        public static MvcHtmlString JqFieldCurrency(this HtmlHelper helper, string name, string title, int width, string index = "", int precision = 2)
        {
            return MvcHtmlString.Create(string.Format(",{{ name: '{0}',label:'{1}',width:{2} {3}, align: 'right',formatter:'currency' }}\n", name, title, width
                , index == "" ? "" : ",index:'" + index + "'"
                , precision == 2 ? "" : string.Format(",formatoptions:{{decimalPlaces: {0}}}", precision)));
        }
        public static MvcHtmlString JqFieldDate(this HtmlHelper helper, string name, string title, int width = 85, string index = "")
        {
            return MvcHtmlString.Create(string.Format(",{{ name: '{0}',label:'{1}',width:{2} {3}, align: 'right',formatter:'date' }}\n", name, title, width, index == "" ? "" : ",index:'" + index + "'"));
        }
    }

    public class JqField
    {
        public string Name { get; set; }

        public string Label { get; set; }
        public int Width { get; set; }
        public string Index { get; set; }
        public string Type { get; set; }
        public int Precision { get; set; }
        public bool Hidden { get; set; }

        public void fund()
        {
            var list = new List<JqField> { };
        }
    }
}


