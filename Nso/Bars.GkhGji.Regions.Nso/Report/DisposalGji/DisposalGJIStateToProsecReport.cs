namespace Bars.GkhGji.Regions.Nso.Report.DisposalGji
{
    using System.Globalization;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.GkhGji.Regions.Nso.Entities;
    using Bars.GkhGji.Regions.Nso.Entities.Disposal;

    public class NsoDisposalGjiStateToProsecReport : GkhGji.Report.DisposalGjiStateToProsecReport
    {
        public override void PrepareReport(ReportParams reportParams)
        {
            base.PrepareReport(reportParams);

            var nsoDisposalService = Container.Resolve<IDomainService<NsoDisposal>>();
            var nsoDocConfirmService = Container.Resolve<IDomainService<DisposalDocConfirm>>();

            try
            {
                var nsoDisposal = nsoDisposalService.Get(Disposal.Id);

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
                                            .Where(x => x.Disposal.Id == Disposal.Id && x.DocumentName != null)
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
                Container.Release(nsoDisposalService);
                Container.Release(nsoDocConfirmService);
            }
        }
    }
}