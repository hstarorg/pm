using Hstar.PM.Core.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Hstar.PM.Business.Implement
{
    [AutoRegister(typeof(DefaultAccountBusiness))]
    public class DefaultAccountBusiness : IAccountBusiness
    {
        public string GetName()
        {
            return "abc";
        }
    }
}
