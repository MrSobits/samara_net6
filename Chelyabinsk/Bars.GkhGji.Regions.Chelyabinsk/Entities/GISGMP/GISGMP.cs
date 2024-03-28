using Bars.B4.DataAccess;
using Bars.Gkh.Entities;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Regions.Chelyabinsk.Enums;
using System;
using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

namespace Bars.GkhGji.Regions.Chelyabinsk.Entities
{
    public class GisGmp : BaseEntity
    {      
        /// <summary>
        /// ID сообщения в системе СМЭВ
        /// </summary>
        public virtual string MessageId { get; set; }
        
        /// <summary>
        /// ID начисления
        /// </summary>
        public virtual string ChargeId { get; set; }

        /// <summary>
        /// Тип запроса по начислению
        /// </summary>
        public virtual GisGmpChargeType GisGmpChargeType { get; set; }

        /// <summary>
        /// Текущее состояние запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// Текущее состояние запроса
        /// </summary>
        public virtual TypeLicenseRequest TypeLicenseRequest { get; set; }

        /// <summary>
        /// Ответ от сервера
        /// </summary>
        public virtual String Answer { get; set; }

        /// <summary>
        /// Ответ на запрос квитирования
        /// </summary>
        public virtual String ReconcileAnswer { get; set; }

        /// <summary>
        /// УИН трансфера
        /// </summary>
        public virtual String UIN { get; set; }

        /// <summary>
        /// УИН плательщика
        /// </summary>
        public virtual String AltPayerIdentifier { get; set; }

        /// <summary>
        /// УИН плательщика отправленный (поле 201)
        /// </summary>
        public virtual String AltPayerIdentifierSent { get; set; }

        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// Этап запроса СМЭВ
        /// </summary>
        public virtual String SMEVStage { get; set; }

        #region Отправка платежа

        /// <summary>
        /// Постановление
        /// </summary>
        public virtual DocumentGji Protocol { get; set; }

        /// <summary>
        /// Запрос на выдачу лицензии
        /// </summary>
        public virtual ManOrgLicenseRequest ManOrgLicenseRequest { get; set; }

        /// <summary>
        /// Запрос на переоформление/дубликат лицензии
        /// </summary>
        public virtual LicenseReissuance LicenseReissuance { get; set; }

        /// <summary>
        /// КБК
        /// </summary>
        public virtual String KBK { get; set; }

        /// <summary>
        /// КБК отправленный (поле 104)
        /// </summary>
        public virtual String KBKSent { get; set; }

        /// <summary>
        /// ОКТМО
        /// </summary>
        public virtual String OKTMO { get; set; }

        /// <summary>
        /// ОКТМО отправленный (поле 105)
        /// </summary>
        public virtual String OKTMOSent { get; set; }

        /// <summary>
        /// Информация о нормативном правовом (правовом) акте
        /// </summary>
        public virtual String LegalAct { get; set; }

        /// <summary>
        /// Информация о нормативном правовом (правовом) акте отправленная (поле 1010)
        /// </summary>
        public virtual String LegalActSent { get; set; }

        /// <summary>
        /// Дата счета
        /// </summary>
        public virtual DateTime BillDate { get; set; }

        /// <summary>
        /// Дата счета отправленная (поле 4)
        /// </summary>
        public virtual DateTime BillDateSent { get; set; }

        /// <summary>
        /// Назначение платежа (штраф, пени)
        /// </summary>
        public virtual String BillFor { get; set; }

        /// <summary>
        /// Назначение платежа (штраф, пени) отправленное (поле 24)
        /// </summary>
        public virtual String BillForSent { get; set; }

        /// <summary>
        /// Сумма начисления
        /// </summary>
        public virtual Decimal TotalAmount { get; set; }

        /// <summary>
        /// Сумма начисления отправленная (поле 7)
        /// </summary>
        public virtual Decimal TotalAmountSent { get; set; }

        /// <summary>
        /// Основание изменения. Обязателен для "Извещение об уточнении начисления" и "Деаннулирование начисления"
        /// </summary>
        public virtual string Reason { get; set; }

        //---------------------------реквизиты платежа, поидее дергается из постановления----------------------------

        /// <summary>
        /// Статус плательщика — реквизит 101 Распоряжения.
        /// </summary>
        public virtual String Status { get; set; }

        /// <summary>
        /// Статус плательщика отправленный — реквизит 101 Распоряжения (поле 101).
        /// </summary>
        public virtual String StatusSent { get; set; }

        /// <summary>
        /// Статус плательщика — реквизит 101 Распоряжения. выбор из справочника
        /// </summary>
        public virtual GISGMPPayerStatus GISGMPPayerStatus { get; set; }
        
        /// <summary>
        /// Показатель основания платежа — реквизит 106 Распоряжения.
        /// Допустимые значения: ТП, ЗД, БФ, ТР, РС, ОТ, РТ, ПБ, ПР, АП, АР, ИН, ТЛ, ЗТ, ДЕ, ПО, КТ, ИД, ИП, ТУ, БД, КП, ВУ, ДК, ПК, КК, ТК, ПД, КВ, 00, 0
        /// </summary>
        public virtual String PaytReason { get; set; }

        /// <summary>
        /// Показатель основания платежа отправленный — реквизит 106 Распоряжения (поле 106).
        /// Допустимые значения: ТП, ЗД, БФ, ТР, РС, ОТ, РТ, ПБ, ПР, АП, АР, ИН, ТЛ, ЗТ, ДЕ, ПО, КТ, ИД, ИП, ТУ, БД, КП, ВУ, ДК, ПК, КК, ТК, ПД, КВ, 00, 0
        /// </summary>
        public virtual String PaytReasonSent { get; set; }

