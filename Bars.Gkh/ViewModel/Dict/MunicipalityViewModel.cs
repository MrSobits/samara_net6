namespace Bars.Gkh.ViewModel
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Authentification;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;

    public class MunicipalityViewModel : BaseViewModel<Municipality>
    {
        public override IDataResult List(IDomainService<Municipality> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);

            var ids = baseParams.Params.GetAs("Id", string.Empty);

            var level = baseParams.Params.GetAs("levelMun", string.Empty);

            var manOrg = baseParams.Params.GetAs("manorg", string.Empty);

            var parentMuIds = baseParams.Params.GetAs("parentMuIds", string.Empty);
            var listParentIds = !string.IsNullOrEmpty(parentMuIds) ? parentMuIds.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            var listIds = !string.IsNullOrEmpty(ids) ? ids.Split(',').Select(id => id.ToLong()).ToArray() : new long[0];

            // если переданы ид'шники мун. образований верхнего уровня, т.е. необходимо грузить муниципальные образования нижнего уровня
            domain = listParentIds.Length > 0 ? new BaseDomainService<Municipality> { Container = Container } : domain;

            var parents = domain.GetAll()
                .Where(x => x.ParentMo != null)
                .Select(x => x.ParentMo.Id)
                .Distinct();

            var data = domain.GetAll()
                .WhereIf(listIds.Length > 0, x => listIds.Contains(x.Id))
                .WhereIf(listParentIds.Length > 0, x => listParentIds.Contains(x.ParentMo.Id))
                .WhereIf(level == "1", x => x.Level != TypeMunicipality.UrbanSettlement)
                .WhereIf(manOrg != string.Empty, x => !parents.Contains(x.Id))
                .Select(x => new
                {
                    x.Id,
                    x.Code,
                    x.FiasId,
                    x.Group,
                    x.Name,
                    x.Okato,
                    x.Oktmo,
                    x.Description,
                    x.FederalNumber,
                    x.Level,
                    x.Cut
                })
                .Filter(loadParams, Container);

            return new ListDataResult(
                loadParams.Order.Length > 0
                    ? data.Order(loadParams).Paging(loadParams).ToList()
                    : data.OrderBy(x => x.Name).Paging(loadParams).ToList(),
                data.Count());
        }

        public override IDataResult Get(IDomainService<Municipality> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");
            var obj = domainService.Get(id);

            var repository = Container.Resolve<IFiasRepository>();
            if (obj != null && obj.RegionName.IsEmpty())
            {
                var fiasRegions = repository.GetAll().Where(x => x.ParentGuid == string.Empty);
                if (fiasRegions.Count() == 1)
                {
                    obj.RegionName = fiasRegions.First().OffName;
                }
            }

            return obj != null ? new BaseDataResult(
                 new
                 {
                     obj.Id,
                     obj.ExternalId,
                     obj.ObjectCreateDate,
                     obj.ObjectEditDate,
                     obj.ObjectVersion,
                     obj.Code,
                     obj.FiasId,
                     obj.Group,
                     obj.Name,
                     obj.Okato,
                     obj.Oktmo,
                     obj.Description,
                     obj.FederalNumber,
                     obj.Cut,
                     obj.Level,
                     DinamicFias = !string.IsNullOrEmpty(obj.FiasId) ? (DinamicAddress)repository.GetDinamicAddress(obj.FiasId) : null,
                     obj.RegionName,
                     obj.CheckCertificateValidity,
                     obj.ParentMo
                 }) : new BaseDataResult();


        }
    }
}