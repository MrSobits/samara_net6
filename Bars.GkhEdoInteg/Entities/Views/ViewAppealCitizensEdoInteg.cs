namespace Bars.GkhEdoInteg.Entities
{
    using System;

    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;

    /// <summary>
    ///  Данная вьюха предназначена для реестра Обращения граждан, чтобы вернуть Муниципальное образование,количество домов и признак Из Эдо
    /// </summary>
    public class ViewAppealCitizensEdoInteg : PersistentObject
    {
        public virtual AppealCits AppealCits { get; set; }

        /// <summary>
        /// Обращение создано из Эдо
        /// </summary>
        public virtual bool IsEdo { get; set; }

        /// <summary>
        /// Муниципальное образование из первого дома
        /// </summary>
        public virtual string Municipality { get; set; }

        /// <summary>
        /// Муниципальное образование из первого дома
        /// </summary>
        public virtual long? MunicipalityId { get; set; }

        /// <summary>
        /// количество домов во вкладке "Место возникновения"
        /// </summary>
        public virtual int? CountRealtyObj { get; set; }

        /// <summary>
        /// Количество вопросов
        /// </summary>
        public virtual int? QuestionsCount { get; set; }

        /// <summary>
        /// Адреса домов
        /// </summary>
        public virtual string RealObjAddresses { get; set; }

        /// <summary>
        /// Зональная жилищная инспекция
        /// </summary>
        public virtual ZonalInspection ZonalInspection { get; set; }

        /// <summary>
        /// Исполнитель
        /// </summary>
        public virtual Inspector Executant { get; set; }

        /// <summary>
        /// Проверяющий
        /// </summary>
        public virtual Inspector Tester { get; set; }

        /// <summary>
        /// Резолюция
        /// </summary>
        public virtual ResolveGji SuretyResolve { get; set; }

        /// <summary>
        /// Срок исполнения (Поручитель)
        /// </summary>
        public virtual DateTime? ExecuteDate { get; set; }

        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string Number { get; set; }

        /// <summary>
        /// Номер ГЖИ
        /// </summary>
        public virtual string NumberGji { get; set; }

        /// <summary>
        /// Дата обращения
        /// </summary>
        public virtual DateTime? DateFrom { get; set; }

        /// <summary>
        /// Контрольный срок
        /// </summary>
        public virtual DateTime? CheckTime { get; set; }

        /// <summary>
        /// Управляющая организация
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Корреспондент
        /// </summary>
        public virtual string Correspondent { get; set; }

        /// <summary>
        /// Количество тематик
        /// </summary>
        public virtual int? CountSubject { get; set; }

        /// <summary>
        /// Адрес из ЭДО
        /// </summary>
        public virtual string AddressEdo { get; set; }
    }
}