namespace Bars.Gkh.RegOperator.Controllers.Import
{
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Utils;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService.AddressMatching;
    using Bars.Gkh.RegOperator.DomainService.Import.Ches;
    using Bars.Gkh.RegOperator.Entities.Import.Ches;

    /// <summary>
    /// Контроллер данных по импорту ЧЭС
    /// </summary>
    public class ChesImportController : B4.Alt.DataController<ChesImport>
    {
        public IChesImportService Service { get; set; }
        public IAddressMatcher AddressMatcher { get; set; }
        public IChesComparingService ComparingService { get; set; }
        public IChesAccountOwnerComparingService AccountOwnerComparingService { get; set; }
        public IDomainService<ChesNotMatchAccountOwner> ChesNotMatchAccountOwnerDomain { get; set; }
        public IDomainService<ChesNotMatchAddress> ChesNotMatchAddressDomain { get; set; }

        /// <summary>
        /// Вернуть информацию по начислениям за период
        /// </summary>
        public ActionResult ListChargeInfo(BaseParams baseParams)
        {
            return this.Service.ListChargeInfo(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Вернуть информацию по оплатам за период
        /// </summary>
        public ActionResult ListPaymentInfo(BaseParams baseParams)
        {
            return this.Service.ListPaymentInfo(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Вернуть оплаты за период
        /// </summary>
        public ActionResult PaymentsList(BaseParams baseParams)
        {
            return this.Service.PaymentsList(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Вернуть информацию по изменениям сальдо за период
        /// </summary>
        public ActionResult ListSaldoChangeInfo(BaseParams baseParams)
        {
            return this.Service.ListSaldoChangeInfo(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Вернуть информацию по перерасчетам за период
        /// </summary>
        public ActionResult ListRecalcInfo(BaseParams baseParams)
        {
            return this.Service.ListRecalcInfo(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Сопоставить адреса автоматически
        /// </summary>
        public ActionResult MatchAddresses(BaseParams baseParams)
        {
            var addresses = baseParams.Params.GetAs<long[]>("ids");
            var addressDtos = this.ChesNotMatchAddressDomain.GetAll()
                .Where(x => addresses.Contains(x.Id))
                .Select(x => new AddressMatchDto
                {
                    Address = x.ExternalAddress,
                    HouseGuid = x.HouseGuid
                })
                .ToArray();

            var result = this.AddressMatcher.MatchAddresses(addressDtos);
            this.ComparingService.ProcessAddressMatchAdded(result.Data.ToArray());

            return result.ToJsonResult();
        }

        /// <summary>
        /// Сопоставить абонентов автоматически
        /// </summary>
        public ActionResult MatchOwners(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<long[]>("ids");

            IDataResult<IEnumerable<ChesMatchAccountOwner>> result = null;
            this.Container.InTransaction(() =>
            {
                result = this.AccountOwnerComparingService.MatchAutomatically(this.ChesNotMatchAccountOwnerDomain.GetAll().Where(x => ids.Contains(x.Id)));
                result.Data.ForEach(x => this.ComparingService.ProcessOwnerMatchAdded(x));
            });

            return result?.ToJsonResult() ?? this.JsSuccess();
        }

        /// <summary>
        /// Экспортировать данные
        /// </summary>
        public ActionResult Export(BaseParams baseParams)
        {
            var result = this.Service.Export(baseParams);
            if (result.Success)
            {
                var reportResult = result as ReportResult;
                return new ReportStreamResult(reportResult.ReportStream, reportResult.FileName);
            }

            return result.ToJsonResult();
        }

        /// <summary>
        /// Экспортировать информацию об оплатах
        /// </summary>
        public ActionResult ExportPayments(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ChesPaymentsExport");
            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }

        /// <summary>
        /// Удалить загруженную секцию
        /// </summary>
        public ActionResult DeleteSection(BaseParams baseParams)
        {
            return this.Service.DeleteSection(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Получить сверку сальдо за период
        /// </summary>
        public ActionResult ListSaldoCheck(BaseParams baseParams)
        {
            return this.Js(this.Service.ListSaldoCheck(baseParams));
        }

        /// <summary>
        /// Загрузить выбранные сальдо в систему
        /// </summary>
        public ActionResult ImportSaldo(BaseParams baseParams)
        {
            return this.Service.ImportSaldo(baseParams).ToJsonResult();
        }

        /// <summary>
        /// Экспортировать информацию о начислениях
        /// </summary>
        public ActionResult ExportSaldo(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ChesSaldoExport");
            using (this.Container.Using(export))
            {
                return export?.ExportData(baseParams);
            }
        }

        /// <summary>
        /// Запустить проверку
        /// </summary>
        public ActionResult RunCheck(BaseParams baseParams)
        {
            return this.Service.RunCheck(baseParams).ToJsonResult();
        }
    }
}