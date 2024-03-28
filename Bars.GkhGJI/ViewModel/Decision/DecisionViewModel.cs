namespace Bars.GkhGji.ViewModel
{
    using System;
    using System.Linq;
    using B4;
    using Entities;
    using Enums;

    // пустышка на тот случай если от этого класса наследовались в других регионах
    public class DecisionGjiViewModel : DecisionGjiViewModel<Decision>
    {
        // Внимание!! Код override писать в Generic калссе
    }

    // Genric Класс для тго чтобы сущность Disposal расширять через subclass В других регионах
    public class DecisionGjiViewModel<T> : BaseViewModel<T> where T : Decision
    {
        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var serviceDocumentChildren = Container.Resolve<IDomainService<DocumentGjiChildren>>();
            var serviceActCheck = Container.Resolve<IDomainService<ActCheck>>();

            try
            {
                var id = baseParams.Params.GetAs<long>("id");
                var obj = domainService.GetAll()
                               .Where(x => x.Id == id)
                               .Select(
                                   x =>
                                   new
                                   {
                                       x.DateEnd,
                                       x.DateStart,
                                       x.Description,
                                       x.DocumentDate,
                                       x.DocumentDateWithResultAgreement,
                                       x.DocumentNum,
                                       x.DocumentNumber,
                                       x.DocumentNumberWithResultAgreement,
                                       x.DocumentSubNum,
                                       x.DocumentYear,
                                       x.ERKNMID,
                                       x.ERPID,
                                       x.FioProcAproove,
                                       x.GisGkhGuid,
                                       x.GisGkhTransportGuid,
                                       x.Id,
                                       x.Inspection,
                                       x.IssuedDisposal,
                                       x.KindCheck,
                                       x.KindKNDGJI,
                                       x.LiteralNum,
                                       x.NcDate,
                                       x.NcNum,
                                       x.NcSent,
                                       x.ObjectCreateDate,
                                       x.ObjectEditDate,
                                       x.ObjectVersion,
                                       x.ObjectVisitEnd,
                                       x.ObjectVisitStart,
                                       x.PositionProcAproove,
                                       x.ProcAprooveDate,
                                       x.ProcAprooveFile,
                                       x.ProcAprooveNum,
                                       x.RequirDate,
                                       x.RequirNum,
                                       x.Stage,
                                       x.State,
                                       TimeVisitStart =
                                         x.TimeVisitStart.HasValue ? x.TimeVisitStart.Value.ToShortTimeString() : "",
                                       TimeVisitEnd =
                                         x.TimeVisitEnd.HasValue ? x.TimeVisitEnd.Value.ToShortTimeString() : "",
                                         x.TypeAgreementProsecutor,
                                         x.TypeAgreementResult,
                                         x.TypeDisposal,
                                         x.TypeDocumentGji,
                                         x.ProsecutorOffice,
                                       TimeStatement = x.TimeStatement.HasValue ? x.TimeStatement.Value.ToShortTimeString() : "",
                                       x.DateStatement,
                                       x.ProsecutorDecNumber,
                                       x.ProsecutorDecDate,
                                       x.PeriodCorrect,
                                       x.HourOfProceedings,
                                       x.MinuteOfProceedings,
                                       x.RiskCategory

                                   })
                                .FirstOrDefault(); 


                //// среди дочерних идентификаторов получаем либо ID общего акта проверки либо id акта проверки документа ГЖИ
                //obj.ActCheckGeneralId = serviceActCheck.GetAll()
                //    .Where(y => serviceDocumentChildren.GetAll()
                //        .Any(x => x.Children.Id == y.Id
                //            && x.Parent.Id == id
                //            && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck))
                //    .Where(x => (x.TypeActCheck == TypeActCheckGji.ActCheckGeneral || x.TypeActCheck == TypeActCheckGji.ActCheckDocumentGji))
                //    .Select(x => x.Id)
                //    .FirstOrDefault();

                //if (obj.Inspection != null)
                //{
                //    // Для виджетов
                //    obj.TypeBase = obj.Inspection.TypeBase;
                //    obj.InspectionId = obj.Inspection.Id;
                //}

                //obj.HasChildrenActCheck = serviceDocumentChildren.GetAll().Count(x => x.Parent.Id == id && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck) > 0;

                return new BaseDataResult(obj);
            }
            finally
            {
                Container.Release(serviceDocumentChildren);
                Container.Release(serviceActCheck);
            }
        }
    }
}