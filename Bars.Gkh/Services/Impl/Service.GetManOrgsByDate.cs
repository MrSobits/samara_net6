namespace Bars.Gkh.Services.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Services.DataContracts;
    using Bars.Gkh.Services.DataContracts.GetMainInfoManOrg;
    using Bars.Gkh.Services.DataContracts.GetManOrgsByDate;

    public partial class Service
    {
        public GetRoManOrgsResponseByDate GetRoManOrgsByDate(string date)
        {
            var info = Container.Resolve<IDomainService<ManOrgContractRealityObject>>()
                            .GetAll()
                            .Where(x => x.ManOrgContract.StartDate >= date.ToDateTime())
                            .AsEnumerable()
                            .Select(
                                x =>
                                new ManOrgRealObject
                                {
                                    ManOrg = x.ManOrgContract.ManagingOrganization.Id,
                                    RealObj = x.RealityObject.Id,
                                    Date = x.ManOrgContract.StartDate.ToString()
                                })
                            .ToArray();
            
            var result = info.Length == 0 ? Result.DataNotFound : Result.NoErrors;

            return new GetRoManOrgsResponseByDate { Information = info, Result = result };
        }
    }
}