angular.module("sharedModels", ['sharedModule']).

factory("CDocument", ['sharedService', function (sharedService)
{
    var CDocument = function ()
    {

    };

    CDocument.prototype.constructor = CDocument;

    CDocument.prototype =
    {
        prepare: function ()
        {
        },


        getSize: function()
        {
            var roundFn = function(value, decimals)
            {
                return Number(Math.round(value+'e'+decimals)+'e-'+decimals);
            }

            var kb = 1024 * 8;
            var mb = 1024 * kb;
            var gb = 1024 * mb;

            var sze = this.sze;

            if (!sze) return "0B";

            var div = Math.floor(sze / gb);
            if (div > 0) return roundFn((sze / gb), 2) + "GB";

            div = Math.floor(sze / mb);
            if (div > 0) return roundFn((sze / mb), 2) + "MB";

            div = Math.floor(sze / kb);
            if (div > 0) return roundFn((sze / kb), 2) + "KB";
        }

    };

    return CDocument;
}])
;