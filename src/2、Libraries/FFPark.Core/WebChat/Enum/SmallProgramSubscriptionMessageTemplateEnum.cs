using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.WebChat.Enum
{
    /// <summary>
    /// 微信小程序
    /// </summary>
    public enum SmallProgramSubscriptionMessageTemplateEnum
    {
        [Description("yQHn-mZkLtA5C8RJFpOoc5R0Kjrp7PuGWhIJkmTZXuI")]
        居民房屋安全预警提醒 = 1,

        [Description("6KLG6mYaeGJgV3asgEzNCfqJjnmWaqa8PNyh_hdnDno")]
        租户认证审核通知 = 2,

        [Description("DA8cno3_m26MUUfJlT5Xx2Sw5YVLFG_-z4fxEz7K028")]
        续租提醒 = 3,

        [Description("dCA-U5w4mxCdCxo86ruZXsETdpXmQEUGNg3IX3SEynI")]
        待签署账单提醒 = 4,


        [Description("fZcKKIUL6lkBPhe4jtqw2OXo-yiA6nPN7ZnKayBtGpw")]
        交费通知 = 5,

        [Description("7ywD28Cb3iUc2aIH9-a5_iBN90YJuzGgJ3hOCI3b8E0")]
        收租提醒 = 6,

        [Description("QZuBb6h9W4G5k2vI4vOtojgHXQsMOB8PfQxwrX4ZuNE")]
        缴租提醒 = 7,

    }
}
