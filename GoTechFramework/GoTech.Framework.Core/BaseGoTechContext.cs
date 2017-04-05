using GoTech.Framework.Core.Configration;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core
{
    public class BaseGoTechContext : DbContext
    {
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                AppSetting config = new AppSetting();
                optionsBuilder.UseSqlServer(config.GoTech.ConnectionString);
            }
           
            //base.OnConfiguring(optionsBuilder);
        }
        public new DbSet<TEntity> Set<TEntity>() where TEntity : BaseGoTechEntity
        {
            return base.Set<TEntity>();
        }



    }
}
