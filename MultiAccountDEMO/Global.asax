<%@ Application Language="C#" %>

<script runat="server">

    void Application_Start(object sender, EventArgs e) 
    {
        //在应用程序启动时运行的代码
		AppHelper.Init();
    }
    
    void Application_End(object sender, EventArgs e) 
    {
        //在应用程序关闭时运行的代码

    }
        
    void Application_Error(object sender, EventArgs e) 
    { 
        //在出现未处理的错误时运行的代码

    }


	protected void Application_PostResolveRequestCache(object sender, EventArgs e)
	{
		// 在这个事件中，检查当前用户有没有选择帐套，如果没有，则重定向到登录页面。
		HttpApplication app = (HttpApplication)sender;

		// 检查是不是页面请求，或者AJAX请求。
		// 注意：我并没有使用aspx页面，而是使用了纯静态页面，所以检查 htm 扩展名
		if( app.Request.Path.EndsWith(".htm", StringComparison.OrdinalIgnoreCase)
			|| app.Request.Path.EndsWith(".cspx", StringComparison.OrdinalIgnoreCase) ) {

			// 总是允许请求登录验证功能。
			if( string.Compare(app.Request.Path, "/login.htm", StringComparison.OrdinalIgnoreCase) == 0 )
				return;
			if( string.Compare(app.Request.Path, "/AjaxLogin/Login.cspx", StringComparison.OrdinalIgnoreCase) == 0 )
				return;

			string accountDb = AppHelper.GetAccountNameFormRequest(app.Request);

			if( string.IsNullOrEmpty(accountDb) )
				app.Response.Redirect("/login.htm");
			else
				// 保存起来，供后面所有请求使用。
				// AppHelper.ObtainConnectionString()方法将会读取这个数据。
				app.Context.Items[AppHelper.STR_AccountDbKey] = accountDb;
		}
	}
	
	
</script>
