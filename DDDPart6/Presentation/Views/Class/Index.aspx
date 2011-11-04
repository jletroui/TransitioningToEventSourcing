<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ISortedPagination<ClassDTO>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Index of classes
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Index of classes</h2>

    <% foreach (var item in Model)
       { %>
       <div>
       <%= item.Name%> (<%= item.Credits %> credits)
       </div>
    <% } %>
    <%= Html.Pager(Model).QueryParam("page") %>
    <br />
    <div>
    <%= Html.ActionLink("Create new class", "Create") %>
    </div>

</asp:Content>
