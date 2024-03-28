namespace Bars.GkhGji.Regions.Habarovsk.Interceptors
{
    using Entities;
    using System;
    using System.Collections.Generic;
    using Enums;
    using Bars.GkhGji.Enums;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Entities;
    using System.Linq;
    using B4.Modules.States;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.Protocol197;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities.AppealCits;
    using Bars.B4.DataAccess;

    class AppealOrderFileInterceptor : EmptyDomainInterceptor<AppealOrderFile>
    {

        public IRepository<AppealOrder> AppealOrderRepo { get; set; }
        public IDomainService<State> StateDomain { get; set; }

        public override IDataResult BeforeCreateAction(IDomainService<AppealOrderFile> service, AppealOrderFile entity)
        {
            try
            {
                if (entity.FileInfo == null)
                {
                    return Failure("Не прикреплено ни одного файла");                  
                }


                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить задачу");
            }
        }
        public override IDataResult AfterCreateAction(IDomainService<AppealOrderFile> service, AppealOrderFile entity)
        {
            try
            {
                if (entity.FileInfo != null)
                {
                    var order = AppealOrderRepo.Get(entity.AppealOrder.Id);
                    if (!order.PerformanceDate.HasValue)
                    {
                        order.PerformanceDate = DateTime.Now;
                    }
                }


                return Success();
            }
            catch (Exception e)
            {
                return Failure("Не удалось сохранить задачу");
            }
        }


    }
}
