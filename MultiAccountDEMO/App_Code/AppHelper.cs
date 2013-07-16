using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Configuration;
using ClownFish;
using System.IO;

public static class AppHelper
{
	public static readonly string STR_AccountDbKey = Guid.NewGuid().ToString();
	private static string s_connectionStringFormat;

	public static void Init()
	{
		// 注册Access数据库连接字符串。
		ConnectionStringSettings access = ConfigurationManager.ConnectionStrings["MyNorthwind_Access"];

		// 注意最后一个参数，它是一个委托，用于在需要连接字符串时返回一个连接字符串。
		DbContext.RegisterDbConnectionInfo("access", access.ProviderName, string.Empty, ObtainConnectionString);
		s_connectionStringFormat = access.ConnectionString;
	}

	
	private static string ObtainConnectionString(string configname)
	{
		if( HttpContext.Current == null )
			throw new InvalidOperationException();

		// 注意：在Global.asax中，我已订阅了Application_PostResolveRequestCache事件，
		//        会调用GetAccountNameFormRequest()从Cookie中提取帐套数据库的名称，放在HttpContext中。
		string accountDb = HttpContext.Current.Items[STR_AccountDbKey] as string;

		string mdbPath = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data\\" + accountDb + ".mdb");
		return string.Format(s_connectionStringFormat, mdbPath);
	}


	public static string GetAccountNameFormRequest(HttpRequest request)
	{
		if( request == null )
			throw new ArgumentNullException("request");

		HttpCookie cookie = request.Cookies["database"];
		if( cookie == null || string.IsNullOrEmpty(cookie.Value) )
			return null;

		try {
			FormsAuthenticationTicket ticket = FormsAuthentication.Decrypt(cookie.Value);
			return ticket.UserData;
		}
		catch {
			return null;
		}
	}

	public static void SetAccountNameToCookie(string database)
	{
		if( HttpContext.Current == null )
			throw new InvalidOperationException();

		FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(
				2, string.Empty, DateTime.Now, DateTime.Now.AddYears(1), true, database);

		string cookieValue = FormsAuthentication.Encrypt(ticket);

		HttpCookie cookie = new HttpCookie("database", cookieValue);
		cookie.HttpOnly = true;
		HttpContext.Current.Response.Cookies.Add(cookie);
	}
}
