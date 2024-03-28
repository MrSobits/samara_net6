namespace Bars.GkhGji.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;

    /*
     * Вьюха для распоряжений вне инспекторской деятельности
     * на тот случай если понадобится выводить агрегированные показатели
     */
    public class ViewDisposalNullInspection : PersistentObject
    {
        /// <summary>
        /// Статус
        /// </summary>
        public virtual State State { get; set; }

        /// <summary>
        /// Распоряжение
        /// </summary>
        public virtual Disposal Disposal { get; set; }
    }
}