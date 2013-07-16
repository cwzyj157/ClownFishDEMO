using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MyMVC;

// 示例使用了MyMVC框架：http://www.cnblogs.com/fish-li/archive/2012/02/12/2348395.html

public static class AjaxLogin
{
	[Action]
	public static object Login(string username, string password, string database)
	{
		// 说明：在这个示例方法中，不检查用户名与密码是否有效。
		// 也不检查 database 变量中的内容是否有效。

		AppHelper.SetAccountNameToCookie(database);
		return new MyMVC.RedirectResult("/ProductList.htm");
	}
}
