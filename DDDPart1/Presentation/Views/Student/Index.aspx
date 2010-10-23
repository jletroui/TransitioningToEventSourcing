<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StudentSearchModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
    Index of students
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Index of students</h2>
    <% using(Html.BeginForm("Index", "Student"))
       { %>
       <%= Html.TextBoxFor(x => x.Name) %>
       <input type="submit" value="search" />

    <% foreach (var student in Model.Students)
       { %>
           <div>
               <%= student.FirstName %> 
               <%= student.LastName %>
               <%= Html.ActionLink("Details", "Details", new {studentId = student.Id })%>
               <%= Html.ActionLink("Correct name", "CorrectName", new {studentId = student.Id })%>
               <%= Html.ActionLink("Register to a class", "RegisterToClass", new {studentId = student.Id })%>
           </div>
    <% } %>

    <%= Html.Pager(Model.Students) %>
    <% } %>
    <br />

    <div>
        <%= Html.ActionLink("Create new student", "Create") %>
    </div>
</asp:Content>
