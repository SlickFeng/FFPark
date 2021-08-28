using AutoMapper;
using FFPark.Core.Infrastructure.Mapper;
using FFPark.Entity;
using FFPark.Entity.Park;
using FFPark.Entity.Pay;
using FFPark.Model;
using FFPark.Model.Park;
using FFPark.Model.User;

namespace FFPark.WebAPI.Infrastructure.Mapper.Extensions
{
    public class MapperConfiguration : Profile, IOrderedMapperProfile
    {
        public MapperConfiguration()
        {
            CreateConfigMaps();

        }
        protected virtual void CreateConfigMaps()
        {
            CreateMap<BaseUserModel, BaseUser>();
            CreateMap<BaseUser, BaseUserModel>();


            CreateMap<UserExtension, BaseUserModel>();
            CreateMap<BaseUserModel, UserExtension>();

            CreateMap<BaseRole, BaseRoleModel>();
            CreateMap<BaseRoleModel, BaseRole>();


            CreateMap<PayChannel, PayChannelModel>();
            CreateMap<PayChannelModel, PayChannel>();

            //广告
            CreateMap<AdCurrencyPresentation, AdCurrencyPresentationModel>();
            CreateMap<AdCurrencyPresentationModel, AdCurrencyPresentation>();

            //优惠券
            CreateMap<SpecialDiscount, SpecialDiscountModel>();
            CreateMap<SpecialDiscountModel, SpecialDiscount>();

            //广告位置
            CreateMap<AdPosition, AdPositionModel>();
            CreateMap<AdPositionModel, AdPosition>();
           

            //左侧导航
            CreateMap<BaseNavigationModule, BaseNavigationModuleModel>();
            CreateMap<BaseNavigationModuleModel, BaseNavigationModule>();


            //车场
            CreateMap<BaseParkModel, BasePark>();
            CreateMap<BasePark, BaseParkModel>();

            //车场扩展
            CreateMap<BaseParkExtend, BaseParkExtenModel>();
            CreateMap<BaseParkExtenModel, BaseParkExtend>();
            
        }
        public int Order => 0;
    }
}
