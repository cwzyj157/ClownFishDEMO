using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ClownFish;
using System.Data;
using Test.Models;


// 以下示例代码演示了如何在调用DbContext时，对各个步骤的具体控制。
// 以及在一个连接中执行多次数据库调用的方法，包括事务的使用。
// 同时也演示了在【不同分层】中对连接的控制。


/// <summary>
/// 这个类将演示在所有方法将可以共用一个连接。
/// </summary>
public class CustomerBLL2 : DbContextHolderBase
{
	public bool InsertCustomer(Customer customer)
	{
		// 请注意第三个参数，这种调用可以让“上层”调用类来控制连接对象，
		//  以实现在一个连接中调用多个方法，并且可以支持事务。
		return (DbHelper.ExecuteNonQuery("InsertCustomer", customer, this.DbContext, CommandKind.SpOrXml) > 0);
	}

	public bool DeleteCustomer(int customerId)
	{
		var parameter = new { CustomerID = customerId };
		return (DbHelper.ExecuteNonQuery("DeleteCustomer", parameter, this.DbContext, CommandKind.SpOrXml) > 0);
	}

	
}








/// <summary>
/// 这个类将演示在所有方法中共用一个连接。并手工控制每个调用的细节过程。
/// </summary>
public class ProductBLL2 : DbContextHolderBase
{
	// ##########################################################################
	// 前面演示了直接使用DbHelper静态方法的使用，
	// 这个类演示使用DbContext的成员方法来完成类似的操作。
	// 
	// 其实为了实现这些功能，也可以直接调用DbHelper，仍然只需要一行代码。
	// ##########################################################################

	public bool InsertProduct(Product product)
	{
		// 创建命令
		this.DbContext.CreateXmlCommand("InsertProduct");
		// 设置当前命令中的所有参数
		this.DbContext.SetCommandParameters(product);
		// 调用存储过程
		return (this.DbContext.ExecuteNonQuery() > 0);
	}

	public bool DeleteProduct(int productId)
	{
		// 创建命令
		this.DbContext.CreateXmlCommand("DeleteProduct");

		//// 设置当前命令中的参数
		//this.DbContext.GetCommandParameter("ProductID").Value = productId;

		// 或者下面的方法也是可以的。
		this.DbContext.SetCommandParameters(new { ProductID = productId });

		// 调用存储过程
		return (this.DbContext.ExecuteNonQuery() > 0);
	}


	public Product GetProductById(int productId)
	{
		// 创建命令
		this.DbContext.CreateXmlCommand("GetProductById");

		// 设置当前命令中的参数
		this.DbContext.GetCommandParameter("ProductID").Value = productId;

		// 调用存储过程，并将结果转换成实体对象
		return this.DbContext.GetDataItem<Product>();
	}
}












/// <summary>
/// 演示了在服务类或页面方法中如何使用上面BLL中定义的方法：以共享连接的方式执行数据库的调用
/// </summary>
public static class MyServiceLayer
{
	/// <summary>
	/// 第一个调用示例：用BLL对象控制连接的方法
	/// </summary>
	/// <param name="customer"></param>
	/// <param name="product"></param>
	public static string ShareConnectionDEMO_1(Customer customer, Product product)
	{
		// 用BLL对象控制连接对象的生存时间
		using( CustomerBLL2 bll = new CustomerBLL2() ) {
			// 创建连接对象，以事务的方式工作。
			// 下面执行的四次调用，使用的是同一个连接
			bll.DbContext = bll.CreateDbContext(true);

			// 执行第一次调用，新增一个客户记录
			bll.InsertCustomer(customer);
			// 执行第二次调用，删除前面新增的记录
			bll.DeleteCustomer(customer.CustomerID);

			// 创建另一个业务逻辑层，并共享bll中的连接。
			ProductBLL2 bll2 = bll.CreateHolder<ProductBLL2>();

			// 执行第三次调用，新增一个商品记录
			bll2.InsertProduct(product);
			// 执行第四次调用，删除前面新增的记录
			bll2.DeleteProduct(product.ProductID);

			// 提交事务
			bll.DbContext.CommitTransaction();

			return string.Format("New CustomerId= {0}; New ProuductId= {1}", customer.CustomerID, product.ProductID);
			// 在using结束时，会调用CustomerBLL2.Dispose()方法，释放连接。
		}
	}


	/// <summary>
	/// 第二个调用示例：直接控制DbContext的生存时间的方法
	/// 如果业务逻辑层实现了IDbContextHolder接口，也可以按下面的方式来使用DbContext
	/// </summary>
	/// <param name="customer"></param>
	/// <param name="product"></param>
	public static string ShareConnectionDEMO_2(Customer customer, Product product)
	{

		// 直接控制FishDbContext的生存时间，这里演示了不使用事务的方式
		// 如果要使用事务，只需要在调用FishDbContext的构造方法传递true就可以了
		using( DbContext dbContext = new DbContext(false) ) {
			// 下面执行的四次调用，使用的是同一个连接（当前连接）

			// 创建业务类实例，并设置为当前连接
			CustomerBLL2 bll = dbContext.CreateHolder<CustomerBLL2>();

			// 执行第一，二次调用
			bll.InsertCustomer(customer);
			bll.DeleteCustomer(customer.CustomerID);

			// 创建另一个业务逻辑层，并共享当前连接。
			ProductBLL2 bll2 = dbContext.CreateHolder<ProductBLL2>();

			// 执行第三，四次调用
			bll2.InsertProduct(product);
			bll2.DeleteProduct(product.ProductID);

			return string.Format("New CustomerId= {0}; New ProuductId= {1}", customer.CustomerID, product.ProductID);
			// 在using结束时，释放连接。
		}
	}

}


