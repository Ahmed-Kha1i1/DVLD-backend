using AutoMapper;
using BusinessLayer;
using DataLayerCore.User;

namespace ApiLayer.Profiles
{
    public class ModificationUserProfile : Profile
    {
        public ModificationUserProfile()
        {
            CreateMap<UserForCreateDTO, clsUser>();
            CreateMap<UserForUpdateDTO, clsUser>();
        }
    }
}
