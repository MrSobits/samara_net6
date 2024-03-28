namespace Bars.Gkh.RegOperator.Export
{
    using Bars.B4;
    using Bars.B4.Modules.DataExport.Domain;
    using DomainService;
    using System.Collections;
    using System.Linq;

    /// <summary>
    /// Экспорт реестра займов
    /// </summary>
    public class RealityObjectLoanDataExport : BaseDataExportService
    {
        /// <summary>
        /// Метод получения данных для Экспорта
        /// </summary>
        /// <param name="baseParams">Базовае параметры</param>
        /// <returns>Возвращает список согласно фильтрам</returns>
        public override IList GetExportData(BaseParams baseParams)
        {
            var realityObjectLoanViewService = this.Container.Resolve<IRealityObjectLoanViewService>();
            try
            {
                return realityObjectLoanViewService.List(baseParams, true).ToList();
            }
            finally
            {
                this.Container.Release(realityObjectLoanViewService);
            }
        }
    }
}
