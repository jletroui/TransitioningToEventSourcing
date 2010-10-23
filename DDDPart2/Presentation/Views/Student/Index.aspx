<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ISortedPagination<StudentDTO>>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index of students
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Index of students</h2>

    <% foreach (var student in Model)
       { %>
           <div>
               <%= student.FirstName %> 
               <%= student.LastName %>
               <%= Html.ActionLink("Details", "Details", new {studentId = student.Id })%>
               <%= Html.ActionLink("Correct name", "CorrectName", new {studentId = student.Id })%>
               <%= Html.ActionLink("Register to a class", "RegisterToClass", new {studentId = student.Id })%>
           </div>
    <% } %>

    <%= Html.Pager(Model).QueryParam("page") %>
    <br />

    <div>
        <%= Html.ActionLink("Create new student", "Create") %>
    </div>
</asp:Content>
