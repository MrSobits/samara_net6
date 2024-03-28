namespace Bars.GkhCr.StateChange
{
	using Bars.B4;
    using Bars.B4.Application;
    using Bars.B4.IoC;
    using Bars.B4.Modules.Analytics.Reports.Enums;
    using Bars.B4.Modules.Analytics.Reports.Generators;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhCr.Entities;
	using Bars.GkhCr.Localizers;
    using Bars.GkhCr.Properties;
    using Bars.GkhCr.Report;
    using Castle.Core.Internal;
    using Castle.Windsor;
    using System;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Net.Mail;
    using System.Text;
    using System.Threading.Tasks;

    public class CrObjectBuildContractRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public string Id
        {
            get
            {
                return "CrObjectBuildContractRule";
            }
        }

        public string Name
        {
            get
            {
                return "Перевод статуса из черновика в утверждено в договоре подряда";
            }
        }

        public string TypeId
        {
            get
            {
                return "cr_obj_build_contract";
            }
        }

        public string Description
        {
            get
            {
                return "Отправляет уведомление на почту УК о проведении СМР или ПСД";
            }
        }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var buildConTW = this.Container.Resolve<IDomainService<BuildContractTypeWork>>();

            try
            {
                var entity = statefulEntity as BuildContract;

                if (entity == null)
                {
                    return ValidateResult.No("Внутренняя ошибка.");
                }

                var emailAddress = Container.Resolve<IDomainService<ManagingOrgRealityObject>>()
                    .GetAll()
                    .Where(x => x.RealityObject.Id == entity.ObjectCr.RealityObject.Id)
                    .Select(x => x.ManagingOrganization.Contragent.Email)
                    .FirstOrDefault();
            //    emailAddress = "aksionik@mail.ru";
                if (string.IsNullOrEmpty(emailAddress))
                {
                  //  emailAddress = "aksionik@mail.ru";
                    return ValidateResult.No("Не найден адрес для отправки email");
                }

                int count = buildConTW.GetAll()
                    .Where(x => x.BuildContract.Id == entity.Id)
                    .Count();

                if (count == 0)
                {
                    return ValidateResult.No("Нет работ в договоре.");
                }

                //bool isPSD = buildConTW.GetAll()
                //    .Where(x => x.BuildContract.Id == entity.Id)
                //    .Select(x => x.TypeWork.Work.Name.Contains("ПСД"))
                //    .Any();
                bool isPSD = entity.TypeContractBuild != Enums.TypeContractBuild.Smr ? true : false;

                // TODO: переделать на формирование отчета в сервисе
                /*StiReport report = null;
                report = new StiReport();
                if (isPSD)
                {
                    report.Load(Resources.BuildContractCrToMailReportPSD);
                }
                else
                {
                    report.Load(Resources.BuildContractCrToMailReportSMR);
                }
                try
                {
                    StiSqlDatabase myDb = (StiSqlDatabase)report.Dictionary.Databases[0];
                    myDb.ConnectionString = "Server=195.98.86.15;Database=gkh_voronezh;User ID=bars;Password=123;";
                }
                catch(Exception e)
                {
                    
                }
               // emailAddress = "aksionik@mail.ru";
                report.Dictionary.Variables["build_id"].Value = entity.Id.ToString();
                report["build_id"] = entity.Id;
                report.Render();

                this.Send(emailAddress, report, entity);*/

                return ValidateResult.Yes();
            }
            finally
            {
                Container.Release(buildConTW);
            }
        }

        /*
        private void Send(String toEmail, StiReport repo, BuildContract entity)
        {
            var appSettings = ApplicationContext.Current.Configuration.AppSettings;
            var smtpClient = appSettings.GetAs<string>("smtpClient");
            var smtpPort = appSettings.GetAs<int>("smtpPort");
            var mailFrom = appSettings.GetAs<string>("smtpEmail");
            var smtpLogin = appSettings.GetAs<string>("smtpLogin");
            var smtpPassword = appSettings.GetAs<string>("smtpPassword");
            var enableSsl = appSettings.GetAs<bool>("enableSsl");

            try
            {
                var stream = new MemoryStream();
                repo.ExportDocument(StiExportFormat.Pdf, stream);
                stream.Seek(0, SeekOrigin.Begin);
                MailAddress from = new MailAddress(mailFrom, "Фонд капитального ремонта");
                MailAddress to = new MailAddress(toEmail);
                MailMessage m = new MailMessage(from, to);
                m.Subject = "Уведомление о проведении работ по договору";
                m.Body = $"<h3>Фонд капитального ремонта уведомляет вас о проведении работ по дому {entity.ObjectCr.RealityObject.Address}</h3>";
                m.IsBodyHtml = true;
                m.Attachments.Add(new Attachment(stream, $"Уведомление о проведении работ.pdf"));
                SmtpClient smtp = new SmtpClient(smtpClient, smtpPort);
                smtp.Credentials = new NetworkCredential(smtpLogin, smtpPassword);
                smtp.EnableSsl = enableSsl;
                smtp.Send(m);
            }
            catch (Exception e)
            {

            }
        }
    */
    }
}
