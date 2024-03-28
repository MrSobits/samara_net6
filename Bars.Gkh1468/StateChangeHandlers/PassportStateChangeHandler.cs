namespace Bars.Gkh1468.StateChangeHandlers
{
    using System;

    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Authentification;
    using Bars.Gkh1468.Entities;

    using Castle.Windsor;

    public class PassportStateChangeHandler : IStateChangeHandler
    {
        public IWindsorContainer Container { get; set; }

        public void OnStateChange(IStatefulEntity entity, State s1, State s2)
        {
            var manager = Container.Resolve<IGkhUserManager>();
            
            if (entity is OkiProviderPassport)
            {
                var ds = Container.Resolve<IDomainService<OkiProviderPassport>>();
                var pasport = entity as OkiProviderPassport;
                var curOp = manager.GetActiveOperator();
                pasport.UserName = curOp != null ? curOp.User.Name : string.Empty;
                pasport.SignDate = DateTime.Now;
                ds.Update(pasport);

                Container.Release(ds);
            }
            else if (entity is HouseProviderPassport)
            {
                var ds = Container.Resolve<IDomainService<HouseProviderPassport>>();
                var pasport = entity as HouseProviderPassport;
                var curOp = manager.GetActiveOperator();
                pasport.UserName = curOp != null ? curOp.User.Name : string.Empty;
                pasport.SignDate = DateTime.Now;
                ds.Update(pasport);

                Container.Release(ds);
            }

            Container.Release(manager);
        }
    }
}
