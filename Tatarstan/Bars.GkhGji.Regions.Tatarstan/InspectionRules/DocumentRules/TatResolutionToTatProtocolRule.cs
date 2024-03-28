namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules
{
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    /// <summary>
    /// Правило создания документа 'Протокола' из документа 'Постановление'
    /// </summary>
    public class TatResolutionToTatProtocolRule : ResolutionToProtocolRule<TatarstanResolution, TatProtocol>
    {
        /// <inheritdoc />
        protected override void ProtocolAdditionalProcessing(TatarstanResolution resolution, TatProtocol protocol)
        {
            protocol.DocumentYear = resolution.DocumentYear;
            protocol.DocumentNum = resolution.DocumentNum;
            protocol.DocumentSubNum = resolution.DocumentSubNum;
            protocol.LiteralNum = resolution.LiteralNum;
            protocol.DateWriteOut = resolution.DateWriteOut;
            protocol.Surname = resolution.SurName;
            protocol.Name = resolution.Name;
            protocol.Patronymic = resolution.Patronymic;
            protocol.BirthDate = resolution.BirthDate;
            protocol.BirthPlace = resolution.BirthPlace;
            protocol.FactAddress = resolution.Address;
            protocol.CitizenshipType = resolution.CitizenshipType;
            protocol.Citizenship = resolution.Citizenship;
            protocol.SerialAndNumber = resolution.SerialAndNumber;
            protocol.IssueDate = resolution.IssueDate;
            protocol.IssuingAuthority = resolution.IssuingAuthority;
            protocol.Company = resolution.Company;
            protocol.Snils = resolution.Snils;
        }
    }
}