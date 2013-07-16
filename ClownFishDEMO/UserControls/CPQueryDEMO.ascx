<%@ Control Language="C#" Inherits="MyUserControlView<CPQueryDEMOResult>" %>

<p>Commnad Type is: <%= Model.Command.ToString() %></p>
<p><%= Model.Command.CommandText.HtmlEncode() %></p>

<p>
	<% foreach(System.Data.Common.DbParameter p in Model.Command.Parameters) { %>
	<%= p.ParameterName.HtmlEncode() %> = <%= p.Value.ToString().HtmlEncode() %><br />
	<%} %>
</p>

<hr />

<%= MyMvcEx.DataTableHelper.TableToHtml(Model.QueryResult) %>
