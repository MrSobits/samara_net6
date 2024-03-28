namespace Bars.GkhGji.Regions.Tatarstan.ViewModel.Resolution
{
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.GkhGji.DomainService;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Regions.Tatarstan.Entities.Resolution;
    using System.Linq;

    public class TatarstanResolutionViewModel : ResolutionViewModel<TatarstanResolution>
    {
        public override IDataResult Get(IDomainService<TatarstanResolution> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            var protocolMvd = this.ProtocolMvdDomain.GetAll().FirstOrDefault(x => x.Inspection.Id == obj.Inspection.Id) as TatarstanProtocolMvd;
            var data = new
            {
                obj.Id,
                obj.DocumentNum,
                obj.AbandonReason,
                obj.ChangeReason,
                obj.BecameLegal,
                obj.Contragent,
                obj.DateTransferSsp,
                obj.DateWriteOut,
                obj.DeliveryDate,
                obj.Description,
                obj.DocumentDate,
                obj.DocumentDateStr,
                obj.DocumentNumSsp,
                obj.DocumentNumber,
                obj.LiteralNum,
                obj.DocumentSubNum,
                obj.DocumentTime,
                obj.DocumentYear,
                obj.Executant,
                obj.FineMunicipality,
                obj.GisUin,
                obj.Municipality,
                obj.Official,
                obj.Paided,
                obj.ParentDocumentsList,
                obj.PenaltyAmount,
                obj.PhysicalPerson,
                obj.PhysicalPersonInfo,
                obj.Sanction,
                obj.SectorNumber,
                obj.Stage,
                obj.State,
                obj.TypeDocumentGji,
                obj.TypeInitiativeOrg,
                obj.TypeTerminationBasement,
                obj.RulinFio,
                obj.RulingDate,
                obj.RulingNumber,
                obj.OffenderWas,
                obj.SanctionsDuration,
                ProtocolMvdSerialAndNumber = protocolMvd?.SerialAndNumber,
                ProtocolMvdBirthDate = protocolMvd?.BirthDate,
                ProtocolMvdIssueDate = protocolMvd?.IssueDate,
                ProtocolMvdBirthPlace = protocolMvd?.BirthPlace,
                ProtocolMvdIssuingAuthority = protocolMvd?.IssuingAuthority,
                ProtocolMvdCompany = protocolMvd?.Company,
                ProtocolMvdPhysicalPerson = protocolMvd?.PhysicalPerson,
                ProtocolMvdPhysicalPersonInfo = protocolMvd?.PhysicalPersonInfo,
                ProtocolMvdId = protocolMvd?.Id ?? 0,
                protocolMvd?.TypeExecutant,
                ProtocolMvdName = protocolMvd?.Name,
                ProtocolMvdSurName = protocolMvd?.SurName,
                ProtocolMvdPatronymic = protocolMvd?.Patronymic,
                ProtocolMvdCitizenship = protocolMvd?.Citizenship,
                ProtocolMvdCitizenshipType = protocolMvd?.CitizenshipType ?? CitizenshipType.RussianFederation,
                ProtocolMvdCommentary = protocolMvd?.Commentary,
                obj.PatternDict,
                obj.Name,
                obj.SurName,
                obj.Patronymic,
                obj.Citizenship,
                CitizenshipType = obj.CitizenshipType ?? CitizenshipType.RussianFederation,
                obj.SerialAndNumber,
                obj.BirthDate,
                obj.IssueDate,
                obj.BirthPlace,
                obj.IssuingAuthority,
                obj.Company,
                obj.Address,
                obj.Snils
            };

            return new BaseDataResult(data);
        }
    }
}