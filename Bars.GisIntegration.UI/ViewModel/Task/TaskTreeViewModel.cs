namespace Bars.GisIntegration.UI.ViewModel.Task
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.Security;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base;
    using Bars.GisIntegration.Base.Entities;
    using Bars.GisIntegration.Base.Enums;

    using Castle.Windsor;

    /// <summary>
    /// View - модель дерева задач с асинхронной загрузкой узлов
    /// </summary>
    public class TaskTreeViewModel : ITreeViewModel
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <summary>
        /// Менеджер задач
        /// </summary>
        public ITaskManager TaskManager { get; set; }

        /// <summary>
        /// Получить дочерние узлы
        /// </summary>
        /// <param name="baseParams">Параметры, в т.ч. параметры текущего узла</param>
        /// <returns>Дочерние узлы</returns>
        public IDataResult List(BaseParams baseParams)
        {
            var nodeType = baseParams.Params.GetAs<string>("nodeType");
            var nodeId = baseParams.Params.GetAs<long>("nodeId");

            List<ITreeNode> data;

            if (nodeType == "root")
            {
                data = this.GetTaskTreeNodes(baseParams);
            }
            else if (nodeType == "Task")
            {
                data = this.GetTriggerTreeNodes(nodeId);
            }
            else if (nodeType == "SendingDataTrigger")
            {
                data = this.GetPackageTreeNodes(nodeId);
            }
            else
            {
                data = new List<ITreeNode>();
            }

            return new BaseDataResult(new { success = true, children = data });
        }

        private List<ITreeNode> GetTaskTreeNodes(BaseParams baseParams)
        {
            var tasks = this.GetTasks(baseParams);

            var result = new List<ITreeNode>();

            foreach (var task in tasks)
            {
                result.Add(this.CreateTaskNode(task));
            }

            return result;
        }

        private List<RisTask> GetTasks(BaseParams baseParams)
        {
            DateTime? dateTimeFrom = null;
            DateTime? dateTimeTo = null;

            if (baseParams.Params.ContainsKey("dateTimeFrom"))
            {
                dateTimeFrom = baseParams.Params.GetAs<DateTime>("dateTimeFrom");
            }

            if (baseParams.Params.ContainsKey("dateTimeTo"))
            {
                dateTimeTo = baseParams.Params.GetAs<DateTime>("dateTimeTo");
            }

            var taskDomain = this.Container.ResolveDomain<RisTask>();
            var userIdentity = this.Container.Resolve<IUserIdentity>();
            var userDomain = this.Container.ResolveDomain<User>();

            try
            {
                var currentUser = userDomain.Get(userIdentity.UserId);

                return taskDomain.GetAll()
                    //если текущий пользователь не админ, то показываем только его задачи
                    .WhereIf(currentUser.Roles.All(y => y.Role.Name != "Администратор"), x => x.UserName == currentUser.Login)
                    .WhereIf(dateTimeFrom.HasValue, x => x.StartTime >= dateTimeFrom.Value)
                    .WhereIf(dateTimeTo.HasValue, x => x.StartTime <= dateTimeTo.Value)
                    .OrderBy(x => x.Id)
                    .ToList();
            }
            finally
            {
                this.Container.Release(taskDomain);
                this.Container.Release(userIdentity);
                this.Container.Release(userDomain);
            }
        }

        private ITreeNode CreateTaskNode(RisTask task)
        {
            var result = new TaskTreeNode(task);
            result.State = task.TaskState.GetDisplayName();
            result.Result = task.TaskState == TaskState.SignDataWaiting || task.TaskState == TaskState.SendDataWaiting;
            result.Protocol = false;
            result.Message = task.Message;
            result.Leaf = false;

            switch (task.TaskState)
            {
                case TaskState.PreparingData:
                case TaskState.SendingData:
                case TaskState.Processing:
                case TaskState.Waiting:
                    var taskExecutionDescriptor = this.GetTaskExecutionDescriptor(task);

                    result.Percent = taskExecutionDescriptor.Percent.ToString();
                    result.Children = this.GetTriggerTreeNodes(taskExecutionDescriptor);
                    result.Leaf = result.Children.Count == 0;
                    break;
                case TaskState.SignDataWaiting:
                case TaskState.SendDataWaiting:
                    result.Percent = "50";
                    break;
                case TaskState.CompleteSuccess:
                case TaskState.CompleteWithErrors:
                case TaskState.Error:
                    result.Percent = "100";
                    break;
                default:
                    result.Percent = "0";
                    break;
            }

            return result;
        }

        private List<ITreeNode> GetTriggerTreeNodes(long taskId)
        {
            var task = this.TaskManager.GetTask(taskId);
            var taskExecutionDescriptor = this.GetTaskExecutionDescriptor(task);
            return this.GetTriggerTreeNodes(taskExecutionDescriptor);
        }

        private List<ITreeNode> GetTriggerTreeNodes(TaskExecutionDescriptor taskExecutionDescriptor)
        {
            var result = new List<ITreeNode>();

            var triggerExecutionDescriptors = taskExecutionDescriptor.TriggerExecutionDescriptors;

            foreach (var triggerExecutionDescriptor in triggerExecutionDescriptors)
            {
                var triggerViewModel = this.CreateTriggerNode(triggerExecutionDescriptor);
                result.Add(triggerViewModel);
            }

            return result;
        }

        private ITreeNode CreateTriggerNode(TriggerExecutionDescriptor triggerExecutionDescriptor)
        {
            var triggerState = triggerExecutionDescriptor.TaskTrigger.TriggerState;

            var result = new TaskTreeNode(triggerExecutionDescriptor.TaskTrigger);
            result.State = triggerState.GetDisplayName();
            result.Percent = triggerExecutionDescriptor.Percent.ToString();
            result.Message = triggerExecutionDescriptor.TaskTrigger.Message;

            result.Result = triggerState == TriggerState.CompleteSuccess
                                      || triggerState == TriggerState.CompleteWithErrors;

            result.Protocol = triggerState != TriggerState.Waiting;

            if (triggerExecutionDescriptor.TaskTrigger.TriggerType == TriggerType.SendingData)
            {
                var triggerPackages = this.TaskManager.GetTriggerPackages(triggerExecutionDescriptor.TaskTrigger.Trigger.Id);

                if (triggerState == TriggerState.Processing || triggerState == TriggerState.Waiting)
                {
                    result.Children = triggerPackages.Select(this.CreatePackageNode).ToList();
                    result.Leaf = result.Children.Count == 0;
                }
                else
                {
                    result.Leaf = triggerPackages.Count == 0;
                }
            }

            return result;
        }

        private TaskExecutionDescriptor GetTaskExecutionDescriptor(RisTask task)
        {
            byte percent = 0;

            var triggerExecutionDescriptors = new List<TriggerExecutionDescriptor>();

            var triggers = this.TaskManager.GetRisTaskTriggers(task.Id);

            foreach (var trigger in triggers)
            {
                triggerExecutionDescriptors.Add(this.GetTriggerExecutionDescriptor(trigger));
            }

            var lastSendingDataState =
                triggerExecutionDescriptors.OrderBy(x => x.TaskTrigger.ObjectCreateDate)
                    .LastOrDefault(x => x.TaskTrigger.TriggerType == TriggerType.SendingData);

            if (lastSendingDataState == null)
            {
                var preparingDataState =
                    triggerExecutionDescriptors.FirstOrDefault(x => x.TaskTrigger.TriggerType == TriggerType.PreparingData);

                if (preparingDataState != null)
                {
                    percent = (byte) (preparingDataState.Percent * 50 / 100);
                }
            }
            else
            {
                percent = (byte) (50 + (lastSendingDataState.Percent * 50 / 100));
            }

            return new TaskExecutionDescriptor(task, triggerExecutionDescriptors, percent);
        }

        private TriggerExecutionDescriptor GetTriggerExecutionDescriptor(RisTaskTrigger taskTrigger)
        {
            byte percent = 0;

            var stateCalculator = new SubTaskStateCalculator(this.Container);
            percent = stateCalculator.GetExecutionPercent(taskTrigger);

            return new TriggerExecutionDescriptor(taskTrigger, percent);
        }

        private List<ITreeNode> GetPackageTreeNodes(long triggerId)
        {
            return this.TaskManager.GetTriggerPackages(triggerId).Select(this.CreatePackageNode).ToList();
        }

        private ITreeNode CreatePackageNode(RisPackageTrigger packageTrigger)
        {
            return new TaskTreeNode(packageTrigger);
        }
    }
}