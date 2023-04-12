using FreshShop.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.ViewModels.System.Roles
{
    public class GetUserPagingByRoleRequest : PagingRequestBase
    {
        public Guid Id { get; set; }

        public string Keyword { get; set; }
    }
}
