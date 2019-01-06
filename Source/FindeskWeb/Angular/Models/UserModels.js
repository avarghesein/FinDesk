angular.module("userModels", ['sharedModule', 'sharedModels']).

factory("CUser", ['sharedService', 'CDocument', 'globals', function (sharedService, CDocument, globals)
{
    var CUser = function ()
    {
        //[Call base class constructor] CBase.call(this);
        this.bgp = undefined;
        this.rhf = undefined;
        this.sel = undefined;
    };

    //[Inherit from base class]CUser.prototype = Object.create(CBase.prototype);
    //Constructor
    CUser.prototype.constructor = CUser;

    CUser.prototype =
    {
        prepare: function ()
        {
            //[Base class call]Object.getPrototypeOf(CUser.prototype).prepare(this);
            this.dob = sharedService.parseDate(this.dob);
            this.thmb = sharedService.buildCustomModel(this.thmb, CDocument);
        },

        getThumbnailUrl: function ()
        {
            if (this && this.thmb && this.thmb.id) return globals.getRootUrl() + '/Home/GetThumbnail/' + this.thmb.id;

            if (!this || this.gen == undefined || this.gen)
            {
                return globals.getRootUrl() + '/CSS/img/male.jpg';
            }
            else
            {
                return globals.getRootUrl() + '/CSS/img/female.jpg';
            }
        }
    };

    return CUser;
}]).

factory("CDependent", ['sharedService', 'CUser', function (sharedService, CUser)
{
    var CDependent = function ()
    {
        CDependent.allRelations = undefined;
        this.rln = '';
    };

    CDependent.Self = 0;
    CDependent.Spouse = 1;
    CDependent.Father = 2;
    CDependent.Mother = 3;
    CDependent.FatherInLaw = 4;
    CDependent.MotherInLaw = 5;
    CDependent.Child = 6;
    CDependent.Son = 7;
    CDependent.Daughter = 8;

    CDependent.prototype.constructor = CDependent;

    CDependent.prototype =
    {
        prepare: function ()
        {
            this.usr = sharedService.buildCustomModel(this.usr, CUser);
        },

        getRelationByCode: function (rln)
        {
            switch (rln)
            {
                case CDependent.Self: return 'Self';
                case CDependent.Spouse: return 'Spouse';
                case CDependent.Father: return 'Father';
                case CDependent.Mother: return 'Mother';
                case CDependent.FatherInLaw: return 'FatherInLaw';
                case CDependent.MotherInLaw: return 'MotherInLaw';
                case CDependent.Child: return 'Child';
                case CDependent.Son: return 'Son';
                case CDependent.Daughter: return 'Daughter';
                default: return 'Not Specified';
            }
        },

        getRelation: function ()
        {
            return this.getRelationByCode(this.rln);
        },

        getRelations: function ()
        {
            if (CDependent.allRelations) return CDependent.allRelations;

            var rlnCdes = ['',
            CDependent.Self,
            CDependent.Spouse,
            CDependent.Father,
            CDependent.Mother,
            CDependent.FatherInLaw,
            CDependent.MotherInLaw,
            CDependent.Child,
            CDependent.Son,
            CDependent.Daughter];

            var rlns = [];

            var _self = this;

            angular.forEach(rlnCdes, function (rlnCode)
            {
                rlns.push({ "cde": rlnCode, "nme": _self.getRelationByCode(rlnCode) });
            });

            return (CDependent.allRelations = rlns);
        }
    };

    return CDependent;
}])
;