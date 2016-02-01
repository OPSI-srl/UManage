(function() {
  'use strict';
  var FumProvider, angular, app;

  angular = this.angular;

  app = angular.module('opsiModule', ['opsiModule.controllers', 'opsiModule.directives', 'opsiModule.services']);

  app.factory('appData', function($fum, $window) {
    var modulePath;
    modulePath = $fum.moduleApi;
    return {
      pagination: {
        maxPerPage: 12
      },
      roles: {},
      webservice: {
        dangerZone: {
          deleted: "" + modulePath + "Users/Users_RemoveDeleted",
          unauth: "" + modulePath + "Users/Users_DeleteUnauthorized"
        },
        filters: "" + modulePath + "api/",
        userAuthorize: "" + modulePath + "Users/Users_Authorize?userid={{id}}&mode={{mode}}",
        userDelete: "" + modulePath + "Users/Users_Delete?userid={{id}}&mode={{mode}}",
        userList: "" + modulePath + "Users/Users_List?key={{name}}&roles={{roles}}&deleted={{deleted}}&unauth={{unauth}}&ResultsPerPage={{results}}&CurrentPage={{currentPage}}&orderby={{orderby}}&orderclause={{orderclause}}",
        userSingle: "" + modulePath + "Users/Users_Get?userid=",
        userSetRole: "" + modulePath + "Roles/Roles_AddRemove?userid={{id}}&role={{rolename}}&mode={{mode}}",
        userUpdate: "" + modulePath + "Users/Users_Update",
        roles: "" + modulePath + "Roles/Roles_List"
      }
    };
  });

  app.factory('translationData', function(appData, $http, $rootScope) {
    var translationObject;
    translationObject = {
      errors: {
        loadSingleUserError: 'Whoops. A small error occurred while retrieving user data. Please try again, or ask for support at support@opsi.it',
        newUserError: 'Error creating a user.',
        removeDeletedError: 'An error occurred while trying to remove deleted users',
        removeUnauthError: 'An error occurred while trying to remove unauthorized users',
        translationsLoadError: 'Error loading translations.',
        usersLoadError: 'Error loading users.'
      },
      menuVertical: {
        actions: 'Actions',
        authorized: 'Show only unauth users',
        confirmDeletionDeletedUser: 'This action will permanently delete all deleted users. You cannot get back.',
        confirmDeletionDeletedUserTitle: 'Really?',
        confirmDeletionUnauthUser: 'This action will permanently delete all unauthorized users. You cannot get back.',
        confirmDeletionUnauthUserTitle: 'Really?',
        dangerZone: 'Danger zone',
        dangerZoneRemoveDeleted: 'Remove deleted users',
        dangerZoneRemoveUnauthorized: 'Delete unauthorized users',
        deleted: 'Show only deleted users',
        email: 'Email',
        lessRoles: 'Show less roles',
        moreRoles: 'Show all roles',
        name: 'Name',
        newUser: 'Add new user',
        online: 'Show online users',
        reset: 'Reset filters',
        roles: 'Filter by DNN Roles',
        search: 'Search',
        status: 'Filter by User Status',
        title: 'Filters',
        unconfirmed: 'Unconfirmed users'
      },
      newUser: {
        email: 'User email address',
        password: 'Password',
        roles: 'Add roles to this user',
        username: 'Username'
      },
      userDetail: {
        authorize: 'Authorize user',
        confirm: {
          auth: 'You can unauthorize this user in a second moment.',
          authTitle: 'Authorizing this user.',
          reenable: 'You can remove this user in a second moment.',
          reenableTitle: 'Re-enabling this user.',
          remove: 'You can re-enable this user in a second moment.',
          removeTitle: 'Removing this user.',
          unauth: 'This can be undone by authorizing this user.',
          unauthTitle: 'Unauthorizing this user.'
        },
        deleteUser: 'Delete user',
        displayName: 'Display name',
        email: 'Email',
        emailResetPassword: 'Send reset password link',
        name: 'First name',
        newUserCreatedSuccess: 'You\'ve done! Great!',
        newUserCreatedTitle: 'User successfully created!',
        reenableUser: 'Re-enable user',
        saveNewUser: 'Create new user',
        security: 'User security',
        surname: 'Last name',
        unauthorize: 'Unauthorize user',
        updateUser: 'Update user profile',
        userInfo: 'User data',
        username: 'Username',
        userRoles: 'Role management',
        userUpdatedSuccess: 'You\'re awesome!',
        userUpdatedTitle: 'User updated successfully!'
      },
      userFilters: {
        browsingPage: 'Browsing page',
        browsingPageOf: 'of',
        fromPage: 'Viewing page',
        page: 'View page',
        registeredUsers: 'total users',
        toPage: 'of'
      },
      userTiles: {
        actionsAvailable: 'Actions available',
        creationDate: 'Creation date',
        dnnEdit: 'Full DNN edit',
        email: 'Email',
        fastEdit: 'Fast edit',
        lastAccess: 'Last access',
        noUsersFound: 'Sorry, it seems nobody\'s here.',
        phone: 'Phone'
      }
    };
    return {
      getData: function() {
        return translationObject;
      },
      loadData: function(locale) {
        if (!locale) {
          return;
        }
        return $http.get("" + appData.webservice.translations + "&locale=" + locale).error(function(data) {
          $rootScope.$broadcast('app-error', translationObject.errors.translationsLoadError);
        }).success(function(data) {
          translationObject = data;
        });
      },
      setData: function(data) {
        translationObject = angular.extend(translationObject, data);
        return this;
      }
    };
  });

  FumProvider = (function() {
    var config;

    config = {};

    function FumProvider() {}

    FumProvider.prototype.$get = function() {
      return config;
    };

    FumProvider.prototype.setProperty = function(name, value) {
      return config[name] = value;
    };

    return FumProvider;

  })();

  app.provider('$fum', FumProvider);

}).call(this);
