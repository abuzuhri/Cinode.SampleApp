using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using GoTech.Framework.Core.Configration;

namespace GoTech.Framework.Core
{
    public class BaseGoTechUnitOfWork : IDisposable
    {
        private readonly BaseGoTechContext context;
        private bool disposed;
        private Dictionary<Type, object> repositories;

        private event EventHandler OnSavingChanges;
        private event EventHandler OnSavedChanges;
        private DbTransaction transaction = null;

        public BaseGoTechUnitOfWork(BaseGoTechContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
            this.OnSavingChanges += new EventHandler(context_onSavingChanges);
            this.OnSavedChanges += new EventHandler(context_onSavedChanges);
        }

        //public DbRawSqlQuery SqlQuery(Type elementType, string sql, params object[] parameters)
        //{
        //    return context.Database.SqlQuery(elementType, sql, parameters);
        //}
        //public DbRawSqlQuery<TElement> SqlQuery<TElement>(string sql, params object[] parameters)
        //{
        //    return context.Database.SqlQuery<TElement>(sql, parameters);
        //}
        public int ExecuteSqlCommand(string sql, params object[] parameters)
        {
            return context.Database.ExecuteSqlCommand(sql, parameters);
        }
        //public async Task<int> ExecuteSqlCommandAsync(string sql, params object[] parameters)
        //{
        //    return await context.Database.ExecuteSqlCommandAsync(sql, parameters);
        //}

        public DbTransaction BeginDbTransaction()
        {
            transaction = new DbTransaction(context, OnSavedChanges);
            return transaction;
        }

        private static IBusinessRuleValidation GetBusinessRuleTransationValidation(object sender, Type type)
        {
            IBusinessRuleValidation validator = null;
            string strTypeName = type.Name;

            try
            {
                AppSetting appSetting = new AppSetting();

                string assemblyName = appSetting.GoTech.BusinessRuleValidationAssemblyname;
                string typeName = appSetting.GoTech.BusinessRuleValidationNamespace + "." + strTypeName + "BusinessRule";
                string fullyQualifiedName = typeName + ", " + assemblyName;
                Type validatorType = Type.GetType(fullyQualifiedName);
                validator = Activator.CreateInstance(validatorType) as IBusinessRuleValidation;
            }
            catch { }

            return validator;
        }
        private static IBusinessRuleValidation GetBusinessRuleValidation(object sender, EntityEntry dbEntityEntry)
        {
            IBusinessRuleValidation validator = null;

            string strTypeName = "";
            if (dbEntityEntry.State == EntityState.Added || dbEntityEntry.State == EntityState.Deleted)
                strTypeName = dbEntityEntry.Entity.GetType().Name;
            //else strTypeName = dbEntityEntry.Entity.GetType().BaseType.Name;

            try
            {
                AppSetting appSetting = new AppSetting();

                string assemblyName = appSetting.GoTech.BusinessRuleValidationAssemblyname;
                string typeName = appSetting.GoTech.BusinessRuleValidationNamespace + "." + strTypeName + "BusinessRule";
                string fullyQualifiedName = typeName + ", " + assemblyName;
                Type validatorType = Type.GetType(fullyQualifiedName);
                validator = Activator.CreateInstance(validatorType) as IBusinessRuleValidation;
            }
            catch { }

            return validator;
        }
        private void CheckingBusinessRule(object sender, ChangeState changeState)
        {
            if (context != null)
            {
                if (changeState == ChangeState.Saving)
                {
                    var changeEntities = context.ChangeTracker.Entries().Where(a => a.State == EntityState.Added || a.State == EntityState.Modified || a.State == EntityState.Deleted).ToList();
                    foreach (var entity in changeEntities)
                    {
                        if (transaction != null && transaction.ChangedDBEntities != null)
                        {
                            transaction.AddEntity(new DbEntityEntryChanged(entity));
                        }
                        IBusinessRuleValidation validator = GetBusinessRuleValidation(sender, entity);
                        if (validator != null)
                        {
                            switch (entity.State)
                            {
                                case EntityState.Added:
                                    validator.OnCreating(entity); break;
                                case EntityState.Modified:
                                    validator.OnUpdating(entity); break;
                                case EntityState.Deleted:
                                    validator.OnDeleting(entity); break;

                            }
                        }
                    }
                }
                else if (changeState == ChangeState.Saved && transaction != null && transaction.ChangedDBEntities != null)
                {
                    foreach (var ChangedObj in transaction.ChangedDBEntities)
                    {
                        IBusinessRuleValidation validator = GetBusinessRuleTransationValidation(sender, ChangedObj.entity.Entity.GetType());
                        if (validator != null)
                        {
                            switch (ChangedObj.state)
                            {
                                case EntityState.Added:
                                    validator.OnCreated(ChangedObj.entity); break;
                                case EntityState.Modified:
                                    validator.OnUpdated(ChangedObj.entity); break;
                                case EntityState.Deleted:
                                    validator.OnDeleted(ChangedObj.entity); break;

                            }
                        }
                    }
                }
            }
        }

        private void context_onSavingChanges(object sender, EventArgs e)
        {
            CheckingBusinessRule(sender, ChangeState.Saving);
        }
        private void context_onSavedChanges(object sender, EventArgs e)
        {
            CheckingBusinessRule(sender, ChangeState.Saved);
        }
        public void ClearBag<T>(ICollection<T> bag) where T : BaseGoTechEntity
        {
            if (bag != null && bag.Count > 0)
            {
                context.Set<T>().RemoveRange(bag);
            }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        public virtual void Save()
        {
            AppSetting appSetting = new AppSetting();
            if (appSetting.GoTech.BusinessRuleValidationIsEnabled)
                OnSavingChanges.Invoke(this, null);

            context.SaveChanges();
        }

        public async Task<int> SaveAsync()
        {
            AppSetting appSetting = new AppSetting();
            if (appSetting.GoTech.BusinessRuleValidationIsEnabled)
                OnSavingChanges.Invoke(this, null);

            return  await context.SaveChangesAsync();
        }
    
        public virtual void Dispose(bool disposing)
        {
            if (!disposed)
            {
                if (disposing)
                {
                    if (repositories != null)
                    {
                        repositories.Clear();
                    }
                    context.Dispose();
                }
            }
            disposed = true;
        }

        public BaseDataRepository<T> Repository<T>() where T : BaseGoTechEntity
        {
            if (repositories == null)
            {
                repositories = new Dictionary<Type, object>();
            }

            var type = typeof(T);

            if (!repositories.ContainsKey(type))
            {
                var repositoryType = typeof(BaseDataRepository<>);
                var repositoryInstance = Activator.CreateInstance(repositoryType.MakeGenericType(typeof(T)), context);
                repositories.Add(type, repositoryInstance);
            }
            return (BaseDataRepository<T>)repositories[type];
        }


        public enum ChangeState
        {
            Saving,
            Saved
        }

    }
}
