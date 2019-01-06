
var userModule = angular.module("userModule", ['sharedModule', 'sharedModels', 'userModels', 'ngRoute', 'ui.bootstrap', 'ui.bootstrap.tpls', 'customFilters']);

userModule.config(["$routeProvider", "globals", function ($routeProvider, globals)
{
    $routeProvider.when("/", {
        controller: "userListController",
        templateUrl: globals.getRootUrl() + "/Angular/Views/User/ListView.html"
    });

    $routeProvider.when("/Create", {
        controller: "userAddEditController",
        templateUrl: globals.getRootUrl() + "/Angular/Views/User/UserView.html"
    });

    $routeProvider.when("/Edit/:id", {
        controller: "userAddEditController",
        templateUrl: globals.getRootUrl() + "/Angular/Views/User/UserView.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });
}]);

userModule.factory("userService", ["$http", "$q", '$modal', "sharedService", 'CUser', 'globals', function ($http, $q, $modal, sharedService, CUser, globals)
{
    var _users = [];
    var _isInit = false;

    var _isReady = function ()
    {
        return _isInit;
    }

    var _isUserNameMatch = function (user, pattern)
    {
        if (pattern)
        {
            var regex = new RegExp(pattern, 'gi');

            var isMatch = user.fnm && user.fnm.match(regex);

            if (!isMatch && user.lnm)
            {
                isMatch = user.lnm.match(regex);
            }

            if (!isMatch && user.mnm)
            {
                isMatch = user.mnm.match(regex);
            }

            return isMatch;
        }

        return true;
    };

    var _getUsers = function ()
    {

        var deferred = $q.defer();

        $http.get(globals.getRootUrl() + "/api/User/GetAll")
          .then(function (result)
          {
              result.data = sharedService.buildCustomModel(result.data, CUser);
              angular.copy(result.data, _users);
              _isInit = true;
              deferred.resolve();
          },
           function (error)
           {
               // error
               deferred.reject(error.data);
           });

        return deferred.promise;
    };

    var _createUser = function (newUser)
    {
        var deferred = $q.defer();

        $http.post(globals.getRootUrl() + "/api/User/Create", newUser)
         .then(function (result)
         {
             result.data = sharedService.buildCustomModel(result.data, CUser);
             var newlyCreatedUser = result.data;
             _users.splice(0, 0, newlyCreatedUser);
             deferred.resolve(newlyCreatedUser);
         },
          function (error)
          {
              // error
              deferred.reject(error.data);
          });

        return deferred.promise;
    };

    function _findUser(id)
    {
        var found = null;

        $.each(_users, function (i, item)
        {
            if (item.id == id)
            {
                found = item;
                return false;
            }
        });

        return found;
    }

    var _getUserById = function (id)
    {
        var deferred = $q.defer();

        if (_isReady())
        {
            var user = _findUser(id);
            if (user)
            {
                deferred.resolve(user);
            } else
            {
                deferred.reject();
            }
        }
        else
        {
            _getUsers()
              .then(function ()
              {
                  // success
                  var user = _findUser(id);
                  if (user)
                  {
                      deferred.resolve(user);
                  } else
                  {
                      deferred.reject();
                  }
              },
              function (error)
              {
                  // error
                  deferred.reject(error.data);
              });
        }

        return deferred.promise;
    };

    var _deleteUser = function (user)
    {
        var deferred = $q.defer();

        $http.post(globals.getRootUrl() + "/api/User/Delete", user)
          .then(function (result)
          {
              if (result.data && result.data == true)
              {
                  var index = _users.indexOf(user);

                  if (index >= 0)
                  {
                      _users.splice(index, 1);
                  }

                  deferred.resolve(user);
              }
              else
              {
                  deferred.reject('Unknown error');
              }
          },
           function (error)
           {
               // error
               deferred.reject(error.data);
           });

        return deferred.promise;
    }

    var _saveUser = function (user)
    {
        var deferred = $q.defer();

        $http.post(globals.getRootUrl() + "/api/User/Edit", user)
          .then(function (result)
          {
              result.data = sharedService.buildCustomModel(result.data, CUser);
              deferred.resolve(result.data);
          },
           function (error)
           {
               // error
               deferred.reject(error.data);
           });

        return deferred.promise;
    };

    var _showSelectUsersUI = function (selectedUsers)
    {
        var defer = $q.defer();

        var modalInstance = $modal.open({
            animation: false,
            size: "lg",
            templateUrl: globals.getRootUrl() + '/Angular/Views/User/SelectView.html',
            controller: 'userSelectController',
            resolve:
                {
                    defer: function () { return defer; },
                    selectedUsers: function () { return selectedUsers; }
                }
        });

        return defer.promise;
    }

    return {
        users: _users,
        isUserNameMatch: _isUserNameMatch,
        getUsers: _getUsers,
        createUser: _createUser,
        isReady: _isReady,
        getUserById: _getUserById,
        saveUser: _saveUser,
        deleteUser: _deleteUser,
        showSelectUsersUI: _showSelectUsersUI
    };
}]);

userModule.controller('userListController', ["$scope", "$http", "userService", "sharedService",
  function ($scope, $http, userService, sharedService)
  {
      $scope.data = userService;
      $scope.isBusy = false;

      if (userService.isReady() == false)
      {
          $scope.isBusy = true;

          userService.getUsers()
            .then(function ()
            {

                // success
            },
            function (error)
            {
                $scope.errorString = error;
                $scope.showError = true;
            })
            .then(function ()
            {
                $scope.isBusy = false;
            });
      }

      $scope.deleteUserByIndex = function (index)
      {
          var user = $scope.data.users[index];

          sharedService.showConfirmDialog(
               'Confirm Deletion!',
               'Are you sure to delete user <b>' + user.fnm + ' ' + user.lnm + '</b>?')
               .then(function ()
               {
                   userService.deleteUser(user)
                    .then(function (user)
                    {
                        // success
                    },
                    function (error)
                    {
                        sharedService.showConfirmDialog(
                       'Warning!',
                       'Unable to delete user <b>' + user.fnm + ' ' + user.lnm + '<b><br/>Details: ' + error);
                    });
               },
               function ()
               {
               });
      }

      $scope.nmeFilter = undefined;

      $scope.customUserFilter = function (user)
      {
          return userService.isUserNameMatch(user, $scope.nmeFilter);
      };

      $scope.closeError = function ()
      {
          $scope.showError = false;
      };
  }]);

userModule.controller('userAddEditController',
    ["$scope", "$http", "userService", "sharedService", "$window", "$routeParams", 'CUser', 'CDocument',
function ($scope, $http, userService, sharedService, $window, $routeParams, CUser, CDocument)
{
    $scope.phoneNumberRegx = /^\+?[\d- ]+\d$/;
    $scope.orgUser = sharedService.buildCustomModel({}, CUser);
    $scope.user = sharedService.buildCustomModel({}, CUser);
    $scope.userId = $routeParams.id;
    $scope.isEdit = $scope.userId != undefined;
    $scope.showMessage = false;
    $scope.showError = false;

    $scope.$on('image-image-guid', function (event, data)
    {
        var thumb = sharedService.buildCustomModel(data, CDocument);
        $scope.user.thmb = thumb;
    });

    if ($scope.isEdit)
    {
        userService.getUserById($routeParams.id)
          .then(function (user)
          {
              $scope.orgUser = user;
              // success
              $scope.user = angular.copy(user);
          },
          function ()
          {
              // error
              $window.location = "#/";
          });
    }

    $scope.saveUser = function ()
    {
        if ($scope.isEdit)
        {
            userService.saveUser($scope.user)
              .then(function (user)
              {
                  angular.copy(user, $scope.orgUser);
                  // success
                  $scope.user = user;

                  $scope.showMessage = true;
              },
              function ()
              {
                  $scope.user = angular.copy($scope.orgUser);
                  $scope.showError = true;
              });
        }
        else
        {
            userService.createUser($scope.user).then(function (user)
            {
                $scope.orgUser = user;
                $scope.user = angular.copy(user);

                $scope.isEdit = true;
                // success
                $window.location = "#/";
            },
              function ()
              {
                  $scope.showError = true;
              });
        }

    };

    $scope.closeMessage = function ()
    {
        $scope.showMessage = false;
    }

    $scope.closeError = function ()
    {
        $scope.showError = false;
    }

    $scope.back = function ()
    {
        if ($scope.userForm.$dirty)
        {
            sharedService.showConfirmDialog(
                'Confirm!',
                'Any unsaved edit will be discarded. Are you sure to navigate back?')
                .then(function ()
                {
                    location.href = '#/';
                },
                function ()
                {
                });
        }
        else
        {
            location.href = '#/';
        }
    };

    $scope.calendarPopupOpen = function ($event)
    {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.isCalendarOpened = true;
    };
}]);

userModule.controller('userSelectController', ["$scope", "$http", '$filter', "userService", "$modalInstance", "defer", "selectedUsers",
  function ($scope, $http, $filter, userService, $modalInstance, defer, selectedUsers)
  {
      $scope.data = userService;
      $scope.isBusy = false;

      if (userService.isReady() == false)
      {
          $scope.isBusy = true;

          userService.getUsers()
            .then(function ()
            {

                // success
            },
            function (error)
            {
                $scope.errorString = error;
                $scope.showError = true;
            })
            .then(function ()
            {
                $scope.isBusy = false;
            });
      }
      else
      {
          angular.forEach($scope.data.users, function (usr)
          {
              usr.sel = false;
          })
      }

      $scope.nmeFilter = undefined;

      $scope.customUserFilter = function (user)
      {
          return userService.isUserNameMatch(user, $scope.nmeFilter);
      };

      $scope.closeError = function ()
      {
          $scope.showError = false;
      };

      /* Acting as a user search control - Start*/

      $scope.ok = function ()
      {
          var selUsers = [];
          angular.copy($filter('filter')($scope.data.users, { sel: true }), selUsers);

          $modalInstance.close();
          defer.resolve(selUsers);
      };

      $scope.cancel = function ()
      {
          $modalInstance.dismiss();
          defer.reject();
      };
      /* Acting as a user search control - End*/
  }]);



