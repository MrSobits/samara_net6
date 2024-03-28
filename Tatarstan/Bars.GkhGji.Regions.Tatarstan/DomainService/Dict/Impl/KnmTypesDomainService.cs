namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Dict.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

    public class KnmTypesDomainService : BaseDomainService<KnmTypes>
    {
        public IDomainService<KnmTypeKindCheck> KnmTypeKindCheckDomainService { get; set; }
        
        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var values = new List<KnmTypes>();
            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();
                    
                    SaveInternal(value);
                    values.Add(value);

                    SaveValues(record.NonObjectProperties["KindCheck"], value);
                }
            });

            return new SaveDataResult(values);
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<KnmTypes>();

            InTransaction(() =>
            {
                var saveParam = GetSaveParam(baseParams);
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();
                    UpdateInternal(value);
                    values.Add(value);

                    SaveValues(record.NonObjectProperties["KindCheck"], value);
                }
            });

            return new BaseDataResult(values);
        }

        private void SaveValues(object vals, KnmTypes value)
        {
            var atrValues = vals as List<object>;
            
            // В этом словаре будут существующие виды проверок связанные с видом кнм
            // key - идентификатор вида проверки
            // value - идентификатор вида кнм
            var kindChecksList = this.KnmTypeKindCheckDomainService.GetAll()
                .Where(x => x.KnmTypes.Id == value.Id)
                .GroupBy(x=>x.KindCheckGji.Id)
                .ToDictionary(x=>x.Key, z => z.FirstOrDefault());
                
            if (atrValues != null)
            {
                foreach (var atrValue in atrValues)
                {
                    var val = atrValue as DynamicDictionary;

                    if (val == null) 
                        continue;

                    var id = val.GetAs<long>("Id");

                    // Если связка уже есть в бд, оставляем, если нет добавляем
                    if (!kindChecksList.ContainsKey(id))
                    {
                        this.KnmTypeKindCheckDomainService.Save(new KnmTypeKindCheck()
                        {
                            KnmTypes = value, 
                            KindCheckGji = new KindCheckGji { Id = id}
                        });
                    }
                    else
                    {
                        kindChecksList.Remove(id);
                    }
                }
                
                //Удаялем связки которые были в бд, но не передались с формы
                foreach (var item in kindChecksList.Values)
                {
                    this.KnmTypeKindCheckDomainService.Delete(item.Id);
                }
            }
        }
    }
}