namespace Bars.GkhGji.Regions.Tatarstan.Export
{
    using System;
    using System.Collections;

    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using Bars.GkhGji.DomainService;

    public class TatarstanDisposalDataExport : BaseDataExportService
    {
        public override IList GetExportData(BaseParams baseParams)
        {
            var disposalService = baseParams.Params.GetAs<IDisposalService>("DisposalService");

            if (disposalService == null)
            {
                throw new Exception("Не найдена реализация сервиса получения данных для выгрузки");
            }

            var result = disposalService.ListView(baseParams);

            return result.Success
                ? (IList)result.Data
                : throw new Exception($"Произошла ошибка при выгрузке: {result.Message}");
        }
    }
}