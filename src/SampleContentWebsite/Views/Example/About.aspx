﻿<%@ Page Language="C#" MasterPageFile="~/Views/Shared/Site.Master" Inherits="System.Web.Mvc.ViewPage<SampleFeature.AboutViewModel>" %>

<asp:Content ID="aboutContent" ContentPlaceHolderID="MainContent" runat="server">

    <p>This is the example controller About view.</p>

    <p>Id is: <%:Model.Id %></p>

    <p>SomethingElse is: <%:Model.SomethingElse %></p>

    <p><%=Html.ActionLink("Link to example controller Index view", "Index", "Example") %></p>

</asp:Content>
