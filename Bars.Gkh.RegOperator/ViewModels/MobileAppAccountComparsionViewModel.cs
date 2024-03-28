//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;

namespace Bars.Gkh.RegOperator.ViewModels
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.Utils;
    class MobileAppAccountComparsionViewModel : BaseViewModel<MobileAppAccountComparsion>
    {
        public override IDataResult List(IDomainService<MobileAppAccountComparsion> domainService, BaseParams baseParams)
        {
            var loadParam = GetLoadParam(baseParams);
            var showCloseAppeals = baseParams.Params.GetAs("showCloseAppeals", true);
            if (showCloseAppeals)
            {
                var data = domainService.GetAll()
               .Select(x => new
               {
                   x.Id,
                   x.IsViewed,
                   x.IsWorkOut,
                   x.MobileAccountNumber,
                   x.MobileAccountOwnerFIO,
                   x.ExternalAccountNumber,
                   x.FkrUserFio,
                   x.PersonalAccountOwnerFIO,
                   x.PersonalAccountNumber,
                   x.OperatinDate,
                   x.DecisionType

               }).Filter(loadParam, this.Container);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
            }
            else
            {
                var data = domainService.GetAll()
                    .Where(x => x.IsWorkOut == false)
                    .Select(x => new
                    {
                        x.Id,
                        x.IsViewed,
                        x.IsWorkOut,
                        x.MobileAccountNumber,
                        x.MobileAccountOwnerFIO,
                        x.ExternalAccountNumber,
                        x.FkrUserFio,
                        x.PersonalAccountOwnerFIO,
                        x.PersonalAccountNumber,
                        x.OperatinDate,
                        x.DecisionType

                    }).Filter(loadParam, this.Container);

                return new ListDataResult(data.Order(loadParam).Paging(loadParam).ToList(), data.Count());
            }


        }
    }
}
