using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core
{
    public class DbEntityEntryChanged
    {
        public EntityEntry entity;
        public EntityState state;

        public DbEntityEntryChanged(EntityEntry entity)
        {
            this.entity = entity;
            this.state = entity.State;
        }
    }
}
