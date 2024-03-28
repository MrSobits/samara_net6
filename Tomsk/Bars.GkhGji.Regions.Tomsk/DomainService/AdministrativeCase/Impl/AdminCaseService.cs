namespace Bars.GkhGji.Regions.Tomsk.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    using Castle.Windsor;

    public class AdminCaseService : IAdminCaseService
    {
        public IWindsorContainer Container { get; set; }

        public IDomainService<AdministrativeCase> AdminCase { get; set; }

        public IDomainService<DocumentGjiChildren> ChildrenDomain { get; set; }

        public IDomainService<AdministrativeCaseDescription> DescriptionDomain { get; set; }

        public IDataResult GetInfo(BaseParams baseParams)
        {
            var documentId = baseParams.Params.GetAs<long>("documentId");

            var adminCase = AdminCase.GetAll().FirstOrDefault(x => x.Id == documentId);

            var parentDoc =
                ChildrenDomain.GetAll().Where(x => x.Children.Id == documentId).Select(x => new { x.Parent.TypeDocumentGji, x.Parent.DocumentDate, x.Parent.DocumentNumber }).FirstOrDefault();

            string result = string.Empty;

            if (parentDoc != null)
            {
                result = string.Format(
                    "{0} №{1} от {2}",
                    parentDoc.TypeDocumentGji.GetEnumMeta().Display,
                    parentDoc.DocumentNumber,
                    parentDoc.DocumentDate.HasValue ? parentDoc.DocumentDate.Value.ToShortDateString() : null);
            }
            else
            {
                // Если АД сформировано не из документа то определяем так
                if (adminCase.Inspection != null)
                {
                    switch (adminCase.Inspection.TypeBase)
                    {

                        case TypeBase.CitizenStatement:
                            {
                                // если распоряжение создано на основе обращения граждан
                                GetInfoCitizenStatement(ref result, adminCase.Inspection.Id);
                            }

                            break;

                            // Если поднадобится еще доп условия писать тут
                    }
                }
            }

            return new BaseDataResult(new { parentDocument = result });
        }

        protected virtual void GetInfoCitizenStatement(ref string result, long inspectionId)
        {
            // распоряжение создано на основе обращения граждан,
            // поле planName = "Обращение № Номер ГЖИ"

            // Получаем из основания наименование плана
            var baseStatement =
                Container.Resolve<IDomainService<InspectionAppealCits>>()
                    .GetAll()
                    .Where(x => x.Inspection.Id == inspectionId)
                    .Select(x => new { x.AppealCits.NumberGji, x.AppealCits.DateFrom })
                    .FirstOrDefault();

            if (baseStatement != null)
            {
                result = string.Format("Обращение № {0} от {1}", baseStatement.NumberGji, baseStatement.DateFrom.ToDateTime().ToShortDateString());
            }
        }

        public IDataResult ListView(BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            /*
             * В качестве фильтров приходят следующие параметры
             * dateStart - Необходимо получить документы больше даты начала
             * dateEnd - Необходимо получить документы меньше даты окончания
             * realityObjectId - Необходимо получить документы по дому
            */

            var dateStart = baseParams.Params.GetAs("dateStart", DateTime.MinValue);
            var dateEnd = baseParams.Params.GetAs("dateEnd", DateTime.MinValue);
            var realityObjectId = baseParams.Params.GetAs<long>("realityObjectId", 0);

            var data =
                AdminCase.GetAll()
                    .WhereIf(realityObjectId > 0, x => x.RealityObject.Id == realityObjectId)
                    .WhereIf(dateStart > DateTime.MinValue, x => x.DocumentDate >= dateStart)
                    .WhereIf(dateEnd > DateTime.MinValue, x => x.DocumentDate <= dateEnd)
                    .Select(
                        x =>
                        new
                            {
                                x.Id,
                                x.State,
                                x.DocumentNumber,
                                x.DocumentDate,
                                Municipality = x.RealityObject.Municipality.Name,
                                RealityObject = x.RealityObject.Address,
                                Inspector = x.Inspector.Fio,
                                x.Inspection.TypeBase,
                                InspectionId = x.Inspection.Id,
                                x.TypeDocumentGji
                            })
                    .OrderIf(loadParam.Order.Length == 0, true, x => x.Municipality)
                    .Filter(loadParam, Container);

            var totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }
    }
}