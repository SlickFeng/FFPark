using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FFPark.Core.Result
{
    public interface IApiResult
    {

        JsonResult SetOK(string message);

        JsonResult SetOK(string message, object data);


        JsonResult SetError(string message);

        JsonResult SetError(string message, object data);


        JsonResult SetOK(int code, string message, object data);


        JsonResult SetOK(int code, string message);


        JsonResult SetError(int code, string message, object data);


        JsonResult SetError(int code, string message);





    }
}
