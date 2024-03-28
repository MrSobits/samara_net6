namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using B4;
    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Entities;
    using System;
    using System.Linq;

    public class ERKNMViewModel : BaseViewModel<ERKNM>
    {
        public override IDataResult List(IDomainService<ERKNM> domainService, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var dateStart = baseParams.Params.GetAs("dateStart", new DateTime());
            var dateEnd = baseParams.Params.GetAs("dateEnd", new DateTime());

            var viewErknmDomain = this.Container.ResolveDomain<ViewERKNM>();

            var data = viewErknmDomain.GetAll()
                .Where(x => x.RequestDate >= dateStart && x.RequestDate <= dateEnd)
                .Filter(loadParams, Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }
        private string GetDocData(DocumentGji disp, AppealCitsAdmonition admon)
        {
            if (disp != null)
            {
                if (disp.DocumentDate.HasValue)
                    return $"{disp.DocumentNumber} от {disp.DocumentDate.Value.ToString("dd.MM.yyyy")}";
            }
            else if (admon != null)
            {
                if (admon.DocumentDate.HasValue)
                {
                    return $"{admon.DocumentNumber} от {admon.DocumentDate.Value.ToString("dd.MM.yyyy")}";
                }
            }
            return "";
        }

    }
}
