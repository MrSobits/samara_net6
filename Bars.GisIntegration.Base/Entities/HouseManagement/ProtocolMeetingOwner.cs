namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Протокол собрания собственников
    /// </summary>
    public class ProtocolMeetingOwner : BaseRisEntity
    {
        /// <summary>
        /// Устав
        /// </summary>
        public virtual Charter Charter { get; set; }

        /// <summary>
        /// Вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }

        /// <summary>
        /// Договор управления
        /// </summary>
        public virtual RisContract Contract { get; set; }
    }
}