using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters;
using System.Collections.Generic;
using System.Linq;

namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Dict.Impl
{
    public class KnmCharacterDomainService : BaseDomainService<KnmCharacter>
    {
        public IDomainService<KnmCharacterKindCheck> KnmCharacterKindCheckDomainService { get; set; }

        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var saveParam = GetSaveParam(baseParams);
            var values = new List<KnmCharacter>();

            InTransaction(() =>
            {
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();

                    SaveInternal(value);
                    values.Add(value);

                    SaveValues(record.NonObjectProperties["KindCheck"], value);
                }
            });

            return new BaseDataResult(values);
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<KnmCharacter>();
            var saveParam = GetSaveParam(baseParams);

            InTransaction(() =>
            {
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

        /// <summary>
        /// Сохранение значений
        /// </summary>
        private void SaveValues(object vals, KnmCharacter value)
        {
            var knmCharacterKindChecksList = this.KnmCharacterKindCheckDomainService.GetAll()
                .Where(x => x.KnmCharacter.Id == value.Id)
                .GroupBy(x => x.KindCheckGji.Id)
                .ToDictionary(x => x.Key, z => z.FirstOrDefault());

            if (vals is List<object> atrValues)
            {
                foreach (var atrValue in atrValues)
                {
                    if (!(atrValue is DynamicDictionary val))
                        continue;

                    var id = val.GetAs<long>("Id");

                    // Если связка уже есть в бд, оставляем, если нет добавляем
                    if (!knmCharacterKindChecksList.ContainsKey(id))
                    {
                        this.KnmCharacterKindCheckDomainService.Save(new KnmCharacterKindCheck()
                        {
                            KnmCharacter = value,
                            KindCheckGji = new KindCheckGji { Id = id }
                        });
                    }
                    else
                    {
                        knmCharacterKindChecksList.Remove(id);
                    }
                }

                //Удаялем связки которые были в бд, но не передались с формы
                foreach (var item in knmCharacterKindChecksList.Values)
                {
                    this.KnmCharacterKindCheckDomainService.Delete(item.Id);
                }
            }
        }
    }
}