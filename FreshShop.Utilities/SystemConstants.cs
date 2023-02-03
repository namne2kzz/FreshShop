using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Utilities
{
    public class SystemConstants
    {
        public const string DefaultLanguageId= "DefaultLanguageId";

        public const string Token = "Token";

        public const string BaseAddress = "http://localhost:5001";

        public const string AdminRoleName = "admin";

        public const string UserRoleName = "user";

        public const int DefaultStatusOrder = 0;

        public const int StandardDelivery = 1;

        public const decimal StandardDeliveryCost = 0;

        public const int ExpressDelivery = 2;

        public const decimal ExpressDeliveryCost = 30000;

        public const int BusinessDelivery = 3;

        public const decimal BusinessDeliveryCost =50000;

    }
}
