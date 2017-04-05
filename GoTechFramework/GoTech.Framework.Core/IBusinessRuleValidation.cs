using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Text;

namespace GoTech.Framework.Core
{
    public interface IBusinessRuleValidation
    {
        void OnCreating(EntityEntry entityEntry);
        void OnUpdating(EntityEntry entityEntry);
        void OnDeleting(EntityEntry entityEntry);

        void OnCreated(EntityEntry entityEntry);
        void OnUpdated(EntityEntry entityEntry);
        void OnDeleted(EntityEntry entityEntry);
    }
}
