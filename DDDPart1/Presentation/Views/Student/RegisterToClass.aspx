<%@ Page Title="" Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<RegisterToClassModel>" %>

<asp:Content ID="Content1" ContentPlaceHolderID="TitleContent" runat="server">
	Register <%=Model.StudentName %> To a class
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <h2>Register <%= Model.StudentName %> To a class</h2>

            <% using (Html.BeginForm("DoRegisterToClass", "Student"))
       { %>
       <%= Html.HiddenFor(x => x.StudentId) %>
       <label for="FirstName">Choose class: </label>
       <%= Html.DropDownListFor(x => x.ClassId, Model.Classes.Select(x => new SelectListItem(){Text=x.Name, Value=x.Id.ToString()})) %>
       <%= Html.ValidationMessageFor(x => x.ClassId)%>
       <br />
       <input type="submit" value="Register" />
    <% } %>

</asp:Content>
