﻿using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;
using Castle.Windsor;
using Fasterflect;


namespace Bars.GkhGji.Regions.Habarovsk.Tasks
{
    /// <summary>
    /// Провайдер задачи на запрос данных
    /// </summary>
    public class SendSocialHireRequestTaskProvider : ITaskProvider
    {
        #region Fields

        private readonly IWindsorContainer container;

        #endregion

        #region Properties

        public string TaskCode => "SendSocialHireRequestTaskProvider";

        #endregion

        #region Constructors

        public SendSocialHireRequestTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }

        #endregion

        #region Public methods

        /// <summary>
        /// Создает задачу на запрос данных
        /// </summary>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var @params = baseParams.Params.DeepClone();

            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Сведения о переводe статуса помещения",
                        SendSocialHireRequestTaskExecutor.Id,
                           new BaseParams { Params = @params })
                }
            );
        }

        #endregion
    }
}
