namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    public class DocumentGjiViewModel : BaseViewModel<DocumentGji>
    {
        public override IDataResult List(IDomainService<DocumentGji> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             В данном методе возможны слудующие параметры
             
             parentDocumentId - это означает что необходимо получить все дочерние документы по переданному Id родителя
             
             documentType - необходимо получить все документы по типу
             
             actCheckForContragentId - это означает что необходимо получить все акты проверки
                    - по проверкам данного контрагента,
                    - Результат проверки = Нарушения выявлены.
                    - Тип основания проверки = Требование прокуратуры
             
             prescriptionsForDispHeadId - это означает, что необходимо получить
                предписания по домам поручения руководства, у которых есть хотя бы одно нарушение без даты фактического исполнения
             
             protocolsForDispHeadId - это означает, что необходимо получить
                то протоколы по этому домам Поручения руководства, в которых во вкладке Реквизиты в поле «Документы переданы в суд» стоит «Да» 
                и есть хотя бы одно нарушение, у которого нет даты факт.исполнения.
            */

            var parentDocumentId = baseParams.Params.ContainsKey("parentDocumentId")
                                  ? baseParams.Params["parentDocumentId"].ToInt()
                                  : 0;

            var documentType = baseParams.Params.ContainsKey("documentType")
                                  ? baseParams.Params["documentType"].ToInt()
                                  : 0;

            var actCheckForContragentId = baseParams.Params.ContainsKey("actCheckForContragentId")
                                  ? baseParams.Params["actCheckForContragentId"].ToInt()
                                  : 0;

            var prescriptionsForDispHeadId = baseParams.Params.ContainsKey("prescriptionsForDispHeadId")
                                  ? baseParams.Params["prescriptionsForDispHeadId"].ToInt()
                                  : 0;

            var protocolsForDispHeadId = baseParams.Params.ContainsKey("protocolsForDispHeadId")
                                  ? baseParams.Params["protocolsForDispHeadId"].ToInt()
                                  : 0;

            var listForInspection = baseParams.Params.GetAs("listForInspection", false);

            var listIds = new List<long>();
            if (parentDocumentId > 0)
            {
                listIds.AddRange(Container.Resolve<IDomainService<DocumentGjiChildren>>().GetAll()
                                     .Select(x => x.Children.Id).ToList());
            }

            if (actCheckForContragentId > 0)
            {
                // Получаем идентификаторы актов проверки где выявлены нарушения по нужному нам контрагенту
                listIds.AddRange(Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                                     .Where(x => x.ActCheck.Inspection.Contragent.Id == actCheckForContragentId
                                            && x.HaveViolation == YesNoNotSet.Yes
                                            && x.ActCheck.Inspection.TypeBase == TypeBase.ProsecutorsClaim)
                                     .Select(x => x.ActCheck.Id).Distinct().ToList());
            }
            else if (prescriptionsForDispHeadId > 0)
            {
                // Для данной проверки распоряжения руководства получаем предписания для домов проверки у которых есть хотябы одно нарушение
                // без даты устранения

                // получаем список домов проверки
                var listRObjectIds = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                    .Where(x => x.Inspection.Id == prescriptionsForDispHeadId)
                    .Select(x => x.RealityObject.Id)
                    .ToList();

                listIds.AddRange(Container.Resolve<IDomainService<PrescriptionViol>>().GetAll()
                                     .Where(x => listRObjectIds.Contains(x.InspectionViolation.RealityObject.Id)
                                         && x.InspectionViolation.DateFactRemoval == null)
                                     .Select(x => x.Document.Id)
                                     .Distinct()
                                     .ToList());
            }
            else if (protocolsForDispHeadId > 0)
            {
                // Для данной проверки распоряжения руководства получаем предписания для домов проверки у которых есть хотябы одно нарушение
                // без даты устранения

                // Получаем список домов проверки
                var listRObjectIds = Container.Resolve<IDomainService<InspectionGjiRealityObject>>().GetAll()
                    .Where(x => x.Inspection.Id == protocolsForDispHeadId)
                    .Select(x => x.RealityObject.Id)
                    .ToList();

                // Получаем протоколы с нарушениями по этим домам если у нарушения нет даты факт устранения
                var listProtocolsIds = Container.Resolve<IDomainService<ProtocolViolation>>().GetAll()
                                         .Where(x => listRObjectIds.Contains(x.InspectionViolation.RealityObject.Id)
                                             && x.InspectionViolation.DateFactRemoval == null)
                                         .Select(x => x.Document.Id)
                                         .Distinct()
                                         .ToList();

                // Из полученных протоколов получаем те которые переданы в суд
                listIds.AddRange(Container.Resolve<IDomainService<Protocol>>().GetAll()
                                         .Where(x => listProtocolsIds.Contains(x.Id) && x.ToCourt)
                                         .Select(x => x.Id)
                                         .ToList());
            }

            var data = domainService.GetAll()
                .WhereIf(parentDocumentId > 0, x => listIds.Contains(x.Id))
                .WhereIf(documentType > 0, x => x.TypeDocumentGji == (TypeDocumentGji)documentType)
                .WhereIf(actCheckForContragentId > 0, x => listIds.Contains(x.Id))
                .WhereIf(prescriptionsForDispHeadId > 0, x => listIds.Contains(x.Id))
                .WhereIf(protocolsForDispHeadId > 0, x => listIds.Contains(x.Id))
                .WhereIf(listForInspection, x => x.TypeDocumentGji == TypeDocumentGji.ActCheck
                                                        || x.TypeDocumentGji == TypeDocumentGji.Prescription
                                                        || x.TypeDocumentGji == TypeDocumentGji.ActRemoval)
                .Select(x => new
                {
                    x.Id,
                    State = x.State.Name,
                    x.TypeDocumentGji,
                    x.DocumentDate,
                    DocumentDateStr = x.DocumentDate.ToDateTime().ToShortDateString(),
                    x.DocumentNumber
                })
                .Filter(loadParam, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}