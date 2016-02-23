<%@ Control CodeBehind="Launcher.ascx.cs" Inherits="OPSI.UManage.Pages.Launcher" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<div class="app">
  <div class="app-section">

    <div id="panel_normal" runat="server">
        <h5>Click here to launch the module</h5>
        <asp:LinkButton cssClass="dnnPrimaryAction" runat="server" ID="lnk_launcher" onclick="lnk_launcher_click">
          Launch the module
        </asp:LinkButton>
    </div>

      <div id="panel_unregistereduser" runat="server" visible="false">
        The module is not available to non authenticated users.
    </div>

  </div>
</div>

<dnn:DnnCssInclude ID="fum_icons_css" runat="server" FilePath="~/DesktopModules/UManage/app/bower_components/entypo/font/entypo.css" />
