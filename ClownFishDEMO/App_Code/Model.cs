using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClownFish;


namespace Test.Models
{
	public class Abbbbbbbb
	{
		public string aaaa;

		public Abbbbbbbb(string s)
		{
			this.aaaa = s;
		}

		public int this[int xx]
		{
			get { return xx + 1; }
		}

		public string this[string s]
		{
			get { return s + "abc"; }
		}
	}

	public class BBBBBB { }

	public class GetMostValuableCustomersParameters
	{
		public int TopN { get; set; }
	}

	public class GetProductTopNParameters
	{
		public int TopN { get; set; }
	}

	public class Customer
	{
		public int CustomerID { get; set; }
		public string CustomerName { get; set; }
		public string ContactName { get; set; }
		public string Address { get; set; }
		public string PostalCode { get; set; }
		public string Tel { get; set; }
	}

	public class Category
	{
		public int CategoryID { get; set; }
		public string CategoryName { get; set; }
	}

	public class Product
	{
		public int ProductID { get; set; }
		public string ProductName { get; set; }
		public int CategoryID { get; set; }
		public string Unit { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }
		public string Remark { get; set; }

		public override string ToString()
		{
			return string.Format("id: {0}, name: {1}, cid: {2}, unit: {3}, price: {4}, quantity: {5}, remark: {6}",
				this.ProductID, this.ProductName, this.CategoryID, this.Unit,
				this.UnitPrice.ToString("F2"), this.Quantity, this.Remark);
		}
	}

	public enum TestEnum
	{
		aaaaaaa,
		bbbbbbb,
		cccccccccccc
	}

	public class ProductNestTest : Product
	{
		public NestLevelOne One;

		public TestEnum EnumValue;

		public SubClass SubItem;

		public PrivateSubClass SubItem2;

		public class SubClass
		{
			public string SSSS;
			public int TTTTT;
		}

		public class PrivateSubClass
		{
			public string aaaa;
		}
	}

	public class NestLevelOne
	{
		public string AAAAA;
		public int BBBBB { get; set; }

		[DbColumn(SubItemPrefix = "t2.")]
		public NestLevelTwo Two { get; set; }

		public short TestShort;
		public double TestDouble;
		public decimal TestDecimal;
		public float TestFloat;
	}

	public class NestLevelTwo
	{
		public DateTime CCCCC { get; set; }
		public Decimal DDDDD;
		public Byte BB1;
		public Byte[] BB2;
		public Guid TestGuid;
		public char TestChar;
	}



	public class Order
	{
		public int OrderID { get; set; }
		public int? CustomerID { get; set; }
		public DateTime OrderDate { get; set; }
		public decimal SumMoney { get; set; }
		public string Comment { get; set; }
		public bool Finished { get; set; }

		public string CustomerName { get; set; }
		public List<OrderDetail> Detail { get; set; }
	}


	public class OrderDetail
	{
		public int OrderID { get; set; }
		public int ProductID { get; set; }
		public decimal UnitPrice { get; set; }
		public int Quantity { get; set; }

		public string ProductName { get; set; }
		public string Unit { get; set; }
	}




}
