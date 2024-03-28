namespace Bars.GisIntegration.UI.ViewModel.Protocol
{
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.GisIntegration.Base;
    using Bars.GisIntegration.Base.Tasks.SendData;
    using Bars.Gkh.Quartz.Scheduler.Log;

    using Castle.Windsor;

    /// <summary>
    /// View - модель протокола выполнения триггера
    /// </summary>
    public class TriggerProtocolViewModel: ITriggerProtocolViewModel
    {
        /// <summary>
        /// IoC Контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Получить список записей протокола
        /// </summary>
        /// <param name="baseParams">Параметры, содержащие
        /// triggerId - идентификатор триггера,
        /// sendDataProtocol - признак, характеризующий тип триггера</param>
        /// <returns>Результат выполнения операции, 
        /// содержащий список записей протокола</returns>
        public IDataResult List(BaseParams baseParams)
        {
            var triggerId = baseParams.Params.GetAs<long>("triggerId");
            var sendDataProtocol = baseParams.Params.GetAs<bool>("sendDataProtocol");

            if (triggerId == 0)
            {
                return new BaseDataResult(false, "Пустой идентификатор триггера.");
            }

            var taskManager = this.Container.Resolve<ITaskManager>();

            List<ILogRecord> protocol;

            try
            {
                protocol = taskManager.GetTriggerProtocol(triggerId);

                if (protocol == null || protocol.Count == 0)
                {
                    return new ListDataResult(null, 0);
                }
            }
            finally
            {
                this.Container.Release(taskManager);
            }

            var loadParams = baseParams.GetLoadParam();

            if (sendDataProtocol)
            {
                var protocolView = this.GetSendingDataProtocolView(protocol);

                var data = protocolView
                .AsQueryable()
                .OrderBy(x => x.DateTime)
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
            else
            {
                var protocolView = this.GetPreparingDataProtocolView(protocol);

                var data = protocolView
                .AsQueryable()
                .OrderBy(x => x.DateTime)
                .Filter(loadParams, this.Container);

                return new ListDataResult(data.Paging(loadParams).ToList(), data.Count());
            }
        }

        private List<DataSendingProtocolRecordView> GetSendingDataProtocolView(List<ILogRecord> log)
        {
            var result = new List<DataSendingProtocolRecordView>();

            foreach (var logRecord in log)
            {
                var sendingLogRecord = logRecord as PackageSendingLogRecord;

                if (sendingLogRecord != null)
                {
                    result.Add(new DataSendingProtocolRecordView
                    {
                        DateTime = sendingLogRecord.DateTime,
                        PackageName = sendingLogRecord.PackageName ?? string.Empty,
                        Text = sendingLogRecord.Text ?? string.Empty,
                        Type = sendingLogRecord.Type
                    });
                }
                else
                {
                    result.Add(new DataSendingProtocolRecordView
                    {
                        DateTime = logRecord.DateTime,
                        Text = logRecord.Text ?? string.Empty,
                        Type = logRecord.Type
                    });
                }
            }

            return result;
        }

        private List<PreparingDataProtocolRecordView> GetPreparingDataProtocolView(List<ILogRecord> log)
        {
            var result = new List<PreparingDataProtocolRecordView>();

            foreach (var logRecord in log)
            {
                result.Add(new PreparingDataProtocolRecordView
                {
                    DateTime = logRecord.DateTime,
                    Text = logRecord.Text ?? string.Empty,
                    Type = logRecord.Type
                });
            }

            return result;
        }
    }
}
