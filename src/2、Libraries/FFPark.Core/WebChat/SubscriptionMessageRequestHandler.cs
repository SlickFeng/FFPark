using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FFPark.Core.Http;
using FFPark.Core.WebChat.Enum;
using FFPark.Core.WebChat.Model;
using FFPark.Core.Extensions;
namespace FFPark.Core.WebChat
{
    public class SubscriptionMessageRequestHandler : ISubscriptionMessageRequestHandler
    {

        private readonly FFParkClient _client;

        private readonly string _smallProgramSubscribeBizsendUrl = "https://api.weixin.qq.com/cgi-bin/message/subscribe/send";
        public SubscriptionMessageRequestHandler(FFParkClient client)
        {
            _client = client;
        }

        public async Task<SmallProgramCodeResponseModel> GetSmallProgramOpenIdBySessionCode(string code, string appid, string appsecret)
        {
            string url = WebChatUrl.WebChatBaseUrl + string.Format("/sns/jscode2session?appid={0}&secret={1}&js_code={2}&grant_type=authorization_code", appid, appsecret, code);
            var result = await _client.GetAsync<SmallProgramCodeResponseModel>(url);
            return result;
        }
        /// <summary>
        /// 获取微信用户信息
        /// </summary>
        /// <param name="accessToken">access_token</param>
        /// <param name="openId">openid</param>
        /// <returns></returns>
        public async Task<WebChatUserModel> GetWebChatUserModel(string accessToken, string openId)
        {
            string url = WebChatUrl.WebChatBaseUrl + string.Format("/cgi-bin/user/info?access_token={0}&openid={1}&lang=zh_CN", accessToken, openId);
            var result = await _client.GetAsync<WebChatUserModel>(url);
            return result;
        }
        /// <summary>
        /// 获取小程序Token
        /// </summary>
        /// <param name="appId"></param>
        /// <param name="appSecret"></param>
        /// <returns></returns>
        public async Task<WebChatTokenMessageResponse> GetSmallAccesToken(string appId, string appSecret)
        {
            string url = WebChatUrl.WebChatBaseUrl + string.Format("/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}", appId, appSecret);
            var result = await _client.GetAsync<WebChatTokenMessageResponse>(url);
            return result;
        }
        public async Task<WebChatTemplate> GetSmallTemplate(string token)
        {
            string url = WebChatUrl.WebChatBaseUrl + string.Format("/wxaapi/newtmpl/gettemplate?access_token={0}", token);
            var result = await _client.GetAsync<WebChatTemplate>(url);
            return result;
        }

        /// <summary>
        /// 微信小程序发送订阅通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SubscriptionMessagesResponseModel> SmallProgramSendSubscription(SendRequestModel model)
        {
            var result = await _client.PostWebChatAsync<SubscriptionMessagesResponseModel>(_smallProgramSubscribeBizsendUrl, model.access_token, model);
            return result;
        }
        /// <summary>
        /// 居民房屋安全预警提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SubscriptionMessagesResponseModel> HouseSafeWarningNotice(HouseSafeWarningNoticeModel model)
        {

            SendRequestModel modelSend = new SendRequestModel()
            {
                access_token = model.access_token,
                touser = model.touser,
                //可以采用枚举形式
                template_id = SmallProgramSubscriptionMessageTemplateEnum.居民房屋安全预警提醒.EnumDescription(),

                data = new
                {
                    //预警时间
                    date4 = new { value = model.WarningTime },
                    //预警内容
                    thing3 = new { value = model.WarningContent },
                    //预警位置
                    thing2 = new { value = model.WarningLocation },
                    //预警类型
                    thing1 = new { value = model.WarningType },
                    // 温馨提示
                    thing5 = new { value = model.WarningRemark }
                },
                miniprogram_state = model.miniprogram_state,
                page = model.page
            };
            SubscriptionMessagesResponseModel response = await SmallProgramSendSubscription(modelSend);
            return response;
        }

        /// <summary>
        /// 租户认证审核通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SubscriptionMessagesResponseModel> RentAttestation(RentAttestationModel model)
        {

            SendRequestModel modelSend = new SendRequestModel()
            {
                access_token = model.access_token,
                touser = model.touser,
                //可以采用枚举形式
                template_id = SmallProgramSubscriptionMessageTemplateEnum.租户认证审核通知.EnumDescription(),
                data = new
                {
                    //审核时间
                    time3 = new { value = model.CheckTime },
                    //认证公司
                    thing2 = new { value = model.CheckCompany },
                    //审核状态
                    phrase1 = new { value = model.CheckStatus },
                    //备注
                    thing5 = new { value = model.CheckRemark },
                    // 拒绝事由
                    thing4 = new { value = model.CheckRefuse }
                },
                miniprogram_state = model.miniprogram_state,
                page = model.page
            };
            SubscriptionMessagesResponseModel response = await SmallProgramSendSubscription(modelSend);
            return response;
        }


