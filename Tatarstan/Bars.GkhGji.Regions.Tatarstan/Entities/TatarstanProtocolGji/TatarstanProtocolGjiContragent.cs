namespace Bars.GkhGji.Regions.Tatarstan.Entities.TatarstanProtocolGji
{
    using System;

    using Bars.Gkh.Entities;

    /// <summary>
    /// Сущность, расширяющая протокол ГЖИ РТ
    /// </summary>
    public class TatarstanProtocolGjiContragent : TatarstanProtocolGji
    {
        /// <summary>
        /// Организация
        /// </summary>
        public virtual Contragent Contragent { get; set; }

        /// <summary>
        /// ФИО законного представителя
        /// </summary>
        public virtual string DelegateFio { get; set; }

        /// <summary>
        /// Место работы, должность законного представителя
        /// </summary>
        public virtual string DelegateCompany { get; set; }

        /// <summary>
        /// Доверенность номер
        /// </summary>
        public virtual string ProcurationNumber { get; set; }

        /// <summary>
        /// Доверенность дата
        /// </summary>
        public virtual DateTime? ProcurationDate { get; set; }

        /// <summary>
        /// Ранее к административной ответственности по ч. 1 ст. 20.6 КоАП РФ привлекались
        /// </summary>
        public virtual bool DelegateResponsibilityPunishment { get; set; }
    }
}
