namespace Bars.GkhGji.ViewModel
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;

    using Entities;
    using Enums;
    using NHibernate.Linq;

    public class PrescriptionViewModel : PrescriptionViewModel<Prescription>
    {
    }

    public class PrescriptionViewModel<T> : BaseViewModel<T>
        where T : Prescription
    {
        #region Dependency injection members

        public IDomainService<InspectionGji> InspectionGjiDomain { get; set; }

        #endregion

        public override IDataResult List(IDomainService<T> domain, BaseParams baseParams)
        {
            var serviceChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceDoc = Container.Resolve<IDomainService<DocumentGji>>();

            try
            {
                var loadParam = baseParams.GetLoadParam();

                var stageId = baseParams.Params.GetAs<long>("stageId");
                var parentId = baseParams.Params.GetAs<long>("parentId");

                List<long> prescriptionsHasNotDisposal = null;
                if (parentId > 0)
                {
                    // Если передан Идентификатор родительского документа значит
                    // Необходимо получить все Предписания порожденные из этого документа, у которых нет распоряжения

                    // все предписания по родительскому документу
                    var prescriptions = serviceChildren.GetAll()
                            .Where(x => x.Parent.Id == parentId
                                        && x.Children.TypeDocumentGji == TypeDocumentGji.Prescription)
                            .Select(x => x.Children.Id)
                            .Distinct()
                            .ToList();

                    var parentDoc = serviceDoc.GetAll()
                        .Where(x => x.Id == parentId && x.Stage.Parent != null)
                        .Select(x => new
                        {
                            x.TypeDocumentGji,
                            ParentStageId = x.Stage.Parent.Id,
                            InspectionId = x.Inspection.Id
                        }).FirstOrDefault();

                    if (parentDoc != null && parentDoc.TypeDocumentGji == TypeDocumentGji.ActCheck)
                    {
                        prescriptions.AddRange(domain.GetAll()
                            .Where(x => x.Inspection.Id == parentDoc.InspectionId && x.Stage.Parent.Id == parentDoc.ParentStageId)
                            .Select(x => x.Id)
                            .Distinct()
                            .ToList());
                    }

                    // предписания, у которых сформировано распоряжение
                    var prescriptionsHasDisposal = serviceChildren.GetAll()
                            .Where(x => prescriptions.Contains(x.Parent.Id)
                                        && x.Children.TypeDocumentGji == TypeDocumentGji.Disposal)
                            .Select(x => x.Parent.Id)
                            .ToList();

                    // предписания, у которых  не сформировано распоряжение
                    prescriptionsHasNotDisposal = prescriptions.Where(x => !prescriptionsHasDisposal.Contains(x)).ToList();
                }

                var data = domain
                    .GetAll()
                    .WhereIf(stageId > 0, x => x.Stage.Id == stageId)
                    .WhereIf(prescriptionsHasNotDisposal != null, x => prescriptionsHasNotDisposal.Contains(x.Id))
                    .Select(x => new
                    {
                        x.Id,
                        DocumentId = x.Id,
                        x.Inspection,
                        x.Stage,
                        x.TypeDocumentGji,
                        x.DocumentDate,
                        x.DocumentNumber
                    })
                    .Filter(loadParam, Container);

                int totalCount = data.Count();

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
            }
            finally
            {
                Container.Release(serviceChildren);
                Container.Release(serviceDoc);
            }
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs("id", 0L);
            var obj = domainService.GetAll().Fetch(x => x.Inspection)
                .ThenFetch(x => x.State)
                .FirstOrDefault(x => x.Id == id);

            // Для виджетов
            obj.TypeBase = obj.Inspection.TypeBase;
            obj.InspectionId = obj.Inspection.Id;

            return new BaseDataResult(obj);
        }
    }
}