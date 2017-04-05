using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using GoTech.Framework.Core.Configration;

namespace GoTech.Framework.Core
{
    public class DbTransaction : IDisposable
    {
        public IList<DbEntityEntryChanged> ChangedDBEntities;

        private readonly BaseGoTechContext context;
        private readonly EventHandler OnSavedChanges;
        private bool isCommited = false;

        public DbTransaction(BaseGoTechContext context, EventHandler OnSavedChanges)
        {
            this.context = context;
            this.OnSavedChanges = OnSavedChanges;
            this.context.Database.BeginTransaction();
            this.ChangedDBEntities = new List<DbEntityEntryChanged>();
        }
        public void AddEntity(DbEntityEntryChanged entity)
        {
            ChangedDBEntities.Add(entity);
        }
        public void Commit()
        {
            AppSetting appSetting = new AppSetting();
            if (appSetting.GoTech.BusinessRuleValidationIsEnabled)
                OnSavedChanges.Invoke(this, null);
            ChangedDBEntities.Clear();
            context.Database.CurrentTransaction.Commit();
            isCommited = true;
        }
        public void RollbackChangeTracker()
        {
            var changedEntities = context.ChangeTracker.Entries().Where(a => a.State != EntityState.Unchanged).ToList();
            foreach (var entity in changedEntities.Where(a => a.State == EntityState.Modified))
            {
                entity.CurrentValues.SetValues(entity.OriginalValues);
                entity.State = EntityState.Unchanged;
            }
            foreach (var entity in changedEntities.Where(a => a.State == EntityState.Added))
            {
                entity.State = EntityState.Detached;
            }
            foreach (var entity in changedEntities.Where(a => a.State == EntityState.Detached))
            {
                entity.State = EntityState.Unchanged;
            }

        }
        public void Rollback()
        {
            ChangedDBEntities.Clear();
            RollbackChangeTracker();
            if (context.Database.CurrentTransaction != null)
                context.Database.CurrentTransaction.Rollback();
        }
        public void Dispose()
        {
            ChangedDBEntities.Clear();
            if (!isCommited)
            {
                Rollback();
            }
            if (context.Database.CurrentTransaction != null)
                context.Database.CurrentTransaction.Dispose();
        }
    }
}
