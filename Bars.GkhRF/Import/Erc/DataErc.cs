namespace Bars.GkhRf.Import.Erc
{
    public class DataErc
    {
        public int Year { get; set; }

        public int Month { get; set; }

        /// <summary>
        /// Код дома ЕРЦ
        /// </summary>
        public string CodeErc { get; set; }

        /// <summary>
        /// Код улицы
        /// </summary>
        public string CodeStreet { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        public string HouseNum { get; set; }

        /// <summary>
        /// Тип оплаты(услуги)
        /// </summary>
        public string TypePayment { get; set; }

        /// <summary>
        /// Входящее сальдо
        /// </summary>
        public virtual decimal? IncomeBalance { get; set; }

        /// <summary>
        /// Исходящее сальдо
        /// </summary>
        public virtual decimal? OutgoingBalance { get; set; }

        /// <summary>
        /// Перерасчет прошлого периода
        /// </summary>
        public virtual decimal? Recalculation { get; set; }

        /// <summary>
        /// Начислено населению
        /// </summary>
        public virtual decimal? ChargePopulation { get; set; }

        /// <summary>
        /// Оплачено населением
        /// </summary>
        public virtual decimal? PaidPopulation { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal? TotalArea { get; set; }

        /// <summary>
        /// Строка из файла
        /// </summary>
        public string LineNum { get; set; }
    }
}
