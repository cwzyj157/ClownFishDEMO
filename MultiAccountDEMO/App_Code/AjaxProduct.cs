using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using ClownFish;
using MyMVC;
using MyMvcEx;


// 示例使用了MyMVC框架：http://www.cnblogs.com/fish-li/archive/2012/02/12/2348395.html

// MyMvcEx的下载地址：http://www.cnblogs.com/fish-li/archive/2012/03/04/2379612.html


public static class AjaxProduct
{
	[Action]
	public static object GetProducts(int? topN)
	{
		int count = (topN.HasValue && topN.Value > 0) ? topN.Value : 50;
		string query = string.Format("select top {0} ProductName, Quantity, Unit, UnitPrice from Products", count);

		DataTable table = DbHelper.FillDataTable(query, null, CommandKind.SqlTextNoParams);
		return new DataTableResult(table);
	}

}
