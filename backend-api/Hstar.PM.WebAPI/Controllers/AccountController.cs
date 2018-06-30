using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Hstar.PM.Business;
using Hstar.PM.Core.Models;
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

        /// <summary>
        /// Execute login
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("login")]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new string[] { "value1", "value2", this._accountBiz.GetName() };
        }

        /// <summary>
        /// Two error type.
        /// </summary>
        /// <returns></returns>
        [HttpGet, Route("error")]
        public ActionResult<IEnumerable<string>> ThrowError()
        {
            var rnd = new Random().Next(0, 2);
            if (rnd % 2 == 0)
            {
                int a = 0;
                int b = 5 / a;
            }
            else
            {
                throw new BusinessException("这是一个业务错误");
            }

            return new string[] { "value1", "value2", this._accountBiz.GetName() };
        }

    }
}
