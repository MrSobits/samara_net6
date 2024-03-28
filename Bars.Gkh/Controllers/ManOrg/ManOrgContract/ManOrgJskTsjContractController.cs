namespace Bars.Gkh.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Modules.FileStorage;
    using Bars.B4.Modules.DataExport.Domain;
    using DomainService;
    using Entities;

    /// <summary>
    /// Контролер "Управление домами (ТСЖ / ЖСК)"
    /// </summary>
    public class ManOrgJskTsjContractController : FileStorageDataController<ManOrgJskTsjContract>
    {
        /// <summary>
        /// Возвращает объект недвижимости договора
        /// </summary>
        /// <returns></returns>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Жилой дом</returns>
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = Container.Resolve<IManOrgJskTsjContractService>().GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        /// <summary>
        /// Проверка на дату договора управления 
        /// </summary>
        /// <param name="baseParams">Базовые параметры</param>
        /// <returns>Результат проверки</returns>
        public ActionResult VerificationDate(BaseParams baseParams)
        {
            var result = Container.Resolve<IManOrgJskTsjContractService>().VerificationDate(baseParams);
            return result.Success ? JsonNetResult.Success : JsonNetResult.Failure(result.Message);
        }

        public override ActionResult Update(BaseParams baseParams)
        {
            var matchFieldName = new Dictionary<string, string>
            {
                { "FileInfo", "Файл (Реквизиты)" },
                { "ProtocolFileInfo", "Протокол (Протокол)" },
                { "TerminationFile", "Файл (Расторжение договора)" },
                { "CompanyPaymentProtocolFile", "Протокол собрания (платежи членов тов., кооп.)" },
                { "PaymentProtocolFile", "Протокол собрания (платежи собственников, не являющихся членами тов., кооп.)" }
            };

            var fieldNames = baseParams.Files?
                .Where(x => matchFieldName.ContainsKey(x.Key) && x.Value.Data.Length == 0)?
                .Select(y => $"<p><b>{matchFieldName[y.Key]}:</b> Загружаемый файл пустой.</p>");

            if (fieldNames?.Any() ?? false)
            {
                var msg = string.Join("", fieldNames);
                throw new Exception($"<p>Следующие поля содержат ошибки:</p>{msg}");
            }

            return base.Update(baseParams);
        }

        public ActionResult Export(BaseParams baseParams)
        {
            var export = this.Container.Resolve<IDataExportService>("ManOrgContactDataExport");
            return export != null ? export.ExportData(baseParams) : null;
        }
    }
}