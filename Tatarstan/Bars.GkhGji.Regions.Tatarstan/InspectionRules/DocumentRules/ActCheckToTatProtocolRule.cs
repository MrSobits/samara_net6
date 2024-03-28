using Bars.GkhGji.Entities;
using Bars.GkhGji.InspectionRules;
using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;

namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules
{
    /// <summary>
    /// Правило создания документа 'Протокол' из документа 'Акт проверки'
    /// </summary>
    public class ActCheckToTatProtocolRule : BaseActCheckToProtocolRule<ActCheck, TatProtocol>
    {
        /// <inheritdoc/>
        protected override void ProtocolAdditionalProcessing(ActCheck document, TatProtocol protocol)
        {
            protocol.ProceedingsPlace = document.DocumentPlaceFias?.AddressName;
            protocol.DocumentYear = document.DocumentYear;
            protocol.DocumentNum = document.DocumentNum;
            protocol.LiteralNum = document.LiteralNum;
            protocol.DocumentSubNum = document.DocumentSubNum;
        }
    }
}