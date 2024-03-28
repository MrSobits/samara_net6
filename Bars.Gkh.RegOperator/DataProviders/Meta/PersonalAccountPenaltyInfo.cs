namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// Операции по лицевому счету
    /// </summary>
    public class PersonalAccountPenaltyInfo
    {
        public string НаименованиеПериода { get; set; }

        public string Период { get; set; }

        public decimal Задолженность { get; set; }

        public int ЧислоДнейПросрочки { get; set; }

        public decimal СтавкаРефинансирования { get; set; }

        public decimal Итого { get; set; }
    }
}