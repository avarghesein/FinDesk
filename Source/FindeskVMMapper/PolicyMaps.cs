using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MODEL = Findesk.Model.Policy;
using VIEWMODEL = Findesk.VM.Policy;

namespace Findesk.VM.Mapper
{
    internal static class PolicyMaps
    {
        public static void Map()
        {
            AutoMapper.Mapper.CreateMap<MODEL.Policy, VIEWMODEL.Policy>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.Dependent, VIEWMODEL.Dependent>().ReverseMap();
        }
    };
};
