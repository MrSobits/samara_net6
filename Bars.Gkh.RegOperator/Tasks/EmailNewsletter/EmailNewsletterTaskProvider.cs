namespace Bars.Gkh.RegOperator.Tasks.EmailNewsletter
{
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Modules.Tasks.Common.Contracts;
    using Bars.B4.Modules.Tasks.Common.Contracts.Result;
    using Bars.B4.Modules.Tasks.Common.Service;

    using Castle.Windsor;

    /// <summary>
    /// Провайдер для создания задач рассылки на эл. почту
    /// </summary>
    public class EmailNewsletterTaskProvider : ITaskProvider
    {
        private readonly IWindsorContainer container;

        /// <summary>Конструктор</summary>
        /// <param name="container">Контейнер</param>
        public EmailNewsletterTaskProvider(IWindsorContainer container)
        {
            this.container = container;
        }
        
        /// <summary>Код задачи</summary>
        public string TaskCode { get { return "EmailNewsletterSend"; } }

        /// <summary>Создать задачи </summary>
        /// <param name="baseParams">Параметры</param>
        /// <returns>Описатель задачи</returns>
        public CreateTasksResult CreateTasks(BaseParams baseParams)
        {
            var descriptors = new List<TaskDescriptor>
            {
                new TaskDescriptor(
                    "Рассылка на эл. почту",
                    EmailNewsletterTaskExecutor.Id,
                    baseParams)
                {
                    Dependencies = new[]
                    {
                        new Dependency
                        {
                            Key = this.TaskCode,
                            Scope = DependencyScope.InsideGlobalTasks
                        }
                    }
                }
            };

            return new CreateTasksResult(descriptors.ToArray());
        }
    }
}
