angular.module("policyModels", ['sharedModule', 'sharedModels', 'userModels']).

factory("CPolicy", ['sharedService', 'CUser', 'CDependent', 'CDocument', function (sharedService, CUser, CDependent, CDocument)
{
    var CPolicy = function ()
    {
        this.deps = [];
        this.docs = [];
    };

    CPolicy.prototype =
    {
        prepare: function ()
        {
            this.dop = sharedService.parseDate(this.dop);
            this.doe = sharedService.parseDate(this.doe);
            this.usr = sharedService.buildCustomModel(this.usr, CUser);
            this.deps = sharedService.buildCustomModel(this.deps, CDependent);
            this.docs = sharedService.buildCustomModel(this.docs, CDocument);
        }        
    };

    return CPolicy;
}]);