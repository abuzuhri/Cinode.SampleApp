using Cinode.RateApp.Data.Entity;
using GoTech.Framework.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cinode.RateApp.Data.Context
{
    public class MainContext : BaseGoTechContext
    {
        public DbSet<Rate> Rates { get; set; }
        public DbSet<Skill> Skills { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<UserSkill> UserSkills { get; set; }
    }
}
