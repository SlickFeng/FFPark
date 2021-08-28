using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.WebChat.Model
{
    public class WebChatUserModel
    {
        /// <summary>
        /// 用户是否订阅该公众号标识，值为0时，代表此用户没有关注该公众号，拉取不到其余信息。
        /// </summary>
        public string subscribe { get; set; }
        /// <summary>
        /// 用户的标识，对当前公众号唯一
        /// </summary>
        public string openid { get; set; }
        /// <summary>
        /// 用户的昵称
        /// </summary>
        public string nickname { get; set; }
        /// <summary>
        /// 用户的性别，值为1时是男性，值为2时是女性，值为0时是未知
        /// </summary>
        public string sex { get; set; }
        /// <summary>
        /// 用户的语言，简体中文为zh_CN
        /// </summary>
        public string language { get; set; }
        /// <summary>
        /// 用户所在城市
        /// </summary>
        public string city { get; set; }
        /// <summary>
        /// 用户所在省份
        /// </summary>
        public string province { get; set; }
        /// <summary>
        /// 用户所在国家
        /// </summary>
        public string country { get; set; }
        /// <summary>
        /// 用户头像，最后一个数值代表正方形头像大小（有0、46、64、96、132数值可选，0代表640*640正方形头像），用户没有头像时该项为空。若用户更换头像，原有头像URL将失效。
        /// </summary>
        public string headimgurl { get; set; }
        /// <summary>
        /// 用户关注时间，为时间戳。如果用户曾多次关注，则取最后关注时间
        /// </summary>
        public string subscribe_time { get; set; }
        /// <summary>
        /// 只有在用户将公众号绑定到微信开放平台帐号后，才会出现该字段。
        /// </summary>
        public string unionid { get; set; }
        /// <summary>
        /// 公众号运营者对粉丝的备注，公众号运营者可在微信公众平台用户管理界面对粉丝添加备注
        /// </summary>
        public string remark { get; set; }
        /// <summary>
        /// 用户所在的分组ID（兼容旧的用户分组接口）
        /// </summary>
        public string groupid { get; set; }
        /// <summary>
        /// 用户被打上的标签ID列表
        /// </summary>
        public string tagid_list { get; set; }
        /// <summary>
        /// 返回用户关注的渠道来源，ADD_SCENE_SEARCH 公众号搜索，
        /// ADD_SCENE_ACCOUNT_MIGRATION 公众号迁移，
        /// ADD_SCENE_PROFILE_CARD 名片分享，
        /// ADD_SCENE_QR_CODE 扫描二维码，
        /// ADD_SCENE_PROFILE_LINK 图文页内名称点击，
        /// ADD_SCENE_PROFILE_ITEM 图文页右上角菜单，
        /// ADD_SCENE_PAID 支付后关注,
        /// ADD_SCENE_WECHAT_ADVERTISEMENT 微信广告，
        /// ADD_SCENE_OTHERS 其他
        /// </summary>
        public string subscribe_scene { get; set; }
        /// <summary>
        /// 二维码扫码场景（开发者自定义）
        /// </summary>
        public string qr_scene { get; set; }
        /// <summary>
        /// 二维码扫码场景描述（开发者自定义）
        /// </summary>
        public string qr_scene_str { get; set; }
    }




    /// <summary>
    /// 小程序登录接口返回值
    /// </summary>
    public class SmallProgramCodeResponseModel: WebChatMessageResponseBase
    {
        /// <summary>
        /// 用户唯一标识
        /// </summary>
        public string openid { get; set; }

        /// <summary>
        /// 会话密钥
        /// </summary>
        public string session_key { get; set; }
        /// <summary>
        /// 用户在开放平台的唯一标识符，若当前小程序已绑定到微信开放平台帐号下会返回，详见 UnionID 机制说明。
        /// </summary>
        public string unionid { get; set; }



    }




    public class WebChatTokenMessageResponse : WebChatMessageResponseBase
    {
        public string access_token { get; set; }


        public int expires_in { get; set; }



    }

    public class WebChatMessageResponseBase
    {
        public int errcode { get; set; }

        public string errmsg { get; set; }

    }


    public class WebChatTemplate : WebChatMessageResponseBase
    {
        public List<TemplateData> data { get; set; }
    }

    public class TemplateData
    {
        /// <summary>
        ///  string 添加至帐号下的模板 id，发送小程序订阅消息时所需
        /// </summary>

        public string priTmplId { get; set; }
        /// <summary>
        /// string 模版标题
        /// </summary>
        public string title { get; set; }
        /// <summary>
        /// string 模版内容
        /// </summary>
        public string content { get; set; }
        /// <summary>
        /// string 模板内容示例
        /// </summary>
        public string example { get; set; }
        /// <summary>
        /// number  模版类型，2 为一次性订阅，3 为长期订阅{
        /// </summary>
        public int type { get; set; }

    }
}
