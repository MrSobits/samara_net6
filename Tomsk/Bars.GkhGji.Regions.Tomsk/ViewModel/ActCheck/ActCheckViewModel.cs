namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class ActCheckViewModel : GkhGji.ViewModel.ActCheckViewModel
    {
        public IDomainService<ActCheckTime> ServiceActCheckTime { get; set; }

        public override IDataResult Get(IDomainService<ActCheck> domainService, BaseParams baseParams)
        {
            var intId = baseParams.Params.GetAs<long>("id", ignoreCase: true);

            var obj = domainService.Get(intId);

            // Тут мы получаем документ переданный в прокуратуру
            if (obj.ToProsecutor == YesNoNotSet.Yes)
            {
                var docRef =
                    Container.Resolve<IDomainService<DocumentGjiReference>>()
                        .GetAll()
                        .FirstOrDefault(x => x.TypeReference == TypeDocumentReferenceGji.ActCheckToProsecutor && (x.Document1.Id == intId || x.Document2.Id == intId));

                if (docRef != null)
                {
                    obj.ResolutionProsecutor = docRef.Document1.Id == intId ? docRef.Document2 : docRef.Document1;
                }
            }

            // Получаем поличество домов в акте
            var actRealityObjectService = Container.Resolve<IDomainService<ActCheckRealityObject>>();

            var cnt = actRealityObjectService.GetAll().Count(x => x.ActCheck.Id == intId);
            if (cnt == 1)
            {
                obj.ActCheckGjiRealityObject = actRealityObjectService.GetAll().FirstOrDefault(x => x.ActCheck.Id == intId);
            }

            var actCheckTime = ServiceActCheckTime.GetAll().FirstOrDefault(x => x.ActCheck.Id == intId);

            DateTime? time = null;

            if (actCheckTime != null)
            {
                time = new DateTime(1, 1, 1, actCheckTime.Hour, actCheckTime.Minute, 0);
            }

            return
                new BaseDataResult(
                    new
                        {
                            obj.Id,
                            obj.ActCheckGjiRealityObject,
                            obj.Area,
                            obj.TypeActCheck,
                            obj.Inspection,
                            obj.ResolutionProsecutor,
                            obj.ToProsecutor,
                            obj.DateToProsecutor,
                            obj.RealityObjectsList,
                            obj.ParentDocumentsList,
                            obj.TypeDocumentGji,
                            obj.State,
                            obj.Flat,
                            obj.DocumentDate,
                            obj.DocumentDateStr,
                            obj.DocumentNum,
                            obj.DocumentNumber,
                            obj.LiteralNum,
                            obj.DocumentSubNum,
                            obj.Stage,
                            CreationTime = time != null ? time.Value.ToString("HH:mm") : null
                        });
        }
    }
}