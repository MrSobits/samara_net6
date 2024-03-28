namespace Bars.Gkh.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    /// <summary>Реализация BaseDomainService</summary>
    public class ManOrgWorkModeViewModel : BaseViewModel<ManagingOrgWorkMode>
    {
        public override IDataResult List(IDomainService<ManagingOrgWorkMode> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var manorgId = baseParams.Params.ContainsKey("manorgId")
                    ? baseParams.Params.GetValue("manorgId").ToInt()
                    : -1;

            var typeMode = baseParams.Params.ContainsKey("typeMode")
                ? baseParams.Params.GetValue("typeMode").ToInt()
                : -1;

            var dataCurrentWorkMode = domain
                .GetAll()
                .Where(x => x.ManagingOrganization.Id == manorgId)
                .WhereIf(typeMode != -1, x => x.TypeMode == typeMode.To<TypeMode>())
                .ToList();

            var dataNewtWorkMode = new List<ManagingOrgWorkMode>();

            foreach (TypeDayOfWeek day in Enum.GetValues(typeof(TypeDayOfWeek)))
            {
                var workModeRecord = dataCurrentWorkMode.FirstOrDefault(x => x.TypeDayOfWeek == day);

                var managingOrgWorkMode = new ManagingOrgWorkMode { Id = (int)day, TypeDayOfWeek = day, TypeMode = typeMode.To<TypeMode>() };
                if (workModeRecord != null)
                {
                    managingOrgWorkMode.StartDate = workModeRecord.StartDate;
                    managingOrgWorkMode.EndDate = workModeRecord.EndDate;
                    managingOrgWorkMode.Pause = workModeRecord.Pause;
                    managingOrgWorkMode.AroundClock = workModeRecord.AroundClock;
                }

                dataNewtWorkMode.Add(managingOrgWorkMode);
            }

            var dataMain = dataNewtWorkMode.AsQueryable();
            int totalCountMain = dataMain.Count();

            dataMain = dataMain.Order(loadParams).Paging(loadParams);

            return new ListDataResult(dataMain.ToList(), totalCountMain);
        }
    }
} 