using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.WebChat.Model
{
    /// <summary>
    /// 微信公众号和小程序订阅消息统一返回Model
    /// </summary>
    public class SubscriptionMessagesResponseModel
    {
        /// <summary>
        /// 错误码
        /// </summary>
        public int errcode { get; set; }
        /// <summary>
        /// 错误信息
        /// </summary>
        public string errmsg { get; set; }
    }
    /// <summary>
    /// 添加模板
    /// </summary>
    public class AddTemplateMessagesResponseModel : SubscriptionMessagesResponseModel
    {
        /// <summary>
        /// 添加至帐号下的模板id，发送订阅通知时所需
        /// </summary>
        public string priTmplId { get; set; }
    }
    public class BaseRequestModel
    {

        /// <summary>
        /// 是 接口调用凭证
        /// </summary>
        public string access_token { get; set; }
    } /// <summary>
      /// send发送订阅通知请求参数
      /// </summary>
    public class SendRequestModel : BaseRequestModel
    {

        /// <summary>
        ///  是 接收者（用户）的 openid
        /// </summary>
        public string touser { get; set; }

        /// <summary>
        ///  是 所需下发的订阅模板id
        /// </summary>
        public string template_id { get; set; }
        /// <summary>
        /// 点击模板卡片后的跳转页面，仅限本小程序内的页面。支持带参数,（示例index? foo = bar）。该字段不填则模板无跳转。
        /// </summary>
        public string page { get; set; }
        /// <summary>
        ///跳转小程序类型：developer为开发版；trial为体验版；formal为正式版；默认为正式版
        /// </summary>

        public string miniprogram_state { get; set; }

        /// <summary>
        /// 模板内容，格式形如 { "key1": { "value": any }, "key2": { "value": any } }
        /// </summary>
        public object data { get; set; }

        /// <summary>
        /// 进入小程序查看”的语言类型，支持zh_CN(简体中文)、en_US(英文)、zh_HK(繁体中文)、zh_TW(繁体中文)，默认为zh_CN
        /// </summary>

        public string lang { get; set; }

    }





    #region    小程序消息参数Model

    /// <summary>
    /// 居民房屋安全预警提醒
    /// </summary>
    public class HouseSafeWarningNoticeModel : SendRequestModel
    {
        /// <summary>
        /// 预警时间
        /// </summary>
        public string WarningTime { get; set; }

        /// <summary>
        /// 预警内容
        /// </summary>
        public string WarningContent { get; set; }
        /// <summary>
        /// 预警位置
        /// </summary>
        public string WarningLocation { get; set; }
        /// <summary>
        /// 预警类型
        /// </summary>
        public string WarningType { get; set; }
        /// <summary>
        /// 温馨提示
        /// </summary>
        public string WarningRemark { get; set; }

    }

    /// <summary>
    /// 租户认证审核通知
    /// </summary>
    public class RentAttestationModel : SendRequestModel
    {
        /// <summary>
        /// 审核时间
        /// </summary>
        public string CheckTime { get; set; }
        /// <summary>
        /// 认证公司
        /// </summary>
        public string CheckCompany { get; set; }
        /// <summary>
        /// 审核状态
        /// </summary>
        public string CheckStatus { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string CheckRemark { get; set; }
        /// <summary>
        /// 拒绝事由
        /// </summary>
        public string CheckRefuse { get; set; }
    }

    /// <summary>
    /// 续租提醒
    /// </summary>
    public class ReletNoticeModel : SendRequestModel
    {
        /// <summary>
        /// 公寓名称
        /// </summary>
        public string ApartmentName { get; set; }
        /// <summary>
        /// 剩余租期
        /// </summary>
        public string SurplusTenancy { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get; set; }
    }


    public class AwaitSignBillNoticeModel : SendRequestModel
    {
        /// <summary>
        /// 账单号
        /// </summary>
        public string BillNo { get;  set; }
        /// <summary>
        /// 发起方
        /// </summary>
        public string FromName { get;  set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string BillType { get;  set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string BillMoney { get;  set; }
        /// <summary>
        /// 备注
        /// </summary>
        public string Remark { get;  set; }
    }
    /// <summary>
    /// 交费通知
    /// </summary>
    public class PayFeeNoticeModel : SendRequestModel
    {
        /// <summary>
        /// 截止日期
        /// </summary>
        public string Deadline { get; set; }
        /// <summary>
        /// 交费金额
        /// </summary>
        public string PayFeeMoney { get; set; }
        /// <summary>
        /// 项目标题
        /// </summary>
        public string SubjectTitle { get; set; }
    }

    /// <summary>
    /// 收租提醒
    /// </summary>
    public class ChargeRentNoticeModel : SendRequestModel
    {
        /// <summary>
        /// 房产名称
        /// </summary>
        public string HouseEquity { get; set; }
        /// <summary>
        /// 收租日期
        /// </summary>
        public string ChargeRentTime { get; set; }
        /// <summary>
        /// 缴费周期
        /// </summary>
        public string PayFees { get; set; }
        /// <summary>
        /// 应收租金
        /// </summary>
        public string ReceivableMoney { get; set; }
    }



    /// <summary>
    /// 下单成功通知
    /// </summary>
    public class PlaceOrderSuccessNoticeModel : SendRequestModel
    {
        /// <summary>
        /// 订单编号
        /// </summary>
        public string OrderNo { get; set; }
        /// <summary>
        /// 店铺名称
        /// </summary>
        public string StoreName { get; set; }
        /// <summary>
        /// 服务项目
        /// </summary>
        public string ServiceItems { get; set; }
        /// <summary>
        /// 订单金额
        /// </summary>
        public string OrderMoney { get; set; }
        /// <summary>
        /// 下单时间
        /// </summary>
        public string PlaceOrderTime { get; set; }
    }

    /// <summary>
    /// 缴租提醒
    /// </summary>
    public class PayRentNoticeModel : SendRequestModel
    {
        /// <summary>
        /// 公寓名称
        /// </summary>
        public string ApartmentName { get; set; }
        /// <summary>
        /// 金额
        /// </summary>
        public string RentMoney { get; set; }
        /// <summary>
        /// 缴租日期
        /// </summary>
        public string PayRentTime { get; set; }
        /// <summary>
        /// 房东
        /// </summary>
        public string Landlord { get; set; }
        /// <summary>
        /// 账单类型
        /// </summary>
        public string BillType { get; set; }
    }
    #endregion
}
