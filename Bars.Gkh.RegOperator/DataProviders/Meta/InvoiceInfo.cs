namespace Bars.Gkh.RegOperator.DataProviders.Meta
{
    /// <summary>
    /// Сущность для документа на оплату
    /// </summary>
    public class InvoiceInfo
    {
        public long AccountId { get; set; }

        public string ВнешнийЛС { get; set; }

        public long OwnerId { get; set; }

        public string Плательщик { get; set; }

        public string ФондСпецСчет { get; set; }

        public string МесяцГодНачисления { get; set; }

        public string ЛицевойСчет { get; set; }

        public string СтатусЛС { get; set; }

        public string АдресКвартиры { get; set; }

        public decimal? Тариф { get; set; }

        public decimal? БазовыйТариф { get; set; }

        public decimal? ОбщаяПлощадь { get; set; }

        public string НаименованиеПериода { get; set; }

        public string ДатаДокумента { get; set; }

        public string ДатаНачалаПериода { get; set; }

        public string ДатаОкончанияПериода { get; set; }

        public string АгентДоставФактАдрес { get; set; }

        public string НаселенныйПункт { get; set; }

        public string Улица { get; set; }

        public string Дом { get; set; }

        public string Литер { get; set; }

        public string Корпус { get; set; }

        public string Секция { get; set; }

        public string ТипПомещения { get; set; }

        /// <summary>
        /// Чистые начисления по базовому тарифу
        /// </summary>
        public decimal? НачисленоБазовый { get; set; }

        public decimal? НачисленоТарифРешения { get; set; }

        public decimal? НачисленоПени { get; set; }


        public decimal? ПерерасчетБазовый { get; set; }

        public decimal? ПерерасчетТарифРешения { get; set; }

        public decimal? ПришлоСоСпецсчета { get; set; }

        public decimal? ПерерасчетПени { get; set; }


        public decimal? ОплаченоБазовый { get; set; }

        public decimal? ОплаченоТарифРешения { get; set; }

        public decimal? ОплаченоПени { get; set; }


        public decimal? ДолгБазовыйНаНачало { get; set; }

        /// <summary>
        /// задолженность + начислено + перерасчет - оплачено + корректировки
        /// </summary>
        public decimal? ДолгБазовыйНаКонец { get; set; }

        public decimal? ДолгТарифРешенияНаНачало { get; set; }

        public decimal? ДолгТарифРешенияНаКонец { get; set; }

        public decimal? ДолгПениНаНачало { get; set; }

        public decimal? ДолгПениНаКонец { get; set; }


        public decimal? СоцПоддержка { get; set; }


        public decimal? ОтменыБазовый { get; set; }

        public decimal? ОтменыТарифРешения { get; set; }

        public decimal? ОтменыПени { get; set; }


        public decimal? ОтменыКорректировкаБазовый { get; set; }

        public decimal? ОтменыКорректировкаТарифРешения { get; set; }

        public decimal? ОтменыКорректировкаПени { get; set; }


        public decimal? КорректировкаБазовый { get; set; } //Изменение сальдо, Зачет средств за выполненные работы

        public decimal? КорректировкаТарифРешения { get; set; }

        public decimal? КорректировкаПени { get; set; }

        /// <summary>
        /// Перенос долга при слиянии
        /// </summary>
        public decimal? СлияниеБазовый { get; set; }

        public decimal? СлияниеТарифРешения { get; set; }

        public decimal? СлияниеПени { get; set; }

        /// <summary>
        /// Зачет средств за ранее выполненные работы
        /// </summary>
        public decimal? ЗачетСредствБазовый { get; set; }

        public decimal? ЗачетСредствТарифРешения { get; set; }

        /// <summary>
        /// НачисленоБазовый + НачисленоТарифРешения + ПерерасчетБазовый + ПерерасчетТарифРешения 
        /// </summary>
        public decimal? ИтогоПоТарифу { get; set; }

        /// <summary>
        /// НачисленоПени + ПерерасчетПени
        /// </summary>
        public decimal? ИтогоПоПени { get; set; }

        /// <summary>
        /// ИтогоПоТарифу + ИтогоПоПени
        /// </summary>
        public decimal? ИтогоКОплате { get; set; }


        public int Номер { get; set; }

        public int ПорядковыйНомерВГоду { get; set; }


        public string НаименованиеПолучателя { get; set; }

        public string ИННСобственника { get; set; }

        public string КППСобственника { get; set; }

        public string ИннПолучателя { get; set; }

        public string КппПолучателя { get; set; }

        public string ОргнПолучателя { get; set; }

        public string РсчетПолучателя { get; set; }

        public string АдресБанка { get; set; }

        public string НаименованиеБанка { get; set; }

        public string НаименованиеБанкаДляПечати { get; set; }

        public string БикБанка { get; set; }

        public string КсБанка { get; set; }

        /// <summary>
        /// Id расчетного счета дома
        /// </summary>
        public long RoCalcAccountId { get; set; }

        public string ТелефоныПолучателя { get; set; }

        public string АдресПолучателя { get; set; }

        public string EmailПолучателя { get; set; }

        public string WebSiteПолучателя { get; set; }

        public string Информация { get; set; }

        public string ЗначениеQRКода { get; set; }

        public decimal? УплаченоФКР { get; set; }

        public decimal? ПотраченоНаКР { get; set; }

        public string ШтрихКод { get; set; }


        public string ОплатитьДо { get; set; }

        public bool ПечататьАкт { get; set; }

        public string Индекс { get; set; }

        public string СпособФормированияФонда { get; set; }

        public string АгентДоставки { get; set; }


        public string ЕдиницаПериодаПросрочки { get; set; }

        public int ПериодПросрочки { get; set; }

        public string ПечататьУведомление { get; set; }

        public string УчитыватьСумму { get; set; }

        public string СтатусЗадолженности { get; set; }

        public int КоличествоДнейПросрочки { get; set; }

        public int КоличествоМесяцевПросрочки { get; set; }

        public int OwnerType { get; set; }

        public string Municipality { get; set; }

        public string Settlement { get; set; }

        public int RoomType { get; set; }

        public string НомерПомещения { get; set; }

        public string Примечание { get; set; }

        public string НомерДокумента { get; set; }

        /// <summary>
        /// Дата последней оплаты
        /// </summary>
        public string ДатаПоследнейОплаты { get; set; }

        /// <summary>
        /// Сумма начисленной льготы
        /// </summary>
        public decimal? Льгота { get; set; }

        /// <summary>
        /// Из справочника "Группа льготных категорий граждан"
        /// </summary>
        public string КодЛьготы { get; set; }

        //информация юр лица:
        public string Руководитель { get; set; }

        public string СуммаСтрокой { get; set; }

        public string ГлавБух { get; set; }

        public string РуководительПолучателя { get; set; }

        public string ГлБухПолучателя { get; set; }

        public string НаименованиеПолучателяКраткое { get; set; }

        /// <summary>
        /// Id контрагента 
        /// </summary>
        public long ContragentId { get; set; }

        public string АдресКонтрагента { get; set; }

        public string ИндексАдресаКонтрагента { get; set; }

        public string АдресКонтрагентаСИндексом { get; set; }

        public string ПочтовыйАдресКонтрагента { get; set; }

        public string ПочтовыйИндексКонтрагента { get; set; }

        public string ПериодНачислений { get; set; }

        public string Наименование { get; set; }

        public string РСчетБанка { get; set; }

        public string ОКПО { get; set; }

        public string ОКОНХ { get; set; }

        public string БИК { get; set; }

        public string КорреспондентскийСчет { get; set; }

        /// <summary>
        /// Фактический адрес контрагента
        /// </summary>
        public string ФактическийАдрес { get; set; }

        public string НаселенныйПунктФактАдрес { get; set; }

        public string УлицаФактАдрес { get; set; }

        public string ДомФактАдрес { get; set; }

        public string ЛитерФактАдрес { get; set; }

        public string КорпусФактАдрес { get; set; }

        public string СекцияФактАдрес { get; set; }

        public string НомерПомещенияФактАдрес { get; set; }

        public string ФамилияCобственника { get; set; }

        /// <summary>
        /// ИмяCобственника
        /// </summary>
        public string ИмяCобственника { get; set; }

        /// <summary>
        /// ОтчествоCобственника
        /// </summary>
        public string ОтчествоCобственника { get; set; }

        /// <summary>
        /// ФИОСобственника
        /// </summary>
        public string ФИОСобственника { get; set; }

        /// <summary>
        /// Дата закрытия периода
        /// </summary>
        public string ДатаЗакрытияПериода { get; set; }

        /// <summary>
        /// Информация выбирается по ещё не закрытому периоду
        /// </summary>
        public bool ПоОткрытомуПериоду { get; set; }

        public int КоличествоЛС { get; set; }

        public string НомерКомнаты { get; set; }
        /// <summary>
        /// Площадь помещения Для Смоленска
        /// </summary>
        public decimal? ПлощадьПомещения { get; set; }
        /// <summary>
        /// Доля собственности Для Смоленска
        /// </summary>
        public decimal? ДоляСобственности { get; set; }
        /// <summary>
        /// дляСмоленска
        /// </summary>
        public decimal? КолличетсвоДнейПросрочки { get; set; }
        /// <summary>
        /// дляСмоленска
        /// </summary>
        public decimal? СуммаЗадолженостиИзДолжников { get; set; }


        #region General old properties

        public decimal? ДолгНаКонец { get; set; }

        public decimal? Итого { get; set; }

        public decimal? Пени { get; set; }

        public decimal? НачисленоБазовыйПериод { get; set; }

        public decimal? НачисленоПениПериод { get; set; }

        public decimal? НачисленоТарифРешенияПериод { get; set; }

        public decimal? Перерасчет { get; set; }

        public decimal? ПерерасчетБазовыйПериод { get; set; }

        public decimal? ПерерасчетПениПериод { get; set; }

        public decimal? ПерерасчетТарифРешенияПериод { get; set; }

        public decimal? СальдоБазТарифНачало { get; set; }

        public decimal? СальдоТарифРешНачало { get; set; }

        public decimal? СуммаВсего { get; set; }

        #endregion

        #region Old properties for individual and one-room legal

        public decimal? ДолгНаНачало { get; set; }

        public decimal? Начислено { get; set; }

        public decimal? НачисленоСверхБазового { get; set; }

        public decimal? Оплачено { get; set; }

        public decimal? ПереплатаНаКонец { get; set; }

        public decimal? ПереплатаНаНачало { get; set; }

        public decimal? ПереплатаПениНаКонец { get; set; }

        public decimal? ПереплатаПениНаНачало { get; set; }

        #endregion

        #region Old properties for registry

        public decimal? НачисленоВсего { get; set; }

        public decimal? НачисленоПениВсего { get; set; }

        public decimal? ОбщийДебет { get; set; }

        public decimal? ОбщийДебетПени { get; set; }

        public decimal? ОбщийКредит { get; set; }

        public decimal? ОбщийКредитПени { get; set; }

        public decimal? ПерерасчетВсего { get; set; }

        #endregion
    }
}