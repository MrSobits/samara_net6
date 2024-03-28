namespace Bars.GisIntegration.Smev.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FileStorage;
    using Bars.GisIntegration.Base;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Extensions;
    using Bars.Gkh.Quartz.Scheduler.Log;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    public class BaseIntegrationService : IBaseIntegrationService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult GetTriggerProtocolView(BaseParams baseParams)
        {
            var triggerId = baseParams.Params.GetAs<long>("triggerId");

            if (triggerId == 0)
            {
                throw new Exception("Пустой идентификатор триггера.");
            }

            var loadParams = baseParams.GetLoadParam();

            return this.GetLogRecords(triggerId)
                .Select(logRecord => new
                {
                    logRecord.DateTime,
                    Text = logRecord.Text ?? string.Empty,
                    logRecord.Type
                })
                .ToListDataResult(loadParams, this.Container);
        }

        /// <inheritdoc />
        public IDataResult GetXmlData(BaseParams baseParams)
        {
            var taskId = baseParams.Params.GetAs<long>("taskId");
            var isRequest = baseParams.Params.GetAs<bool>("isRequest");
            if (taskId == default(long))
            {
                return BaseDataResult.Error("Не удалось найти XML файл");
            }

            var risTaskDomain = this.Container.ResolveDomain<RisTask>();
            var fileManager = this.Container.Resolve<IFileManager>();

            using (this.Container.Using(risTaskDomain, fileManager))
            {
                var task = risTaskDomain.Get(taskId);
                if (task == null)
                {
                    return BaseDataResult.Error("Не удалось найти XML файл");
                }

                var file = isRequest ? task.RequestXmlFile : task.ResponseXmlFile;
                if (file == null)
                {
                    return BaseDataResult.Error("Не удалось загрузить XML файл");
                }

                var fileStream = fileManager.GetFile(file);
                var fileData = this.StreamToByteArray(fileStream);

                return new BaseDataResult(XmlExtensions.GetFormattedXmlString(fileData));
            }
        }

        /// <inheritdoc />
        public IDataResult GetLogFile(BaseParams baseParams)
        {
            var triggerId = baseParams.Params.GetAs<long>("triggerId");

            if (triggerId == default)
            {
                throw new Exception("Пустой идентификатор триггера.");
            }

            var protocol = this.GetLogRecords(triggerId);
            var csvFileContent = this.GetFileContent(protocol, baseParams);

            var fileManager = this.Container.Resolve<IFileManager>();
            try
            {
                var fileInfo = fileManager.SaveFile($"protocol_{triggerId}.csv", Encoding.GetEncoding(1251).GetBytes(csvFileContent));
                return new BaseDataResult(fileManager.LoadFile(fileInfo.Id));
            }
            finally
            {
                this.Container.Release(fileManager);
            }
        }

        /// <summary>
        /// Получить содержимое для файла
        /// </summary>
        private string GetFileContent(List<ILogRecord> protocol, BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();
            var stringBuilder = new StringBuilder();

            var data = protocol
                .Select(logRecord => new
                {
                    logRecord.DateTime,
                    Text = logRecord.Text ?? string.Empty,
                    logRecord.Type
                })
                .AsQueryable()
                .Filter(loadParams, Container)
                .Order(loadParams).ToList();

            data.ForEach(x => stringBuilder.AppendLine($"{x.DateTime},{x.Text},{x.Type}"));
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Получить записи логов
        /// </summary>
        private List<ILogRecord> GetLogRecords(long triggerId)
        {
            var taskManager = this.Container.Resolve<ITaskManager>();

            try
            {
                return taskManager.GetTriggerProtocol(triggerId);
            }
            finally
            {
                this.Container.Release(taskManager);
            }
        }

        /// <summary>
        /// Получить массив изи stream'а 
        /// </summary>
        private byte[] StreamToByteArray(Stream input)
        {
            var buffer = new byte[16 * 1024];
            using (var ms = new MemoryStream())
            {
                int read;
                while ((read = input.Read(buffer, 0, buffer.Length)) > 0)
                {
                    ms.Write(buffer, 0, read);
                }

                return ms.ToArray();
            }
        }
    }
}