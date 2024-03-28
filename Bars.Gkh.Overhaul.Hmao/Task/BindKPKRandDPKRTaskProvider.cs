using Bars.B4;
using Bars.B4.Modules.Tasks.Common.Contracts;
using Bars.B4.Modules.Tasks.Common.Contracts.Result;
using Bars.B4.Modules.Tasks.Common.Service;

namespace Bars.Gkh.Overhaul.Hmao.Task
{
    public class BindKPKRandDPKRTaskProvider : ITaskProvider
    {
        public string TaskCode => "BindKPKRandDPKRTaskProvider";

        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            return new CreateTasksResult(
                new TaskDescriptor[] {
                    new TaskDescriptor(
                        "Сопоставление записей КПКР и ДПКР",
                        BindKPKRandDPKRTaskExecutor.Id,
                        baseParams
                        )
                }
            );
        }
    }
}
