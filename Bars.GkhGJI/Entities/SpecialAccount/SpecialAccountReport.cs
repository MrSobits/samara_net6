namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Entities;
    using Enums;

    /// <summary>
    /// Отчет по спецсчетам
    /// </summary>
    public class SpecialAccountReport : BaseGkhEntity
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Банк
        /// </summary>
        public virtual string Author { get; set; }

        /// <summary>
        /// Ответственный исполнитель
        /// </summary>
        public virtual string Executor { get; set; }

        /// <summary>
        /// Сведения о сертификате
        /// </summary>
        public virtual string Sertificate { get; set; }

        // <summary>
        /// Месяц
        /// </summary>
        public virtual MonthEnums MonthEnums { get; set; }

        // <summary>
        /// Год
        /// </summary>
        public virtual YearEnums YearEnums { get; set; }


        // <summary>
        /// Год
        /// </summary>
        public virtual AmmountMeasurement AmmountMeasurement { get; set; }

        /// <summary>
        /// Подписанный XML файл
        /// </summary>
        public virtual FileInfo SignedXMLFile { get; set; }

        /// <summary>
        /// Дата принятия отчета ГЖИ
        /// </summary>
        public virtual DateTime? DateAccept { get; set; }

        // <summary>
        /// Рассчетная площадь
        /// </summary>
        public virtual Decimal Tariff { get; set; }

        /// <summary>
        /// Файл
        /// </summary>
        public virtual FileInfo File { get; set; }

        /// <summary>
        /// Подпись
        /// </summary>
        public virtual FileInfo Signature { get; set; }

        /// <summary>
        /// Сертификат
        /// </summary>
        public virtual FileInfo Certificate { get; set; }
    }
}