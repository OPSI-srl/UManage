(function() {
  'use strict';
  var $appStatusTemplate, $opsiDialogTemplate, $opsiFilterRolesTemplate, $opsiRolesTemplate, $opsiUserFiltersTemplate, angular, app;

  angular = this.angular;

  app = angular.module('opsiModule.directives', []);

  app.directive('appError', function() {
    return {
      controller: 'AppErrorController',
      controllerAs: 'status',
      replace: true,
      templateUrl: 'templates/app-status.html'
    };
  });

  app.directive('appMenu', function($fum, $timeout) {
    return {
      controller: 'MenuVertical',
      link: function($scope, $element) {
        var timeoutPromise, timeoutTimer;
        timeoutPromise = void 0;
        timeoutTimer = 500;
        return $scope.watchFilters = function() {
          $element.find('input[type="checkbox"], select').off('change').on('change', function() {
            var hasCancelled;
            if (timeoutPromise) {
              hasCancelled = $timeout.cancel(timeoutPromise);
            }
            return timeoutPromise = $timeout(function() {
              return $scope.postFilters();
            }, timeoutTimer);
          });
          return $element.find('input[type="text"]').off('keyup').on('keyup', function() {
            var hasCancelled;
            if (timeoutPromise) {
              hasCancelled = $timeout.cancel(timeoutPromise);
            }
            return timeoutPromise = $timeout(function() {
              return $scope.postFilters();
            }, timeoutTimer);
          });
        };
      },
      priority: 1,
      replace: true,
      restrict: 'E',
      templateUrl: "" + $fum.modulePath + "/ngapp/partials/vertical-menu.html"
    };
  });

  app.directive('opsiDialog', function() {
    return {
      controller: 'OpsiDialog',
      controllerAs: 'dialog',
      replace: true,
      templateUrl: 'templates/app-dialog.html'
    };
  });

  app.directive('opsiDialogModal', function() {
    return {
      link: function($scope, $element) {
        $scope.$on('show-modal', function() {
          $element.addClass('visible');
          return angular.element('body').addClass('overflowing');
        });
        return $scope.$on('hide-modal', function() {
          $element.removeClass('visible');
          return angular.element('body').removeClass('overflowing');
        });
      },
      replace: true,
      template: '<div class="app-modal"></div>'
    };
  });

  app.directive('opsiRoles', function() {
    return {
      scope: {
        change: '&',
        id: '@',
        limit: '=',
        roles: '='
      },
      replace: true,
      templateUrl: 'templates/opsi-roles.html'
    };
  });

  app.directive('opsiFilterRoles', function() {
    return {
      scope: {
        id: '@',
        roles: '=',
        translate: '=',
        showMaximumRoles: '='
      },
      replace: true,
      templateUrl: 'templates/filter-roles.html'
    };
  });

  app.directive('opsiMainMenu', function() {
    return {
      link: function($scope, $element) {
        var icon, menu;
        icon = $element.children('.icon-menu');
        menu = $element.children('.menu-main');
        icon.on('click', function() {
          return $element.toggleClass('show-menu');
        });
      },
      restrict: 'C'
    };
  });

  app.directive('opsiUserDetail', function($fum) {
    return {
      controller: 'OpsiUserDetail',
      link: function($scope, $element) {
        var $$hackTheGoddamnScrollbar;
        $$hackTheGoddamnScrollbar = function() {
          var inner, outer, w1, w2;
          inner = document.createElement('p');
          outer = document.createElement('div');
          inner.style.width = '100%';
          inner.style.height = '200px';
          outer.style.position = 'absolute';
          outer.style.top = '0px';
          outer.style.left = '0px';
          outer.style.visibility = 'hidden';
          outer.style.width = '200px';
          outer.style.height = '150px';
          outer.style.overflow = 'hidden';
          outer.appendChild(inner);
          document.body.appendChild(outer);
          w1 = inner.offsetWidth;
          outer.style.overflow = 'scroll';
          w2 = inner.offsetWidth;
          if (w1 === w2) {
            w2 = outer.clientWidth;
          }
          document.body.removeChild(outer);
          return w1 - w2;
        };
        return angular.element($element).children().last().css('left', -1 * $$hackTheGoddamnScrollbar());
      },
      replace: true,
      restrict: 'E',
      scope: {
        css: '@'
      },
      templateUrl: "" + $fum.modulePath + "/ngapp/partials/user-detail.html"
    };
  });

  app.directive('opsiUserFilters', function() {
    return {
      controller: 'OpsiUserFilters',
      replace: true,
      scope: {
        id: '@',
        pageNumber: '=',
        pages: '=',
        orderby: '=',
        orderclause: '=',
        userCount: '='
      },
      templateUrl: 'templates/user-filters.html'
    };
  });

  app.directive('opsiUserTiles', function($fum) {
    return {
      controller: 'OpsiUserTiles',
      controllerAs: 'tiles',
      replace: true,
      restrict: 'E',
      templateUrl: "" + $fum.modulePath + "/ngapp/partials/user-tiles.html"
    };
  });

  $opsiDialogTemplate = '<section class="app-dialog" data-ng-class="{visible: dialog.config.show}"> <header data-ng-class="{danger: dialog.config.danger}"> <h2>{{dialog.config.title}}</h2> <span class="icon-cancel-circled" data-ng-click="dialog.abort()"></span> </header> <div data-ng-if="dialog.config.message"> <p>{{dialog.config.message}}</p> </div> <footer data-ng-show="dialog.config.confirm || dialog.config.abort"> <a class="button icon-check" data-ng-class="{danger: dialog.config.danger, active: !dialog.config.danger}" data-ng-show="dialog.config.confirm" data-ng-click="dialog.confirm()">confirm</a> <a class="button active icon-cancel" data-ng-show="dialog.config.abort" data-ng-click="dialog.abort()">abort</a> </footer> </section>';

  $opsiRolesTemplate = '<span> <span class="role checkbox-switch" data-ng-class="{on: role.checked == true}" data-ng-repeat="role in roles" data-ng-show="$index < limit"> <input data-ng-change="change(role)" id="filter-{{id}}-role-{{$index}}" type="checkbox" class="normalCheckBox" data-ng-init="role.checked = false" data-ng-model="role.checked" data-ng-true-value="true" data-ng-false-value="false" /> <label for="filter-{{id}}-role-{{$index}}"> <span>{{role.RoleName}}</span> </label> </span> </span>';

  //$opsiUserFiltersTemplate = '<div class="app-pager"> <div class="item"> <select class="page-filter" data-ng-model="orderby" data-ng-change="changePage()"> <option value="displayname" data-ng-bind="translate.order.displayname"></option> <option value="email" data-ng-bind="translate.order.email"></option> <option value="lastlogindate" data-ng-bind="translate.order.lastlogindate"></option> <option value="lastname" data-ng-bind="translate.order.lastname"></option> <option value="name" data-ng-bind="translate.order.name"></option> <option value="regdate" data-ng-bind="translate.order.regdate"></option> <option value="userid" data-ng-bind="translate.order.userid"></option> <option value="username" data-ng-bind="translate.order.username"></option> </select> <div class="page-filter"> <span data-ng-show="orderclause == \'ASC\'" data-ng-click="orderclause = \'DESC\'; changePage()"><i class="icon-down-circled"></i> Ascending</span> <span data-ng-show="orderclause == \'DESC\'" data-ng-click="orderclause = \'ASC\'; changePage()"><i class="icon-up-circled"></i> Descending</span> </div> </div> <div class="item"> <div class="show-type"> <span class="icon-layout"></span> <span class="icon-menu"></span> </div> </div> <div class="item users-count right"> <strong>{{userCount}}</strong> {{translate.registeredUsers}} - {{translate.browsingPage}} {{pageNumber}} {{translate.browsingPageOf}} {{pages.length}}. </div> <div class="item"> <input class="page-select" type="number" min="1" max="{{pages.length}}" data-ng-model="pageNumber" data-ng-change="changePage();" placeholder="Write page"> </div> </div>';

  $opsiUserFiltersTemplate = '<div class="app-pager"> <div class="item"> <select class="page-filter" data-ng-model="orderby" data-ng-change="changePage()"> <option value="displayname" data-ng-bind="translate.order.displayname"></option> <option value="email" data-ng-bind="translate.order.email"></option> <option value="lastlogindate" data-ng-bind="translate.order.lastlogindate"></option> <option value="lastname" data-ng-bind="translate.order.lastname"></option> <option value="name" data-ng-bind="translate.order.name"></option> <option value="regdate" data-ng-bind="translate.order.regdate"></option> <option value="userid" data-ng-bind="translate.order.userid"></option> <option value="username" data-ng-bind="translate.order.username"></option> </select> <div class="page-filter"> <span data-ng-show="orderclause == \'ASC\'" data-ng-click="orderclause = \'DESC\'; changePage()"><i class="icon-down-circled"></i> {{translate.order.Ascending}}</span> <span data-ng-show="orderclause == \'DESC\'" data-ng-click="orderclause = \'ASC\'; changePage()"><i class="icon-up-circled"></i> {{translate.order.Descending}}</span> </div> </div> <div class="item"> <div class="show-type"> <span class="icon-layout" data-ng-click="switchLayout(0);"  data-ng-model="typeView" data-ng-class="{ \'type-sel\' : typeView==0}"></span> <span class="icon-menu" data-ng-click="switchLayout(1)"   data-ng-model="typeView" data-ng-class="{ \'type-sel\' : typeView==1}"></span> </div> </div> <div class="item users-count right"> <strong>{{userCount}}</strong> {{translate.registeredUsers}} - {{translate.browsingPage}} {{pageNumber}} {{translate.browsingPageOf}} {{pages.length}}. </div> <div class="item"> <input class="page-select" type="number" min="1" max="{{pages.length}}" data-ng-model="pageNumber" data-ng-change="changePage();" placeholder="Write page"> </div> </div>'

  $appStatusTemplate = '<div class="app-error button center {{status.css}}" data-ng-class="{visible: status.visible}" data-ng-click="status.visible = false"> <strong> <span data-ng-bind="status.title"></span> <i class="icon-cancel"></i> </strong><br /> <span data-ng-bind="status.message"></span> </div>';

  $opsiFilterRolesTemplate = '<span> <opsi-roles id="id" data-roles="roles" data-limit="showMaximumRoles"></opsi-roles> <span> <a class="button active icon-plus" data-ng-show="roles.length > 5 && showMaximumRoles == 5" data-ng-click="showMaximumRoles = roles.length; watchFilters()" data-ng-bind="translate.moreRoles"></a> <a class="button active icon-minus" data-ng-show="showMaximumRoles > 5" data-ng-click="showMaximumRoles = 5" data-ng-bind="translate.lessRoles"></a> </span> </span>';

  app.run([
    '$templateCache', function($templateCache) {
      $templateCache.put('templates/app-status.html', $appStatusTemplate);
      $templateCache.put('templates/app-dialog.html', $opsiDialogTemplate);
      $templateCache.put('templates/filter-roles.html', $opsiFilterRolesTemplate);
      $templateCache.put('templates/opsi-roles.html', $opsiRolesTemplate);
      return $templateCache.put('templates/user-filters.html', $opsiUserFiltersTemplate);
    }
  ]);

}).call(this);
