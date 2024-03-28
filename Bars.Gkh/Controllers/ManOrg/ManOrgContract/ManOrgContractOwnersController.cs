namespace Bars.Gkh.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Mvc;

    using B4;
    using B4.Modules.FileStorage;
    using DomainService;
    using Entities;

    /// <summary>
    /// Контроллер для ManOrgContractOwners
    /// </summary>
    public class ManOrgContractOwnersController : FileStorageDataController<ManOrgContractOwners>
    {
        /// <summary>
        /// Получить информацию по жилому дому
        /// </summary>
        /// <param name="baseParams">Параметры запроса</param>
        /// <returns>Результат запроса</returns>
        public ActionResult GetInfo(BaseParams baseParams)
        {
            var result = this.Container.Resolve<IManOrgContractOwnersService>().GetInfo(baseParams);
            return result.Success ? new JsonNetResult(result.Data) : JsonNetResult.Failure(result.Message);
        }

        public override ActionResult Update(BaseParams baseParams)
        {
            var matchFieldName = new Dictionary<string, string>
            {
                { "ProtocolFileInfo", "Протокол (Протокол)" },
                { "OwnersSignedContractFile", "Реестр собственников, подписавших договор (Протокол)" },
                { "FileInfo", "Договор управления (Договор управления)" },
                { "TerminationFile", "Файл (Расторжение договора)" },
                { "PaymentProtocolFile", "Протокол (Сведения о плате)" }
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
    }
}