namespace Bars.GkhGji.Regions.Tatarstan.InspectionRules.DocumentRules
{
    using Bars.GkhGji.InspectionRules;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Protocol;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;

    /// <summary>
    /// Правило создание документа 'Постановления' из документа 'Протокол'
    /// </summary>
    public class TatProtocolToTatResolutionRule : ProtocolToResolutionRule<TatProtocol, TatarstanResolution>
    {
        /// <inheritdoc />
        protected override void ResolutionAdditionalProcessing(TatProtocol protocol, TatarstanResolution resolution)
        {
            resolution.DocumentYear = protocol.DocumentYear;
            resolution.DocumentNum = protocol.DocumentNum;
            resolution.DocumentSubNum = protocol.DocumentSubNum;
            resolution.LiteralNum = protocol.LiteralNum;
            resolution.DateWriteOut = protocol.DateWriteOut;
            resolution.SurName = protocol.Surname;
            resolution.Name = protocol.Name;
            resolution.Patronymic = protocol.Patronymic;
            resolution.BirthDate = protocol.BirthDate;
            resolution.BirthPlace = protocol.BirthPlace;
            resolution.Address = protocol.FactAddress;
            resolution.CitizenshipType = protocol.CitizenshipType;
            resolution.Citizenship = protocol.Citizenship;
            resolution.SerialAndNumber = protocol.SerialAndNumber;
            resolution.IssueDate = protocol.IssueDate;
            resolution.IssuingAuthority = protocol.IssuingAuthority;
            resolution.Company = protocol.Company;
            resolution.Snils = protocol.Snils;
        }
    }
}