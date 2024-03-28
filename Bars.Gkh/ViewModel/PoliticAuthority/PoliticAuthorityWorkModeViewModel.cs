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
    public class PoliticAuthorityWorkModeViewModel : BaseViewModel<PoliticAuthorityWorkMode>
    {
        public override IDataResult List(IDomainService<PoliticAuthorityWorkMode> domain, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var politicAuthId = baseParams.Params.ContainsKey("politicAuthId")
                    ? baseParams.Params.GetValue("politicAuthId").ToInt()
                    : -1;

            var typeMode = baseParams.Params.ContainsKey("typeMode")
                ? baseParams.Params.GetValue("typeMode").ToInt()
                : -1;

            var dataCurrentWorkMode = domain
                .GetAll()
                .Where(x => x.PoliticAuthority.Id == politicAuthId)
                .WhereIf(typeMode != -1, x => x.TypeMode == typeMode.To<TypeMode>())
                .ToList();

            var dataNewtWorkMode = new List<PoliticAuthorityWorkMode>();

            foreach (TypeDayOfWeek day in Enum.GetValues(typeof(TypeDayOfWeek)))
            {
                var workModeRecord = dataCurrentWorkMode.FirstOrDefault(x => x.TypeDayOfWeek == day);

                var politicAuthorityWorkMode = new PoliticAuthorityWorkMode { Id = (int)day, TypeDayOfWeek = day, TypeMode = typeMode.To<TypeMode>() };
                if (workModeRecord != null)
                {
                    politicAuthorityWorkMode.StartDate = workModeRecord.StartDate;
                    politicAuthorityWorkMode.EndDate = workModeRecord.EndDate;
                    politicAuthorityWorkMode.Pause = workModeRecord.Pause;
                    politicAuthorityWorkMode.AroundClock = workModeRecord.AroundClock;
                }

                dataNewtWorkMode.Add(politicAuthorityWorkMode);
            }

            var dataMain = dataNewtWorkMode.AsQueryable();
            int totalCountMain = dataMain.Count();

            dataMain = dataMain.Order(loadParams).Paging(loadParams);

            return new ListDataResult(dataMain.ToList(), totalCountMain);
        }
    }
} 