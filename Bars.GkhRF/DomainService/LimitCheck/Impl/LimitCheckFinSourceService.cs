using System;
using System.Collections.Generic;
using System.Linq;
using Castle.Windsor;

namespace Bars.GkhRf.DomainService
{
    using B4;
    using B4.Utils;

    using Entities;
    using GkhCr.Entities;

    public class LimitCheckFinSourceService : ILimitCheckFinSourceService
    {
        public IWindsorContainer Container { get; set; }

        public IDataResult AddFinSources(BaseParams baseParams)
        {
            try
            {
                /*
                 * limitCheckId - идентификатор проверки
                 * objectIds - идентификаторы выбранных источников финансирования
                 */
                var limitCheckId = baseParams.Params.ContainsKey("limitCheckId") ? baseParams.Params["limitCheckId"].ToLong() : 0;
                var objectIds = baseParams.Params.ContainsKey("objectIds") ? baseParams.Params["objectIds"].ToString() : "";

                if (limitCheckId > 0 && !string.IsNullOrEmpty(objectIds))
                {
                    var service = Container.Resolve<IDomainService<LimitCheckFinSource>>();

                    var existsRecs = service.GetAll().Where(x => x.LimitCheck.Id == limitCheckId).Select(x => x.FinanceSource.Id).ToList();

                    foreach (var id in objectIds.Split(',').Select(x => x.ToLong()))
                    {
                        if (!existsRecs.Contains(id))
                        {
                            var newRec = new LimitCheckFinSource
                                {
                                    FinanceSource = new FinanceSource { Id = id },
                                    LimitCheck = new LimitCheck { Id = limitCheckId }
                                };

                            service.Save(newRec);
                        }
                    }
                }

                return new BaseDataResult();
            }
            catch (ValidationException e)
            {
                return new BaseDataResult {Message = e.Message, Success = false};
            }
        }
    }
}