        /// <summary>
        /// 续租提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SubscriptionMessagesResponseModel> ReletNotice(ReletNoticeModel model)
        {
            SendRequestModel modelSend = new SendRequestModel()
            {
                access_token = model.access_token,
                touser = model.touser,
                //可以采用枚举形式
                template_id = SmallProgramSubscriptionMessageTemplateEnum.续租提醒.EnumDescription(),
                data = new
                {
                    //公寓名称
                    thing1 = new { value = model.ApartmentName },
                    //剩余租期
                    thing2 = new { value = model.SurplusTenancy },
                    //备注
                    thing3 = new { value = model.Remark },
                },
                miniprogram_state = model.miniprogram_state,
                page = model.page
            };
            SubscriptionMessagesResponseModel response = await SmallProgramSendSubscription(modelSend);
            return response;
        }


        /// <summary>
        /// 待签署账单提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SubscriptionMessagesResponseModel> AwaitSignBillNotice(AwaitSignBillNoticeModel model)
        {

            SendRequestModel modelSend = new SendRequestModel()
            {
                access_token = model.access_token,
                touser = model.touser,
                //可以采用枚举形式
                template_id = SmallProgramSubscriptionMessageTemplateEnum.待签署账单提醒.EnumDescription(),
                data = new
                {
                    //账单号
                    number1 = new { value = model.BillNo },
                    //发起方
                    thing2 = new { value = model.FromName },
                    //类型
                    thing3 = new { value = model.BillType },
                    //金额
                    amount4 = new { value = model.BillMoney },
                    // 备注
                    thing5 = new { value = model.Remark }
                },
                miniprogram_state = model.miniprogram_state,
                page = model.page
            };
            SubscriptionMessagesResponseModel response = await SmallProgramSendSubscription(modelSend);
            return response;
        }

        /// <summary>
        /// 下单成功通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SubscriptionMessagesResponseModel> SmallPlaceOrderSuccessNotice(PlaceOrderSuccessNoticeModel model)
        {
            SendRequestModel modelSend = new SendRequestModel()
            {
                access_token = model.access_token,
                touser = model.touser,
                template_id = model.template_id,
                data = new
                {
                    //订单编号
                    character_string4 = new { value = model.OrderNo },
                    //店铺名称
                    name1 = new { value = model.StoreName },
                    // 服务项目
                    thing6 = new { value = model.ServiceItems },
                    // 订单金额
                    amount3 = new { value = model.OrderMoney },
                    //下单时间
                    date2 = new { value = model.PlaceOrderTime }
                }
            };
            SubscriptionMessagesResponseModel response = await SmallProgramSendSubscription(modelSend);
            return response;
        }
        /// <summary>
        /// 交费通知
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SubscriptionMessagesResponseModel> PayFeeNotice(PayFeeNoticeModel model)
        {
            SendRequestModel modelSend = new SendRequestModel()
            {
                access_token = model.access_token,
                touser = model.touser,
                //可以采用枚举形式
                template_id = SmallProgramSubscriptionMessageTemplateEnum.交费通知.EnumDescription(),
                data = new
                {
                    //截止日期
                    date3 = new { value = model.Deadline },
                    //交费金额
                    amount2 = new { value = model.PayFeeMoney },
                    //项目标题
                    thing1 = new { value = model.SubjectTitle },
                },
                miniprogram_state = model.miniprogram_state,
                page = model.page
            };
            SubscriptionMessagesResponseModel response = await SmallProgramSendSubscription(modelSend);
            return response;
        }
        /// <summary>
        /// 收租提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

        public async Task<SubscriptionMessagesResponseModel> ChargeRentNotice(ChargeRentNoticeModel model)
        {
            SendRequestModel modelSend = new SendRequestModel()
            {
                access_token = model.access_token,
                touser = model.touser,
                //可以采用枚举形式
                template_id = SmallProgramSubscriptionMessageTemplateEnum.收租提醒.EnumDescription(),

                data = new
                {
                    //房产名称
                    thing1 = new { value = model.HouseEquity },
                    //收租日期---精确到年月日
                    time2 = new { value = model.ChargeRentTime },
                    //缴费周期
                    time3 = new { value = model.PayFees },
                    //应收租金
                    amount4 = new { value = model.ReceivableMoney }
                },
                miniprogram_state = model.miniprogram_state,
                page = model.page
            };
            SubscriptionMessagesResponseModel response = await SmallProgramSendSubscription(modelSend);
            return response;
        }


        /// <summary>
        /// 缴租提醒
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<SubscriptionMessagesResponseModel> PayRentNotice(PayRentNoticeModel model)
        {
            SendRequestModel modelSend = new SendRequestModel()
            {
                access_token = model.access_token,
                touser = model.touser,
                //可以采用枚举形式
                template_id = SmallProgramSubscriptionMessageTemplateEnum.缴租提醒.EnumDescription(),
                data = new
                {
                    //公寓名称
                    thing1 = new { value = model.ApartmentName },
                    //金额
                    amount2 = new { value = model.RentMoney },
                    //缴租日期
                    date3 = new { value = model.PayRentTime },
                    //房东
                    name4 = new { value = model.Landlord },
                    // 账单类型
                    thing5 = new { value = model.BillType }
                },
                miniprogram_state = model.miniprogram_state,
                page = model.page
            };
            SubscriptionMessagesResponseModel response = await SmallProgramSendSubscription(modelSend);
            return response;
        }

    }
}
