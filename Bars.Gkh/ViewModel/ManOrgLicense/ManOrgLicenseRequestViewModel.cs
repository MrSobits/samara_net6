namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Enums.Licensing;

    public class ManOrgLicenseRequestViewModel : BaseViewModel<ManOrgLicenseRequest>
    {
        public IManOrgLicenseRequestService Service { get; set; }

        public override IDataResult List(IDomainService<ManOrgLicenseRequest> domain, BaseParams baseParams)
        {
            int totalCount;

            var result = this.Service.GetList(baseParams, true, out totalCount);

            return new ListDataResult(result, totalCount);
        }

        public override IDataResult Get(IDomainService<ManOrgLicenseRequest> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAsId();

            var record = domainService.Get(id);

            var revokeLicense = record.Type == LicenseRequestType.GrantingLicense && record.Contragent.IsNotNull()
                ? this.Service
                    .ListLicenseByManOrg(record.Contragent.Id)
                    .FirstOrDefault(x => x.State.Code == "003")
                : null;

            var docFile = record.ManOrgLicense != null
                ? this.Service.ListLicenseDocs(record.ManOrgLicense.Id)
                    .OrderByDescending(x => x.Id)
                    .Select(x => x.File)
                    .FirstOrDefault()
                : null;

            ManOrgLicense lic = null;

            if (record.Type == LicenseRequestType.ExtractFromRegisterLicense)
            {
                lic = this.Service.ListLicenseByManOrg(record.Contragent.Id)
                    .Where(x => x.DateDisposal <= record.DateRequest)
                    .OrderByDescending(x => x.DateDisposal)
                    .FirstOrDefault();
            }

            var result = new RequestDto
            {
                Id = record.Id,
                Contragent = record.Contragent,
                Municipality = record.Contragent?.Municipality,
                DateRequest = record.DateRequest,
                RegisterNumber = record.RegisterNumber,
                RegisterNum = record.RegisterNum,
                ConfirmationOfDuty = record.ConfirmationOfDuty,
                ReasonOffers = record.ReasonOffers,
                File = record.File,
                ReasonRefusal = record.ReasonRefusal,
                State = record.State,
                Type = record.Type,
                Note = record.Note,
                RPGUNumber = record.RPGUNumber,
                ReplyTo = record.ReplyTo,
                MessageId = record.MessageId,
                ManOrgLicense = record.ManOrgLicense,
                LicenseNum = record.ManOrgLicense?.LicNum,
                LicenseState = record.ManOrgLicense?.State.Name,
                ExtractLicenseNum = lic?.LicNum,
                ExtractLicenseState = lic?.State.Name,
                DateIssued = record.ManOrgLicense?.DateIssued,
                ProvDocFile = docFile,
                Applicant = record.Applicant,
                TaxSum = record.TaxSum,
                LicenseRegistrationReason = record.LicenseRegistrationReason,
                LicenseRejectReason = record.LicenseRejectReason,
                NoticeAcceptanceDate = record.NoticeAcceptanceDate,
                NoticeViolationDate = record.NoticeViolationDate,
                ReviewDate = record.ReviewDate,
                NoticeReturnDate = record.NoticeReturnDate,
                ReviewDateLk = record.ReviewDateLk,
                PreparationOfferDate = record.PreparationOfferDate,
                SendResultDate = record.SendResultDate,
                SendMethod = record.SendMethod,
                RevokeLicenseId = revokeLicense?.Id,
                RevokeNumberLicense = revokeLicense?.LicNumber,
                RevokeStateLicense = revokeLicense?.State.Name,
                OrderDate = record.OrderDate,
                OrderNumber = record.OrderNumber,
                OrderFile = record.OrderFile,
                TypeIdentityDocument = record.TypeIdentityDocument,
                IdSerial = record.IdSerial,
                IdNumber = record.IdNumber,
                IdIssuedBy = record.IdIssuedBy,
                IdIssuedDate = record.IdIssuedDate
            };

            return new BaseDataResult(result);
        }

        private class RequestDto
        {
            public long Id { get; set; }
            public Contragent Contragent { get; set; }
            public Municipality Municipality { get; set; }
            public DateTime? DateRequest { get; set; }
            public string RegisterNumber { get; set; }
            public int? RegisterNum { get; set; }
            public string ConfirmationOfDuty { get; set; }
            public string ReasonOffers { get; set; }
            public string ReplyTo { get; set; }
            public string RPGUNumber { get; set; }
            public string MessageId { get; set; }
            public FileInfo File { get; set; }
            public string ReasonRefusal { get; set; }
            public State State { get; set; }
            public LicenseRequestType? Type { get; set; }
            public string Note { get; set; }
            public ManOrgLicense ManOrgLicense { get; set; }
            public string Applicant { get; set; }
            public decimal TaxSum { get; set; }
            public int? LicenseNum { get; set; }
            public string LicenseState { get; set; }
            public int? ExtractLicenseNum { get; set; }
            public string ExtractLicenseState { get; set; }
            public DateTime? DateIssued { get; set; }
            public FileInfo ProvDocFile { get; set; }
            public LicenseRegistrationReason LicenseRegistrationReason { get; set; }
            public LicenseRejectReason LicenseRejectReason { get; set; }
            public DateTime? NoticeAcceptanceDate { get; set; }
            public DateTime? NoticeViolationDate { get; set; }
            public DateTime? ReviewDate { get; set; }
            public DateTime? NoticeReturnDate { get; set; }
            public DateTime? ReviewDateLk { get; set; }
            public DateTime? PreparationOfferDate { get; set; }
            public DateTime? SendResultDate { get; set; }
            public SendMethod? SendMethod { get; set; }
            public long? RevokeLicenseId { get; set; }
            public string RevokeNumberLicense { get; set; }
            public string RevokeStateLicense { get; set; }
            public DateTime? OrderDate { get; set; }
            public string OrderNumber { get; set; }
            public FileInfo OrderFile { get; set; }
            public virtual TypeIdentityDocument? TypeIdentityDocument { get; set; }
            public virtual string IdSerial { get; set; }
            public virtual string IdNumber { get; set; }
            public virtual string IdIssuedBy { get; set; }
            public virtual DateTime? IdIssuedDate { get; set; }
        }
    }
}