namespace Bars.GkhGji.Regions.BaseChelyabinsk.Report.DisposalGji
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Disposal;

    public class ChelyabinskDisposalGjiStateToProsecReport : GkhGji.Report.DisposalGjiStateToProsecReport
    {
        public override void PrepareReport(ReportParams reportParams)
        {
            base.PrepareReport(reportParams);

            var nsoDisposalService = this.Container.Resolve<IDomainService<ChelyabinskDisposal>>();
            var nsoDocConfirmService = this.Container.Resolve<IDomainService<DisposalDocConfirm>>();

            try
            {
                var nsoDisposal = nsoDisposalService.Get(this.Disposal.Id);

                if (nsoDisposal != null)
                {
                    if (nsoDisposal.PoliticAuthority != null)
                    {
                        reportParams.SimpleReportParams["ОрганПрокуратуры"] = nsoDisposal.PoliticAuthority.Contragent.Name;
                    }
                    reportParams.SimpleReportParams["ДатаЗаявления"] = nsoDisposal.DateStatement.HasValue
                                                                           ? nsoDisposal.DateStatement.Value
                                                                                        .ToShortDateString()
                                                                           : string.Empty;

                    reportParams.SimpleReportParams["ВремяЗаявления"] = nsoDisposal.TimeStatement.HasValue
                                                                           ? nsoDisposal.TimeStatement.Value
                                                                                        .ToString("hh:mm")
                                                                           : string.Empty;

                    reportParams.SimpleReportParams["ПредоставляемыеДокументы"] =
                        nsoDocConfirmService.GetAll()
                                            .Where(x => x.Disposal.Id == this.Disposal.Id && x.DocumentName != null)
                                            .Select(x => x.DocumentName)
                                            .ToList()
                                            .Aggregate(string.Empty, (x, y) => x + (!string.IsNullOrEmpty(x) ? ", " + y : y));

                    var contragent = nsoDisposal.Inspection.Contragent;
                    if (contragent != null)
                    {
                        reportParams.SimpleReportParams["УправОргСокр"] = contragent.ShortName;
                        reportParams.SimpleReportParams["АдресКонтрагента"] = contragent.FiasJuridicalAddress.AddressName;
                        reportParams.SimpleReportParams["АдресКонтрагентаФакт"] = contragent.FiasFactAddress.AddressName;
                        reportParams.SimpleReportParams["НачалоПериодаВыезд"] =
                            nsoDisposal.DateStart.ToDateTime().ToString("«dd» MMMM yyyy", CultureInfo.CurrentCulture);
                    }

                    var issuedDisposal = nsoDisposal.IssuedDisposal;
                    if (issuedDisposal != null)
                    {
                        reportParams.SimpleReportParams["РуководительДолжность"] = issuedDisposal.Position;
                        reportParams.SimpleReportParams["РуководительФИОСокр"] = issuedDisposal.ShortFio;
                    }
                }
            }
            finally
            {
                this.Container.Release(nsoDisposalService);
                this.Container.Release(nsoDocConfirmService);
            }
        }
    }
}