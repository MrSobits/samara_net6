namespace Bars.Gkh.Controllers
{
    using System;
    using Bars.B4;
    using B4.Modules.Queue;
    using Microsoft.AspNetCore.Mvc;
    using Bars.B4.Modules.Queue.Contracts;
    using B4.DataAccess;
    using B4.Modules.Tasks.Common.Entities;
    using System.Linq;

    /// <summary>
    /// Контроллер очистки очереди RabbitMQ
    /// </summary>
    public class GkhTasksController : BaseController
    {
        /// <summary>
        /// Action очистки очереди RabbitMQ
        /// </summary>
        /// <returns>Результат выполнения</returns>
        public ActionResult ClearQueueRabbitMQ()
        {
            var client = this.Container.Resolve<IQueueClient>();
            bool flag = true;
            try
            {
                while (flag)
                {
                    var message = client.Get("tasks");
                    if (message != null)
                    {
                        client.NotifyQueue(
                            new NotifyOptions
                            {
                                MessageId = message.Id,
                                MessageState = MessageState.Success
                            });
                    }
                    else
                    {
                        flag = false;
                    }
                }
                return this.JsSuccess();
            }
            catch (Exception ex)
            {
                return JsonNetResult.Failure(ex.Message);
            }
            finally
            {
                this.Container.Release(client);
            }
        }

        /// <summary>
        /// Action удаления задачи
        /// </summary>
        /// <param name="ParentId"></param>
        /// <returns>Результат выполнения</returns>
        public ActionResult DeleteTask(long ParentId)
        {
            var taskHistoryDomain = this.Container.ResolveDomain<TaskHistory>();
            var taskEntryDomain = this.Container.ResolveDomain<TaskEntry>();
            try
            {
                //получаем контейнер taskentryes с Id дочек
                var taskentryes = taskEntryDomain.GetAll()
                    .Where(x => x.Parent.Id == ParentId)
                    .Select(x => x.Id)
                    .ToArray();

                //получаем контейнер taskhistories с Id дочек
                var taskhistories = taskHistoryDomain.GetAll()
                    .Where(x => taskentryes.Contains(x.Task.Id))
                    .Select(x => x.Id)
                    .ToArray();

                foreach (var taskhistory in taskhistories)
                {
                    taskHistoryDomain.Delete(taskhistory);
                }

                foreach (var taskentry in taskentryes)
                {
                    taskEntryDomain.Delete(taskentry);
                }

                taskEntryDomain.Delete(ParentId);
            }
            catch (Exception ex)
            {
                return JsonNetResult.Failure(ex.Message);
            }
            finally
            {
                this.Container.Release(taskHistoryDomain);
                this.Container.Release(taskEntryDomain);
            }
            return this.JsSuccess();
        }
    }
}
