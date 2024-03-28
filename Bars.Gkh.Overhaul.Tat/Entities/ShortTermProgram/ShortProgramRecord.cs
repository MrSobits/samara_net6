namespace Bars.Gkh.Overhaul.Tat.Entities
{
    using Bars.B4.DataAccess;
    using Bars.Gkh.Entities.Dicts;
    using Bars.Gkh.Overhaul.Tat.Enum;

    /// <summary>
    /// Запись краткосрочной программы
    /// </summary>
    public class ShortProgramRecord : BaseEntity
    {
        /// <summary>
        /// Ссылка на дом в краткосрочной программе
        /// </summary>
        public virtual ShortProgramRealityObject ShortProgramObject { get; set; }

        /// <summary>
        /// Ссылка на запись 1 этапа
        /// </summary>
        public virtual VersionRecordStage1 Stage1 { get; set; }

        /// <summary>
        /// Вид работы
        /// </summary>
        public virtual Work Work { get; set; }

        /// <summary>
        /// Объем
        /// </summary>
        public virtual decimal Volume { get; set; }

        /// <summary>
        /// Стоимость (то есть стоимость без улуг - чистая стоимость)
        /// </summary>
        public virtual decimal Cost { get; set; }

        /// <summary>
        /// Отдельно стоимость услуг
        /// </summary>
        public virtual decimal ServiceCost { get; set; }

        /// <summary>
        /// Общая стоимость - то есть стоимость вместе с улугами на эту работу
        /// </summary>
        public virtual decimal TotalCost { get; set; }

        /// <summary>
        /// тип записи ДПКР
        /// </summary>
        public virtual TypeDpkrRecord TypeDpkrRecord { get; set; }
    }
}