using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using PerformanceTestApp.Model;
using System.Data;
using System.Data.SqlClient;

namespace PerformanceTestApp
{
	public static class TestHelper
	{
		public static readonly string QueryText = @"
select top (@TopN) d.OrderID, d.OrderDate, d.SumMoney, d.Comment, d.Finished,
dt.ProductID, dt.UnitPrice, dt.Quantity, 
p.ProductName, p.CategoryID, p.Unit, p.Remark,
c.CustomerID, c.CustomerName, c.ContactName, c.Address, c.PostalCode, c.Tel
from Orders d 
inner join [Order Details] dt on d.OrderId = dt.OrderId
inner join Products p on dt.ProductId = p.ProductId
left join Customers c on d.CustomerId = c.CustomerId
";

		public static List<OrderInfo> ExecuteQuery(SqlConnection conn, UiParameters uiParam)
		{
			SqlCommand command = new SqlCommand(QueryText, conn);
			command.Parameters.Add("TopN", SqlDbType.Int).Value = uiParam.PageSize;

			List<OrderInfo> list = new List<OrderInfo>(uiParam.PageSize);

			using( SqlDataReader reader = command.ExecuteReader() ) {
				while( reader.Read() )
					list.Add(LoadOrderInfo(reader));
			}

			return list;
		}

		private static OrderInfo LoadOrderInfo(SqlDataReader reader)
		{
			OrderInfo info = new OrderInfo();
			info.OrderID = (int)reader["OrderID"];
			info.OrderDate = (DateTime)reader["OrderDate"];
			info.SumMoney = (decimal)reader["SumMoney"];
			info.Comment = (string)reader["Comment"];
			info.Finished = (bool)reader["Finished"];
			info.ProductID = (int)reader["ProductID"];
			info.UnitPrice = (decimal)reader["UnitPrice"];
			info.Quantity = (int)reader["Quantity"];
			info.ProductName = (string)reader["ProductName"];
			info.CategoryID = (int)reader["CategoryID"];
			info.Unit = (string)reader["Unit"];
			info.Remark = (string)reader["Remark"];

			object customerId = reader["CustomerID"];
			if( customerId != DBNull.Value ) {
				info.CustomerID = (int)customerId;
				info.CustomerName = (string)reader["CustomerName"];
				info.ContactName = (string)reader["ContactName"];
				info.Address = (string)reader["Address"];
				info.PostalCode = (string)reader["PostalCode"];
				info.Tel = (string)reader["Tel"];
			}
			return info;
		}


		public static List<OrderInfo> LoadFromDataTable(DataTable table)
		{
			List<OrderInfo> list = new List<OrderInfo>(table.Rows.Count);
			foreach( DataRow dataRow in table.Rows )
				list.Add(LoadOrderInfo(dataRow));

			return list;
		}

		private static OrderInfo LoadOrderInfo(DataRow dataRow)
		{
			OrderInfo info = new OrderInfo();
			info.OrderID = (int)dataRow["OrderID"];
			info.OrderDate = (DateTime)dataRow["OrderDate"];
			info.SumMoney = (decimal)dataRow["SumMoney"];
			info.Comment = (string)dataRow["Comment"];
			info.Finished = (bool)dataRow["Finished"];
			info.ProductID = (int)dataRow["ProductID"];
			info.UnitPrice = (decimal)dataRow["UnitPrice"];
			info.Quantity = (int)dataRow["Quantity"];
			info.ProductName = (string)dataRow["ProductName"];
			info.CategoryID = (int)dataRow["CategoryID"];
			info.Unit = (string)dataRow["Unit"];
			info.Remark = (string)dataRow["Remark"];

			object customerId = dataRow["CustomerID"];
			if( customerId != DBNull.Value ) {
				info.CustomerID = (int)customerId;
				info.CustomerName = (string)dataRow["CustomerName"];
				info.ContactName = (string)dataRow["ContactName"];
				info.Address = (string)dataRow["Address"];
				info.PostalCode = (string)dataRow["PostalCode"];
				info.Tel = (string)dataRow["Tel"];
			}
			return info;
		}


		private static DataTable s_OrderInfoTable;

		public static DataTable GetOrderInfoTable()
		{
			// 把结果用静态变量缓存起来，避免影响测试时间
			// 由于在运行测试前，会有一次单独的调用，所以并没有线程安全问题。

			if( s_OrderInfoTable == null ) {
				s_OrderInfoTable = ClownFish.DbHelper.FillDataTable(
							TestHelper.QueryText, new { TopN = 50 }, ClownFish.CommandKind.SqlTextWithParams);
			}

			return s_OrderInfoTable;
		}
	}


}
