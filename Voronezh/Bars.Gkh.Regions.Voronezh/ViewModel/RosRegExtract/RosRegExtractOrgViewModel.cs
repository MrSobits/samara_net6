namespace Bars.Gkh.Regions.Voronezh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;
    using Entities;
    using B4.Utils;

    public class RosRegExtractOrgViewModel : BaseViewModel<RosRegExtractOrg>
    {
        public override IDataResult List(IDomainService<RosRegExtractOrg> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var parentId = loadParams.Filter.GetAs("parentId", 0L);
            var rosRegExtractDomain = this.Container.Resolve<IDomainService<RosRegExtract>>();
            var rosregExtract = rosRegExtractDomain.GetAll()
                 .Where(x => x.desc_id.Id == parentId)
                 .Where(x => x.right_id != null && x.right_id.owner_id != null && x.right_id.owner_id.org_id != null)
                 .Select(x => new
                 {

                     x.right_id.owner_id.org_id.Id,
                     //Organization
                     x.right_id.owner_id.org_id.Org_Code_SP,
                     x.right_id.owner_id.org_id.Org_Content,
                     x.right_id.owner_id.org_id.Org_Code_OPF,
                     x.right_id.owner_id.org_id.Org_Name,
                     x.right_id.owner_id.org_id.Org_Inn,
                     x.right_id.owner_id.org_id.Org_Code_OGRN,
                     x.right_id.owner_id.org_id.Org_RegDate,
                     x.right_id.owner_id.org_id.Org_AgencyRegistration,
                     x.right_id.owner_id.org_id.Org_Code_CPP,
                     //Organization->Location
                     x.right_id.owner_id.org_id.Org_Loc_ID_Address,
                     x.right_id.owner_id.org_id.Org_Loc_Content,
                     x.right_id.owner_id.org_id.Org_Loc_RegionCode,
                     x.right_id.owner_id.org_id.Org_Loc_RegionName,
                     x.right_id.owner_id.org_id.Org_Loc_Code_OKATO,
                     x.right_id.owner_id.org_id.Org_Loc_Code_KLADR,
                     x.right_id.owner_id.org_id.Org_Loc_CityName,
                     x.right_id.owner_id.org_id.Org_Loc_StreetName,
                     x.right_id.owner_id.org_id.Org_Loc_Level1Name,
                     //Organization->FactLocation
                     x.right_id.owner_id.org_id.Org_FLoc_ID_Address,
                     x.right_id.owner_id.org_id.Org_FLoc_Content,
                     x.right_id.owner_id.org_id.Org_FLoc_RegionCode,
                     x.right_id.owner_id.org_id.Org_FLoc_RegionName,
                     x.right_id.owner_id.org_id.Org_FLoc_Code_OKATO,
                     x.right_id.owner_id.org_id.Org_FLoc_Code_KLADR,
                     x.right_id.owner_id.org_id.Org_FLoc_CityName,
                     x.right_id.owner_id.org_id.Org_FLoc_StreetName,
                     x.right_id.owner_id.org_id.Org_FLoc_Level1Name
               })
                .Filter(loadParams, Container);

            return new ListDataResult(rosregExtract.Order(loadParams).Paging(loadParams).ToList(), rosregExtract.Count());
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<RosRegExtractOrg> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("Id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        //Organization
                        obj.Org_Code_SP,
                        obj.Org_Content,
                        obj.Org_Code_OPF,
                        obj.Org_Name,
                        obj.Org_Inn,
                        obj.Org_Code_OGRN,
                        obj.Org_RegDate,
                        obj.Org_AgencyRegistration,
                        obj.Org_Code_CPP,
                        //Organization->Location
                        obj.Org_Loc_ID_Address,
                        obj.Org_Loc_Content,
                        obj.Org_Loc_RegionCode,
                        obj.Org_Loc_RegionName,
                        obj.Org_Loc_Code_OKATO,
                        obj.Org_Loc_Code_KLADR,
                        obj.Org_Loc_CityName,
                        obj.Org_Loc_StreetName,
                        obj.Org_Loc_Level1Name,
                        //Organization->FactLocation
                        obj.Org_FLoc_ID_Address,
                        obj.Org_FLoc_Content,
                        obj.Org_FLoc_RegionCode,
                        obj.Org_FLoc_RegionName,
                        obj.Org_FLoc_Code_OKATO,
                        obj.Org_FLoc_Code_KLADR,
                        obj.Org_FLoc_CityName,
                        obj.Org_FLoc_StreetName,
                        obj.Org_FLoc_Level1Name
                    });
            }
            return new BaseDataResult();
        }
    }
}
