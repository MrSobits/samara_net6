namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// информация о счете
    /// </summary>
    public class AccountInfo
    {
        /// <summary>
        /// Номер лс
        /// </summary>
        public string НомерЛС { get; set; }

        /// <summary>
        /// Адрес помещения
        /// </summary>
        public string АдресПомещения { get; set; }

        /// <summary>
        /// Тип помещения
        /// </summary>
        public string ТипПомещения { get; set; }

        /// <summary>
        /// Площадь помещения
        /// </summary>
        public decimal ПлощадьПомещения { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public string Примечание { get; set; }

        /// <summary>
        /// Тариф
        /// </summary>
        public decimal Тариф { get; set; }

        /// <summary>
        /// Базовый тариф
        /// </summary>
        public decimal БазовыйТариф { get; set; }

        /// <summary>
        /// Чистые начисления
        /// </summary>
        public decimal НачисленоБазовый { get; set; }

        public decimal НачисленоТарифРешения { get; set; }

        public decimal НачисленоПени { get; set; }


        public decimal ПерерасчетБазовый { get; set; }

        public decimal ПерерасчетТарифРешения { get; set; }

        public decimal ПришлоСоСпецсчета { get; set; }

        public decimal ПерерасчетПени { get; set; }


        public decimal ОплаченоБазовый { get; set; }

        public decimal ОплаченоТарифРешения { get; set; }

        public decimal ОплаченоПени { get; set; }


        public decimal ДолгБазовыйНаНачало { get; set; }

        /// <summary>
        /// задолженность + начислено + перерасчет - оплачено + корректировки
        /// </summary>
        public decimal ДолгБазовыйНаКонец { get; set; }

        public decimal ДолгТарифРешенияНаНачало { get; set; }

        public decimal ДолгТарифРешенияНаКонец { get; set; }

        public decimal ДолгПениНаНачало { get; set; }

        public decimal ДолгПениНаКонец { get; set; }


        public decimal СоцПоддержка { get; set; }


        public decimal ОтменыБазовый { get; set; }

        public decimal ОтменыТарифРешения { get; set; }

        public decimal ОтменыПени { get; set; }


        public decimal ОтменыКорректировкаБазовый { get; set; }

        public decimal ОтменыКорректировкаТарифРешения { get; set; }

        public decimal ОтменыКорректировкаПени { get; set; }


        public decimal КорректировкаБазовый { get; set; } //Изменение сальдо, Зачет средств за выполненные работы

        public decimal КорректировкаТарифРешения { get; set; }

        public decimal КорректировкаПени { get; set; }

        /// <summary>
        /// Перенос долга при слиянии
        /// </summary>
        public decimal СлияниеБазовый { get; set; } 

        public decimal СлияниеТарифРешения { get; set; }

        public decimal СлияниеПени { get; set; }

        /// <summary>
        /// НачисленоБазовый + ПерерасчетБазовый 
        /// </summary>
        public decimal ИтогоПоБазовомуТарифу { get; set; }

        /// <summary>
        /// НачисленоТарифРешения + ПерерасчетТарифРешения 
        /// </summary>
        public decimal ИтогоПоТарифуРешений { get; set; }

        /// <summary>
        /// НачисленоПени + ПерерасчетПени
        /// </summary>
        public decimal ИтогоПоПени { get; set; }

        /// <summary>
        /// ИтогоПоТарифу + ИтогоПоПени
        /// </summary>
        public decimal ИтогоКОплате { get; set; }

        /// <summary>
        /// Наименование работ
        /// </summary>
        public string НаименованиеРабот { get; set; }

        /// <summary>
        /// Идентификатор собственника
        /// </summary>
        public long OwnerId { get; set; }

        /// <summary>
        /// Идентификатор счета
        /// </summary>
        public long AccountId { get; set; }

        /// <summary>
        /// Идентификатор дома
        /// </summary>
        public long RoId { get; set; }

        /// <summary>
        /// Общая площадь жилых и нежилых помещений в доме
        /// </summary>
        public decimal? AreaLivingNotLivingMkd { get; set; }

        /// <summary>
        /// Тип помещения
        /// </summary>
        public int RoomType { get; set; }

        /// <summary>
        /// Адрес дома
        /// </summary>
        public string АдресДома { get; set; }

        public string НаселенныйПункт { get; set; }

        public string Улица { get; set; }

        public string Дом { get; set; }

        public string Литер { get; set; }

        public string Корпус { get; set; }

        public string Секция { get; set; }

        public string Квартира { get; set; }

        public string СтатусЛС { get; set; }

        /// <summary>
        /// Муниципальный район ЛС
        /// </summary>
        public string МрЛицевойСчет { get; set; }

        public string НомерКомнаты { get; set; }

        #region Old properties

        public decimal? НачисленоБазовыйПериод { get; set; }

        public decimal? НачисленоПениПериод { get; set; }

        public decimal? НачисленоТарифРешенияПериод { get; set; }

        public decimal? Оплачено { get; set; }

        public decimal? Перерасчёт { get; set; }

        public decimal? ПерерасчетБазовыйПериод { get; set; }

        public decimal? ПерерасчетПениПериод { get; set; }

        public decimal? ПерерасчетТарифРешенияПериод { get; set; }

        public decimal? Сумма { get; set; }

        public decimal? СуммаБазовый { get; set; }

        public decimal? СуммаПени { get; set; }

        public decimal? СуммаСверхБазового { get; set; }

        public decimal? ДолгНаНачало { get; set; }

        public decimal? ЗачетБазовый { get; set; }

        public decimal? ЗачетРешения { get; set; }

        #endregion
    }
}
