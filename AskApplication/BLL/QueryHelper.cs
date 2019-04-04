using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using System.Text;
using System.Data.SqlClient;
namespace BaseErp.Web
{
    public class Query
    {
        public string Name;
        public Table MainTable;
        public List<Table> TableList = new List<Table>();
        public List<Field> FieldList = new List<Field>();
        public List<Filter> FilterList = new List<Filter>();
        public string DefaultValueStr;
        /// <summary>
        /// 按 fullname=tableAlias.fieldName来判断
        /// </summary>
        /// <param name="fullName"></param>
        /// <returns></returns>
        public Field GetFieldByFullName(string fullName)
        {
            for (int i = 0; i < FieldList.Count; i++)
            {
                if (fullName == FieldList[i].FullName)
                {
                    return FieldList[i];
                }
            }
            return null;
        }
        public Field GetFieldByName(string name)
        {
            for (int i = 0; i < FieldList.Count; i++)
            {
                if (name == FieldList[i].Name)
                {
                    return FieldList[i];
                }
            }
            return null;
        }
        public Table GetTableByName(string tablename)
        {
            for (int i = 0; i < TableList.Count; i++)
            {
                if (tablename == TableList[i].TableName)
                {
                    return TableList[i];
                }
            }
            return null;
        }
        /// <summary>
        /// 按别名返回，如果table对象没指定别名，按表名返回
        /// </summary>
        /// <param name="alias"></param>
        /// <returns></returns>
        public Table GetTableByAlias(string alias)
        {
            for (int i = 0; i < TableList.Count; i++)
            {
                if (TableList[i].Alias == "" && alias == TableList[i].TableName)
                {
                    return TableList[i];
                }
                if (alias == TableList[i].Alias)
                {
                    return TableList[i];
                }
            }
            return null;
        }
        public Filter GetFilterByName(string filtername)
        {
            foreach (Filter f in FilterList)
            {
                if (filtername == f.Name) return f;
            }
            return null;
        }
        /// <summary>
        /// 添加表，排除已经添加的和主表,同时添加关联表
        /// </summary>
        /// <param name="list"></param>
        protected void AddTable(string tableAlias, List<Table> list)
        {
            if (tableAlias != MainTable.Alias && tableAlias != MainTable.TableName)
            {
                foreach (Table t in list)
                {
                    if (t.Alias == tableAlias) return;//已经存在
                }
                Table table = GetTableByAlias(tableAlias);
                if (table != null)
                {
                    //先加关联表
                    if (table.DependOnTable != null)
                    {
                        foreach (string dependT in table.DependOnTable)
                        {
                            AddTable(dependT, list);
                        }
                    }
                    list.Add(table);
                }
            }
        }
        protected void AddTable2Sql(StringBuilder sb, Table t, bool isFirst)
        {
            if (!isFirst)
            {
                if (t.Join == "")
                {
                    sb.Append(" join ");
                }
                else
                {
                    sb.Append(" ");
                    sb.Append(t.Join);
                    sb.Append(" ");
                }
            }
            if (t.Sql.Length > 0)
            {
                sb.Append("(");
                sb.Append(t.Sql);
                sb.Append(")");
            }
            else
            {
                sb.Append(t.TableName);
            }
            sb.Append(" ");
            if (t.Alias.Length > 0)
            {
                sb.Append("as ");
                sb.Append(t.Alias);
            }
            if (t.On.Length > 0)
            {
                sb.Append(" ");
                sb.Append(t.On);
            }
            sb.AppendLine();
        }
        //不考虑dependonfield
        protected void AddField2Sql(StringBuilder sb, Field f)
        {
            if (sb.Length == 0)
            {
                sb.Append("select ");
            }
            else
            {
                sb.Append(",");
            }
            if (f.FunctionStr == "")
            {
                sb.Append(f.FullName);
            }
            else
            {
                sb.Append(f.FunctionStr);
            }
            if (f.Alias.Length > 0)
            {
                sb.Append(" as ");
                sb.Append(f.Alias);
            }
        }
        protected void AddSumField2Sql(StringBuilder sb, Field f)
        {
            if (sb.Length == 0)
            {
                sb.Append("select ");
            }
            else
            {
                sb.Append(",");
            }
            sb.Append("sum(");
            sb.Append(f.FullName);
            sb.Append(") as ");
            if (f.Alias.Length > 0)
            {
                sb.Append(f.Alias);
            }
            else
            {
                sb.Append(f.Name);
            }
        }
        protected void AddGroup2Sql(StringBuilder sb, Field f)
        {
            if (sb.Length == 0)
            {
                sb.Append(" group by ");
            }
            else
            {
                sb.Append(",");
            }
            sb.Append(f.FullName);
        }
        /// <summary>
        /// showField 使用 tableAlias.fieldName 的形式
        /// </summary>
        /// <param name="showField"></param>
        /// <returns></returns>
        public string BuildSql(string[] showField, FilterBuilder fb, string order)
        {
            //确定需要添加的表,从显示栏目和查询条件中遍历
            List<Table> tablelist = new List<Table>();
            foreach (Table t in TableList)
            {
                if (t.IsDefault)
                {
                    AddTable(t.Alias, tablelist);//这里不直接 list.add(t),是需要同时添加dependOn表
                }
            }
            foreach (string s in showField)
            {
                AddField2Table(s, tablelist);
            }
            foreach (Filter filter in fb.FilterList)
            {
                AddField2Table(filter.FieldFullName, tablelist);
            }
            StringBuilder fie = new StringBuilder();

            foreach (string s in showField)
            {
                Field f = GetFieldByFullName(s);
                if (f != null)
                {
                    if (f.DependOnField != null)
                    {
                        foreach (string df in f.DependOnField)
                        {
                            Field f2 = GetFieldByFullName(df);
                            if (f2 != null)
                            {
                                AddField2Sql(fie, f2);
                            }
                        }
                    }
                    else
                    {
                        AddField2Sql(fie, f);
                    }
                }
            }

            StringBuilder tab = new StringBuilder(" from ");
            AddTable2Sql(tab, MainTable, true);
            foreach (Table table in tablelist)
            {
                AddTable2Sql(tab, table, false);
            }
            StringBuilder fullsb = new StringBuilder();
            fullsb.Append(fie.ToString());
            fullsb.Append(tab.ToString());
            fullsb.Append(fb.BuildWhere());
            if (order.Length > 0)
            {
                fullsb.Append(" order by ");
                fullsb.Append(order);
            }
            return fullsb.ToString();
        }
        public void AddField2Table(string fullFieldName, List<Table> tablelist)
        {
            Field f = GetFieldByFullName(fullFieldName);
            if (f != null)
            {
                if (f.DependOnField == null)// 非计算字段
                {
                    AddTable(f.TableAlias, tablelist);
                }
                else
                {
                    foreach (string df in f.DependOnField)
                    {
                        Field f2 = GetFieldByFullName(df);
                        if (f2 != null)
                        {
                            AddTable(f2.TableAlias, tablelist);
                        }
                    }
                }
            }
        }
        public string BuildGroupSql(string[] groupField, string[] sumField, FilterBuilder fb, string order)
        {
            //确定需要添加的表,从显示栏目和查询条件中遍历
            List<Table> tablelist = new List<Table>();
            foreach (string s in groupField)
            {
                AddField2Table(s, tablelist);
            }
            foreach (string s in sumField)
            {
                AddField2Table(s, tablelist);
            }
            foreach (Filter filter in fb.FilterList)
            {
                AddTable(filter.FieldFullName, tablelist);
            }
            //准备select 字段
            StringBuilder fie = new StringBuilder();
            foreach (string s in groupField)
            {
                Field f = GetFieldByFullName(s);
                if (f != null)
                {
                    AddField2Sql(fie, f);//group字段不考虑depend on                     
                }
            }
            foreach (string s in sumField)
            {
                Field f = GetFieldByFullName(s);
                if (f != null)
                {
                    if (f.DependOnField != null)
                    {
                        foreach (string df in f.DependOnField)
                        {
                            Field f2 = GetFieldByFullName(df);
                            if (f2 != null)
                            {
                                AddSumField2Sql(fie, f2);
                            }
                        }
                    }
                    else
                    {
                        AddField2Sql(fie, f);
                    }
                }
            }
            // 准备 groupby 
            StringBuilder groupStr = new StringBuilder();
            foreach (string s in groupField)
            {
                Field f = GetFieldByFullName(s);
                if (f != null)
                {
                    AddGroup2Sql(groupStr, f);
                }
            }

            StringBuilder tab = new StringBuilder(" from ");
            AddTable2Sql(tab, MainTable, true);
            foreach (Table table in tablelist)
            {
                AddTable2Sql(tab, table, false);
            }
            StringBuilder fullsb = new StringBuilder();
            fullsb.Append(fie.ToString());
            fullsb.Append(tab.ToString());
            fullsb.Append(fb.BuildWhere());
            fullsb.Append(groupStr.ToString());
            if (order.Length > 0)
            {
                fullsb.Append(" order by ");
                fullsb.Append(order);
            }
            return fullsb.ToString();
        }

