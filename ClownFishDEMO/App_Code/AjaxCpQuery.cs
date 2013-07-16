using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data.SqlClient;
using System.Data.Common;
using System.Data;
using MyMVC;
using FishDemoCodeLib;
using ClownFish;
using Test.Models;



public class AjaxCpQuery
{
	[Action]
	public object DEMO(Product p, string database)
	{
		// 创建动态查询
		var query = BuildDynamicQuery(p, database);
		//var query = BuildDynamicQuery2(p, database);

		CPQueryDEMOResult result = new CPQueryDEMOResult();

		// 一般情况下，使用下面这行代码就可以了。
		//DataTable table = query.FillDataTable();


		// 示例中为了要输出结果与命令细节，所以多写了二行代码。
		using( DbContext db = new DbContext(database) ) {
			result.QueryResult = query.FillDataTable(db);
			result.Command = db.CurrentCommand;
		}

		// 输出HTML结果
		return new UcResult("/UserControls/CPQueryDEMO.ascx", result);
	}

	private CPQuery GetEmptyQuery(string database)
	{
		CPQuery query = CPQuery.New();

		if( database == "access" )	// OLEDB
			query.SetEvalNameDelegate(p => "?", n => n);

		return query;
	}

	private CPQuery BuildDynamicQuery(Product p, string database)
	{
		string select = "select ProductID,ProductName,Unit,UnitPrice from Products where (1=1) ";

		// 绝大多数情况下，下面二行代码是有效的，而且是等价的，可根据喜好选择。
		//var query = select.AsCPQuery();
		//var query = CPQuery.New() + select;


		// 如果需要支持多数据库（主要是OLEDB与ODBC），那么就要使用下面的方法。
		// 事实上，继续使用AsCPQuery()也是可以的，它也有重载版本。
		var query = GetEmptyQuery(database) + select;

		// 注意：下面的拼接代码中不能写成: query += .....

		if( p.ProductID > 0 )
			query = query + " and ProductID = " + p.ProductID;	// 给查询添加一个整数参数。

		if( string.IsNullOrEmpty(p.ProductName) == false )
			// 给查询添加一个字符串参数。
			query = query + " and ProductName like " + p.ProductName.AsQueryParameter();

		if( p.CategoryID > 0 )
			query = query + " and CategoryID = " + p.CategoryID;

		if( string.IsNullOrEmpty(p.Unit) == false )
			query = query + " and Unit = " + (QueryParameter)p.Unit;	// 字符串参数的另一种处理方式

		if( p.UnitPrice > 0 )
			query = query + " and UnitPrice >= " + p.UnitPrice;	// 给查询添加一个decimal参数。

		if( p.Quantity > 0 )
			// 如果您认为给所有的参数添加强类型转换看起来可读性更好，那么也行（尽管是多余的）。
			query = query + " and Quantity >= " + (QueryParameter)p.Quantity;

		return query;
	}


	private CPQuery BuildDynamicQuery2(Product p, string database)
	{
		// 处理常见的字符串拼接方式

		var query = CPQuery.New(true);

		if( database == "access" )
			query.SetEvalNameDelegate(a => "?", n => n);

		query = query + "SELECT ProductID, ProductName, Unit, UnitPrice FROM Products WHERE (1=1) ";

		if( p.ProductID > 0 )
			query = query + " and ProductID = " + p.ProductID.ToString();

		if( string.IsNullOrEmpty(p.ProductName) == false )
			query = query + " and ProductName like '" + p.ProductName + "'";

		if( p.CategoryID > 0 )
			query = query + " and CategoryID = " + p.CategoryID.ToString();

		if( string.IsNullOrEmpty(p.Unit) == false )
			query = query + " and Unit = '" + p.Unit + "'";

		if( p.UnitPrice > 0 )
			query = query + " and UnitPrice >= " + p.UnitPrice.ToString();

		if( p.Quantity > 0 )
			query = query + " and Quantity >= " + p.Quantity.ToString();

		return query;
	}

}



/// <summary>
/// DEMO的返回结果。（不想创建新文件，坏习惯别模仿）
/// </summary>
public class CPQueryDEMOResult
{
	public DbCommand Command;
	public DataTable QueryResult;
}