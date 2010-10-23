<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StudentModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Correct student name
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Correct student name</h2>

        <% using (Html.BeginForm("DoCorrectName", "Student"))
       { %>
       <%= Html.HiddenFor(x => x.Id) %>
       <label for="FirstName">First name: </label>
       <%= Html.TextBoxFor(x => x.FirstName) %>
       <%= Html.ValidationMessageFor(x => x.FirstName) %>
       <br />
       <label for="LastName">Last name: </label>
       <%= Html.TextBoxFor(x => x.LastName) %>
       <%= Html.ValidationMessageFor(x => x.LastName)%>
       <br />
       <input type="submit" value="Correct name" />
    <% } %>

</asp:Content>
