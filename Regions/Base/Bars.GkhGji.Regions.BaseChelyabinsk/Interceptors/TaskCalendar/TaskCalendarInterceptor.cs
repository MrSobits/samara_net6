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

    class TaskCalendarInterceptor : EmptyDomainInterceptor<TaskCalendar>
    {
        public IGkhUserManager UserManager { get; set; }
        public IDomainService<ProdCalendar> ProdCalendarDomain { get; set; }

       public override IDataResult BeforeCreateAction(IDomainService<TaskCalendar> service, TaskCalendar entity)
        {
            try
            {
                var pkDay = ProdCalendarDomain.GetAll()
                .FirstOrDefault(x => x.ProdDate == entity.DayDate);
                if (pkDay != null)
                {
                    entity.DayType = GkhCalendar.Enums.DayType.Holiday;
                }
                else
                {
                    if (entity.DayDate.DayOfWeek == DayOfWeek.Saturday || entity.DayDate.DayOfWeek == DayOfWeek.Sunday)
                    {
                        entity.DayType = GkhCalendar.Enums.DayType.DayOff;
                    }
                }
                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось создать запрос");
            }
            
        }
    }
}
