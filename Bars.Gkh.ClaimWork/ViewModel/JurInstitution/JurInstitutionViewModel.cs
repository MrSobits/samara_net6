namespace Bars.Gkh.ViewModel
{
    using B4.Utils;
    using Bars.Gkh.Modules.ClaimWork.Entities;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using Modules.ClaimWork.Enums;

    public class JurInstitutionViewModel : BaseViewModel<JurInstitution>
    {
        public override IDataResult List(IDomainService<JurInstitution> domain, BaseParams baseParams)
        {
            var jurInstitutionRealObjDomain = Container.ResolveDomain<JurInstitutionRealObj>();

            try
            {
                var loadParams = GetLoadParam(baseParams);

                var type = baseParams.Params.GetAs<JurInstitutionType>("type");

                var data = domain.GetAll()
                    .WhereIf(type != 0, x => x.JurInstitutionType == type)
                    .Select(x => new
                    {
                        x.Id,
                        Municipality = x.Municipality.Name,
                        x.Name,
                        x.ShortName,
                        x.OutsideAddress,
                        x.Address,
                        RealObjCount = jurInstitutionRealObjDomain.GetAll().Count(y => y.JurInstitution.Id == x.Id),
                        x.Code
                    })
                    .Filter(loadParams, Container);

                return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
            }
            finally
            {
                Container.Release(jurInstitutionRealObjDomain);
            }
        }
    }
}