namespace Bars.Gkh.Overhaul.Tat.DomainService
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;

    /// <summary>
    /// Интерфейс сервиса работы с КПКР
    /// </summary>
    public interface IShortProgramRecordService
    {
        /// <summary>
        /// Метод формирвоания краткосрочной программы
        /// </summary>
        IDataResult CreateShortProgram(BaseParams baseParams);

        IDataResult ListWork(BaseParams baseParams);

        IDataResult AddWorks(BaseParams baseParams);

        /// <summary>
        /// Метод актуализирующий версию
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ActualizeVersion(BaseParams baseParams);

        IDataResult ListForMassStateChange(BaseParams baseParams);

        IDataResult MassStateChange(BaseParams baseParams);

        IDataResult GetYears(BaseParams baseParams);

        void CalculationCosts(Dictionary<string, decimal> dictServices, List<ShortRecordProxy> works);
    }
    
    public class ShortRecordProxy
    {
        public long Id { get; set; }

        public long RealityObjectId { get; set; }

        public long ShortProgramObjectId { get; set; }

        public long Stage1Id { get; set; }

        public VersionRecordStage1 Stage1 { get; set; }

        // наслучай если по одному виду работ нескольк озаписе 1 этапа
        // Например в 2 этапе 4 лифта, но вид работы только 1
        public List<VersionRecordStage1> Stage1List { get; set; }

        // неачльный год краткосрочной программы
        public int ShortYearStart { get; set; }

        // конечный год краткосрочно программы - на случай если в РТ перейдут не на 1год а несколько годов
        public int ShortYearEnd { get; set; } 

        public decimal Cost { get; set; }

        public decimal TotalCost { get; set; }

        public decimal ServiceCost { get; set; }

        public decimal Volume { get; set; }

        public long WorkId { get; set; }

        public string WorkName { get; set; }

        public TypeDpkrRecord TypeDpkrRecord { get; set; }
    }
}
