namespace Bars.GkhGji.Regions.Voronezh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;
    using Enums;
    using System;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Enums.SMEV;

    using Bars.B4.Modules.FileStorage;

    public class SMEVEGRUL : BaseEntity
    {
        /// <summary>
        /// Инициатор запроса
        /// </summary>
        public virtual Inspector Inspector { get; set; }

        /// <summary>
        /// дата запроса
        /// </summary>
        public virtual DateTime CalcDate { get; set; }

        /// <summary>
        /// дата рождения
        /// </summary>
        public virtual string INNReq { get; set; }

        // остальное для ответа
        /// <summary>
        /// ПолнНаимОПФ 
        /// </summary>
        public virtual string OPFName { get; set; }

        /// <summary>
        /// ИНН
        /// </summary>
        public virtual string INN { get; set; }

        /// <summary>
        /// КПП
        /// </summary>
        public virtual string KPP { get; set; }

        /// <summary>
        /// ОГРН
        /// </summary>
        public virtual string OGRN { get; set; }

        /// <summary>
        /// ДатаОГРН
        /// </summary>
        public virtual DateTime? OGRNDate { get; set; }

        /// <summary>
        /// ДатаВып Дата формирования выписки
        /// </summary>
        public virtual DateTime?  ResponceDate { get; set; }

        /// <summary>
        /// НаимЮЛПолн
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// НаимЮЛСокр
        /// </summary>
        public virtual string ShortName { get; set; }

        /// <summary>
        /// Сведения об адресе (месте нахождения)
        /// </summary>
        public virtual string AddressUL { get; set; }

        /// <summary>
        /// НаимСпОбрЮЛ Сведения о регистрации (образовании) юридического лица Способ образования 
        /// </summary>
        public virtual string CreateWayName { get; set; }

        /// <summary>
        /// НаимНО Наименование регистрирующего органа
        /// </summary>
        public virtual string RegOrgName { get; set; }

        /// <summary>
        /// АдрРО Адрес регистрирующего органа
        /// </summary>
        public virtual string AddressRegOrg { get; set; }

        /// <summary>
        /// КодНО Код регистрирующего органа
        /// </summary>
        public virtual string CodeRegOrg { get; set; }

        /// <summary>
        /// НаимСтатусЮЛ Сведения о статсуе Юр лица
        /// </summary>
        public virtual string StateNameUL { get; set; }

        /// <summary>
        /// СумКап Сведения о сумме уставного капиталла
        /// </summary>
        public virtual decimal AuthorizedCapitalAmmount { get; set; }

        /// <summary>
        /// НаимВидКап Сведения о наименовании уставного капиталла
        /// </summary>
        public virtual string AuthorizedCapitalType { get; set; }

        /// <summary>
        /// НаимВидДолжн Наименование вида должности
        /// </summary>
        public virtual string TypePozitionName { get; set; }

        /// <summary>
        /// НаимДолжн Наименование должности
        /// </summary>
        public virtual string Pozition { get; set; }

        /// <summary>
        /// СвФл ФИО должностного лица
        /// </summary>
        public virtual string FIO { get; set; }

        /// <summary>
        /// НаимОКВЭД Наименование вмдов деятельности
        /// </summary>
        public virtual string OKVEDNames { get; set; }

        /// <summary>
        /// Коды ОКВЭД
        /// </summary>
        public virtual string OKVEDCodes { get; set; }

        /// <summary>
        /// Ответ
        /// </summary>
        public virtual string Answer { get; set; }

        /// <summary>
        /// Статус запроса
        /// </summary>
        public virtual RequestState RequestState { get; set; }

        /// <summary>
        /// MessageId
        /// </summary>
        public virtual string MessageId { get; set; }

        /// <summary>
        /// Тип идентификатора
        /// </summary>
        public virtual InnOgrn InnOgrn { get; set; }

        /// <summary>
        /// XML выписки
        /// </summary>
        public virtual FileInfo XmlFile { get; set; }
    }
}
