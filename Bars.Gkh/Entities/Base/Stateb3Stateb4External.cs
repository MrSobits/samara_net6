namespace Bars.Gkh.Entities
{
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;

    /// <summary>
    /// Таблица для конвертации связь статусов б3 с б4
    /// </summary>
    public class Stateb3Stateb4External : BaseEntity
    {
        /// <summary>
        /// Id статуса в б3 
        /// </summary>
        public virtual int StateB3Id { get; set; }

        /// <summary>
        /// Id статуса в б4
        /// </summary>
        public virtual State State { get; set; }
    }
}
