<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<StudentDTO>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Register <%= Model.FirstName %> <%= Model.LastName %> To a class
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Register <%= Model.FirstName %> <%= Model.LastName %> To a class</h2>

    <% using (Html.BeginForm("DoRegisterToClass", "Student"))
       { %>
       <%= Html.HiddenFor(x => x.Id) %>
       <label for="FirstName">Choose class: </label>
       <%= Html.DropDownListFor(x => x.ClassToRegister, Model.NotRegisteredClasses.Select(x => new SelectListItem(){Text=x.Name, Value=x.Id.ToString()})) %>
       <%= Html.ValidationMessageFor(x => x.ClassToRegister)%>
       <br />
       <input type="submit" value="Register" />
    <% } %>

</asp:Content>
