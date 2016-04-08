(function () {
    'use strict';
    var $AppErrorController, $MenuVertical, $OpsiDialog, $OpsiUserDetail, $OpsiUserFilters, $OpsiUserTiles, angular, app;

    angular = this.angular;

    app = angular.module('opsiModule.controllers', []);

    $AppErrorController = function ($scope) {
        var $self;
        $self = this;
        $self.message = '';
        $self.title = '';
        $self.visible = false;
        return $scope.$on('app-status', function (event, isError, title, message) {
            $self.css = isError === true ? 'danger' : 'safe';
            $self.message = message && message.length ? message : '';
            $self.title = title && title.length ? title : '';
            return $self.visible = true;
        });
    };

    $MenuVertical = function (appData, translationData, $http, $scope, $timeout) {
        var $rootscope, Model;
        $rootscope = $scope.$root;
        Model = function () {
            return {
                filters: {
                    currentPage: 0,
                    deleted: false,
                    name: '',
                    online: false,
                    results: appData.pagination.maxPerPage,
                    roles: [],
                    uconf: false,
                    unauth: false,
                    orderby: 'displayname',
                    orderclause: 'ASC'
                },
                roles: [],
                sort: {}
            };
        };
        $scope.model = new Model;
        $scope.collapsed = true;
        $scope.translate = translationData.getData().menuVertical;
        $scope.collapseMenu = function () {
            return $scope.collapsed = !$scope.collapsed;
        };
        $scope.newUser = function () {
            return $rootscope.$broadcast('app-open-detail', {
                UserID: -1
            });
        };
        $scope.postFilters = function () {
            return $timeout(function () {
                var filters;
                filters = angular.copy($scope.model.filters);
                filters.roles = [];
                if (!/^[0-9]+$/g.test(filters.results)) filters.results = appData.pagination.maxPerPage;
                angular.forEach($scope.roles, function (role) {
                    if (role.checked) {
                        return filters.roles.push(role.Id);
                    }
                });
                return $rootscope.$broadcast('app-load-users', filters);
            }, 10);
        };
        $scope.removeUnauth = function () {
            return $rootscope.$broadcast('app-dialog', {
                abort: function () { },
                confirm: function () {
                    return $http.get(appData.webservice.dangerZone.unauth).error(function () {
                        return $rootscope.$broadcast('app-status', true, translations.errors.removeUnauthError, error);
                    }).success(function () {
                        return $scope.postFilters();
                    });
                },
                danger: true,
                message: $scope.translate.confirmDeletionUnauthUser,
                title: $scope.translate.confirmDeletionUnauthUserTitle
            });
        };
        $scope.removeDeleted = function () {
            return $rootscope.$broadcast('app-dialog', {
                abort: function () { },
                confirm: function () {
                    return $http.get(appData.webservice.dangerZone.deleted).error(function () {
                        return $rootscope.$broadcast('app-status', true, translations.errors.removeDeletedError, error);
                    }).success(function () {
                        return $scope.postFilters();
                    });
                },
                danger: true,
                message: $scope.translate.confirmDeletionDeletedUser,
                title: $scope.translate.confirmDeletionDeletedUserTitle
            });
        };
        $scope.resetFilters = function () {
            $scope.form_vertical_menu.$setPristine();
            $scope.model = new Model;
            angular.forEach($scope.roles, function (role) {
                return role.checked = false;
            });
            return $scope.postFilters();
        };
        $http.get(appData.webservice.roles).success(function (data) {
            $scope.model.roles = data.Roles;
            $scope.roles = data.Roles;
            if (!$scope.alreadyWatching) {
                $timeout(function () {
                    return $scope.watchFilters();
                }, 0);
            }
            $scope.alreadyWatching = true;
            return $scope.postFilters();
        });
        $scope.$on('apply-user-filters', function () {
            return $scope.postFilters();
        });
    };

    $OpsiDialog = function (translationData, $scope) {
        var $self;
        $self = this;
        $self.config = {
            show: false
        };
        $self.abort = function () {
            $self.close();
            if (typeof $self.config.abort !== 'function') {
                return;
            }
            return $self.config.abort();
        };
        $self.confirm = function () {
            $self.close();
            if (typeof $self.config.confirm !== 'function') {
                return;
            }
            return $self.config.confirm();
        };
        $self.close = function () {
            $self.config.show = false;
            return $scope.$root.$broadcast('hide-modal');
        };
        return $scope.$on('app-dialog', function (event, config) {
            // console.log(config);
            $self.config = config;
            $self.config.show = config ? true : false;
            if ($self.config.show) {
                return $scope.$root.$broadcast('show-modal');
            }
        });
    };

    $OpsiUserDetail = function (appData, translationData, $scope, $http, $interpolate, $window) {
        var $rootscope, Model;
        Model = function () {
            this.DisplayName = '';
            this.Email = '';
            this.FirstName = '';
            this.LastName = '';
            this.Password = '';
            this.PasswordConfirm = '';
            this.UserID = -1;
            return this.Username = '';
        };
        $rootscope = $scope.$root;
        $scope.isNew = false;
        $scope.user = new Model;
        $scope.selectedTab = 0;
        $scope.translate = translationData.getData().userDetail;
        $scope.visible = false;
        $scope.assignRoles = function () {
            return angular.forEach($scope.roles, function ($$role) {
                var $$set;
                $$set = false;
                return angular.forEach($scope.user.Roles, function ($$roleName) {
                    if ($$role.RoleName === $$roleName && !$$set) {
                        $$role.checked = true;
                        return $$set = true;
                    }
                });
            });
        };
        $scope.change = function (role) {
            var $$webservice;
            $$webservice = $interpolate(appData.webservice.userSetRole);
            $$webservice = $$webservice({
                id: $scope.user.UserID,
                rolename: role.RoleName,
                mode: role.checked ? 'add' : 'remove'
            });
            return $http.get($$webservice).error(function (data) { }).success(function (data) {
                $rootscope.$broadcast('apply-user-filters');
            });
        };
        $scope.close = function () {
            $scope.visible = false;
            return $rootscope.$broadcast('hide-modal');
        };
        $scope.deleteUser = function (userID, mode) {
            var $$webservice;
            $$webservice = $interpolate(appData.webservice.userDelete);
            $$webservice = $$webservice({
                id: userID,
                mode: mode
            });
            return $rootscope.$broadcast('app-dialog', {
                abort: function () {
                    return $rootscope.$broadcast('show-modal');
                },
                confirm: function () {
                    return $http.get($$webservice).error(function () {
                        return $rootscope.$broadcast('app-status', true, translations.errors.removeDeletedError, error);
                    }).success(function () {
                        $scope.close();
                        return $rootscope.$broadcast('apply-user-filters');
                    });
                },
                danger: mode !== 'restore',
                message: mode === 'restore' ? $scope.translate.confirm.reenable : $scope.translate.confirm.remove,
                title: mode === 'restore' ? $scope.translate.confirm.reenableTitle : $scope.translate.confirm.removeTitle
            });
        };
        $scope.reset = function () {
            $scope.form_user_detail.$setPristine();
            $scope.user = new Model;
            return angular.forEach($scope.roles, function ($$role) {
                return $$role.checked = false;
            });
        };
        $scope.resetPassword = function () {
            $http.get(appData.webservice.userResetPassword + $scope.user.UserID).success(function (data) {
                return $rootscope.$broadcast('app-status', false, $scope.translate.passwordResetOkTitle, $scope.translate.passwordResetOk);
            }).error(function (error) {
                return $rootscope.$broadcast('app-status', true, $scope.translate.passwordResetOkTitle, $scope.translate.passwordResetOk);
            });
        };
        $scope.unauthUser = function (userID, mode) {
            var $$webservice;
            $$webservice = $interpolate(appData.webservice.userAuthorize);
            $$webservice = $$webservice({
                id: userID,
                mode: mode
            });
            return $rootscope.$broadcast('app-dialog', {
                abort: function () {
                    return $rootscope.$broadcast('show-modal');
                },
                confirm: function () {
                    return $http.get($$webservice).error(function () {
                        return $rootscope.$broadcast('app-status', true, translations.errors.removeDeletedError, error);
                    }).success(function () {
                        $scope.close();
                        return $rootscope.$broadcast('apply-user-filters');
                    });
                },
                danger: mode !== 'authorize',
                message: mode === 'authorize' ? $scope.translate.confirm.auth : $scope.translate.confirm.unauth,
                title: mode === 'authorize' ? $scope.translate.confirm.authTitle : $scope.translate.confirm.unauthTitle
            });
        };
        $scope.update = function () {
            var isvalid = true;
            var $model;
            $model = {};
            $model.DisplayName = $scope.user.DisplayName;
            $model.Email = $scope.user.Email;
            $model.FirstName = $scope.user.FirstName;
            $model.LastName = $scope.user.LastName;
            $model.Password = $scope.user.password;
            $model.Telephone = $scope.user.Telephone;
            $model.UserID = $scope.user.UserID;
            $model.Username = $scope.user.Username;
            angular.forEach($scope.form_user_detail.$error, function (errorType) {
                angular.forEach(errorType, function (errorItem) {
                    isvalid = false;
                    errorItem.$setDirty()
                });
            });
            console.log(translationData.getData().errors, translationData.getData().errors.passwordNotLongEnough)
            if (!isvalid) return;
            if ($model.Password.length < 7) return $rootscope.$broadcast('app-status', true, translationData.getData().errors.passwordNotLongEnough, translationData.getData().errors.passwordNotLongEnough);
            return $http.post(appData.webservice.userUpdate, $model).error(function (error) {
                return $rootscope.$broadcast('app-status', true, translationData.getData().errors.newUserError, error);
            }).success(function (data) {
                if (data.UserID === -1) {
                    $rootscope.$broadcast('app-status', true, translationData.getData().errors.newUserError, data.errorMessage);
                    return;
                }
                if ($scope.isNew) {
                    $rootscope.$broadcast('app-open-detail', data);
                    $rootscope.$broadcast('app-status', false, translationData.getData().userDetail.newUserCreatedTitle, translationData.getData().userDetail.newUserCreatedSuccess);
                } else {
                    $rootscope.$broadcast('app-status', false, translationData.getData().userDetail.userUpdatedTitle, translationData.getData().userDetail.userUpdatedSuccess);
                }
                return $rootscope.$broadcast('apply-user-filters');
            });
        };
        $scope.$on('app-open-detail', function (event, user) {
            if (user.UserID === -1 || user.UserID === void 0) {
                $rootscope.$broadcast('show-modal');
                $scope.isNew = user.UserID === -1;
                $scope.reset();
                $scope.selectedTab = 0;
                $scope.visible = true;
                return;
            }
            return $http.get("" + appData.webservice.userSingle + user.UserID).error(function (error) {
                return $rootscope.$broadcast('app-status', true, translationData.getData().errors.loadSingleUserError, error);
            }).success(function (data) {
                $scope.isNew = false;
                $scope.reset();
                $scope.user = data && data.user ? data.user : {};
                $scope.visible = data && data.user ? true : false;
                if ($scope.visible) {
                    $rootscope.$broadcast('show-modal');
                }
                $scope.assignRoles();
                $scope.selectedTab = 0;
                user.edit = false;
            });
        });
        $http.get(appData.webservice.roles).success(function (data) {
            return $scope.roles = data.Roles;
        });
    };

    $OpsiUserFilters = function (appData, translationData, $scope, $rootScope) {
        $scope.translate = translationData.getData().userFilters;
        $scope.orderby = 'displayname';
        $scope.orderclause = 'ASC';
        $scope.typeView = 0;

        $scope.changePage = function () {
            if (!$scope.pageNumber) {
                return;
            }
            if (/\D/.test($scope.pageNumber.toString())) {
                return;
            }
            if ($scope.pageNumber > $scope.pages.length) {
                return;
            }
            return $scope.$root.$broadcast('app-load-users', {
                currentPage: $scope.pageNumber - 1,
                orderby: $scope.orderby,
                orderclause: $scope.orderclause
            });
        };

        $scope.switchLayout = function (type) {
            $scope.typeView = type;
            this.typeView = type;
            return $rootScope.$broadcast('app-switch-layout', $scope.typeView);
        };
    };

    $OpsiUserTiles = function (appData, translationData, $fum, $interpolate, $scope, $http, $window, $location, $q) {
        var $rootscope, $self, defer, promise, translations;
        defer = $q.defer();
        promise = defer.promise;
        translations = translationData.getData();
        $self = this;
        $rootscope = $scope.$root;
        $self.isLoading = false;
        $self.limit = appData.pagination.maxPerPage;
        $self.model = {
            currentPage: 1
        };
        $self.pages = [];
        $self.users = [];
        $self.picPath = $fum.profilePicPath;
        $self.searchFilters = {};
        $self.sudo = parseInt($fum.sudo) === 1;
        $self.translate = translations.userTiles;
        $self.usersCount = 0;
        $self.typeView = 0;

        $self.exportUsers = function () {
            $window.open($fum.modulePath +'/ExcelExport.aspx?pid=' + $fum.portalID + '&rpp=9999&cp=0' + '&key=' + $scope.model.filters.name + '&roles=' + $scope.model.filters.roles.join() + '&deleted=' + $scope.model.filters.deleted + '&unauth=' + $scope.model.filters.unauth + '&orderby=' + $scope.model.filters.orderby + '&orderclause=' + $scope.model.filters.orderclause);
        }
        $self.fullEdit = function (user) {
            var url;
            url = $interpolate($fum.fullEditPath);
            url = url({
                userid: user.UserID,
                location: $window.location.host,
                tabid: $fum.tabID
            });
            return $window.dnnModal.show(url, false, 550, 950, false, '');
        };
        $self.loadUsers = function (filters) {
            var interpolate;
            filters.roles = filters && filters.roles ? filters.roles.join() : '';
            interpolate = $interpolate(appData.webservice.userList);
            defer.reject('Asked for a new promise');
            $self.isLoading = true;
            return $http.get(interpolate(filters), {
                timeout: promise
            }).error(function (error) {
                return $rootscope.$broadcast('app-status', true, translations.errors.usersLoadError, error);
            }).success(function (data) {
                var _i, _ref, _results;
                $self.usersCount = data.Users[0] ? data.Users[0].TotalRows : 0;
                $self.pages = data.Users[0] ? (function () {
                    _results = [];
                    for (var _i = 0, _ref = data.Users[0].Pages; 0 <= _ref ? _i < _ref : _i > _ref; 0 <= _ref ? _i++ : _i--) { _results.push(_i); }
                    return _results;
                }).apply(this) : [0];
                $self.cacheUsers = data.Users;
                $self.users = data.Users;
                return defer.notify('foo');
            }).then(function () {
                defer.resolve();
                return promise["finally"](function () {
                    $self.isLoading = false;
                    if ($self.pages.length < 2) {
                        return $self.model.currentPage = 1;
                    }
                });
            });
        };
        $self.openDetail = function (user) {
            return $rootscope.$broadcast('app-open-detail', user);
        };
        $self.impersonate = function (user) {
            var gotoUrl = angular.element(document.querySelector("#VAR_PageBase")).val() + "/iu/" + user.UserID;
            $window.location.href = gotoUrl;
        };

        $scope.$on('app-switch-layout', function (event, type) {
            $self.typeView = type;
        });
        $scope.$on('app-load-users', function (event, filters) {
            $self.searchFilters = angular.extend($self.searchFilters, filters);
            return $self.loadUsers($self.searchFilters);
        });
    };

    app.controller('AppErrorController', ['$scope', $AppErrorController]);

    app.controller('OpsiDialog', ['translationData', '$scope', $OpsiDialog]);

    app.controller('OpsiUserDetail', ['appData', 'translationData', '$scope', '$http', '$interpolate', '$window', $OpsiUserDetail]);

    app.controller('OpsiUserFilters', ['appData', 'translationData', '$scope', '$rootScope', $OpsiUserFilters]);

    app.controller('OpsiUserTiles', ['appData', 'translationData', '$fum', '$interpolate', '$scope', '$http', '$window', '$location', '$q', $OpsiUserTiles]);

    app.controller('MenuVertical', ['appData', 'translationData', '$http', '$scope', '$timeout', $MenuVertical]);

}).call(this);
