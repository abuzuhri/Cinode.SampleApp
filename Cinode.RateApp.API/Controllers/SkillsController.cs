using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinode.RateApp.Business.Service;
using Cinode.RateApp.Data.Model;

namespace Cinode.RateApp.API.Controllers
{
    [Route("api/[controller]")]
    public class SkillsController : Controller
    {
        SkillService service = new SkillService();

        [HttpGet("{Name}")]
        public IList<SkillModel> Get(string Name)
        {
            return service.GetSkill(Name);
        }
    }
}