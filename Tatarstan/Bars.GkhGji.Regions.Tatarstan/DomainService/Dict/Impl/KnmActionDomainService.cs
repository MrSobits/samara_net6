using Bars.B4;
using Bars.B4.IoC;
using Bars.B4.Utils;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmActions;
using System.Collections.Generic;
using System.Linq;
using System;
using Bars.B4.DataAccess;

namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Dict.Impl
{
    using Bars.Gkh.Domain;

    public class KnmActionDomainService : BaseDomainService<KnmAction>
    {
        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams) => this.BaseSaveData(baseParams, this.SaveInternal);

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams) => this.BaseSaveData(baseParams, this.UpdateInternal);

        private IDataResult BaseSaveData(BaseParams baseParams, Action<KnmAction> action)
        {
            var values = new List<KnmAction>();
            var saveParam = this.GetSaveParam(baseParams);
            
            this.InTransaction(() =>
            {
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();
                    action.Invoke(value);
                    values.Add(value);

                    this.SaveValues<KnmActionKnmType>(record.NonObjectProperties["KnmType"], value);
                    this.SaveValues<KnmActionControlType>(record.NonObjectProperties["ControlType"], value);
                    this.SaveValues<KnmActionKindAction>(record.NonObjectProperties["KindAction"], value);
                }
            });
            
            return new BaseDataResult(values);
        }

        /// <summary>
        /// Сохранение значений
        /// </summary>
        private void SaveValues<TEntity>(object values, KnmAction value)
            where TEntity : KnmActionBundle, new()
        {
            if (values is string strValues && string.IsNullOrEmpty(strValues))
            {
                values = new List<object>();
            }

            if (values is List<object> atrValues)
            {
                var domainService = this.Container.Resolve<IDomainService<TEntity>>();
                using (this.Container.Using(domainService))
                {
                    var entitiesList = domainService.GetAll()
                        .Where(x => x.KnmAction.Id == value.Id)
                        .ToDictionary(x => x.GkhGjiEntity.Id, y => y.Id);

                    foreach (var atrValue in atrValues)
                    {
                        long id = 0;
                        
                        switch (atrValue)
                        {
                            case DynamicDictionary val:
                                id = val.GetAsId();
                                break;
                            case long itemId:
                                id = itemId;
                                break;
                            default:
                                continue;
                        }

                        // Если связка уже есть в бд, оставляем, если нет добавляем
                        if (!entitiesList.ContainsKey(id))
                        {
                            domainService.Save(
                                new TEntity
                                {
                                    KnmAction = value,
                                    GkhGjiEntity = new BaseEntity { Id = id }
                                });
                        }
                        else
                        {
                            entitiesList.Remove(id);
                        }
                    }

                    //Удаялем связки которые были в бд, но не передались с формы
                    foreach (var id in entitiesList.Values)
                    {
                        domainService.Delete(id);
                    }
                }
            }
        }
    }
}
