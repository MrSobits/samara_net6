namespace Bars.GkhGji.Regions.Tomsk.ViewModel
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Regions.Tomsk.Entities;

    /// <summary>
    /// Перекрыт в модуле Интеграция с ЭДО
    /// </summary>
    public class AppealCitsAnswerAddresseeViewModel : BaseViewModel<AppealCitsAnswerAddressee>
    {
        public override IDataResult List(IDomainService<AppealCitsAnswerAddressee> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var answerId = baseParams.Params.GetAs("answerId", 0L);

            var data = domainService.GetAll()
                .Where(x => x.Answer.Id == answerId)
                .Select(x => new
                {
                    x.Id,
                    Addressee = x.Addressee.Name
                })
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            data = data.Order(loadParams).Paging(loadParams);

            return new ListDataResult(data.ToList(), totalCount);
        }
    }
}