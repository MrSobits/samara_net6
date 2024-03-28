namespace Bars.GkhGji.Report
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using B4.Modules.Reports;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Report;
    using Bars.GkhGji.Entities;

    public class BusinessActivityReport : GjiBaseReport
    {
        private long DocumentId { get; set; }

        protected override string CodeTemplate { get; set; }

        public BusinessActivityReport()
            : base(new ReportTemplateBinary(Properties.Resources.BlockGJI_BusinessActivity_1))
        {
        }

        public override string Id
        {
            get { return "BusinessActivity"; }
        }

        public override string CodeForm
        {
            get { return "BusinessActivity"; }
        }

        public override string Name
        {
            get { return "Уведомление о начале предпринимательской деятельности"; }
        }

        public override string Description
        {
            get { return "Уведомление о начале предпринимательской деятельности"; }
        }

        public override void SetUserParams(UserParamsValues userParamsValues)
        {
            DocumentId = userParamsValues.GetValue<object>("DocumentId").ToLong();
        }

        public override void PrepareReport(ReportParams reportParams)
        {
            var act = Container.Resolve<IDomainService<BusinessActivity>>().Get(DocumentId);
            if (act == null)
            {
                throw new ReportProviderException("Не удалось получить акт проверки");
            }

            CodeTemplate = "BlockGJI_BusinessActivity_1";

            reportParams.SimpleReportParams["ВхНомер"] = act.IncomingNotificationNum;
            reportParams.SimpleReportParams["ДатаРегистрации"] = act.DateRegistration != null ? act.DateRegistration.ToDateTime().ToString("dd MMMM yyyy") : String.Empty;
            reportParams.SimpleReportParams["ОрганПринявшийУведомление"] = act.AcceptedOrganization;
            reportParams.SimpleReportParams["ДатаУведомления"] = act.DateNotification != null ? act.DateNotification.ToDateTime().ToString("dd MMMM yyyy") : String.Empty;

            if (act.Contragent != null)
            {
                reportParams.SimpleReportParams["НаименованиеЮрЛица"] = act.Contragent.Name;
                reportParams.SimpleReportParams["ИНН"] = act.Contragent.Inn;
                reportParams.SimpleReportParams["ПочтАдрес"] = act.Contragent.FiasMailingAddress.AddressName;

                reportParams.SimpleReportParams["ОргПравФорма"] = act.Contragent.OrganizationForm.Name;
                reportParams.SimpleReportParams["ДатаДеятельности"] = act.Contragent.ActivityDateStart != null ? act.Contragent.ActivityDateStart.ToDateTime().ToString("dd MM yyyy") : String.Empty;

                var headContragent =
                   Container.Resolve<IDomainService<ContragentContact>>().GetAll()
                           .Where(x => x.Contragent.Id == act.Contragent.Id && x.DateStartWork.HasValue
                                        && (x.DateStartWork.Value <= DateTime.Now && (!x.DateEndWork.HasValue || x.DateEndWork.Value >= DateTime.Now)))
                           .FirstOrDefault(x => x.Position != null && (x.Position.Code == "1"));

                if (headContragent != null)
                {
                    reportParams.SimpleReportParams["Должность"] = headContragent.Position.Name;

                    reportParams.SimpleReportParams["ФИОРуководителя"] = string.Format(
                        "{0}. {1}. {2}",
                        headContragent.Name.Substring(0, 1),
                        headContragent.Patronymic.Substring(0, 1),
                        headContragent.Surname);
                }

                var works = Container.Resolve<IDomainService<ServiceJuridicalGji>>().GetAll()
                           .Where(x => x.BusinessActivityNotif.Id == act.Id).Select(x => x.KindWorkNotif.Name).ToList();
                
                if (works.Count > 0)
                {
                    reportParams.SimpleReportParams["УслугаРабота"] = String.Format("{0}.", String.Join(",", works));
                }
            }
        }

        public override List<TemplateInfo> GetTemplateInfo()
        {
            return new List<TemplateInfo>
                       {
                           new TemplateInfo
                               {
                                   Code = "BlockGJI_BusinessActivity_1",
                                   Name = "BusinessActivity",
                                   Description =
                                       "Уведомление о начале предпринимательской деятельности",
                                   Template = Properties.Resources.BlockGJI_BusinessActivity_1
                               }
                       };
        }
    }
}
