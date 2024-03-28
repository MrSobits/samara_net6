namespace Bars.Gkh.RegOperator.ViewModels.PersonalAccount.PayDoc
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Analytics.Reports.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities.PersonalAccount.PayDoc;

    /// <summary>
    /// Вьюмодель для <see cref="PaymentDocumentTemplate"/>
    /// </summary>
    public class PaymentDocumentTemplateViewModel : BaseViewModel<PaymentDocumentTemplate>
    {
        /// <inheritdoc />
        public override IDataResult List(IDomainService<PaymentDocumentTemplate> domainService, BaseParams baseParams)
        {
            var snapshotDomain = this.Container.ResolveDomain<PaymentDocumentSnapshot>();

            try
            {
                var loadParam = this.GetLoadParam(baseParams);

                var data = domainService.GetAll();

                var codedReportsKeys = data.Select(x => x.TemplateCode).Distinct().ToArray();

                var codedReports = this.Container.Resolve<ICodedReportService>()
                    .GetAll()
                    .Where(x => codedReportsKeys.Contains(x.Key))
                    .GroupBy(x => x.Key)
                    .ToDictionary(x => x.Key, x => x.Select(y => y.Name).First());

                //var snapshotsByPeriods = snapshotDomain
                //    .GetAll()
                //    .Select(x => x.Period.Id)
                //    .Distinct()
                //    .ToArray();

                var result = data
                    .Select(
                        x => new
                        {
                            x.Id,
                            x.TemplateCode,
                            ReportName = codedReports[x.TemplateCode],
                            PeriodName = x.Period.Name,
                            x.Period,
                            HasSnapshots = false
                        })
                    .ToArray()
                    .AsQueryable()
                    .Filter(loadParam, this.Container);

                return new ListDataResult(
                    result.Order(loadParam)
                        .OrderIf(loadParam.Order.Length == 0, true, x => x.Period.StartDate)
                        .Paging(loadParam)
                        .ToArray(),
                    result.Count());
            }
            finally
            {
                this.Container.Release(snapshotDomain);   
            }           
        }
    }
}