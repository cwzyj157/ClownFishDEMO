using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PerformanceTestApp.Model
{
	public class OrderInfo
	{
		public int OrderID { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal SumMoney { get; set; }
		public string Comment { get; set; }
		public bool Finished { get; set; }
		public int ProductID { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public string ProductName { get; set; }
		public int CategoryID { get; set; }
		public string Unit { get; set; }
		public string Remark { get; set; }
		
		// 注意：客户信息有可能会是DBNull
		public int? CustomerID { get; set; }
		public string CustomerName { get; set; }
		public string ContactName { get; set; }
		public string Address { get; set; }
		public string PostalCode { get; set; }
		public string Tel { get; set; }
	}
}
