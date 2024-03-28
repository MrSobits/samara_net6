namespace Bars.GisIntegration.Base.Entities.HouseManagement
{
    using Bars.GisIntegration.Base.Entities;

    /// <summary>
    /// Документы, подтверждающие полномочия заключать договор
    /// </summary>
    public class RisVotingProtocolAttachment : BaseRisEntity
    {
        /// <summary>
        /// Ссылка на протокол общего собрания 
        /// </summary>
        public virtual RisVotingProtocol VotingProtocol { get; set; }

        /// <summary>
        /// Ссылка на вложение
        /// </summary>
        public virtual Attachment Attachment { get; set; }
    }
}
