
var policyModule = angular.module("policyModule", ['sharedModule', 'userModule', 'sharedModels', 'userModels', 'policyModels', 'ngRoute', 'ui.bootstrap', 'ui.bootstrap.tpls', 'customFilters']);

policyModule.config(["$routeProvider", 'globals', function ($routeProvider, globals)
{
    $routeProvider.when("/", {
        controller: "policyListController",
        templateUrl: globals.getRootUrl() + "/Angular/Views/Policy/ListView.html"
    });

    $routeProvider.when("/Create", {
        controller: "policyAddEditController",
        templateUrl: globals.getRootUrl() + "/Angular/Views/Policy/PolicyView.html"
    });

    $routeProvider.when("/Edit/:id", {
        controller: "policyAddEditController",
        templateUrl: globals.getRootUrl() + "/Angular/Views/Policy/PolicyView.html"
    });

    $routeProvider.otherwise({ redirectTo: "/" });
}]);

policyModule.factory("policyService", ["$http", "$q", "sharedService", 'userService', 'CPolicy', 'globals', function ($http, $q, sharedService, userService, CPolicy, globals)
{

    var _policies = [];
    var _isInit = false;

    var _isReady = function ()
    {
        return _isInit;
    }

    var _isPolicyNameMatch = function (policy, pattern)
    {
        if (pattern)
        {
            var regex = new RegExp(pattern, 'gi');

            var isMatch = policy.nme && policy.nme.match(regex);

            if (!isMatch && policy.num)
            {
                regex = new RegExp('^' + pattern, 'gi');
                isMatch = policy.num.match(regex);
            }

            return isMatch;
        }

        return true;
    };

    var _isPolicyDOEMatch = function (policy, selDate)
    {
        if (policy && selDate)
        {
            var startDate = new Date();
            var endDate = selDate;

            startDate = new Date(startDate.getFullYear(), startDate.getMonth(), startDate.getDate());
            endDate = new Date(endDate.getFullYear(), endDate.getMonth(), endDate.getDate());
            var doe = new Date(policy.doe.getFullYear(), policy.doe.getMonth(), policy.doe.getDate());

            if(startDate > endDate)
            {
                var tmpDte = startDate;
                startDate = endDate;
                endDate = tmpDte;
            }

            return startDate <= doe && doe <= endDate;
        }

        return true;
    };

    var _isPolicyUserNameMatch = function (policy, pattern)
    {
        if (pattern && policy)
        {
            return userService.isUserNameMatch(policy.usr, pattern);
        }

        return true;
    };

    var _getPolicies = function ()
    {

        var deferred = $q.defer();

        $http.get(globals.getRootUrl() + "/api/Policy/GetAll")
          .then(function (result)
          {
              result.data = sharedService.buildCustomModel(result.data, CPolicy);
              angular.copy(result.data, _policies);
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

    var _createPolicy = function (newPolicy)
    {
        var deferred = $q.defer();

        $http.post(globals.getRootUrl() + "/api/Policy/Create", newPolicy)
         .then(function (result)
         {
             result.data = sharedService.buildCustomModel(result.data, CPolicy);
             var newlyCreatedPolicy = result.data;
             _policies.splice(0, 0, newlyCreatedPolicy);
             deferred.resolve(newlyCreatedPolicy);
         },
          function (error)
          {
              // error
              deferred.reject(error.data);
          });

        return deferred.promise;
    };

    function _findPolicy(id)
    {
        var found = null;

        $.each(_policies, function (i, item)
        {
            if (item.id == id)
            {
                found = item;
                return false;
            }
        });

        return found;
    }

    var _getPolicyById = function (id)
    {
        var deferred = $q.defer();

        if (_isReady())
        {
            var policy = _findPolicy(id);
            if (policy)
            {
                deferred.resolve(policy);
            } else
            {
                deferred.reject();
            }
        }
        else
        {
            _getPolicies()
              .then(function ()
              {
                  // success
                  var policy = _findPolicy(id);
                  if (policy)
                  {
                      deferred.resolve(policy);
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

    var _deletePolicy = function (policy)
    {
        var deferred = $q.defer();

        $http.post(globals.getRootUrl() + "/api/Policy/Delete", policy)
          .then(function (result)
          {
              if (result.data && result.data == true)
              {
                  var index = _policies.indexOf(policy);

                  if (index >= 0)
                  {
                      _policies.splice(index, 1);
                  }

                  deferred.resolve(policy);
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

    var _savePolicy = function (policy)
    {
        var deferred = $q.defer();

        $http.post(globals.getRootUrl() + "/api/Policy/Edit", policy)
          .then(function (result)
          {
              result.data = sharedService.buildCustomModel(result.data, CPolicy);
              deferred.resolve(result.data);
          },
           function (error)
           {
               // error
               deferred.reject(error.data);
           });

        return deferred.promise;
    };

    return {
        policies: _policies,
        isPolicyNameMatch: _isPolicyNameMatch,
        isPolicyUserNameMatch: _isPolicyUserNameMatch,
        isPolicyDOEMatch: _isPolicyDOEMatch,
        getPolicies: _getPolicies,
        createPolicy: _createPolicy,
        deletePolicy: _deletePolicy,
        isReady: _isReady,
        getPolicyById: _getPolicyById,
        savePolicy: _savePolicy
    };
}]);

policyModule.controller('policyListController', ["$scope", "$http", "policyService", 'sharedService',
  function ($scope, $http, policyService, sharedService)
  {
      $scope.data = policyService;
      $scope.isBusy = false;

      if (policyService.isReady() == false)
      {
          $scope.isBusy = true;

          policyService.getPolicies()
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

      $scope.deletePolicyByIndex = function (index)
      {
          var policy = $scope.data.policies[index];

          sharedService.showConfirmDialog(
               'Confirm Deletion!',
               'Are you sure to delete the policy <b>' + policy.nme + ' ' + policy.num + '</b>?')
               .then(function ()
               {
                   policyService.deletePolicy(policy)
                    .then(function (policy)
                    {
                        // success
                    },
                    function (error)
                    {
                        sharedService.showConfirmDialog(
                       'Warning!',
                       'Unable to delete policy <b>' + policy.nme + ' ' + policy.num + '<b><br/>Details: ' + error);
                    });
               },
               function ()
               {
               });
      }

      $scope.doeFilter = undefined;
      $scope.usrNmeFilter = undefined;
      $scope.plcyNmeFilter = undefined;

      $scope.customPolicyFilter = function (policy)
      {
          return (policyService.isPolicyDOEMatch(policy, $scope.doeFilter) &&
          policyService.isPolicyUserNameMatch(policy, $scope.usrNmeFilter) &&
          policyService.isPolicyNameMatch(policy, $scope.plcyNmeFilter));
      };

      $scope.calendarPopupOpen = function ($event)
      {
          $event.preventDefault();
          $event.stopPropagation();
          $scope.isCalendarOpened = true;
      };

      $scope.closeError = function ()
      {
          $scope.showError = false;
      };

  }]);

policyModule.controller('policyAddEditController',
    ["$scope", "$http", '$filter', "policyService", "sharedService", 'userService', "$window", "$routeParams", 'CPolicy', 'CDependent','CDocument',
function ($scope, $http, $filter, policyService, sharedService, userService, $window, $routeParams, CPolicy, CDependent, CDocument)
{
    $scope.phoneNumberRegx = /^\+?[\d- ]+\d$/;
    $scope.orgPolicy = sharedService.buildCustomModel({}, CPolicy);
    $scope.policy = sharedService.buildCustomModel({}, CPolicy);
    $scope.policyId = $routeParams.id;
    $scope.isEdit = $scope.policyId != undefined;
    $scope.showMessage = false;
    $scope.showError = false;

    $scope.$on('document-document-guid', function (event, data)
    {
        var doc = sharedService.buildCustomModel(data, CDocument);
        $scope.policy.docs.push(doc);
    });

    if ($scope.isEdit)
    {
        policyService.getPolicyById($routeParams.id)
          .then(function (policy)
          {
              $scope.orgPolicy = policy;
              // success
              $scope.policy = angular.copy(policy);
          },
          function ()
          {
              // error
              $window.location = "#/";
          });
    }

    $scope.savePolicy = function ()
    {
        var selfCount = 0, spouseCount = 0, fatherCount = 0, motherCount = 0, fatherInLawCount = 0, motherInLawCount = 0;

        angular.forEach($scope.policy.deps, function (dep)
        {
            switch (dep.rln)
            {
                case CDependent.Self:
                    ++selfCount;
                    return;

                case CDependent.Spouse:
                    ++spouseCount;
                    return;

                case CDependent.Father:
                    ++fatherCount;
                    return;

                case CDependent.Mother:
                    ++motherCount;
                    return;

                case CDependent.FatherInLaw:
                    ++fatherInLawCount;
                    return;

                case CDependent.MotherInLaw:
                    ++motherInLawCount;
                    return;
            }
        });

        if (selfCount != 1 || spouseCount > 1 || fatherCount > 1 || motherCount > 1 || fatherInLawCount > 1 || motherInLawCount > 1)
        {
            sharedService.showConfirmDialog(
                'Dependents Validations!',
                '<b>Please check the following:</b><br/>'+
                '1. There must be exactly one insuree (self). <br/>'+
                '2. There should not be more than one spouse. <br/>'+
                '3. There should not be more than one father or in law. <br/>'+
                '4. There should not be more than one mother or in law. <br/>'
                ,'md');

            return;
        }


        if ($scope.isEdit)
        {
            policyService.savePolicy($scope.policy)
              .then(function (policy)
              {
                  angular.copy(policy, $scope.orgPolicy);
                  // success
                  $scope.policy = policy;

                  $scope.showMessage = true;
              },
              function ()
              {
                  $scope.policy = angular.copy($scope.orgPolicy);

                  $scope.errorString =
                      "Unable to save policy '" + $scope.policy.num +
                      + '-' + $scope.policy.nme + "'. Please try again";

                  $scope.showError = true;
              });
        }
        else
        {
            policyService.createPolicy($scope.policy).then(function (policy)
            {
                $scope.orgPolicy = policy;
                $scope.policy = angular.copy(policy);

                $scope.isEdit = true;
                // success
                $window.location = "#/";
            },
              function ()
              {
                  $scope.errorString =
                      "Unable to create policy '" + $scope.policy.num +
                      + '-' + $scope.policy.nme + "'. Please try again";

                  $scope.showError = true;
              });
        }

    };

    $scope.closeMessage = function ()
    {
        $scope.showMessage = false;
    };

    $scope.closeError = function ()
    {
        $scope.showError = false;
    };

    $scope.back = function ()
    {
        if ($scope.policyForm.$dirty)
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

    $scope.removeDependentByIndex = function(index)
    {
       $scope.policy.deps.splice(index, 1);
    }

    $scope.removeDocumentByIndex = function (index)
    {
        $scope.policy.docs.splice(index, 1);
    }

    $scope.selectUsers = function ()
    {
        userService.showSelectUsersUI(userService)
        .then(function (selUsers)
        {
            angular.forEach(selUsers, function (user)
            {
                var exists = $filter('filter')($scope.policy.deps, function (dep)
                {
                    if (dep && dep.usr && dep.usr.id && dep.usr.id == user.id)
                    {
                        return true;
                    }

                    return false;
                });

                if (!exists.length)
                {
                    var dep = new CDependent();
                    dep.usr = user;
                    $scope.policy.deps.push(dep);
                }
            });
        },
        function ()
        {
        });
    };

    $scope.calendarPopupOpen = function ($event,property)
    {
        $scope.isDopCalendarOpened = false;
        $scope.isDoeCalendarOpened = false;
        $event.preventDefault();
        $event.stopPropagation();
        $scope[property] = true;
    };
}]);



