namespace Bars.Gkh.Entities
{
    /// <summary>
    /// Таблица для конвертации связь старого родного Распоряжения и схлопнутого 
    /// </summary>
    public class DisposalExternal : BaseGkhEntity
    {
        /// <summary>
        /// новый идентификатор распоряжения
        /// </summary>
        public virtual string NewExternalId { get; set; }
    }
}