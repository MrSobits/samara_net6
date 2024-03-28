using Bars.B4.Modules.States;
using Bars.Gkh.Domain.CollectionExtensions;

namespace Bars.Gkh.Interceptors
{
    using System;
    using System.Linq;

    using B4;

    using Bars.B4.Utils;

    using Entities;

    public class ManOrgRequestRPGUInterceptor : EmptyDomainInterceptor<ManOrgRequestRPGU>
    {
        public override IDataResult BeforeCreateAction(IDomainService<ManOrgRequestRPGU> service, ManOrgRequestRPGU entity)
        {

            entity.Date = DateTime.Now;
            if (entity.RequestRPGUState != Enums.RequestRPGUState.Queued)
            {
                entity.RequestRPGUState = Enums.RequestRPGUState.Waiting;
            }

            return Success();
        }     
        
      
    }
}