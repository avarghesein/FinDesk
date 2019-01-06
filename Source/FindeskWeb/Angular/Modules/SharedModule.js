
angular.module('customFilters', []).filter('iif', function () {
    return function (input, trueValue, falseValue) {
        return input ? trueValue : falseValue;
    };
});

angular.module('sharedModule', ['ngSanitize', 'ui.bootstrap', 'ui.bootstrap.tpls']).

constant('globals',
{
    getRootUrl: function ()
    {
        var url = $("#rootUrl").attr("href");

        if (url.trim() == '/') return '';

        return url;
    }
}).

factory("sharedService", ["$http", "$q", "$sanitize", "$rootScope", "$modal", "globals", function ($http, $q, $sanitize, $rootScope, $modal, globals) {
    var _parseDate = function (date) {
        if (date != undefined) {
            return new Date(date);
        }

        return undefined;
    };

    var _buildCustomModel = function (jsonResult, constructor) {
        var _transformObject = function (json) {
            var model = new constructor();
            angular.extend(model, json);
            if (model.prepare) model.prepare();
            return model;
        };

        if (angular.isArray(jsonResult)) {
            var models = [];
            angular.forEach(jsonResult, function (object) {
                models.push(_transformObject(object, constructor));
            });
            return models;
        }
        else {
            if (jsonResult == undefined) return undefined;
            return _transformObject(jsonResult, constructor);
        }
    };

    var _uploadImageAndCreateThumbnail = function (name, width, height, file) {
        var fd = new FormData();
        //Take the first selected file
        fd.append("image", file);
        fd.append("width", width);
        fd.append("height", height);

        $http.post(globals.getRootUrl() + '/api/Shared/UploadImageAndGenerateThumbnail', fd, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).then(function (data) {
            $rootScope.$broadcast(name + '-image-guid', data.data);

        }, function () {
        });
    };

    var _uploadDocument = function (name, file) {
        var fd = new FormData();
        //Take the first selected file
        fd.append("document", file);

        $http.post(globals.getRootUrl() + '/api/Shared/UploadDocument', fd, {
            withCredentials: true,
            headers: { 'Content-Type': undefined },
            transformRequest: angular.identity
        }).then(function (data) {
            $rootScope.$broadcast(name + '-document-guid', data.data);

        }, function () {
        });
    };

    var _showConfirmDialog = function (title, message, size) {
        var defer = $q.defer();

        var modalInstance = $modal.open({
            animation: true,
            size: size || "sm",
            templateUrl: globals.getRootUrl() + '/Angular/Views/Shared/ConfirmationBox.html',
            controller: function ($scope, $modalInstance) {
                $scope.title = title;
                $scope.message = message;

                $scope.ok = function () {
                    modalInstance.close();
                    defer.resolve();
                };

                $scope.cancel = function () {
                    $modalInstance.dismiss();
                    defer.reject();
                };
            }
        });

        return defer.promise;
    }

    return {
        parseDate: _parseDate,
        buildCustomModel: _buildCustomModel,
        uploadImageAndCreateThumbnail: _uploadImageAndCreateThumbnail,
        uploadDocument: _uploadDocument,
        showConfirmDialog: _showConfirmDialog
    };

}]).

directive('generateThumbnail', ["sharedService", "globals", function (sharedService, globals) {
    return {

        transclude: true,

        scope: {},

        templateUrl: globals.getRootUrl() + '/Angular/Views/Shared/ImageUpload.html',

        controller: function ($scope, $http, $attrs) {
            $scope.title = $attrs.title || "Upload Image";

            $scope.name = $attrs.name || "image";

            $scope.width = $attrs.width || 413;

            $scope.height = $attrs.height || 531;

            $scope.uploadFile = function (files) {
                if (!files || files.length <= 0) {
                    return;
                }

                sharedService.uploadImageAndCreateThumbnail(
                    $scope.name, $scope.width, $scope.height, files[0]);
            }
        }
    }
}]).

directive('uploadDocument', ["sharedService", "globals", function (sharedService, globals) {
    return {

        transclude: true,

        scope: {},

        templateUrl: globals.getRootUrl() + '/Angular/Views/Shared/DocumentUpload.html',

        controller: function ($scope, $http, $attrs) {
            $scope.title = $attrs.title || "Upload Document";

            $scope.name = $attrs.name || "document";

            $scope.uploadFile = function (files) {
                if (!files || files.length <= 0) {
                    return;
                }

                sharedService.uploadDocument(
                    $scope.name, files[0]);
            }
        }
    }
}])

;
