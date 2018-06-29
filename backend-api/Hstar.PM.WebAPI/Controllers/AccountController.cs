using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hstar.PM.Business;
using Microsoft.AspNetCore.Mvc;

namespace Hstar.PM.WebAPI.Controllers
{
    [Route("account")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccountBusiness _accountBiz;
        public AccountController(IAccountBusiness accountBiz)
        {
            this._accountBiz = accountBiz;
        }

        [HttpGet, Route("login")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", this._accountBiz.GetName() };
        }

    }
}
