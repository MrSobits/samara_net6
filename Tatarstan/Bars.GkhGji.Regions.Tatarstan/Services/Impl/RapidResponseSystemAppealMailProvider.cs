namespace Bars.GkhGji.Regions.Tatarstan.Services.Impl
{
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Entities.Administration;
    using Bars.Gkh.Enums.Administration.EmailMessage;
    using Bars.Gkh.Services.Impl;
    using Bars.Gkh.Services.ServiceContracts.Mail;
    using Bars.GkhGji.Regions.Tatarstan.Entities.RapidResponseSystem;
    using Bars.GkhGji.Regions.Tatarstan.Models;
    using Bars.GkhGji.Regions.Tatarstan.Properties;

    /// <inheritdoc cref="Bars.GkhGji.Regions.Tatarstan.Services.IRapidResponseSystemAppealMailProvider" />
    public class RapidResponseSystemAppealMailProvider : BaseMailProvider<RapidResponseAppealMailInfo>, IRapidResponseSystemAppealMailProvider
    {
        // Данные письма
        private RapidResponseAppealMailInfo data;
        
        #region Dependency Injection
        private readonly IDomainService<RapidResponseSystemAppealDetails> appealDetailsDomain;
        
        /// <inheritdoc />
        public RapidResponseSystemAppealMailProvider(IPostalService postalService,
            IDomainService<EmailMessage> mailMessageDomain,
            IFileManager fileManager,
            IDomainService<RapidResponseSystemAppealDetails> appealDetailsDomain)
            : base(postalService, mailMessageDomain, fileManager)
        {
            this.appealDetailsDomain = appealDetailsDomain;
        }
        #endregion

        /// <inheritdoc />
        public override RapidResponseAppealMailInfo PrepareData(BaseParams baseParams)
        {
            var appealDetailsId = baseParams.Params.GetAsId("appealDetailsId");

            var mailData = this.appealDetailsDomain.GetAll()
                .Where(x => x.Id == appealDetailsId)
                .Select(x => new RapidResponseAppealMailInfo
                {
                    Address = x.AppealCitsRealityObject.RealityObject.Address,
                    AppealNumber = x.RapidResponseSystemAppeal.AppealCits.DocumentNumber,
                    OrganizationName = x.RapidResponseSystemAppeal.Contragent.Name,
                    ControlPeriod = x.ControlPeriod,
                    ContragentId = x.RapidResponseSystemAppeal.Contragent.Id
                })
                .SingleOrDefault();

            this.data = mailData;

            return mailData;
        }

        /// <inheritdoc />
        public override string PrepareMessage(RapidResponseAppealMailInfo mailData)
        {
            var template = Encoding.GetEncoding(1251).GetString(Resources.RapidResponseSystemAppealMailTemplate);

            return string.Format(template, mailData.OrganizationName, mailData.AppealNumber, mailData.ControlPeriod.ToShortDateString(), mailData.Address);
        }

        /// <inheritdoc />
        protected override void PrepareLogRecord(EmailMessage message)
        {
            message.EmailMessageType = EmailMessageType.RapidResponseSystem;
            message.RecipientContragent = new Contragent { Id = this.data.ContragentId };
            message.AdditionalInfo = this.data.AppealNumber;
        }
    }
}