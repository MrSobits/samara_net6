namespace Bars.GkhGji.DomainService
{
    using B4;
    using B4.Utils;
    using Bars.Gkh.Domain;
    using Entities;
    using System;
    using System.Linq;

    public class ResolutionViewModel : ResolutionViewModel<Resolution>
    {
    }

    public class ResolutionViewModel<T> : BaseViewModel<T>
        where T : Resolution
    {
        public IDomainService<ProtocolMvd> ProtocolMvdDomain { get; set; }

        public override IDataResult List(IDomainService<T> domainService, BaseParams baseParams)
        {
            var loadParam = baseParams.GetLoadParam();

            // Для получения списка распоряжений в раскрытии
            var contragentId = baseParams.Params.ContainsKey("contragentId") ? baseParams.Params["contragentId"].ToLong() : 0;
            var periodDiDateStart = baseParams.Params.ContainsKey("periodDiDateStart")
                ? baseParams.Params["periodDiDateStart"].ToDateTime()
                : DateTime.MinValue;
            var periodDiDateEnd = baseParams.Params.ContainsKey("periodDiDateEnd") ? baseParams.Params["periodDiDateEnd"].ToDateTime() : DateTime.MinValue;

            var stageId = baseParams.Params.ContainsKey("stageId") ? baseParams.Params["stageId"].ToLong() : 0;

            var data = domainService.GetAll()
                .WhereIf(contragentId <= 0, x => x.Stage.Id == stageId)
                .WhereIf(
                    contragentId > 0,
                    x => x.Inspection.Contragent.Id == contragentId // Для раскрытия
                        && x.DocumentDate >= periodDiDateStart
                        && x.DocumentDate <= periodDiDateEnd)
                .Select(
                    x => new
                    {
                        x.Id,
                        DocumentId = x.Id,
                        x.Inspection,
                        x.Stage,
                        ConcederationResult = x.ConcederationResult!=null? x.ConcederationResult.Name:"",
                        x.TypeDocumentGji,
                        x.DocumentDate,
                        x.DocumentNumber,
                        x.DocumentNum,
                        x.GisUin
                    })
                .Filter(loadParam, Container);


            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<T> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();
            var obj = domainService.GetAll().FirstOrDefault(x => x.Id == id);

            var protocolMvd = this.ProtocolMvdDomain.GetAll().FirstOrDefault(x => x.Inspection.Id == obj.Inspection.Id);
            var data = new
            {
                obj.Id,
                obj.DocumentNum,
                obj.AbandonReason,
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
                obj.InLawDate,
                obj.ConcederationResult,
                obj.DocumentYear,
                obj.Executant,
                obj.FineMunicipality,
                obj.GisUin,
                obj.Municipality,
                obj.Official,
                obj.Paided,
                obj.PayStatus,
                obj.ParentDocumentsList,
                obj.PenaltyAmount,
                obj.Surname,
                obj.FirstName,
                obj.Patronymic,
                obj.Position,
                obj.PhysicalPerson,
                obj.PhysicalPersonInfo,
                obj.Sanction,
                obj.SectorNumber,
                obj.Stage,
                obj.State,
                obj.TypeDocumentGji,
                obj.PhysicalPersonDocType,
                obj.PhysicalPersonDocumentNumber,
                obj.PhysicalPersonDocumentSerial,
                obj.TypeInitiativeOrg,
                obj.TypeTerminationBasement,
                obj.RulinFio,
                obj.RulingDate,
                obj.RulingNumber,
                obj.OffenderWas,
                protocolMvd?.SerialAndNumber,
                protocolMvd?.BirthDate,
                protocolMvd?.IssueDate,
                protocolMvd?.BirthPlace,
                protocolMvd?.IssuingAuthority,
                protocolMvd?.Company,
                ProtocolMvdPhysicalPerson = protocolMvd?.PhysicalPerson,
                ProtocolMvdPhysicalPersonInfo = protocolMvd?.PhysicalPersonInfo,
                ProtocolMvdId = protocolMvd?.Id ?? 0,
                protocolMvd?.TypeExecutant,
                obj.Comment,
                obj.DateEndExecuteSSP,
                obj.DateExecuteSSP,
                obj.DateOSPListArrive,
                obj.ExecuteSSPNumber,
                obj.OSP,
                obj.OSPDecisionType,
                obj.DecisionNumber,
                obj.DecisionDate,
                obj.DecisionEntryDate,
                obj.Violation,
                obj.JudicalOffice,
                obj.Payded50Percent,
                obj.WrittenOff,
                obj.WrittenOffComment,
                obj.PersonBirthDate,
                obj.PersonBirthPlace,
                obj.PersonFactAddress,
                obj.PersonRegistrationAddress,
                obj.TypePresence,
                obj.Representative,
                obj.ReasonTypeRequisites
            };

            return new BaseDataResult(data);
        }
    }
}