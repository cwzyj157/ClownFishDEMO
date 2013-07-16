using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;
using System.Data;
using PerformanceTestApp.Model;
using System.Reflection;
using MyTestAppFramework;

namespace PerformanceTestApp
{
	public interface IPerformanceTest : IDisposable
	{
		List<OrderInfo> Run();
	}

	// 说明：TestMethod的构造函数接受二个参数：
	//        1. 一个字符串，表示测试名称
	//        2. 显示序号。用于决定在下拉列表中的显示位置。

	[TestMethod("手工代码，SQLSERVER", 1)]
	public class Test_Adonet_ShareConnection : IPerformanceTest
	{
		private UiParameters uiParam;
		private SqlConnection conn;

		public Test_Adonet_ShareConnection(UiParameters param)
		{
			this.uiParam = param;
			this.conn = new SqlConnection(Program.ConnectionString);
			this.conn.Open();
		}

		public List<OrderInfo> Run()
		{
			return TestHelper.ExecuteQuery(conn, uiParam);
		}

		public void Dispose()
		{
			conn.Dispose();
		}
	}


	[TestMethod("ClownFish，SQLSERVER", 2)]
	public class Test_ClownFish_ShareConnection : IPerformanceTest
	{
		private UiParameters uiParam;
		private ClownFish.DbContext db;

		public Test_ClownFish_ShareConnection(UiParameters param)
		{
			this.uiParam = param;
			this.db = new ClownFish.DbContext(false);
		}

		public List<OrderInfo> Run()
		{
			var parameter = new { TopN = uiParam.PageSize };
			return ClownFish.DbHelper.FillList<OrderInfo>(
						TestHelper.QueryText, parameter, db, ClownFish.CommandKind.SqlTextWithParams);
		}

		public void Dispose()
		{
			db.Dispose();
		}
	}
	
	[TestMethod("FishWebLib，SQLSERVER", 3)]
	public class Test_FishWebLib_ShareConnection : IPerformanceTest
	{
		private UiParameters uiParam;
		private FishWebLib.FishDbContext db;

		public Test_FishWebLib_ShareConnection(UiParameters param)
		{
			this.uiParam = param;
			this.db = new FishWebLib.FishDbContext(false);
		}

		public List<OrderInfo> Run()
		{
			db.CreateCommand(TestHelper.QueryText, CommandType.Text);
			db.AddParameterToCommand("TopN", uiParam.PageSize, DbType.Int32);
			return db.ExecuteSelectCommandToList<OrderInfo>(true, false);
		}

		public void Dispose()
		{
			db.Dispose();
		}
	}

	[TestMethod(SeparatorComboBox.SeparatorString, 4)]
	public class aaaaaaaaa 
	{
		// 这只是一个用于定义分隔符的临时类型。
		// 注意TestMethod构造函数中的第一个参数。
	}

	[TestMethod("手工代码，DataTable", 5)]
	public class Test_Adonet_LoadDataTable : IPerformanceTest
	{
		public Test_Adonet_LoadDataTable(UiParameters param) { }
		public void Dispose() { }

		public List<OrderInfo> Run()
		{
			return TestHelper.LoadFromDataTable(TestHelper.GetOrderInfoTable());
		}		
	}

	[TestMethod("ClownFish，DataTable", 6)]
	public class Test_ClownFish_LoadDataTable : IPerformanceTest
	{
		public Test_ClownFish_LoadDataTable(UiParameters param) { }
		public void Dispose() { }

		public List<OrderInfo> Run()
		{
			return ClownFish.DbHelper.FillListFromTable<OrderInfo>(TestHelper.GetOrderInfoTable());
		}		
	}


	[TestMethod("FishWebLib，DataTable", 7)]
	public class Test_FishWebLib_LoadDataTable : IPerformanceTest
	{
		public Test_FishWebLib_LoadDataTable(UiParameters param) { }
		public void Dispose() { }

		public List<OrderInfo> Run()
		{
			return FishWebLib.FishItemHelper.LoadItemsFromDataTable<OrderInfo>(TestHelper.GetOrderInfoTable());
		}		
	}





}
