using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.Common
{
    public class ApiSuccessResult<T> : ApiResult<T>
    {

        public ApiSuccessResult()
        {
            IsSuccessed = true;
            Message = "Thao tác thành công";
        }

        public ApiSuccessResult(T resultObj)
        {
            IsSuccessed = true;
            ResultObj = resultObj;
        }       

       
    }
}
