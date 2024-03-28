namespace Bars.GkhGji.Regions.BaseChelyabinsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using Bars.B4.Modules.States;

    class SMEVComplaintsInterceptor : EmptyDomainInterceptor<SMEVComplaints>
    {
        public IGkhUserManager UserManager { get; set; }

       public override IDataResult BeforeCreateAction(IDomainService<SMEVComplaints> service, SMEVComplaints entity)
        {
            var servStateProvider = Container.Resolve<IStateProvider>();

            try
            {
                servStateProvider.SetDefaultState(entity);
            }
            finally
            {
                Container.Release(servStateProvider);
            }
            return Success();

        }      
    }
}
