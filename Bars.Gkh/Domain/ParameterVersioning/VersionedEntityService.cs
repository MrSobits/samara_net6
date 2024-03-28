namespace Bars.Gkh.Domain.ParameterVersioning
{
    using System;
    using System.Linq;
    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Modules.FileStorage;
    using B4.Modules.Security;
    using B4.Utils;
    using Castle.Windsor;
    using Entities;

    public class VersionedEntityService : IVersionedEntityService
    {
        public VersionedEntityService(
            IDomainService<EntityLogLight> logDomain,
            IFileManager fileManager,
            IUserIdentity userIdentity,
            IRepository<User> userRepo,
            IWindsorContainer container)
        {
            this.logDomain = logDomain;
            this.fileManager = fileManager;
            this.userIdentity = userIdentity;
            this.userRepo = userRepo;
            this.container = container;
        }

        public IDataResult SaveParameterVersion(BaseParams baseParams, bool updateEntity = true)
        {
            var className = baseParams.Params.GetAs<string>("className", ignoreCase: true);
            var propName = baseParams.Params.GetAs<string>("propertyName", ignoreCase: true);
            var value = baseParams.Params.GetAs<object>("value", ignoreCase: true);
            var id = baseParams.Params.GetAs<long>("entityId", ignoreCase: true);
            var factDate = baseParams.Params.GetAs<DateTime>("factDate", ignoreCase: true);

            // не дает обновить версионируемое поле при создании entityLogLight, 
            // если дата начала действия значения больше текущей
            var deferredUpdate = baseParams.Params.GetAs<bool>("deferredUpdate", false, ignoreCase: true);

            if (id == 0)
            {
                return new BaseDataResult(true);
            }

            string paramName;

            if (!VersionedEntityHelper.TryGetParameterName(className, propName, out paramName))
            {
                return new BaseDataResult(false, "Параметр не найден");
            }

            var result = new BaseDataResult();
            this.container.UsingForResolved<IDataTransaction>((c, tr) =>
            {
                try
                {
                    if (!deferredUpdate || factDate.Date <= DateTime.Now.Date)
                    {
                        this.UpdateEntity(id, className, propName, value, factDate, paramName);
                    }

                    var login = this.userRepo.Get(this.userIdentity.UserId).Return(u => u.Login);

                    var file = baseParams.Files.FirstOrDefault();

                    FileInfo fileInfo = null;

                    if (file.Value != null)
                    {
                        fileInfo = this.fileManager.SaveFile(file.Value);
                    }

                    var entityLogLight = new EntityLogLight()
                    {
                        ClassName = className,
                        EntityId = id,
                        PropertyName = propName,
                        PropertyValue = value.ToStr(),
                        DateActualChange = factDate,
                        DateApplied = this.GetDateApplied(),
                        Document = fileInfo,
                        ParameterName = paramName,
                        User = login.IsEmpty() ? "anonymous" : login
                    };

                    this.logDomain.Save(entityLogLight);

                    tr.Commit();
                }
                catch (ValidationException ex)
                {
                    tr.Rollback();
                    result = new BaseDataResult(false, ex.Message);
                }
                catch
                {
                    tr.Rollback();
                    throw;
                }
            });

            return result;
        }

        public virtual DateTime GetDateApplied()
        {
            return DateTime.UtcNow;
        }

        public IDataResult ListChanges(StoreLoadParams storeLoadParams)
        {
            var className = storeLoadParams.Params.GetAs<string>("className");
            var propName = storeLoadParams.Params.GetAs<string>("propName");
            var propNames = propName.Split(',');
            var entityId = storeLoadParams.Params.GetAs<long>("entityId");

            if (entityId == 0 || string.IsNullOrWhiteSpace(className) || string.IsNullOrWhiteSpace(propName))
            {
                return new BaseDataResult(true);
            }

            var result = this.logDomain.GetAll()
                .Where(x => x.EntityId == entityId)
                .Where(x => x.ClassName == className)
                .Where(x => propNames.Contains(x.PropertyName))
                .OrderIf(storeLoadParams.Order == null || storeLoadParams.Order.Fields == null || !storeLoadParams.Order.Fields.Any(), false, x => x.DateApplied)
                .Order(storeLoadParams);

            return new ListDataResult(result.Paging(storeLoadParams).ToList(), result.Count());
        }

        private void UpdateEntity(long id, string className, string propName, object value, DateTime factDate, string paramName)
        {
            var type = VersionedEntityHelper.GetTypeByClassName(className);

            if (type == null)
            {
                return;
            }

            var domainType = typeof(IDomainService<>).MakeGenericType(type);
            dynamic domain = this.container.Resolve(domainType);

            try
            {
                var entity = domain.Get(id);

                if (entity != null)
                {
                    using (new VersionedEntityChangeContext(id, className, propName, value, factDate, paramName))
                    {
                        object newValue = null;
                        var property = type.GetProperty(propName);

                        var converter =
                            typeof(ObjectParseExtention).GetMethod("To", new[] { typeof(object) });

                        if (converter != null)
                        {
                            converter = converter.MakeGenericMethod(property.PropertyType);
                            newValue = converter.Invoke(null, new[] { value });
                        }
                        else
                        {
                            newValue = Convert.ChangeType(value, property.PropertyType);
                        }

                        IDataResult validation = VersionedEntityHelper.Validate(entity, newValue, factDate, paramName);

                        if (!validation.Success)
                        {
                            throw new ValidationException(validation.Message);
                        }

                        property.SetValue(entity, newValue, new object[0]);

                        domain.Update(entity);
                    }
                }
            }
            finally
            {
                this.container.Release(domain);
            }
        }

        private readonly IDomainService<EntityLogLight> logDomain;
        private readonly IFileManager fileManager;
        private readonly IUserIdentity userIdentity;
        private readonly IRepository<User> userRepo;
        private readonly IWindsorContainer container;
    }
}