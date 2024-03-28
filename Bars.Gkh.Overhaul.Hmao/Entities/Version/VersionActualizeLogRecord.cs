namespace Bars.Gkh.Overhaul.Hmao.Entities.Version
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Запись лога актуализации версии программы
    /// </summary>
    public class VersionActualizeLogRecord : BaseEntity
    {
        /// <summary>
        /// Лог актуализации версии программы
        /// </summary>
        public virtual VersionActualizeLog ActualizeLog { get; set; }

        /// <summary>
        /// Действие
        /// </summary>
        public virtual string Action { get; set; }

        /// <summary>
        /// Жилой дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Код работы
        /// </summary>
        public virtual string WorkCode { get; set; }

        /// <summary>
        /// ООИ
        /// </summary>
        public virtual string Ceo { get; set; }

        /// <summary>
        /// Плановый год
        /// </summary>
        public virtual int PlanYear { get; set; }

        /// <summary>
        /// Изменение плановый год
        /// </summary>
        public virtual int ChangePlanYear { get; set; }

        /// <summary>
        /// Опубликованный год
        /// </summary>
        public virtual int PublishYear { get; set; }

        /// <summary>
        /// Изменение опубликованный год
        /// </summary>
        public virtual int ChangePublishYear { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal Volume { get; set; }

        /// <summary>
        /// Изменение объема
        /// </summary>
        public virtual decimal ChangeVolume { get; set; }

        /// <summary>
        /// Сумма
        /// </summary>
        public virtual decimal Sum { get; set; }

        /// <summary>
        /// Изменение суммы
        /// </summary>
        public virtual decimal ChangeSum { get; set; }

        /// <summary>
        /// Номер
        /// </summary>
        public virtual int Number { get; set; }

        /// <summary>
        /// Изменение номера
        /// </summary>
        public virtual int ChangeNumber { get; set; }
    }
}