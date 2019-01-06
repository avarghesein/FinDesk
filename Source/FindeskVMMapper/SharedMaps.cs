using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using MODEL = Findesk.Model.Shared;
using VIEWMODEL = Findesk.VM.Shared;

namespace Findesk.VM.Mapper
{
    internal static class SharedMaps
    {
        public static void Map()
        {
            AutoMapper.Mapper.CreateMap<MODEL.Module, VIEWMODEL.Module>();

            AutoMapper.Mapper.CreateMap<MODEL.ModuleWrapper, VIEWMODEL.Module>().
                ConvertUsing(src => AutoMapper.Mapper.Map<VIEWMODEL.Module>(src.Module));

            AutoMapper.Mapper.CreateMap<VIEWMODEL.Module, MODEL.ModuleWrapper>().
                ForMember(tar => tar.Module, opt => opt.MapFrom(src => src));

            AutoMapper.Mapper.CreateMap<MODEL.Module, VIEWMODEL.Module>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.WorkGroupRole, VIEWMODEL.WorkGroupRole>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.BloodGroup, VIEWMODEL.BloodGroup>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.Gender, VIEWMODEL.Gender>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.Relation, VIEWMODEL.Relation>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.WorkGroupMemberRole, VIEWMODEL.WorkGroupMemberRole>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.WorkGroupMember, VIEWMODEL.WorkGroupMember>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.WorkGroup, VIEWMODEL.WorkGroup>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.Document, VIEWMODEL.Document>().ReverseMap();
            AutoMapper.Mapper.CreateMap<MODEL.User, VIEWMODEL.User>().ReverseMap();
        }
    };
};
