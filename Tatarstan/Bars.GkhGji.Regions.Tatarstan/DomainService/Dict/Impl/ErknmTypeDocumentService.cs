namespace Bars.GkhGji.Regions.Tatarstan.DomainService.Dict.Impl
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.ErknmTypeDocuments;

    public class ErknmTypeDocumentService : BaseDomainService<ErknmTypeDocument>
    {
        public IDomainService<ErknmTypeDocumentKindCheck> ErknmTypeDocumentKindCheckDomain { get; set; }

        /// <inheritdoc />
        public override IDataResult Save(BaseParams baseParams)
        {
            var saveParam = GetSaveParam(baseParams);
            var values = new List<ErknmTypeDocument>();

            InTransaction(() =>
            {
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();

                    SaveInternal(value);
                    values.Add(value);

                    if (record.NonObjectProperties["KindCheck"] is List<object> checkedKind)
                    {
                        var ids = checkedKind.Select(x => x as DynamicDictionary).Select(x => x.GetAsId()).ToList();
                        SaveKindCheck(ids, value);
                    }
                }
            });

            return new BaseDataResult(values);
        }

        /// <inheritdoc />
        public override IDataResult Update(BaseParams baseParams)
        {
            var values = new List<ErknmTypeDocument>();
            var saveParam = GetSaveParam(baseParams);

            InTransaction(() =>
            {
                foreach (var record in saveParam.Records)
                {
                    var value = record.AsObject();
                    UpdateInternal(value);
                    values.Add(value);

                    if (record.NonObjectProperties["KindCheck"] is List<object> checkedKind)
                    {
                        var ids = checkedKind.Select(x => x as DynamicDictionary).Select(x => x.GetAsId()).ToList();
                        SaveKindCheck(ids, value);
                    }
                }
            });

            return new BaseDataResult(values);
        }

        /// <summary>
        /// Сохранение значений
        /// </summary>
        private IDataResult SaveKindCheck(List<long> kindCheckIds, ErknmTypeDocument erknmTypeDocument)
        {
            var kindCheck = this.ErknmTypeDocumentKindCheckDomain.GetAll()
                .Where(x => x.ErknmTypeDocument.Id == erknmTypeDocument.Id)
                .Select(x => new
                {
                    x.Id,
                    KindCheckId = x.KindCheck.Id
                })
                .ToDictionary(x => x.Id, x => x.KindCheckId);

            var kindCheckForAdd = kindCheckIds.Where(x => !kindCheck.Values.Contains(x));
            var kindCheckForDelete = kindCheck.Where(x => !kindCheckIds.Contains(x.Value))
                .Select(x => x.Key);

            kindCheckForDelete
                .ForEach(x => this.ErknmTypeDocumentKindCheckDomain.Delete(x));

            kindCheckForAdd
                .ForEach(x =>
                    this.ErknmTypeDocumentKindCheckDomain.Save(new ErknmTypeDocumentKindCheck
                    {
                        ErknmTypeDocument = new ErknmTypeDocument { Id = erknmTypeDocument.Id },
                        KindCheck = new KindCheckGji() { Id = x }
                    }));

            return new BaseDataResult();
        }
    }
}