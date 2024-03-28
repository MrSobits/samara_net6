namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    public class DisposalViewModel : GkhGji.ViewModel.DisposalViewModel
    {
        public IDomainService<DisposalProvidedDocNum> ServiceProvidedDocDate { get; set; }

        public IDomainService<ActCheck> actCheckDomain { get; set; }

        public IDomainService<DocumentGjiChildren> DocChildrenDomain { get; set; }

        public override IDataResult Get(IDomainService<Disposal> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id", 0);
            var obj = domainService.Get(id);

            // среди дочерних идентификаторов получаем либо ID общего акта проверки либо id акта проверки документа ГЖИ
            obj.ActCheckGeneralId = actCheckDomain.GetAll()
                .Where(y => DocChildrenDomain.GetAll()
                    .Any(x => x.Children.Id == y.Id
                        && x.Parent.Id == id 
                        && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck))
                .Where(x => (x.TypeActCheck == TypeActCheckGji.ActCheckGeneral || x.TypeActCheck == TypeActCheckGji.ActCheckDocumentGji))
                .Select(x => x.Id)
                .FirstOrDefault();

            if (obj.Inspection != null)
            {
                // Для виджетов
                obj.TypeBase = obj.Inspection.TypeBase;
                obj.InspectionId = obj.Inspection.Id;
            }

            obj.HasChildrenActCheck = DocChildrenDomain.GetAll().Count(x => x.Parent.Id == id && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck) > 0;

            var parentIds = new List<long>();

            if (obj.TypeDisposal == TypeDisposalGji.DocumentGji)
            {
                parentIds =
                    DocChildrenDomain.GetAll()
                                     .Where(x => x.Children.Id == id)
                                     .Select(x => x.Parent.Id)
                                     .Distinct()
                                     .ToList();
            }
                

            var provideDocumentsNum = ServiceProvidedDocDate.GetAll()
                .Where(x => x.Disposal.Id == id)
                .Select(x => x.ProvideDocumentsNum)
                .FirstOrDefault();
            
            return new BaseDataResult(new
                {
                    obj.ActCheckGeneralId,
                    obj.DateEnd,
                    obj.DateStart,
                    obj.Description,
                    obj.DocumentDateStr,
                    obj.DocumentNum,
                    obj.DocumentNumber,
                    obj.LiteralNum,
                    obj.DocumentSubNum,
                    obj.DocumentYear,
                    obj.Id,
                    obj.InspectionId,
                    obj.IssuedDisposal,
                    obj.KindCheck,
                    obj.ObjectVisitEnd,
                    obj.ObjectVisitStart,
                    obj.OutInspector,
                    obj.ParentDocumentsList,
                    obj.ResponsibleExecution,
                    obj.Stage,
                    obj.State,
                    obj.TypeAgreementProsecutor,
                    obj.TypeAgreementResult,
                    obj.TypeBase,
                    obj.TypeDisposal,
                    obj.TypeDocumentGji,
                    obj.DocumentDate,
                    ParentIds = parentIds,
                    ProvideDocumentsNum = provideDocumentsNum
            });
        }
    }
}