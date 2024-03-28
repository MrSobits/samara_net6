namespace Bars.GkhGji.Entities
{
    using System;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    /// <summary>
    /// Обращениям граждан - Предостережение
    /// </summary>
    public class PreventiveVisit : DocumentGji
    {
        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual TypePreventiveAct TypePreventiveAct { get; set; }

        /// <summary>
        /// Контрагент
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// Объект проверки
        /// </summary>
        public virtual PersonInspection PersonInspection { get; set; }

        /// <summary>
        /// Физическое лицо
        /// </summary>
        public virtual string PhysicalPerson { get; set; }

        /// <summary>
        /// INN Физического лица
        /// </summary>
        public virtual string PhysicalPersonINN { get; set; }

        /// <summary>
        /// Реквизиты физ. лица
        /// </summary>
        public virtual string PhysicalPersonInfo { get; set; }

        /// <summary>
        /// Адрес Физическое лицо
        /// </summary>
        public virtual string PhysicalPersonAddress { get; set; }

        /// <summary>
        /// место составления акта
        /// </summary>
        public virtual string ActAddress { get; set; }

        /// <summary>
        /// использованы дистационные методы
        /// </summary>
        public virtual bool UsedDistanceTech { get; set; }

        /// <summary>
        /// описание дистационного мероприятия
        /// </summary>
        public virtual string DistanceDescription { get; set; }

        /// <summary>
        /// дата дистационного мероприятия
        /// </summary>
        public virtual DateTime? DistanceCheckDate { get; set; }

        /// <summary>
        /// Номер в ЕРКНМ
        /// </summary>
        public virtual string ERKNMID { get; set; }

        /// <summary>
        /// GUID в ЕРКНМ
        /// </summary>
        public virtual string ERKNMGUID { get; set; }

        /// <summary>
        /// выгружено в еркнм
        /// </summary>
        public virtual bool SentToERKNM { get; set; }

        /// <summary>
        /// Аксес гуид в ЕРКНМ
        /// </summary>
        public virtual string AccessGuid { get; set; }

        /// <summary>
        /// Ссылка на видео
        /// </summary>
        public virtual string VideoLink { get; set; }

        /// <summary>
        /// Вид контроля/надзора
        /// </summary>
        public virtual KindKNDGJI KindKND { get; set; }

    }
}