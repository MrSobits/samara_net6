namespace Bars.Gkh.RegOperator.Tasks.Reports
{
    using B4.Utils;
    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Castle.Windsor;
    using Gkh.Utils;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Провайдер выгрузки информации по ЛС
    /// </summary>
    public class PersonalAccountInfoExportTaskProvider : ITaskProvider
    {
        /// <summary>
        /// Создание задачи
        /// </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Описатель задачи</returns>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var municipalityIds = baseParams.Params.GetAs<long[]>("municipalityIds") ?? new long[0];
            var municipalityName = baseParams.Params.GetAs<string>("municipalityName");
            if (municipalityIds.Length > 1)
                municipalityName = string.Format("{0} районов", municipalityIds.Length);

            return new CreateTasksResult(new[]
            {
                new TaskDescriptor(string.Format("Информация по ЛС ({0})", municipalityName), PersonalAccountInfoExportTaskExecutor.Id, baseParams)
            });
        }

        /// <summary>
        /// Код задачи
        /// </summary>
        public string TaskCode { get { return "PersonalAccauntInfoExport"; } }
    }
}
