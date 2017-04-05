using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cinode.RateApp.Business.Service;
using Cinode.RateApp.Data.Model;
using Cinode.RateApp.Data.Entity;

namespace Cinode.RateApp.API.Controllers
{
    [Route("api/[controller]")]
    public class UserSkillsController : Controller
    {
        SkillService service = new SkillService();

        [HttpGet("{Id}")]
        public IList<UserSkill> Get(long Id)
        {
            return service.GetUserSkills(Id);
        }


        [HttpPost]
        public void Post([FromBody]AddEditUserSkill userSkill)
        {
            service.AddEditUserSkill(userSkill);
        }




        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            service.DeleteUserSkill(id);
        }
    }
}