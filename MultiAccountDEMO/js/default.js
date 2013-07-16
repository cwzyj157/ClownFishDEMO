
jQuery.fn.SetGridViewColor = function(){
	return this.each(function(){ 
		$(this).addClass("GridView")
			.find(">thead>tr").addClass("GridView_HeaderStyle").end()
			.find(">tbody>tr")
			.filter(':odd').addClass("GridView_AlternatingRowStyle").end()
			.filter(':even').addClass("GridView_RowStyle");
	});
}

$(function() {
	$.ajax({
		url: "/AjaxProduct/GetProducts.cspx",
		cache: false,
		success: function(html) {
			if (html == "not login.")
				window.location.href = "/login.htm";
			else {
				$("#divProductList").html(html);
				$("table.myGridVew").SetGridViewColor();
			}
		}
	});
});