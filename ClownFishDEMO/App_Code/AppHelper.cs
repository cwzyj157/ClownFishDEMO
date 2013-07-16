using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Configuration;
using ClownFish;
using Test.Models;
using System.IO;

public static class AppHelper
{
	public static void Init()
	{
		
		// 设置配置参数：当成功执行数据库操作后，如果有输出参数，则自动获取返回值并赋值到实体对象的对应数据成员中。
		DbContextDefaultSetting.AutoRetrieveOutputValues = true;

		// 加载XmlCommand
		string path = System.IO.Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data\XmlCommand");
		XmlCommandManager.LoadCommnads(path);

		// 注册编译失败事件，用于检查在编译实体加载器时有没有失败。
		BuildManager.OnBuildException += new BuildExceptionHandler(BuildManager_OnBuildException);

		// 开始准备向ClownFishSQLProfiler发送所有的数据库访问操作日志
		Profiler.ApplicationName = "ClownFishDEMO";
		Profiler.TryStartClownFishProfiler();


		// 注册SQLSERVER数据库连接字符串
		ConnectionStringSettings setting = ConfigurationManager.ConnectionStrings["MyNorthwind_MSSQL"];
		DbContext.RegisterDbConnectionInfo("sqlserver", setting.ProviderName, "@", setting.ConnectionString);

		try {
			// 注册MySql数据库连接字符串（存储过程）
			ConnectionStringSettings mysql = ConfigurationManager.ConnectionStrings["MyNorthwind_MySql"];
			DbContext.RegisterDbConnectionInfo("mysql", mysql.ProviderName, "_", mysql.ConnectionString);

			// 注册MySql数据库连接字符串（XmlCommand）
			DbContext.RegisterDbConnectionInfo("mysql-xcmd", mysql.ProviderName, "@", mysql.ConnectionString);
		}
		catch { /* 没有安装 mysql-connector-net */ }


		// 注册Access数据库连接字符串。
		ConnectionStringSettings access = ConfigurationManager.ConnectionStrings["MyNorthwind_Access"];
		string mdbPath = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data\\MyNorthwind.mdb");
		string mdbConnectionString = string.Format(access.ConnectionString, mdbPath);
		DbContext.RegisterDbConnectionInfo("access", access.ProviderName, string.Empty, mdbConnectionString);


		// 注册SQLite数据库连接字符串。
		ConnectionStringSettings sqlite = ConfigurationManager.ConnectionStrings["MyNorthwind_SQLite"];
		string db3Path = Path.Combine(HttpRuntime.AppDomainAppPath, "App_Data\\MyNorthwind.db3");
		string sqliteConnectionString = string.Format(sqlite.ConnectionString, db3Path);
		DbContext.RegisterDbConnectionInfo("sqlite", sqlite.ProviderName, "@", sqliteConnectionString);



		// 启动自动编译数据实体加载器的工作模式。
		// 编译的触发条件：请求实体加载器超过2000次，或者，等待编译的类型数量超过100次
		BuildManager.StartAutoCompile(() => BuildManager.RequestCount > 2000 || BuildManager.WaitTypesCount > 100);

		// 启动自动编译数据实体加载器的工作模式。每10秒【固定】启动一个编译过程。
		// 注意：StartAutoCompile只能调用一次，第二次调用时，会引发异常。
		//BuildManager.StartAutoCompile(() => true, 10000);


		// 手动提交要编译加载器的数据实体类型。
		// 说明：手动提交与自动编译不冲突，不论是同步还是异步。
		Type[] models = BuildManager.FindModelTypesFromCurrentApplication(t => t.FullName.StartsWith("Test.Models."));
		BuildManager.CompileModelTypesSync(models, true);
		//BuildManager.CompileModelTypesAsync(models);

	}

	static void BuildManager_OnBuildException(Exception ex)
	{
		CompileException ce = ex as CompileException;
		if( ce != null )
			SafeLogException(ce.GetDetailMessages());
		else
			// 未知的异常类型
			SafeLogException(ex.ToString());
	}
	public static void SafeLogException(string message)
	{
		try {
			string logfilePath = Path.Combine(HttpRuntime.AppDomainAppPath, @"App_Data\ErrorLog.txt");

			File.AppendAllText(logfilePath, "=========================================\r\n" + message, System.Text.Encoding.UTF8);
		}
		catch { }
	}

	
}
