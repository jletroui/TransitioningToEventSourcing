<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<Student>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Details for <%=  Model.FirstName %> <%= Model.FirstName %>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Details for <%=  Model.FirstName %> <%= Model.FirstName %></h2>
    <br />
    <strong>First name: </strong><%=  Model.FirstName %><br />
    <strong>Last name: </strong><%=  Model.LastName %><br />
    <strong>Has graduated: </strong><%=  Model.HasGraduated %><br />
    <strong>Registrations: </strong><br />
    <% foreach (var reg in Model.Registrations)
       { %>
           <div>
               <%= reg.Class.Name %> (<%= reg.Class.Credits %> credits)
               <%= Html.ActionLink("Make pass", "MakePass", new { studentId = Model.Id, registrationId = reg.Id })%>
               <%= Html.ActionLink("Make fail", "MakeFail", new { studentId = Model.Id, registrationId = reg.Id })%>
           </div>
    <% } %>
</asp:Content>
