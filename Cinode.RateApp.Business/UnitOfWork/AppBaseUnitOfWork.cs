using Cinode.RateApp.Data.Context;
using Cinode.RateApp.Data.Entity;
using GoTech.Framework.Core;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinode.RateApp.Business.UnitOfWork
{
    public class AppBaseUnitOfWork : BaseGoTechUnitOfWork
    {
        public BaseDataRepository<Rate> RateRepository { get; set; }
        public BaseDataRepository<Skill> SkillRepository { get; set; }
        public BaseDataRepository<User> UserRepository { get; set; }
        public BaseDataRepository<UserSkill> UserSkillRepository { get; set; }

        public AppBaseUnitOfWork() : base(new MainContext())
        {
            RateRepository = this.Repository<Rate>();
            SkillRepository = this.Repository<Skill>();
            UserRepository = this.Repository<User>();
            UserSkillRepository = this.Repository<UserSkill>();
        }
    }
}
