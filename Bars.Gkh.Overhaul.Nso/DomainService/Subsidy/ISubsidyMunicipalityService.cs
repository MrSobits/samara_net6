namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.IO;

    using Bars.B4;

    public interface ISubsidyMunicipalityService
    {
        IDataResult GetSubsidy(BaseParams baseParams);

        IDataResult CalcFinanceNeedBefore(BaseParams baseParams);

        IDataResult CalcValues(BaseParams baseParams);

        /// <summary>
        /// Корректировка ДПКР
        /// </summary>
        IDataResult CorrectDpkr(BaseParams baseParams);

        /// <summary>
        /// Проверка на выполненность расчета показателей
        /// </summary>
        bool CheckCalculation(BaseParams baseParams);
        
        Stream PrintReport(BaseParams baseParams);
    }
}