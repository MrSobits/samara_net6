namespace Bars.GkhGji.Regions.Nso.ViewModel
{
    using System.Linq;
    using B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Utils;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;
    using Entities;
    using Bars.B4.Utils;
    
    public class NsoDisposalViewModel : BaseViewModel<NsoDisposal>
    {
        public override IDataResult Get(IDomainService<NsoDisposal> domainService, BaseParams baseParams)
        {
            var serviceDocumentChildren = Container.ResolveDomain<DocumentGjiChildren>();
            var serviceActCheck = Container.ResolveDomain<ActCheck>();
            var serviceDisposalFactViolation = Container.ResolveDomain<DisposalFactViolation>();

            try
            {
                var id = baseParams.Params.GetAs<long>("id");
                var hasCildrenActChack = serviceDocumentChildren.GetAll()
                                       .Count(
                                           x =>
                                           x.Parent.Id == id && x.Children.TypeDocumentGji == TypeDocumentGji.ActCheck) > 0;

                var factViols = serviceDisposalFactViolation.GetAll()
                    .Where(x => x.Disposal.Id == id)
                    .Select(x => x.TypeFactViolation.Name)
                    .ToList()
                    .AggregateWithSeparator(", ");

                var factViolIds = serviceDisposalFactViolation.GetAll()
                    .Where(x => x.Disposal.Id == id)
                    .Select(x => x.TypeFactViolation.Id.ToStr())
                    .ToList()
                    .AggregateWithSeparator(", ");

                var obj =
                    domainService.GetAll()
                                 .Where(x => x.Id == id)
                                 .Select(
                                     x =>
                                     new
                                         {
                                             x.Id,
                                             TypeBase = (x.Inspection != null ? x.Inspection.TypeBase : TypeBase.Default),
                                             InspectionId = (x.Inspection != null ? x.Inspection.Id : 0),
                                             HasChildrenActCheck = hasCildrenActChack,
                                             TimeVisitStart =
                                         x.TimeVisitStart.HasValue ? x.TimeVisitStart.Value.ToShortTimeString() : "",
                                             TimeVisitEnd =
                                         x.TimeVisitEnd.HasValue ? x.TimeVisitEnd.Value.ToShortTimeString() : "",
                                             x.IssuedDisposal,
                                             x.ResponsibleExecution,
                                             x.DateStart,
                                             x.DateEnd,
                                             x.TypeDisposal,
                                             x.TypeAgreementProsecutor,
                                             x.TypeAgreementResult,
                                             x.KindCheck,
                                             x.TypeDocumentGji,
                                             x.Description,
                                             x.ObjectVisitStart,
                                             x.ObjectVisitEnd,
                                             x.OutInspector,
                                             x.DocumentDate,
                                             x.DocumentNum,
                                             x.DocumentNumber,
                                             x.LiteralNum,
                                             x.DocumentSubNum,
                                             x.DocumentYear,
                                             x.PoliticAuthority,
                                             x.DateStatement,
                                             TimeStatement = x.TimeStatement.HasValue ? x.TimeStatement.Value.ToShortTimeString() : "",
                                             x.State,
                                             x.NcNum,
                                             x.NcDate,
                                             x.NcNumLatter,
                                             x.NcDateLatter,
                                             x.NcObtained,
                                             x.NcSent,
                                             x.MotivatedRequestNumber,
                                             x.MotivatedRequestDate,
                                             x.PeriodCorrect,
                                             x.NoticeDateProtocol,
                                             x.NoticeDescription,
                                             x.NoticePlaceCreation,
                                             NoticeTimeProtocol = x.NoticeTimeProtocol.HasValue ? x.NoticeTimeProtocol.Value.ToShortTimeString() : "",
                                             FactViols = factViols,
                                             FactViolIds = factViolIds,
                                             x.ProsecutorDecNumber,
                                             x.ProsecutorDecDate,
                                         })
                                 .FirstOrDefault();

                if (obj != null && obj.PoliticAuthority != null)
                {
                    obj.PoliticAuthority.ContragentName = obj.PoliticAuthority.Contragent.Name;
                }

                return new BaseDataResult(obj);

            }
            finally
            {
                Container.Release(serviceDocumentChildren);
                Container.Release(serviceActCheck);
                Container.Release(serviceDisposalFactViolation);
            }
        }
    }
}