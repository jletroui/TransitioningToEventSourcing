<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StudentDTO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create a new student
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create a new student</h2>

    <% using (Html.BeginForm("DoCreate", "Student"))
       { %>
       <label for="FirstName">First name: </label>
       <%= Html.TextBoxFor(x => x.FirstName) %>
       <%= Html.ValidationMessageFor(x => x.FirstName) %>
       <br />
       <label for="LastName">Last name: </label>
       <%= Html.TextBoxFor(x => x.LastName) %>
       <%= Html.ValidationMessageFor(x => x.LastName)%>
       <br />
       <input type="submit" value="Create Student" />
    <% } %>
</asp:Content>
