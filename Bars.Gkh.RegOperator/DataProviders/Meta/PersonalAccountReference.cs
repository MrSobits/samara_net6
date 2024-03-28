namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// Справка по лицевому счету
    /// </summary>
    public class PersonalAccountReference
    {
        public string НомерСчета { get; set; }

        public string ВнешнийЛС { get; set; }

        public string ДатаОтчета { get; set; }

        public string Абонент { get; set; }

        public string Адрес { get; set; }

        public string Площадь { get; set; }

        public string Доля { get; set; }

        public string Тариф { get; set; }

        public string ДатаОткрытия { get; set; }

        public string ДатаЗакрытия { get; set; }

        public decimal НачисленоПоМинимальному { get; set; }

        public decimal НачисленоРешение { get; set; }

        public decimal НачисленоПени { get; set; }

        public decimal ИтогоНачислено { get; set; }

        public decimal УплаченоПоМинимальному { get; set; }

        public decimal УплаченоРешение { get; set; }

        public decimal УплаченоПени { get; set; }

        public decimal ИтогоУплачено { get; set; }

        public decimal ЗадолженностьПоМинимальному { get; set; }

        public decimal ЗадолженностьРешение { get; set; }

        public decimal ЗадолженностьПени { get; set; }

        public decimal ИтогоЗадолженность { get; set; }

        public decimal ЗачетСредств { get; set; }
    }
}