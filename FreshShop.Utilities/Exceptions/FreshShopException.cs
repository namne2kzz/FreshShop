using System;
using System.Collections.Generic;
using System.Text;

namespace FreshShop.Utilities.Exceptions
{
    
    public class FreshShopException :Exception
    {
        public FreshShopException()
        {

        }

        public FreshShopException(string message):base(message)
        {

        }

        public FreshShopException(string message, Exception inner) : base(message, inner)
        {

        }
    }
}