        /// <summary>
        /// Показатель налогового периода или код таможенного органа, осуществляющего в соответствии с законодательством РФ функции по выработке государственной политики и нормативному регулированию, контролю и надзору в области таможенного дела – реквизит 107 Распоряжения.
        /// Если длина значения в поле 10 символов, то:
        /// •	символы 1-й и 2-й могут принимать значение: МС, КВ, ПЛ, ГД;
        /// •	символы 4-й и 5-й могут принимать значение: для месячных платежей номер месяца текущего отчетного года, для квартальных платежей - номер квартала, для полугодовых - номер полугодия;
        /// </summary>
        public virtual String TaxPeriod { get; set; }

        /// <summary>
        /// Показатель налогового периода или код таможенного органа отправленный (поле 107), осуществляющего в соответствии с законодательством РФ функции по выработке государственной политики и нормативному регулированию, контролю и надзору в области таможенного дела – реквизит 107 Распоряжения.
        /// Если длина значения в поле 10 символов, то:
        /// •	символы 1-й и 2-й могут принимать значение: МС, КВ, ПЛ, ГД;
        /// •	символы 4-й и 5-й могут принимать значение: для месячных платежей номер месяца текущего отчетного года, для квартальных платежей - номер квартала, для полугодовых - номер полугодия;
        /// </summary>
        public virtual String TaxPeriodSent { get; set; }

        /// <summary>
        /// Показатель номера документа – реквизит 108 Распоряжения.
        /// </summary>
        public virtual String TaxDocNumber { get; set; }

        /// <summary>
        /// Показатель номера документа отправленный – реквизит 108 Распоряжения (поле 108).
        /// </summary>
        public virtual String TaxDocNumberSent { get; set; }

        /// <summary>
        /// Показатель даты документа – реквизит 109 Распоряжения.
        /// Особенности заполнения:
        /// •	первые два символа обозначают календарный день(могут принимать значения от 01 до 31);
        /// •	4-й и 5-й символы - месяц(значения от 01 до 12);
        /// •	с 7-го по 10-й год;
        /// •	в 3-м и 6-м символах в качестве разделительных знаков проставляется точка(".")
        /// </summary>
        public virtual String TaxDocDate { get; set; }

        /// <summary>
        /// Показатель даты документа отправленный – реквизит 109 Распоряжения (поле 109).
        /// Особенности заполнения:
        /// •	первые два символа обозначают календарный день(могут принимать значения от 01 до 31);
        /// •	4-й и 5-й символы - месяц(значения от 01 до 12);
        /// •	с 7-го по 10-й год;
        /// •	в 3-м и 6-м символах в качестве разделительных знаков проставляется точка(".")
        /// </summary>
        public virtual String TaxDocDateSent { get; set; }

        /// <summary>
        /// коды платежей из распоряжения
        /// </summary>
        public virtual String PaymentType { get; set; }

        //---------------------------параметры плательщика----------------------------

        /// <summary>
        /// Тип плательщика
        /// </summary>
        public virtual PayerType PayerType { get; set; }

        //--------------параметры юр.лица / ИП--------------
        /// <summary>
        /// Наименование плательщика
        /// </summary>
        public virtual String PayerName { get; set; }

        /// <summary>
        /// Наименование плательщика отправленное (поле 8)
        /// </summary>
        public virtual String PayerNameSent { get; set; }

        public virtual String INN { get; set; }

        public virtual String KPP { get; set; }

        public virtual String KIO { get; set; }

        public virtual IdentifierType IdentifierType { get; set; } = IdentifierType.INN;

        /// <summary>
        /// Является ли резидентом РФ
        /// </summary>
        public virtual Boolean IsRF { get; set; }

        //---------------------------параметры физ.лица----------------------------
        /// <summary>
        /// Тип документа, удостоверяющего личность
        /// </summary>
        public virtual FLDocType FLDocType { get; set; }
        
        /// <summary>
        /// Тип документа, удостоверяющего личность
        /// </summary>
        public virtual PhysicalPersonDocType PhysicalPersonDocType { get; set; }

        /// <summary>
        /// Номер документа физлица
        /// </summary>
        public virtual String DocumentNumber { get; set; }

        /// <summary>
        /// Серия документа физлица
        /// </summary>
        public virtual String DocumentSerial { get; set; }

        #endregion

        #region Запрос платежей

        /// <summary>
        ///Тип платежей
        /// </summary>
        public virtual GisGmpPaymentsKind GisGmpPaymentsKind { get; set; } = GisGmpPaymentsKind.PAYMENT;

        /// <summary>
        ///Тип запросов оплат
        /// </summary>
        public virtual GisGmpPaymentsType GisGmpPaymentsType { get; set; }

        /// <summary>
        /// Оплаты по КБК
        /// </summary>
        public virtual String PaymentKBK { get; set; }

        /// <summary>
        /// Получить оплаты с
        /// </summary>
        public virtual DateTime? GetPaymentsStartDate { get; set; }

        /// <summary>
        /// Получить оплаты по
        /// </summary>
        public virtual DateTime? GetPaymentsEndDate { get; set; }
        #endregion

        /// <summary>
        /// Номера и даты всех документов
        /// </summary>
        public virtual String DocNumDate { get; set; }
    }
}
