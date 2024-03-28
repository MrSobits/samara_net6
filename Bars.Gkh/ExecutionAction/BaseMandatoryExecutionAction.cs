namespace Bars.Gkh.ExecutionAction
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities.Administration.ExecutionAction;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Абстрактный класс для обязательных к выполнению действий
    /// </summary>
    public abstract class BaseMandatoryExecutionAction : BaseExecutionAction, IMandatoryExecutionAction
    {
        /// <summary>
        /// Проверка необходимости выполнения действия
        /// </summary>
        public virtual bool IsNeedAction()
        {
            var result = false;
            this.Container.UsingForResolved<IRepository<ExecutionActionResult>>((ioc, repository) =>
            {
                result = repository.GetAll()
                    .Where(x => x.Task.ActionCode == this.Code)
                    .Where(x => x.Status == ExecutionActionStatus.Success)
                    .IsEmpty();
            });

            return result;
        }
    }
}