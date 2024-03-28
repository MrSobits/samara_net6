namespace Bars.GkhGji.Regions.BaseChelyabinsk.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.GkhGji.Regions.BaseChelyabinsk.Entities;

    public class SMEVComplaintsExecutantViewModel : BaseViewModel<SMEVComplaintsExecutant>
    {
		public override IDataResult List(IDomainService<SMEVComplaintsExecutant> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);
			var complaintId = baseParams.Params.GetAs<long>("complaintId");

            var data = domainService.GetAll()
				.Where(x => x.SMEVComplaints.Id == complaintId)
                .Select(x => new
                {
                    x.Id,
                    Author = x.Author.Fio,
                    Executant = x.Executant.Fio,
                    x.PerformanceDate,
                    IsResponsible = x.IsResponsible ? "Да" : "Нет",
                    x.OrderDate,
                    x.Description
                })
                .Filter(loadParams, this.Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }
    }
}