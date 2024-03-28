namespace Bars.Gkh.Overhaul.Hmao.Entities
{
    using System;
    
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Overhaul.Hmao.Enum;
    using Gkh.Entities;

    /// <summary>
    /// Протоколы собственников помещений МКД
    /// </summary>
    public class PropertyOwnerProtocols : BaseImportableEntity
    {
        /// <summary>
        /// Объект долгосрочной программы
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// тип протокола (не отображается)
        /// </summary>
        public virtual PropertyOwnerProtocolType TypeProtocol { get; set; }

        /// <summary>
        /// тип протокола (новый)
        /// </summary>
        public virtual OwnerProtocolType ProtocolTypeId { get; set; }

        /// <summary>
        /// Дата
        /// </summary>
        public virtual DateTime? DocumentDate { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual string DocumentNumber { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description  { get; set; }

        /// <summary>
        /// Файл (документ)
        /// </summary>
        public virtual FileInfo DocumentFile { get; set; }

        /// <summary>
        /// Количество голосов (кв.м.)
        /// </summary>
        public virtual decimal? NumberOfVotes { get; set; }

        /// <summary>
        /// Общее количество голосов (кв.м.)
        /// </summary>
        public virtual decimal? TotalNumberOfVotes { get; set; }

        /// <summary>
        /// Доля принявших участие (%)
        /// </summary>
        public virtual decimal? PercentOfParticipating { get; set; }

        /// <summary>
        /// Способ голосования
        /// </summary>
        public virtual VoteForm VoteForm { get; set; }

        /// <summary>
        /// Статус протокола
        /// </summary>
        public virtual ProtocolMKDState ProtocolMKDState { get; set; }

        /// <summary>
        /// Источник протокола
        /// </summary>
        public virtual ProtocolMKDSource ProtocolMKDSource { get; set; }

        /// <summary>
        /// Инициатор протокола
        /// </summary>
        public virtual ProtocolMKDIniciator ProtocolMKDIniciator { get; set; }

        /// <summary>
        /// Дата Регистрации
        /// </summary>
        public virtual DateTime? RegistrationDate { get; set; }

        /// <summary>
        /// Регистрационный номер
        /// </summary>
        public virtual string RegistrationNumber { get; set; }

        /// <summary>
        /// Сотрудник
        /// </summary>
        public virtual Inspector Inspector { get; set; }


    }
}