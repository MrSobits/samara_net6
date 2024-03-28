namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Таблица для конвертации связь страого external_id с историей статуса в новой
    /// </summary>
    public class StateHistoryExternal : BaseGkhEntity
    {
        /// <summary>
        /// Id статуса в новой 
        /// </summary>
        public virtual long StateHistoryId { get; set; }
    }
}
