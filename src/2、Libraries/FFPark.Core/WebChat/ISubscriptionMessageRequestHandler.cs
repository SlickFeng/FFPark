using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.WebChat.Model;
namespace FFPark.Core.WebChat
{
    /// <summary>
    /// 微信小程序订阅接口
    /// </summary>
    public interface ISubscriptionMessageRequestHandler
    {



        Task<WebChatTemplate> GetSmallTemplate(string token);
        Task<WebChatUserModel> GetWebChatUserModel(string accessToken, string openId);

        Task<WebChatTokenMessageResponse> GetSmallAccesToken(string appId, string appSecret);


        /// <summary>
        /// 根据小程序返回的code  获取用户的openId
        /// </summary>
        /// <param name="code">微信小程序获取的Code</param>
        /// <param name="appid">微信小程序appId</param>
        ///<param name="appsecret">微信小程序AppSecret</param>
        /// <returns></returns>
        Task<SmallProgramCodeResponseModel> GetSmallProgramOpenIdBySessionCode(string code, string appid, string appsecret);


        /// <summary>
        /// 发送微信小程序订阅通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SubscriptionMessagesResponseModel> SmallProgramSendSubscription(SendRequestModel model);

        /// <summary>
        /// 居民房屋安全预警提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        Task<SubscriptionMessagesResponseModel> HouseSafeWarningNotice(HouseSafeWarningNoticeModel model);


        /// <summary>
        /// 租户认证审核通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SubscriptionMessagesResponseModel> RentAttestation(RentAttestationModel model);


        /// <summary>
        /// 续租提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SubscriptionMessagesResponseModel> ReletNotice(ReletNoticeModel model);



        /// <summary>
        /// 待签署账单提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SubscriptionMessagesResponseModel> AwaitSignBillNotice(AwaitSignBillNoticeModel model);

        /// <summary>
        /// 交费通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SubscriptionMessagesResponseModel> PayFeeNotice(PayFeeNoticeModel model);

        /// <summary>
        /// 收租提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        Task<SubscriptionMessagesResponseModel> ChargeRentNotice(ChargeRentNoticeModel model);


        /// <summary>
        /// 缴租提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SubscriptionMessagesResponseModel> PayRentNotice(PayRentNoticeModel model);
        /// <summary>
        /// 下单成功通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        Task<SubscriptionMessagesResponseModel> SmallPlaceOrderSuccessNotice(PlaceOrderSuccessNoticeModel model);
    }
}
