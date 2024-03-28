namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.FileStorage;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Utils;

    using Ionic.Zip;
    using Ionic.Zlib;

    /// <summary>
    /// Обновление МО у контрагента"
    /// </summary>
    public class UpdateMoContragentAction : BaseExecutionAction
    {
        private readonly StringBuilder finalLog = new StringBuilder();

        /// <summary>
        /// Контрагент
        /// </summary>
        public IRepository<Contragent> СontragentDomain { get; set; }

        /// <summary>
        /// Лог
        /// </summary>
        public IRepository<LogOperation> LogOperationRepo { get; set; }

        /// <summary>
        /// Пользователь
        /// </summary>
        public IGkhUserManager UserManager { get; set; }

        /// <summary>
        /// Представлющий методы для работы с файлами
        /// </summary>
        public IFileManager FileManager { get; set; }

        /// <summary>
        /// Название
        /// </summary>
        public override string Name => "Обновление МО у контрагента";

        /// <summary>
        /// Комментарий
        /// </summary>
        public override string Description => "Обновление МО у контрагента";

        /// <summary>
        /// Экшен
        /// </summary>
        public override Func<IDataResult> Action => this.UpdateMoContragent;

        /// <summary>
        /// UpdateMoContragent
        /// </summary>
        /// <returns></returns>
        public BaseDataResult UpdateMoContragent()
        {
            var startDate = DateTime.UtcNow;

            var contragents = this.СontragentDomain.GetAll().Where(x => x.MoSettlement == null);

            this.finalLog.AppendLine("Наименование абонента;Статус копирования");
            this.Container.InTransaction(
                () =>
                {
                    foreach (var contragent in contragents)
                    {
                        try
                        {
                            if (contragent.FiasJuridicalAddress != null)
                            {
                                contragent.MoSettlement = Utils.GetMoSettlement(this.Container, contragent.FiasJuridicalAddress);
                                this.СontragentDomain.Update(contragent);
                                this.finalLog.AppendLine(contragent.Name + ";Успешнож;");
                            }
                            else
                            {
                                this.finalLog.AppendLine(contragent.Name + ";Юр адрес пустой");
                            }
                        }
                        catch (ValidationException e)
                        {
                            this.finalLog.AppendFormat("{0};{1};", contragent.Name, e.Message);
                        }
                    }
                });

            var logOperation = new LogOperation
            {
                StartDate = startDate,
                Comment = "Обновление МО у контрагента",
                OperationType = LogOperationType.RunAction,
                EndDate = DateTime.UtcNow,
                User = this.UserManager.GetActiveUser()
            };

            var logsZip = new ZipFile(Encoding.UTF8)
            {
                CompressionLevel = CompressionLevel.Level3,
                AlternateEncoding = Encoding.GetEncoding("cp866")
            };

            using (var logFile = new MemoryStream())
            {
                var log = Encoding.GetEncoding(1251).GetBytes(this.finalLog.ToString());

                logsZip.AddEntry($"{logOperation.OperationType.GetEnumMeta().Display}.csv", log);

                logsZip.Save(logFile);

                var logFileInfo = this.FileManager.SaveFile(logFile, $"{logOperation.OperationType.GetEnumMeta().Display}.zip");

                logOperation.LogFile = logFileInfo;
            }

            this.LogOperationRepo.Save(logOperation);
            return new BaseDataResult();
        }
    }
}