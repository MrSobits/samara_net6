namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Таблица для конвертации связь страого external_id  с статусом в новой
    /// </summary>
    public class StateExternal : BaseGkhEntity
    {
        /// <summary>
        /// Id статуса в новой 
        /// </summary>
        public virtual long StateId { get; set; }
    }
}
