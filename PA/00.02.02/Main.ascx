<%@ Control CodeBehind="Main.ascx.cs" Inherits="OPSI.UManage.Pages.Main" %>
<%@ Register TagPrefix="dnn" Namespace="DotNetNuke.Web.Client.ClientResourceManagement" Assembly="DotNetNuke.Web.Client" %>

<!-- some help -->
<asp:Label ID="VAR_ModulePath" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
<asp:Label ID="VAR_ProfilePicBasePath" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
<asp:Label ID="VAR_FullEditPath" runat="server" ClientIDMode="Static" style="display:none;"></asp:Label>
<asp:Label ID="VAR_IsAdmin" runat="server" ClientIDMode="Static" style="display:none;">0</asp:Label>
<asp:Label ID="VAR_PageBase" runat="server" ClientIDMode="Static" style="display:none;">0</asp:Label>
<!-- the app css -->
<dnn:DnnCssInclude ID="fum_module_css" runat="server" FilePath="~/DesktopModules/UManage/css/style.css" />
<dnn:DnnCssInclude ID="fum_icons_css" runat="server" FilePath="~/DesktopModules/UManage/app/bower_components/entypo/font/entypo.css" />
<!-- app libraries -->
<dnn:DnnJsInclude ID="fum_bower_angular" runat="server" FilePath="~/DesktopModules/UManage/app/bower_components/angular/angular.min.js" priority="5"/>
<!-- app ng-app-js -->
<dnn:DnnJsInclude ID="fum_app_controllers" runat="server" FilePath="~/DesktopModules/UManage/app/js/controllers/controller.js" priority="7"/>
<dnn:DnnJsInclude ID="fum_app_directives" runat="server" FilePath="~/DesktopModules/UManage/app/js/directives/directives.js" priority="8"/>
<dnn:DnnJsInclude ID="fum_app_services" runat="server" FilePath="~/DesktopModules/UManage/app/js/services/service.js" priority="9"/>
<dnn:DnnJsInclude ID="fum_app_launcher" runat="server" FilePath="~/DesktopModules/UManage/app/js/app.js" priority="10"/>

<!-- the app view -->
<section data-ng-app="opsiModule" class="app">
  <header class="app-header">
    <h1 class="app-logo"><a>UManage <small>- User Manager</small></a></h1>
    <nav class="app-menu opsi-main-menu">
      <a class="closeme" href="<%=CloseModule_URL%>" ><span class="icon-cancel" style="font-size: 2em;vertical-align: bottom;"></span> Close Module</a>
      <span style="display:none;">
      <span class="icon-menu"></span>
      <ul class="menu-main">
        <li class="item"><a href="<%=CloseModule_URL%>">Go back</a>
        </li>
      </ul>
      </span>
    </nav>
  </header>
  <app-menu></app-menu>
  <main class="app-section">
    <opsi-user-tiles></opsi-user-tiles>
    <opsi-user-detail></opsi-user-detail>
  </main>
  <footer class="app-footer">
    Module developed with <span class="icon-heart" title="code"></span>by <strong><a href="http://www.opsi.it" target="_blank">OPSI </a></strong>©2015. Released under GNU/GPL licence.
  </footer>
  <opsi-dialog></opsi-dialog>
  <opsi-dialog-modal></opsi-dialog-modal>
  <app-error></app-error>
</section>

<!-- ng-app config -->
<script>
  ;(function () {
    // getting a new serviceFramework instance for the angular app
    var angular, app, serviceFramework;

    angular = this.angular;

    serviceFramework = $.ServicesFramework(<%=ModuleId %>);

    angular.module('opsiModule')
    // configuring http provider
    .config(function($httpProvider) {
      return $httpProvider.defaults.headers.get = $httpProvider.defaults.headers.post = {
        Accept: 'text/html',
        'Content-Type': 'text/html; charset=UTF-8',
        ModuleId: serviceFramework.getModuleId(),
        RequestVerificationToken: serviceFramework.getAntiForgeryValue(),
        TabId: serviceFramework.getTabId(),
        'X-Requested-With': 'XMLHttpRequest'
      };
    })
    // configuring fum provider
    .config(function($fumProvider) {
      $fumProvider.setProperty('fullEditPath', '<%= VAR_FullEditPath.Text %>');
      $fumProvider.setProperty('moduleApi', serviceFramework.getServiceRoot('UManage'));
      $fumProvider.setProperty('moduleID', serviceFramework.getModuleId());
      $fumProvider.setProperty('modulePath', '<%=VAR_ModulePath.Text %>');
      $fumProvider.setProperty('profilePicPath', '<%=VAR_ProfilePicBasePath.Text %>');
      $fumProvider.setProperty('sudo', '<%= VAR_IsAdmin.Text %>');
      $fumProvider.setProperty('tabID', serviceFramework.getTabId());
    });

    // bootstrapping the app
    // setTimeout(function () {
    //   angular.bootstrap(document.querySelector('#opsi-fum-application'), ['opsiModule']);
    // }, 10);
  }).call(this);
</script>