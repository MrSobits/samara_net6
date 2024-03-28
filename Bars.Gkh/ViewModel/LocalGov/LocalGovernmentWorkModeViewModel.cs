namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class LocalGovernmentWorkModeViewModel : BaseViewModel<LocalGovernmentWorkMode>
    {
        public override IDataResult List(IDomainService<LocalGovernmentWorkMode> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var localGovId = baseParams.Params.GetAs("localGovId", -1);

            var typeMode = baseParams.Params.GetAs("typeMode", -1);

            var dataCurrentWorkMode = domain.GetAll()
                .Where(x => x.LocalGovernment.Id == localGovId)
                .WhereIf(typeMode != -1, x => x.TypeMode == typeMode.To<TypeMode>())
                .ToList();

            var dataNewtWorkMode = new List<LocalGovernmentWorkMode>();

            foreach (TypeDayOfWeek day in Enum.GetValues(typeof(TypeDayOfWeek)))
            {
                var workModeRecord = dataCurrentWorkMode.FirstOrDefault(x => x.TypeDayOfWeek == day);

                var localGovernmentWorkMode = new LocalGovernmentWorkMode
                {
                    Id = (int)day, 
                    TypeDayOfWeek = day, 
                    TypeMode = typeMode.To<TypeMode>()
                };

                if (workModeRecord != null)
                {
                    localGovernmentWorkMode.StartDate = workModeRecord.StartDate;
                    localGovernmentWorkMode.EndDate = workModeRecord.EndDate;
                    localGovernmentWorkMode.Pause = workModeRecord.Pause;
                    localGovernmentWorkMode.AroundClock = workModeRecord.AroundClock;
                }

                dataNewtWorkMode.Add(localGovernmentWorkMode);
            }

            var dataMain = dataNewtWorkMode.AsQueryable();
            int totalCountMain = dataMain.Count();

            dataMain = dataMain.Order(loadParams).Paging(loadParams);

            return new ListDataResult(dataMain.ToList(), totalCountMain);
        }
    }
} 