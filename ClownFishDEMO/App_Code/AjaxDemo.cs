using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Text;
using System.Data;
using System.IO;
using MyMVC;
using ClownFish;
using FishDemoCodeLib;
using Test.Models;


// 示例使用了MyMVC框架：http://www.cnblogs.com/fish-li/archive/2012/02/12/2348395.html


public class AjaxDemo
{
	
	[Action]
	public string SimpleCRUD()
	{
		// 注意： 在这个方法中，每次数据库的访问将【不会】共用一个连接
		// 共用连接的示例可参考 SimpleCRUD_MySql(), SimpleCRUD_Access()

		StringBuilder sb = new StringBuilder();

		Product product = CreateTestProduct();

		// 插入一条新记录
		DbHelper.ExecuteNonQuery("InsertProduct", product);
		sb.AppendFormat("InsertProduct OK, ProductId is : {0}\r\n", product.ProductID);

		// 查询新插入的记录
		Product p2 = DbHelper.GetDataItem<Product>("GetProductById", new { ProductID = product.ProductID });

		// 判断是否成功取到新插入的记录
		if( p2 != null && product.ToString() == p2.ToString() )
			sb.AppendFormat("GetProductById({0}) Is OK\r\n", product.ProductID);
		else
			throw new Exception(string.Format("Call GetProductById({0}) failed", product.ProductID));

		// 更新记录
		p2.ProductName = "New ProductName.";
		DbHelper.ExecuteNonQuery("UpdateProduct", p2);

		// 获取更新后的记录
		Product p3 = DbHelper.GetDataItem<Product>("GetProductById", new { ProductID = p2.ProductID });

		// 验证修改后的记录
		if( p3 != null && p3.ProductName == "New ProductName." )
			sb.AppendFormat("UpdateProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
		else
			throw new Exception(string.Format("Call UpdateProduct(ProductId = {0}) failed", p2.ProductID));

		// 删除记录
		DbHelper.ExecuteNonQuery("DeleteProduct", new { ProductID = product.ProductID });

		Product p4 = DbHelper.GetDataItem<Product>("GetProductById", new { ProductID = product.ProductID });
		if( p4 == null )
			sb.AppendFormat("DeleteProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
		else
			throw new Exception(string.Format("Call DeleteProduct(ProductId = {0}) failed", product.ProductID));


		// 查询一个分页列表
		var parameters = new GetProductByCategoryIdParameters {
			CategoryID = 1,
			PageIndex = 0,
			PageSize = 3,
			TotalRecords = 0
		};
		List<Product> list = DbHelper.FillList<Product>("GetProductByCategoryId", parameters);

		sb.AppendLine();
		sb.AppendFormat("存在 {0} 条符合条件的记录。条件：CategoryID = {1}\r\n", parameters.TotalRecords, parameters.CategoryID);
		sb.AppendLine(ClownFish.XmlHelper.XmlSerializerObject(list));

		return sb.ToString().StringToHtml();
	}


	public class GetProductByCategoryIdParameters : PagingInfo
	{
		public int CategoryID { get; set; }
	}




	public class SearchProductParameters : PagingInfo
	{
		public int CategoryID { get; set; }
		public string SearchWord { get; set; }
	}


	[Action]
	public string GetPagedList()
	{
		var parameters = new SearchProductParameters {
			CategoryID = 1,
			SearchWord = string.Empty,
			PageIndex = 1000,	// 指定一个错误的页号。
			PageSize = 5,
			TotalRecords = 0
		};
		List<Product> list = DbHelper.FillListPaged<Product>("SearchProduct", parameters);

		// 实际有效的页号
		int pageIndex = parameters.PageIndex;

		return ClownFish.XmlHelper.XmlSerializerObject(list).StringToHtml();
	}



	// 这里演示如何实现自己的包装方式。
	// 如果你觉得DbHelper提供的静态方法使用不够方便时，可以考虑自己来实现简化的包装方法。
	private static List<T> FillList<T>(string sqlText, object parameters) where T : class, new()
	{
		using( DbContext dbContext = DbHelper.CreateDefaultDbContext(null) ) {
			if( parameters == null )
				return DbHelper.FillList<T>(sqlText, parameters, dbContext, CommandKind.SqlTextNoParams);
			else
				return DbHelper.FillList<T>(sqlText, parameters, dbContext, CommandKind.SqlTextWithParams);
		}
	}


	[Action]
	public string LoadNestModel()
	{
		var parameters = new { TopN = 2 };
		List<NestDemoItem> list1 = DbHelper.FillList<NestDemoItem>("GetMostValuableCustomers", parameters);

		List<NestDemoItem2> list2 = FillList<NestDemoItem2>(NestDemoItem2.LoadSqlText, parameters);

		// 如果你喜欢在代码中拼接SQL语句，那么下面的调用会适合你。
		string query = NestDemoItem2.LoadSqlText.Replace("@TopN", "2");
		List<NestDemoItem2> list3 = FillList<NestDemoItem2>(query, null);


		string xml1 = ClownFish.XmlHelper.XmlSerializerObject(list1);
		string xml2 = ClownFish.XmlHelper.XmlSerializerObject(list2);
		string xml3 = ClownFish.XmlHelper.XmlSerializerObject(list3);

		return string.Concat(xml1, "\r\n\r\n\r\n\r\n", xml2, "\r\n\r\n\r\n\r\n", xml3).StringToHtml();
	}


	[Action]
	public string ShareConnection()
	{
		// 方法 1
		Customer customer = CreateTestCustomer();
		Product product = CreateTestProduct();
		string msg1 = MyServiceLayer.ShareConnectionDEMO_1(customer, product);

		// 方法 2
		Customer customer2 = CreateTestCustomer();
		Product product2 = CreateTestProduct();
		string msg2 = MyServiceLayer.ShareConnectionDEMO_2(customer2, product2);

		return "ShareConnection Is OK" + "<br />" + msg1 + "<br />" + msg2;
	}



	private Customer CreateTestCustomer()
	{
		return new Customer {
			CustomerName = Guid.NewGuid().ToString(),
			Address = "Test_Address",
			ContactName = "Test_ContactName",
			PostalCode = "123456",
			Tel = "87654321"
		};
	}
	private Product CreateTestProduct()
	{
		return new Product {
			CategoryID = 1,
			ProductName = Guid.NewGuid().ToString(),
			Quantity = 1,
			Remark = "Test_Remark",
			Unit = "个",
			UnitPrice = 10
		};
	}



	[Action]
	public string QueryToDataTable()
	{
		string query = "select * from Products";

		DataTable table = DbHelper.FillDataTable(query, null, CommandKind.SqlTextNoParams);
		table.TableName = "Products";

		using( MemoryStream ms = new MemoryStream() ) {
			table.WriteXml(ms, XmlWriteMode.WriteSchema);
			ms.Position = 0;

			using( StreamReader sr = new StreamReader(ms) ) {
				return sr.ReadToEnd().StringToHtml();
			}
		}
	}


	[Action]
	public string QueryToDataSet()
	{
		string query = @"
select * from Categories;
select * from Customers;
select * from Products;
select * from Orders;
select * from [Order Details];
";
		DataSet ds = null;

		using( DbContext dbContext = DbHelper.CreateDefaultDbContext(null) ) {
			dbContext.CreateCommand(query, CommandType.Text);
			ds = dbContext.FillDataSet("Categories", "Customers", "Products", "Orders", "OrderDetails");
			ds.DataSetName = "MyNorthwind";
		}


		string filePath = Path.Combine(MyMVC.HttpContextHelper.AppRootPath, "App_Data\\MyNorthwind.ds.xml");
		ds.WriteXml(filePath, XmlWriteMode.WriteSchema);

		return File.ReadAllText(filePath).StringToHtml();
	}


	[Action]
	public string LoadListFromDataTable()
	{
		string filePath = Path.Combine(MyMVC.HttpContextHelper.AppRootPath, "App_Data\\MyNorthwind.ds.xml");
		if( File.Exists(filePath) == false )
			return "请先执行【查询结果到DataSet】";

		DataSet ds = new DataSet();
		ds.ReadXml(filePath, XmlReadMode.ReadSchema);

		List<Category> list = DbHelper.FillListFromTable<Category>(ds.Tables["Categories"]);

		string xml = ClownFish.XmlHelper.XmlSerializerObject(list);
		return xml.StringToHtml();
	}


	[Action]
	public string LoadFromXmlFile()
	{
		string filePath = Path.Combine(MyMVC.HttpContextHelper.AppRootPath, "App_Data\\Categories.xml");
		if( File.Exists(filePath) == false ) {
			string query = "select * from Categories";
			DataTable table = DbHelper.FillDataTable(query, null, CommandKind.SqlTextNoParams);
			table.TableName = "Categories";
			table.WriteXml(filePath, XmlWriteMode.WriteSchema);
		}


		List<Category> list = DbHelper.FillListFromXmlFile<Category>(filePath);
		string xml = ClownFish.XmlHelper.XmlSerializerObject(list);
		return xml.StringToHtml();
	}



	[Action]
	public string GetXmlCommand(string name)
	{
		if( string.IsNullOrEmpty(name) )
			return "name is empty.";

		var command = XmlCommandManager.GetCommand(name);
		if( command == null )
			return "command not exist.";

		return ClownFish.XmlHelper.XmlSerializerObject(command).StringToHtml();
	}


	[Action]
	public string SimpleCRUD_Access()
	{
		StringBuilder sb = new StringBuilder();

		using( DbContext access = new DbContext("access") ) {
			Product product = CreateTestProduct();

			// 插入一条新记录
			DbHelper.ExecuteNonQuery("InsertProduct_Access", product, access, CommandKind.XmlCommand);

			string query = "select max(ProductID) from Products";
			product.ProductID = DbHelper.ExecuteScalar<int>(query, null, access, CommandKind.SqlTextNoParams);
			sb.AppendFormat("InsertProduct OK, ProductId is : {0}\r\n", product.ProductID);

			// 查询新插入的记录
			Product p2 = DbHelper.GetDataItem<Product>("GetProductById_Access", new { ProductID = product.ProductID }, access);

			// 判断是否成功取到新插入的记录
			if( p2 != null && product.ToString() == p2.ToString() )
				sb.AppendFormat("GetProductById({0}) Is OK\r\n", product.ProductID);
			else
				throw new Exception(string.Format("Call GetProductById({0}) failed", product.ProductID));

			// 更新记录
			p2.ProductName = "New ProductName.";
			DbHelper.ExecuteNonQuery("UpdateProduct_Access", p2, access);

			// 获取更新后的记录
			Product p3 = DbHelper.GetDataItem<Product>("GetProductById_Access", new { ProductID = p2.ProductID }, access);

			// 验证修改后的记录
			if( p3 != null && p3.ProductName == "New ProductName." )
				sb.AppendFormat("UpdateProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
			else
				throw new Exception(string.Format("Call UpdateProduct(ProductId = {0}) failed", p2.ProductID));

			// 删除记录
			DbHelper.ExecuteNonQuery("DeleteProduct_Access", new { ProductID = product.ProductID }, access);

			Product p4 = DbHelper.GetDataItem<Product>("GetProductById_Access", new { ProductID = product.ProductID }, access);
			if( p4 == null )
				sb.AppendFormat("DeleteProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
			else
				throw new Exception(string.Format("Call DeleteProduct(ProductId = {0}) failed", product.ProductID));


			List<Product> list = DbHelper.FillList<Product>("GetProductByCategoryId_Access", new { CategoryID = 1 }, access);
			int count = DbHelper.ExecuteScalar<int>("GetCountByCategoryId_Access", new { CategoryID = 1 }, access);

			sb.AppendLine();			
			sb.AppendFormat("存在 {0} 条符合条件的记录。条件：CategoryID = {1}\r\n", count, 1);
			sb.AppendLine(ClownFish.XmlHelper.XmlSerializerObject(list));
		}

		return sb.ToString().StringToHtml();
	}



	[Action]
	public string SimpleCRUD_SQLite()
	{
		StringBuilder sb = new StringBuilder();

		Product product = CreateTestProduct();

		using( DbContext mysql = new DbContext("sqlite") ) {

			// 插入一条新记录
			product.ProductID = DbHelper.ExecuteScalar<int>("InsertProduct_SQLite", product, mysql);
			sb.AppendFormat("InsertProduct OK, ProductId is : {0}\r\n", product.ProductID);

			// 查询新插入的记录
			Product p2 = DbHelper.GetDataItem<Product>("GetProductById_SQLite", new { ProductID = product.ProductID }, mysql);

			// 判断是否成功取到新插入的记录
			if( p2 != null && product.ToString() == p2.ToString() )
				sb.AppendFormat("GetProductById({0}) Is OK\r\n", product.ProductID);
			else
				throw new Exception(string.Format("Call GetProductById({0}) failed", product.ProductID));

			// 更新记录
			p2.ProductName = "New ProductName.";
			DbHelper.ExecuteNonQuery("UpdateProduct_SQLite", p2, mysql);

			// 获取更新后的记录
			Product p3 = DbHelper.GetDataItem<Product>("GetProductById_SQLite", new { ProductID = p2.ProductID }, mysql);

			// 验证修改后的记录
			if( p3 != null && p3.ProductName == "New ProductName." )
				sb.AppendFormat("UpdateProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
			else
				throw new Exception(string.Format("Call UpdateProduct(ProductId = {0}) failed", p2.ProductID));

			// 删除记录
			DbHelper.ExecuteNonQuery("DeleteProduct_SQLite", new { ProductID = product.ProductID }, mysql);

			Product p4 = DbHelper.GetDataItem<Product>("GetProductById_SQLite", new { ProductID = product.ProductID }, mysql);
			if( p4 == null )
				sb.AppendFormat("DeleteProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
			else
				throw new Exception(string.Format("Call DeleteProduct(ProductId = {0}) failed", product.ProductID));


			List<Product> list = DbHelper.FillList<Product>("GetProductByCategoryId_SQLite", new { CategoryID = 1 }, mysql);
			int count = DbHelper.ExecuteScalar<int>("GetCountByCategoryId_SQLite", new { CategoryID = 1 }, mysql);

			sb.AppendLine();
			sb.AppendFormat("存在 {0} 条符合条件的记录。条件：CategoryID = {1}\r\n", count, 1);
			sb.AppendLine(ClownFish.XmlHelper.XmlSerializerObject(list));
		}

		return sb.ToString().StringToHtml();
	}



	[Action]
	public int MySqlClientIsReady()
	{
		try {
			using( DbContext db = new DbContext("mysql") ) {
				return 1;
			}
		}
		catch { }
		return 0;
	}

	[Action]
	public string SimpleCRUD_MySql()
	{
		// 注意： 在这个方法中，每次数据库的访问将【会】共用一个连接

		StringBuilder sb = new StringBuilder();

		Product product = CreateTestProduct();

		using( DbContext mysql = new DbContext("mysql") ) {
			// 检查MySql数据库中是否包含了数据，如果没有，就用Access数据库中的数据来导入。
			Product test = DbHelper.GetDataItem<Product>("select * from Products limit 0, 1", null, mysql, CommandKind.SqlTextNoParams);
			if( test == null ) {
				using( DbContext access = new DbContext("access") ) {
					List<Product> products = DbHelper.FillList<Product>("select * from Products order by ProductId", null, access, CommandKind.SqlTextNoParams);
					foreach( Product p in products )
						DbHelper.ExecuteNonQuery("InsertProduct_MySql", p, mysql);
				}
			}

			// 插入一条新记录
			DbHelper.ExecuteNonQuery("InsertProduct_MySql", product, mysql);
			sb.AppendFormat("InsertProduct OK, ProductId is : {0}\r\n", product.ProductID);

			// 查询新插入的记录
			Product p2 = DbHelper.GetDataItem<Product>("GetProductById_MySql", new { ProductID = product.ProductID }, mysql);

			// 判断是否成功取到新插入的记录
			if( p2 != null && product.ToString() == p2.ToString() )
				sb.AppendFormat("GetProductById({0}) Is OK\r\n", product.ProductID);
			else
				throw new Exception(string.Format("Call GetProductById({0}) failed", product.ProductID));

			// 更新记录
			p2.ProductName = "New ProductName.";
			DbHelper.ExecuteNonQuery("UpdateProduct_MySql", p2, mysql);

			// 获取更新后的记录
			Product p3 = DbHelper.GetDataItem<Product>("GetProductById_MySql", new { ProductID = p2.ProductID }, mysql);

			// 验证修改后的记录
			if( p3 != null && p3.ProductName == "New ProductName." )
				sb.AppendFormat("UpdateProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
			else
				throw new Exception(string.Format("Call UpdateProduct(ProductId = {0}) failed", p2.ProductID));

			// 删除记录
			DbHelper.ExecuteNonQuery("DeleteProduct_MySql", new { ProductID = product.ProductID }, mysql);

			Product p4 = DbHelper.GetDataItem<Product>("GetProductById_MySql", new { ProductID = product.ProductID }, mysql);
			if( p4 == null )
				sb.AppendFormat("DeleteProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
			else
				throw new Exception(string.Format("Call DeleteProduct(ProductId = {0}) failed", product.ProductID));


			// 查询一个分页列表
			var parameters = new GetProductByCategoryIdParameters {
				CategoryID = 1,
				PageIndex = 0,
				PageSize = 3,
				TotalRecords = 0
			};
			List<Product> list = DbHelper.FillList<Product>("GetProductByCategoryId_MySql", parameters, mysql);

			sb.AppendLine();
			sb.AppendFormat("存在 {0} 条符合条件的记录。条件：CategoryID = {1}\r\n", parameters.TotalRecords, parameters.CategoryID);
			sb.AppendLine(ClownFish.XmlHelper.XmlSerializerObject(list));
		}

		return sb.ToString().StringToHtml();
	}

	[Action]
	public string SimpleCRUD_MySql_XmlCommand()
	{
		StringBuilder sb = new StringBuilder();

		Product product = CreateTestProduct();

		using( DbContext mysql = new DbContext("mysql-xcmd") ) {

			// 插入一条新记录
			product.ProductID = DbHelper.ExecuteScalar<int>("InsertProduct_MySqlCmd", product, mysql);
			sb.AppendFormat("InsertProduct OK, ProductId is : {0}\r\n", product.ProductID);

			// 查询新插入的记录
			Product p2 = DbHelper.GetDataItem<Product>("GetProductById_MySqlCmd", new { ProductID = product.ProductID }, mysql);

			// 判断是否成功取到新插入的记录
			if( p2 != null && product.ToString() == p2.ToString() )
				sb.AppendFormat("GetProductById({0}) Is OK\r\n", product.ProductID);
			else
				throw new Exception(string.Format("Call GetProductById({0}) failed", product.ProductID));

			// 更新记录
			p2.ProductName = "New ProductName.";
			DbHelper.ExecuteNonQuery("UpdateProduct_MySqlCmd", p2, mysql);

			// 获取更新后的记录
			Product p3 = DbHelper.GetDataItem<Product>("GetProductById_MySqlCmd", new { ProductID = p2.ProductID }, mysql);

			// 验证修改后的记录
			if( p3 != null && p3.ProductName == "New ProductName." )
				sb.AppendFormat("UpdateProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
			else
				throw new Exception(string.Format("Call UpdateProduct(ProductId = {0}) failed", p2.ProductID));

			// 删除记录
			DbHelper.ExecuteNonQuery("DeleteProduct_MySqlCmd", new { ProductID = product.ProductID }, mysql);

			Product p4 = DbHelper.GetDataItem<Product>("GetProductById_MySqlCmd", new { ProductID = product.ProductID }, mysql);
			if( p4 == null )
				sb.AppendFormat("DeleteProduct(ProductId = {0}) Is OK\r\n", p2.ProductID);
			else
				throw new Exception(string.Format("Call DeleteProduct(ProductId = {0}) failed", product.ProductID));


			List<Product> list = DbHelper.FillList<Product>("GetProductByCategoryId_MySqlCmd", new { CategoryID = 1 }, mysql);
			int count = DbHelper.ExecuteScalar<int>("GetCountByCategoryId_MySqlCmd", new { CategoryID = 1 }, mysql);

			sb.AppendLine();			
			sb.AppendFormat("存在 {0} 条符合条件的记录。条件：CategoryID = {1}\r\n", count, 1);
			sb.AppendLine(ClownFish.XmlHelper.XmlSerializerObject(list));
		}

		return sb.ToString().StringToHtml();
	}


}
