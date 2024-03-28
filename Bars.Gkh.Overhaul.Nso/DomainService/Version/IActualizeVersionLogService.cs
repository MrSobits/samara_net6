namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System.Collections.Generic;

    using B4;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Overhaul.Nso.Enum;

    public interface IActualizeVersionLogService
    {
        FileInfo CreateLogFile(IEnumerable<ActualizeVersionLogRecord> LogRecords, BaseParams baseParams);
    }

    public class ActualizeVersionLogRecord
    {
        /// <summary>
        /// Действие актуализации
        /// </summary>
        public VersionActualizeType TypeAction { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public string Action { get; set; }

        /// <summary>
        /// Причина
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// ООИ
        /// </summary>
        public string Ceo { get; set; }

        /// <summary>
        /// Плановый год
        /// </summary>
        public int PlanYear { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public decimal Volume { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public decimal Sum { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public int Number { get; set; }

        /// <summary>
        /// Изменение ООИ
        /// </summary>
        public string ChangeCeo { get; set; }

        /// <summary>
        /// Изменение плановый год
        /// </summary>
        public int ChangePlanYear { get; set; }

        /// <summary>
        /// Изменение объема
        /// </summary>
        public decimal ChangeVolume { get; set; }

        /// <summary>
        /// Изменение объема
        /// </summary>
        public decimal ChangeSum { get; set; }

        /// <summary>
        /// Изменение номера
        /// </summary>
        public int ChangeNumber { get; set; }
    }
}
