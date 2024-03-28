namespace Bars.Gkh1468.DomainService.Passport
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Enums;
    using Bars.Gkh1468.ProxyModels;
    using Castle.Windsor;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Microsoft.Extensions.Logging;

    public abstract class BaseProviderPassportRowService<T> : IBaseProviderPassportRowService<T> where T : BaseProviderPassportRow
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<T> DomainService { get; set; }

        public abstract IDomainService DomainServiceProviderPassport { get; }

        public IDomainService<MetaAttribute> DomainServiceMetaAttribute { get; set; }

        public IDomainService<Part> DomainServicePart { get; set; }

        /// <summary>
        /// Отдает значения для конкретной группы множествоенного аттрибута по значению группы multyMetaValue
        /// </summary>
        public IDataResult GetMultyMetaValues(BaseParams baseParams)
        {
            var providerPassportId = baseParams.Params["providerPassportId"].ToLong();
            var values = GetValues(DomainService, providerPassportId)
                .Select(x => new ElementValue
                {
                    MetaId = x.MetaAttribute.Id,
                    Value = x.Value,
                    ValueId = x.Id,
                }).ToArray();

            return new BaseDataResult(values);
        }

        public IDataResult GetMultyMetaValue(BaseParams baseParams)
        {
            var metaId = baseParams.Params["metaId"].ToLong();
            var partId = baseParams.Params["partId"].ToLong();
            var providerPassportId = baseParams.Params["providerPassportId"].ToLong();

            var metaAttributes = DomainServiceMetaAttribute.GetAll().Where(x => x.ParentPart.Id == partId).ToList();
            var meta = metaAttributes.FirstOrDefault(x => x.Id == metaId);
            var metaIds = GetChildren(meta, metaAttributes).Select(x => x.Id).ToList();
            if (meta != null)
            {
                metaIds.Add(meta.Id);
            }

            var values = GetValues(DomainService, providerPassportId).Where(x => metaIds.Contains(x.MetaAttribute.Id)).ToArray();
            var count = 0L;
            var current = 0L;
            if (values.Any())
            {
                var multyValues = values.Where(x => x.MetaAttribute.Id == metaId)
                .GroupBy(x => x.Value);
                count = multyValues.Count();
                current = multyValues.Min(x => x.Key.ToLong());
            }

            return new BaseDataResult(new { 
                data = values.Select(x => new ElementValue
                {
                    MetaId = x.MetaAttribute.Id,
                    Value = x.Value,
                    ValueId = x.Id,
                    ParentValue = x.ParentValue
                }).ToArray(),
                count, 
                current
            });
        }

        public IDataResult GetMetaValues(BaseParams baseParams)
        {
            var metaId = baseParams.Params["metaId"].ToLong();
            var isMulty = baseParams.Params["isMulty"].ToBool();
            var partId = baseParams.Params["partId"].ToLong();
            var parentValueId = baseParams.Params["parentValue"].ToLong();
            var providerPassportId = baseParams.Params["providerPassportId"].ToLong();

            var metaAttributes = DomainServiceMetaAttribute.GetAll().Where(x => x.ParentPart.Id == partId).ToList();
            var metaIds = GetChildren(metaAttributes.FirstOrDefault(x => x.Id == metaId), metaAttributes).Select(x => x.Id).ToList();

            var values = GetValues(DomainService, providerPassportId).Where(x => metaIds.Contains(x.MetaAttribute.Id))
                .WhereIf(isMulty || parentValueId > 0, x => x.ParentValue == parentValueId)
                .Select(x => new ElementValue
                {
                    MetaId = x.MetaAttribute.Id,
                    Value = x.Value,
                    ValueId = x.Id,
                    ParentValue = x.ParentValue
                }).ToArray();

            return new BaseDataResult(values);
        }

        public IDataResult DeleteMultyMetaValues(BaseParams baseParams)
        {
            var valueId = baseParams.Params["valueId"].ToLong();
            var value = DomainService.Get(valueId);
            
            var metaAttributes = DomainServiceMetaAttribute.GetAll()
                .Where(x => x.ParentPart.Id == value.MetaAttribute.ParentPart.Id)
                .ToList();

            var metaIds = GetChildren(metaAttributes.FirstOrDefault(x => x.Id == value.MetaAttribute.Id), metaAttributes, true)
                .Select(x => x.Id).ToList();

            var values = GetValues(DomainService, value.Passport.Id)
                .Where(x => metaIds.Contains(x.MetaAttribute.Id));

            var valuesForDelete = GetGroupValues(value.MetaAttribute.Id, value.Id, values);
            valuesForDelete.Add(value.Id);

            try
            {
                InTransaction(() => valuesForDelete.ForEach(x => DomainService.Delete(x)));
            }
            catch (Exception exc)
            {
                Container.Resolve<ILogger>().LogError(exc, "Ошибка удаления значения множественного аттрибута");
                return new BaseDataResult(false, "Ошибка удаления.");
            }
            
            return new BaseDataResult(true);
        }

        private IList<long> GetGroupValues(long metaId, long multyMetaValueId, IQueryable<T> values)
        {
            var result = new List<long>();
            var queryValues = values.Where(x =>
                x.ParentValue == multyMetaValueId &&
                x.MetaAttribute.Parent != null)
                .Where(x => x.MetaAttribute.Parent.Id == metaId 
                    || x.MetaAttribute.Parent.Parent.Id == metaId);
               

            result.AddRange(queryValues.Select(x => x.Id));

            foreach (var child in queryValues.Where(x => x.MetaAttribute.Type == MetaAttributeType.GroupedComplex))
            {
                result.AddRange(GetGroupValues(child.MetaAttribute.Id, child.Id, values));
            }

            return result;
        }

        private IEnumerable<MetaAttribute> GetChildren(MetaAttribute root, IEnumerable<MetaAttribute> listAttributes, bool allChildren=false)
        {
            var result = new List<MetaAttribute>();
            foreach (var meta in listAttributes.Where(x => x.Parent.Return(y => y.Id, 0) == root.Return(z => z.Id, 0)))
            {
                result.Add(meta);
                if (meta.Type != MetaAttributeType.GroupedComplex || allChildren)
                {
                    result.AddRange(GetChildren(meta, listAttributes, allChildren));
                }
            }

            return result;
        }

        protected abstract IQueryable<T> GetValues(IDomainService<T> domainService, long providerPassportId);

        protected void InTransaction(Action action)
        {
            using (var transaction = Container.Resolve<IDataTransaction>())
            {
                try
                {
                    action();

                    transaction.Commit();
                }
                catch (Exception exc)
                {
                    try
                    {
                        transaction.Rollback();
                    }
                    catch (TransactionRollbackException ex)
                    {
                        throw new DataAccessException(ex.Message, exc);
                    }
                    catch (Exception e)
                    {
                        throw new DataAccessException(
                            string.Format(
                                "Произошла неизвестная ошибка при откате транзакции: \r\nMessage: {0}; \r\nStackTrace:{1};",
                                e.Message,
                                e.StackTrace),
                            exc);
                    }

                    throw;
                }
            }
        }

        public IDataResult SaveRecord(BaseParams baseParams)
        {
            var result = new Dictionary<string, long>();

            InTransaction(() =>
            {
                var forSave = new Dictionary<string, T>();

                foreach (var item in this.GetSaveParam(baseParams).Records)
                {
                    forSave.Add(item.NonObjectProperties.GetAs<string>("FieldId"), item.AsObject());
                }

                var providerPassportId =
                    forSave.Values.Where(x => x.Passport != null).Select(x => x.Passport.Id).FirstOrDefault();

                foreach (var record in forSave)
                {
                    var value = record.Value;
                    if (value.Id > 0)
                    {
                        DomainService.Update(value);
                    }
                    else
                    {
                        DomainService.Save(value);
                    }

                    result.Add(record.Key, value.Id);
                }

                UpdateFillPercent(providerPassportId);
            });

            return new BaseDataResult(result);
        }

        public abstract void UpdateFillPercent(long providerPassportId);

        protected SaveParam<T> GetSaveParam(BaseParams baseParams, bool ignoreCase = false)
        {
            return baseParams.Params.Read<SaveParam<T>>().Execute(container => Converter.ToSaveParam<T>(container, ignoreCase));
        }
    }
}
