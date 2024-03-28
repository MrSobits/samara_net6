namespace Bars.Gkh.Entities
{
    using System;

    /// <summary>
    /// Приборы учета жилого дома
    /// </summary>
    public class RealityObjectMeteringDevice : BaseGkhEntity
    {
        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Прибор учета
        /// </summary>
        public virtual MeteringDevice MeteringDevice { get; set; }

        /// <summary>
        /// Дата постановки на учет
        /// </summary>
        public virtual DateTime? DateRegistration { get; set; }

        /// <summary>
        /// Дата установки(год)
        /// </summary>
        public virtual int? DateInstallation { get; set; }

        /// <summary>
        /// Заводской номер прибора учёта
        /// </summary>
        public virtual string SerialNumber { get; set; }

        /// <summary>
        ///Внесение показаний в ручном режиме
        /// </summary>
        public virtual bool? AddingReadingsManually { get; set; }

        /// <summary>
        /// Обязательности поверки в рамках эксплуатации прибора учета
        /// </summary>
        public virtual bool? NecessityOfVerificationWhileExpluatation { get; set; }

        /// <summary>
        /// Номер лицевого счёта
        /// </summary>
        public virtual string PersonalAccountNum { get; set; }

        /// <summary>
        /// Дата первичной поверки
        /// </summary>
        public virtual DateTime? DateFirstVerification { get; set; }
    }
}
