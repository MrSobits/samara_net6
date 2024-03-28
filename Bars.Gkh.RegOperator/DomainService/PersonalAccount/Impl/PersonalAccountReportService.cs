namespace Bars.Gkh.RegOperator.DomainService.PersonalAccount.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Castle.Windsor;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Controller;
    using Bars.Gkh.Modules.RegOperator.DomainService;
    using Bars.Gkh.RegOperator.CodedReports;
    using Bars.Gkh.Report;

    using Ionic.Zip;
    using Ionic.Zlib;
    using Bars.Gkh.Entities.Administration.PrintCertHistory;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Authentification;

    /// <summary>
    /// Сервис лс для печати отчетов
    /// </summary>
    public class PersonalAccountReportService : IPersonalAccountReportService
    {
        /// <summary>
        /// Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        public IGkhUserManager UserManager { get; set; }

        private static string reportName = "PersonalAccountReport";
        
        private readonly IFileManager fileManager;

        /// <summary>
        /// Конструктор
        /// </summary>
        /// <param name="fileManager">fileManager</param>
        public PersonalAccountReportService(IFileManager fileManager)
        {
            this.fileManager = fileManager;
        }

        /// <summary>
        /// Отчет по лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetReport(BaseParams baseParams)
        {
            var reportId = baseParams.Params.GetAs<string>("reportId");
            var codedReport = this.Container.Resolve<IPersonalAccountCodedReport>(reportId);
            try
            {
                if (codedReport == null)
                {
                    throw new Exception("Реализация отчета по лс не найдена");
                }

                codedReport.AccountId = baseParams.Params.GetAs<long>("accId");
                var accId = baseParams.Params.GetAs<long>("accId");
                var personalAccount = this.Container.Resolve<IDomainService<BasePersonalAccount>>().Get(accId);
                codedReport.Generate();

                var file = this.fileManager.SaveFile(codedReport.ReportFileStream, codedReport.OutputFileName);

                try
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
                    var certHistoryContainer = this.Container.Resolve<IDomainService<PrintCertHistory>>();
                    certHistoryContainer.Save(new PrintCertHistory
                    {
                        AccNum = personalAccount.PersonalAccountNum,
                        Name = personalAccount.AccountOwner.Name,
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        PrintDate = DateTime.Now,
                        Address = personalAccount.Room.RealityObject.Address + ", пом." + personalAccount.Room.RoomNum,
                        Username = thisOperator.Login,
                        Role = thisOperator.Role != null ? thisOperator.Role.Name : thisOperator.User.Roles.FirstOrDefault().Role.Name,
                        ObjectVersion = 1,
                        Type = personalAccount.AccountOwner.OwnerType.ToString()

                    });
                }
                catch (Exception e)
                {
                    Operator thisOperator = UserManager.GetActiveOperator();
                    var certHistoryContainer = this.Container.Resolve<IDomainService<PrintCertHistory>>();
                    certHistoryContainer.Save(new PrintCertHistory
                    {
                        AccNum = personalAccount.PersonalAccountNum,
                        Name = personalAccount.AccountOwner.Name,
                        ObjectCreateDate = DateTime.Now,
                        ObjectEditDate = DateTime.Now,
                        PrintDate = DateTime.Now,
                        //  Address = personalAccount.Room.RealityObject.Address + ", пом." + personalAccount.Room.RoomNum,
                        Username = thisOperator.Login,
                        //Role = thisOperator.Role != null ? thisOperator.Role.Name : thisOperator.User.Roles.FirstOrDefault().Role.Name,
                        ObjectVersion = 1
                        //   Type = personalAccount.AccountOwner.OwnerType
                    });

                }

                return new BaseDataResult(file.Id);
            }
            catch (Exception exception)
            {
                return BaseDataResult.Error(exception.Message);
            }
            finally
            {
                this.Container.Release(codedReport);
            }
        }

        /// <summary>
        /// Отчет по многим лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public IDataResult GetMassReport(BaseParams baseParams)
        {
            var userParam = new UserParamsValues();
            var reports = new List<IPersonalAccountCodedReport>();

            if (baseParams.Params.ContainsKey("userParams") &&
                baseParams.Params["userParams"] is DynamicDictionary)
            {
                userParam.Values = (DynamicDictionary) baseParams.Params["userParams"];

               long[] accountIds = userParam.GetValue("AccIds")
                    .To<List<object>>()
                    .OfType<long>()
                    .ToArray();

                foreach (var accountId in accountIds)
                {
                    var report = this.Container.Resolve<IPersonalAccountCodedReport>(reportName);

                    if (report == null)
                    {
                        throw new Exception("Реализация отчета по лс не найдена");
                    }

                    using (this.Container.Using(report))
                    {
                        report.AccountId = accountId;
                        report.Generate();
                        reports.Add(report);
                    }
                }             
            }

            var archive = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level9,
                ProvisionalAlternateEncoding = Encoding.GetEncoding("cp866")
            };
            var tempDir = Directory.CreateDirectory(Path.Combine(Path.GetTempPath(), Path.GetRandomFileName()));

            foreach (var report in reports)
            {
                File.WriteAllBytes(Path.Combine(tempDir.FullName, report.OutputFileName), report.ReportFileStream.ReadAllBytes());
            }

            archive.AddDirectory(tempDir.FullName);

            using (var ms = new MemoryStream())
            {
                archive.Save(ms);

                var file = this.fileManager.SaveFile(ms, "Отчеты по лс.zip");
                return new BaseDataResult(file.Id);
            }
        }

        /// <summary>
        /// Получить отчет по лс
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        public ReportInfo GetReportInfo(BaseParams baseParams)
        {
            var codedReport = this.Container.Resolve<IPersonalAccountCodedReport>(reportName);
            try
            {
                return new ReportInfo
                {
                    Id = codedReport.Key,
                    Name = codedReport.Name,
                    Description = codedReport.Description
                };
            }
            finally
            {
                this.Container.Release(codedReport);
            }
        }

        public class CodedReport
        {
            public Stream Report;
            public long AccountId;
        }
    }
}
