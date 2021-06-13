using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RapidBlazor.Api.Controllers
{
    public class AdminController : ApiControllerBase
    {
        [HttpGet]
        [Authorize(Policy = nameof(Shared.Policies.Policies.AdminPolicy))]
        public ActionResult<string> Get()
        {
            return Ok("This message is only visible to admins");
        }
    }
}
