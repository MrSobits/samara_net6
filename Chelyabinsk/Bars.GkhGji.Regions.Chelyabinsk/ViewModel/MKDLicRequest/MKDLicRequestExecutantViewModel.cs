namespace Bars.GkhGji.Regions.Chelyabinsk.ViewModel
{
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Regions.Chelyabinsk.Entities;
    using System.Linq;

    public class MKDLicRequestExecutantViewModel : BaseViewModel<MKDLicRequestExecutant>
    {
        public override IDataResult List(IDomainService<MKDLicRequestExecutant> domainService, BaseParams baseParams)
        {
            var loadParams = this.GetLoadParam(baseParams);

            var requestId = baseParams.Params.GetAs<long>("mkdlicrequestId");

            var data = domainService.GetAll()
                                    .Where(x => x.MKDLicRequest.Id == requestId)
                                    .Select(x => new
                                    {
                                        x.Id,
                                        Author = x.Author.Fio,
                                        Executant = x.Executant.Fio,
                                        Controller = x.Controller.Fio,
                                        x.PerformanceDate,
                                        IsResponsible = x.IsResponsible ? "Да" : "Нет",
                                        OnApproval = x.OnApproval ? "Согласование" : "",
                                        x.State,
                                        x.Resolution,
                                        x.OrderDate,
                                        x.Description
                                    })
                                    .Filter(loadParams, this.Container);

            int totalCount = data.Count();

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
        }

        public override IDataResult Get(IDomainService<MKDLicRequestExecutant> domain, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domain.GetAll().FirstOrDefault(x => x.Id == id);

            if (obj != null)
            {

                var executantZjiName = string.Empty;
                var zonalInspectionInspectorDomain = this.Container.Resolve<IDomainService<ZonalInspectionInspector>>();

                if (obj.Executant != null)
                {
                    using (this.Container.Using(zonalInspectionInspectorDomain))
                    {
                        var zjiNames = zonalInspectionInspectorDomain.GetAll()
                            .Where(x => x.Inspector.Id == obj.Executant.Id)
                            .Select(x => x.ZonalInspection.Name)
                            .ToList();

                        executantZjiName = string.Join(", ", zjiNames);
                    }
                }

                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.MKDLicRequest,
                        obj.Author,
                        obj.Executant,
                        obj.Controller,
                        obj.IsResponsible,
                        obj.OrderDate,
                        obj.PerformanceDate,
                        obj.Description,
                        obj.State,
                        ExecutantZji = executantZjiName,
                        obj.Resolution
                    });
            }

            return new BaseDataResult();
        }
    }
}