﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using Fasterflect;

namespace Bars.GkhGji.Regions.Habarovsk.Tasks
{
    /// <summary>
    /// Провайдер задачи на отправку оплаты в ГИС ГМП
    /// </summary>
    public class ERKNMSendInitiateRequestTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "ERKNMSendInitiateRequestTaskProvider";

        #endregion

        #region Constructors

        public ERKNMSendInitiateRequestTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на отправку оплаты в ГИС ГМП
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Отправка проверки в ЕРКНМ",
                        ERKNMSendInitiateRequestTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