        public string FormatOrder(string orderAlias, string sord)
        {
            foreach (Field f in FieldList)//先判断alias 中是否有匹配
            {
                if (f.Alias != null && f.Alias != "")
                {
                    if (orderAlias == f.Alias) return f.FullName + " " + sord;
                }
            }
            foreach (Field f in FieldList)//再判断fieldname是否有匹配
            {
                if (orderAlias == f.FieldName)
                {
                    if (f.DependOnField != null)
                    {
                        string strorder = "";
                        //组合字段
                        foreach (string df in f.DependOnField)
                        {
                            Field f2 = GetFieldByFullName(df);
                            if (f2 != null)
                            {
                                if (strorder == "")
                                {
                                    strorder = f2.FullName + " " + sord;
                                }
                                else
                                {
                                    strorder = f2.FullName + " " + sord + "," + strorder;
                                }
                            }
                        }
                        return strorder;

                    }
                    else
                    {
                        return f.FullName + " " + sord;
                    }
                }
            }
            return "";
        }

        public string FormatOrder(string orderAlias, string sord, string customOrderBy)
        {
            if (String.IsNullOrEmpty(orderAlias))
                return customOrderBy;
            else
                return FormatOrder(orderAlias, sord);
        }

        protected string CookieName
        {
            get
            {
                return "QueryField" + HttpUtility.UrlEncode(Name);
            }
        }

