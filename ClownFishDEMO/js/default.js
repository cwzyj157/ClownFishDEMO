
function ResizeHeight() {
    $("fieldset").height($(window).height() - $("legend").height() - 15);
    $("#divOutput").height($("fieldset").height() - 15);
}

$(function () {
    $("#btnSimpleCRUD").click( function() { CallServer("SimpleCRUD"); } );
	$("#btnSimpleCRUD_MySql").click( function() { CallServer("SimpleCRUD_MySql"); } );
	$("#btnSimpleCRUD_MySqlXCmd").click( function() { CallServer("SimpleCRUD_MySql_XmlCommand"); } );
	$("#btnSimpleCRUD_Access").click( function() { CallServer("SimpleCRUD_Access"); } );
	$("#btnSimpleCRUD_SQLite").click( function() { CallServer("SimpleCRUD_SQLite"); } );
    $("#btnLoadNestModel").click( function() { CallServer("LoadNestModel"); } );
    $("#btnShareConnection").click( function() { CallServer("ShareConnection"); } );
	$("#btnDataTable").click( function() { CallServer("QueryToDataTable"); } );
	$("#btnDataSet").click( function() { CallServer("QueryToDataSet"); } );
	$("#btnLoadDataTable").click(function() { CallServer("LoadListFromDataTable"); });
	$("#btnLoadFromXmlFile").click(function() { CallServer("LoadFromXmlFile"); });
	$("#btnGetPagedList").click( function() { CallServer("GetPagedList"); } );
	$("#btnGetXmlCommand").click( GetXmlCommand );

    ResizeHeight();
    $(window).resize(ResizeHeight);

	$.get("/AjaxDemo/MySqlClientIsReady.cspx", function (result) {
		if( result == "1" )	$("#spanMySql").show(); }   
	);
	
    $.ajaxSetup({
        cache: false,
		beforeSend: function() { $("#divOutput").html("正在调用服务端，请稍后......");  },
        error: function (XMLHttpRequest, textStatus, errorThrown) {
            if (typeof (errorThrown) != "undefined")
                $("#divOutput").html("调用服务器失败。<br />" + errorThrown);
            else {
                var error = "<b style='color: #f00'>" + XMLHttpRequest.status + "  " + XMLHttpRequest.statusText + "</b>";
                var start = XMLHttpRequest.responseText.indexOf("<title>");
                var end = XMLHttpRequest.responseText.indexOf("</title>");
                if (start > 0 && end > start)
                    error += "<br /><br />" + XMLHttpRequest.responseText.substring(start + 7, end);

                $("#divOutput").html("调用服务器失败。<br />" + error);
            }
        }
    });	
});


function CallServer(actionName){
	var url = "/AjaxDemo/" + actionName + ".cspx";
	$.get(url, function (result) { $("#divOutput").html(result); }   );
}

function GetXmlCommand(){
	var name = prompt("请输入一个XmlCommand的名称：\n注意：你可以在运行时手工修改XmlCommand的配置文件，然后可以检查是否已读取到最新版本。"); 
	if( name.length ==0) return;
	var url = "/AjaxDemo/GetXmlCommand.cspx?name=" + encodeURIComponent(name);
	$.get(url, function (result) { $("#divOutput").html(result); }   );
}

