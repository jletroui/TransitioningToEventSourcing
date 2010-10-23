<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<ClassDTO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Create a new class
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Create a new class</h2>

    <% using (Html.BeginForm("DoCreate", "Class"))
       { %>
       <label for="FirstName">Name: </label>
       <%= Html.TextBoxFor(x => x.Name) %>
       <%= Html.ValidationMessageFor(x => x.Name)%>
       <br />
       <label for="LastName">Credits (3-6): </label>
       <%= Html.TextBoxFor(x => x.Credits) %>
       <%= Html.ValidationMessageFor(x => x.Credits)%>
       <br />
       <input type="submit" value="Create Class" />
    <% } %>

</asp:Content>