        protected string CookieNameOrderBy
        {
            get
            {
                return "QueryFieldOrderBy" + HttpUtility.UrlEncode(Name);
            }
        }

        public string GetFieldList(HttpContextBase context)
        {
            if (context.Request.Cookies[CookieName] != null)
            {
                return (context.Request.Cookies[CookieName].Value);
            }
            if (defaultfield == null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Field f in FieldList)
                {
                    if (f.IsDefault || f.IsHidden || f.IsForce)
                    {
                        if (sb.Length > 0) sb.Append(",");
                        sb.Append(f.FullName);
                    }
                }
                defaultfield = sb.ToString();
            }
            //HttpCookie cookie = new HttpCookie(CookieName, defaultfield);
            //cookie.Expires = new DateTime(2100, 1, 1);
            //context.Response.Cookies.Add(cookie);

            return defaultfield;
        }
        /// <summary>
        /// 获取中文名称 add by jinjie
        /// </summary>
        /// <returns></returns>
        public string GetFieldListName()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Field f in FieldList)
            {
                if (!f.IsHidden)
                {
                    if (sb.Length > 0) sb.Append(",");
                    sb.Append(f.Name);
                }
            }
            defaultfield = sb.ToString();
            return defaultfield;
        }

        /// <summary>
        /// f.FieldName+"_"+f.TableAlias 的组合
        /// </summary>
        /// <returns></returns>
        public string GetFildListJsonName()
        {
            StringBuilder sb = new StringBuilder();
            foreach (Field f in FieldList)
            {
                if (!f.IsHidden)
                {
                    if (sb.Length > 0) sb.Append(",");
                    sb.Append(f.Name + "$" + f.FieldName + "_" + f.TableAlias);
                }
            }
            defaultfield = sb.ToString();
            return defaultfield;
        }

        public void SetFieldList(HttpContext context, string list)
        {
            HttpCookie cookie = new HttpCookie(CookieName, list);
            cookie.Expires = new DateTime(2100, 1, 1);
            context.Response.Cookies.Add(cookie);
        }

        public void SetOrderBy(HttpContext context, string orderBy)
        {
            HttpCookie cookie = new HttpCookie(CookieNameOrderBy, orderBy);
            cookie.Expires = new DateTime(2100, 1, 1);
            context.Response.Cookies.Add(cookie);
        }

        public string GetOrderBy(HttpContext context)
        {
            if (context.Request.Cookies[CookieNameOrderBy] != null)
            {
                return (context.Request.Cookies[CookieNameOrderBy].Value);
            }

            return null;
        }
        public string GetFieldList(HttpContext context)
        {
            if (context.Request.Cookies[CookieName] != null)
            {
                return (context.Request.Cookies[CookieName].Value);
            }
            if (defaultfield == null)
            {
                StringBuilder sb = new StringBuilder();
                foreach (Field f in FieldList)
                {
                    if (f.IsDefault || f.IsHidden || f.IsForce)
                    {
                        if (sb.Length > 0) sb.Append(",");
                        sb.Append(f.FullName);
                    }
                }
                defaultfield = sb.ToString();
            }
            //HttpCookie cookie = new HttpCookie(CookieName, defaultfield);
            //cookie.Expires = new DateTime(2100, 1, 1);
            //context.Response.Cookies.Add(cookie);

            return defaultfield;
        }
        public void GetFilterListFromRequest(List<Filter> list, HttpContext context)
        {

        }
        [NonSerialized]
        private string defaultfield = null;

    }
    public class Table
    {
        public string Name;
        public string TableName;
        public string Alias = "";
        public string Sql = "";
        public string On = "";
        /// <summary>
        /// join 为 “” 则默认 join ,否则使用join的实际值
        /// </summary>
        public string Join = "";
        public bool IsDefault = false;
        public string[] DependOnTable;
    }
    public class Field
    {
        public string Name;
        public string FieldName;
        public string Alias = "";
        public string TableAlias = "";
        public bool IsDefault = false;
        public bool IsHidden = false;
        public bool IsForce = false;
        public bool Sortable = true;
        public string SumFormula = "";
        public string[] DependOnField;//计算字段，里面放 room.fullname 这样的格式， 计算字段不需要有tablealias
        public int width = 0;
        public string FunctionStr = "";//做 case when 或其他计算使用
        /// <summary>
        /// 空=字符串 其他参照jqgrid 的fromatter :
        /// integer  , number , area , rate , exchangerate ,  currency , date 
        /// </summary>
        public string FieldType = "";
        /// <summary>
        /// 这个可以省略，省略则从fieldtype中自动取
        /// </summary>
        public string Align = "";

        [NonSerialized]
        string fullName = "";
        public string FullName
        {
            get
            {
                if (fullName == "")
                {
                    if (TableAlias == "")
                    {
                        fullName = FieldName;
                    }
                    else
                    {
                        fullName = TableAlias + "." + FieldName;
                    }
                }
                return fullName;
            }
        }


    }
    /// <summary>
    /// 查询条件类
    /// 
    /// </summary>
    public class Filter
    {
        /// <summary>
        /// 中文名，无用，仅用来方便查看
        /// </summary>
        public string Title = "";
        /// <summary>
        /// 条件名，与postdata中提交的查询条件同名，唯一
        /// </summary>
        public string Name;
        /// <summary>
        /// 对应字段名，字段名采用 contract.cid 这种 alias.fieldname 名称
        /// </summary>
        public string FieldFullName;
        /// <summary>
        /// 查询类型， 有 参数化查询= P，字符串= S ，手动添加 =N 3种
        /// </summary>
        public string BuildType = "P";

        /// <summary>
        /// 
        /// </summary>
        public string ReferenceType = "";
        /// <summary>
        /// 
        /// </summary>
        public string ReferenceName = "";

        public bool IsIncludeAnd = false;

        /// <summary>
        /// 查询条件 
        /// 如果 type=P ,使用 client.cname like {0} 这样格式，会用 @projectid 替换{0}里的值，请在处理like的参数时，自己添加%
        /// 如果 type=S ,使用 contract.projectid={0}， client.cname like '{0}%' 这样的格式 ,会用string.format 替换{0}里的值
        /// </summary>
        public string Sql;
        [NonSerialized]
        public string Value;

    }
    public class FilterBuilder
    {
        protected FilterBuilder() { }
        public static FilterBuilder GetBuilder(Query query, SqlParameterCollection parameterlist)
        {
            FilterBuilder fb = new FilterBuilder();
            fb.query = query;
            fb.parameterList = parameterlist;
            return fb;
        }
        private int filterAdded = 0;
        private bool whereStringAdded = false;
        StringBuilder sb = new StringBuilder();
        public Query query;
        public List<Filter> FilterList = new List<Filter>();
        public SqlParameterCollection parameterList;
        /// <summary>
        /// 如果 Value="" 或者 null ，将不实际添加
        /// </summary>
        /// <param name="filtername"></param>
        /// <param name="Value"></param>
        /// <returns></returns>
        public Filter AddFilter(string filtername, string Value)
        {
            if (Value == null || Value == "" || Value == "%%" || Value == "%") return null;
            Filter f = query.GetFilterByName(filtername);
            if (f != null)
            {
                f.Value = Value;
                FilterList.Add(f);
            }
            return f;
        }
        public Filter AddFilterLike(string filtername, string Value)
        {
            if (Value == null || Value == "" || Value == "%%" || Value == "%") return null;
            Filter f = query.GetFilterByName(filtername);
            if (f != null)
            {
                f.Value ="%"+ Value+"%";
                FilterList.Add(f);
            }
            return f;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="oper"></param>
        /// <returns></returns>
        public Filter AddFilter(string filterName, string fieldName, string value, string oper, string andor, string leftParen, string rightParen)
        {
            if (value == null || value == "") return null;

            Filter f = new Filter();
            f.Name = filterName;
            f.FieldFullName = fieldName;

            if (oper.ToLower() == "in")
            {
                f.BuildType = "S";
                f.Sql = string.Format("{0} {1} {2}", fieldName, oper, value);
            }
            else
            {
                f.BuildType = "P";
                f.Sql = string.Format("{0} {1} @{2}", fieldName, oper, f.Name);
            }

            if (andor != "")
            {
                f.Sql = andor + " " + f.Sql;
                f.IsIncludeAnd = true;
            }

            f.Sql = leftParen + f.Sql + rightParen;

            f.Value = value;
            FilterList.Add(f);

            return f;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="conditions"></param>
        public void AddCustomFilter(string conditions)
        {
            string[] conditionList = conditions.Split("§"[0]);
            int i = 0;
            foreach (string condition in conditionList)
            {
                string[] arr = condition.Split("※"[0]);
                if (arr.Length < 3) continue;

                string andor = "";
                if (i > 0)
                {
                    if (arr.Length > 3)
                        andor = arr[3];
                }

                //左括号
                string leftParen = "";
                if (arr.Length > 4)
                    leftParen = arr[4];

                //右括号
                string rightParen = "";
                if (arr.Length > 5)
                    rightParen = arr[5];

                //整个查询条件前后加上括号
                if (i == 0)
                    leftParen += "(";
                if (i == conditionList.Length - 1)
                    rightParen += ")";

                Filter f = AddFilter("CustomField" + i.ToString(), arr[0], arr[2], arr[1], andor, leftParen, rightParen);

                i++;
            }
        }

        /// <summary>
        /// 生成where 语句,并将 sqlParamater add 到 paramaterList中。
        /// </summary>
        /// <returns></returns>
        public string BuildWhere()
        {
            foreach (Filter f in FilterList)
            {
                sb.Append(" ");
                switch (f.BuildType.ToUpper())
                {
                    case "P":
                        filterAdded++;
                        if (filterAdded > 1 && !f.IsIncludeAnd)
                        {
                            sb.Append("and ");
                        }
                        string s = "@" + f.Name;
                        sb.Append(string.Format(f.Sql, s));
                        SqlParameter p = new SqlParameter(s, f.Value);
                        parameterList.Add(p);
                        break;
                    case "S":
                        filterAdded++;
                        if (filterAdded > 1 && !f.IsIncludeAnd)
                        {
                            sb.Append("and ");
                        }
                        sb.Append(string.Format(f.Sql, f.Value));
                        break;
                    case "N":
                    default:
                        break;
                }
            }
            if (whereStringAdded == false && filterAdded > 0)
            {
                sb.Insert(0, " where ");
            }
            return sb.ToString();
        }
        public void AddOtherFilter(string sql, params SqlParameter[] list)
        {

            if (whereStringAdded == false)
            {
                sb.Append(" where ");
                whereStringAdded = true;
                filterAdded++;
            }
            else
            {
                sb.Append(" and ");
            }
            sb.Append(sql);
            if (list != null)
            {
                parameterList.AddRange(list);
            }
        }
    }
}
