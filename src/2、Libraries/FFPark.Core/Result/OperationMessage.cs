using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Result
{
    public static class OperationMessage
    {
        public static string OperationSuccess { get; set; } = "操作成功";

        public static string OperationFail { get; set; } = "操作失败";


        public static string InsertSuccess { get; set; } = "新增成功";

        public static string InsertFail { get; set; } = "新增失败";



        public static string UpdateSuccess { get; set; } = "修改成功";

        public static string UpdateFail { get; set; } = "修改失败";


        public static string DeleteSuccess { get; set; } = "删除成功";

        public static string DeleteFail { get; set; } = "删除失败";

        public static string SelectSuccess { get; set; } = "查询成功";

        public static string SelectFail { get; set; } = "查询失败";



        public static string UploadSuccess { get; set; } = "上传成功";

        public static string UploadFail { get; set; } = "上传失败";


        public static string NoProvideTimeStamp { get; set; } = "未提供timestamp";

        public static string NoProvideSign { get; set; } = "未提供sign";


        public static string SignError { get; set; } = "验签失败";


        public static string SignErrorNoParamter = "验签参数未提供";


        public static string TimeStampFormatError = "时间戳格式错误";


        public static string ParmterError = "参数错误";

    }
}
