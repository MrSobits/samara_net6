namespace Bars.GkhGji.StateChange
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    /// <summary>
    /// Обработчик события смены статуса уведомления о предпр. деят.
    /// </summary>
    public class BusinessActivityStateChangeHandler : IStateChangeHandler
    {
        public IWindsorContainer Container { get; set; }

        public void OnStateChange(IStatefulEntity entity, State oldState, State newState)
        {
            if (entity.State.TypeId == "gji_business_activity")
            {
                var businessActivity = entity as BusinessActivity;
                if (businessActivity == null)
                {
                    throw new InvalidCastException("Не удалось привести к типу BusinessActivity");
                }

                var maxRegNum =
                    this.Container.Resolve<IDomainService<BusinessActivity>>()
                             .GetAll().Max(x => x.RegNumber);

                if (newState.FinalState && businessActivity.Registered == false)
                {
                    businessActivity.Registered = true;
                    businessActivity.RegNumber = maxRegNum + 1;
                    if (businessActivity.TypeKindActivity == TypeKindActivity.ServiceMkd)
                    {
                        businessActivity.RegNum = string.Format("{0}-О", maxRegNum + 1);
                    }

                    if (businessActivity.TypeKindActivity == TypeKindActivity.ManagmentMkd)
                    {
                        businessActivity.RegNum = string.Format("{0}-У", maxRegNum + 1);
                    }

                    if (businessActivity.TypeKindActivity == TypeKindActivity.ManagmentAndServiceMkd)
                    {
                        businessActivity.RegNum = string.Format("{0}-УО", maxRegNum + 1);
                    }
                }
            }
        }
    }
}