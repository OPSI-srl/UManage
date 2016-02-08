<%@ Control CodeBehind="Launcher.ascx.cs" Inherits="OPSI.UManage.Pages.Launcher" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<div class="app">
  <div class="app-section">

    <div id="panel_normal" runat="server">
        <h5>Clicca qui per lanciare il modulo</h5>
        <asp:LinkButton cssClass="button safe" runat="server" ID="lnk_launcher" onclick="lnk_launcher_click">
          Lancia il modulo
        </asp:LinkButton>
    </div>

      <div id="panel_unregistereduser" runat="server" visible="false">
        Il modulo non è accessibile dagli utenti non autenticati.
    </div>

  </div>
</div>

<dnn:DnnCssInclude ID="fum_module_css" runat="server" FilePath="~/DesktopModules/UManage/css/style.css" />
<dnn:DnnCssInclude ID="fum_icons_css" runat="server" FilePath="~/DesktopModules/UManage/app/bower_components/entypo/font/entypo.css" />