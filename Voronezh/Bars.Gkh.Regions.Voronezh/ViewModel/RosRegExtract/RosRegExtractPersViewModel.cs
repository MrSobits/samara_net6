namespace Bars.Gkh.Regions.Voronezh.ViewModel
{
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;
    using Entities;
    using B4.Utils;

    public class RosRegExtractPersViewModel : BaseViewModel<RosRegExtractPers>
    {
        public override IDataResult List(IDomainService<RosRegExtractPers> domain, BaseParams baseParams)
        {
            var loadParams = GetLoadParam(baseParams);
            var parentId = loadParams.Filter.GetAs("parentId", 0L);
            var rosRegExtractDomain = this.Container.Resolve<IDomainService<RosRegExtract>>();
            var rosregExtract = rosRegExtractDomain.GetAll()
                .Where(x=> x.desc_id.Id == parentId)
                .Where(x=> x.right_id != null && x.right_id.owner_id != null && x.right_id.owner_id.pers_id != null)
                .Select(x=> new
                {
                    x.right_id.owner_id.pers_id,
                    x.Id,
                    //Person
                    x.right_id.owner_id.pers_id.Pers_Code_SP,
                    x.right_id.owner_id.pers_id.Pers_Content,
                    x.right_id.owner_id.pers_id.Pers_FIO_Surname,
                    x.right_id.owner_id.pers_id.Pers_FIO_First,
                    x.right_id.owner_id.pers_id.Pers_FIO_Patronymic,
                    x.right_id.owner_id.pers_id.Pers_DateBirth,
                    x.right_id.owner_id.pers_id.Pers_Place_Birth,
                    x.right_id.owner_id.pers_id.Pers_Citizen,
                    x.right_id.owner_id.pers_id.Pers_Sex,
                    x.right_id.owner_id.pers_id.Pers_DocContent,
                    x.right_id.owner_id.pers_id.Pers_DocType_Document,
                    x.right_id.owner_id.pers_id.Pers_DocName,
                    x.right_id.owner_id.pers_id.Pers_DocSeries,
                    x.right_id.owner_id.pers_id.Pers_DocNumber,
                    x.right_id.owner_id.pers_id.Pers_DocDate,
                    x.right_id.reg_id.Reg_ShareText,
                    //Person->Location
                    x.right_id.owner_id.pers_id.Pers_Loc_ID_Address,
                    x.right_id.owner_id.pers_id.Pers_Loc_Content,
                    x.right_id.owner_id.pers_id.Pers_Loc_CountryCode,
                    x.right_id.owner_id.pers_id.Pers_Loc_CountryName,
                    x.right_id.owner_id.pers_id.Pers_Loc_RegionCode,
                    x.right_id.owner_id.pers_id.Pers_Loc_RegionName,
                    x.right_id.owner_id.pers_id.Pers_Loc_Code_OKATO,
                    x.right_id.owner_id.pers_id.Pers_Loc_Code_KLADR,
                    x.right_id.owner_id.pers_id.Pers_Loc_DistrictName,
                    x.right_id.owner_id.pers_id.Pers_Loc_Urban_DistrictName,
                    x.right_id.owner_id.pers_id.Pers_Loc_LocalityName,
                    x.right_id.owner_id.pers_id.Pers_Loc_StreetName,
                    x.right_id.owner_id.pers_id.Pers_Loc_Level1Name,
                    x.right_id.owner_id.pers_id.Pers_Loc_Level2Name,
                    x.right_id.owner_id.pers_id.Pers_Loc_Level3Name,
                    x.right_id.owner_id.pers_id.Pers_Loc_ApartmentName,
                    x.right_id.owner_id.pers_id.Pers_Loc_Other,
                    //Person->FactLocation
                    x.right_id.owner_id.pers_id.Pers_Floc_ID_Address,
                    x.right_id.owner_id.pers_id.Pers_Floc_Content,
                    x.right_id.owner_id.pers_id.Pers_Floc_CountryCode,
                    x.right_id.owner_id.pers_id.Pers_Floc_CountryName,
                    x.right_id.owner_id.pers_id.Pers_Floc_RegionCode,
                    x.right_id.owner_id.pers_id.Pers_Floc_RegionName,
                    x.right_id.owner_id.pers_id.Pers_Floc_Code_OKATO,
                    x.right_id.owner_id.pers_id.Pers_Floc_Code_KLADR,
                    x.right_id.owner_id.pers_id.Pers_Floc_DistrictName,
                    x.right_id.owner_id.pers_id.Pers_Floc_Urban_DistrictName,
                    x.right_id.owner_id.pers_id.Pers_Floc_FlocalityName,
                    x.right_id.owner_id.pers_id.Pers_Floc_StreetName,
                    x.right_id.owner_id.pers_id.Pers_Floc_Level1Name,
                    x.right_id.owner_id.pers_id.Pers_Floc_Level2Name,
                    x.right_id.owner_id.pers_id.Pers_Floc_Level3Name,
                    x.right_id.owner_id.pers_id.Pers_Floc_ApartmentName,
                    x.right_id.owner_id.pers_id.Pers_Floc_Other
                }).Filter(loadParams, Container); ;
            //long persId = 0;
            //if (rosregExtract.right_id != null && rosregExtract.right_id.owner_id != null && rosregExtract.right_id.owner_id.pers_id != null)
            //{
            //    persId = rosregExtract.right_id.owner_id.pers_id.Id;
            //}

            //var data = domain.GetAll()
            //    .Where(x=> x.Id == persId)
            //   .Select(x => new
            //   {
            //       x.Id,
            //       //Person
            //       x.Pers_Code_SP,
            //       x.Pers_Content,
            //       x.Pers_FIO_Surname,
            //       x.Pers_FIO_First,
            //       x.Pers_FIO_Patronymic,
            //       x.Pers_DateBirth,
            //       x.Pers_Place_Birth,
            //       x.Pers_Citizen,
            //       x.Pers_Sex,
            //       x.Pers_DocContent,
            //       x.Pers_DocType_Document,
            //       x.Pers_DocName,
            //       x.Pers_DocSeries,
            //       x.Pers_DocNumber,
            //       x.Pers_DocDate,
            //       //Person->Location
            //       x.Pers_Loc_ID_Address,
            //       x.Pers_Loc_Content,
            //       x.Pers_Loc_CountryCode,
            //       x.Pers_Loc_CountryName,
            //       x.Pers_Loc_RegionCode,
            //       x.Pers_Loc_RegionName,
            //       x.Pers_Loc_Code_OKATO,
            //       x.Pers_Loc_Code_KLADR,
            //       x.Pers_Loc_DistrictName,
            //       x.Pers_Loc_Urban_DistrictName,
            //       x.Pers_Loc_LocalityName,
            //       x.Pers_Loc_StreetName,
            //       x.Pers_Loc_Level1Name,
            //       x.Pers_Loc_Level2Name,
            //       x.Pers_Loc_Level3Name,
            //       x.Pers_Loc_ApartmentName,
            //       x.Pers_Loc_Other,
            //       //Person->FactLocation
            //       x.Pers_Floc_ID_Address,
            //       x.Pers_Floc_Content,
            //       x.Pers_Floc_CountryCode,
            //       x.Pers_Floc_CountryName,
            //       x.Pers_Floc_RegionCode,
            //       x.Pers_Floc_RegionName,
            //       x.Pers_Floc_Code_OKATO,
            //       x.Pers_Floc_Code_KLADR,
            //       x.Pers_Floc_DistrictName,
            //       x.Pers_Floc_Urban_DistrictName,
            //       x.Pers_Floc_FlocalityName,
            //       x.Pers_Floc_StreetName,
            //       x.Pers_Floc_Level1Name,
            //       x.Pers_Floc_Level2Name,
            //       x.Pers_Floc_Level3Name,
            //       x.Pers_Floc_ApartmentName,
            //       x.Pers_Floc_Other
            //   })
            //    .Filter(loadParams, Container);

            return new ListDataResult(rosregExtract.Order(loadParams).Paging(loadParams).ToList(), rosregExtract.Count());
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<RosRegExtractPers> domainService, BaseParams baseParams)
        {
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        //Person
                        obj.Pers_Code_SP,
                        obj.Pers_Content,
                        obj.Pers_FIO_Surname,
                        obj.Pers_FIO_First,
                        obj.Pers_FIO_Patronymic,
                        obj.Pers_DateBirth,
                        obj.Pers_Place_Birth,
                        obj.Pers_Citizen,
                        obj.Pers_Sex,
                        obj.Pers_DocContent,
                        obj.Pers_DocType_Document,
                        obj.Pers_DocName,
                        obj.Pers_DocSeries,
                        obj.Pers_DocNumber,
                        obj.Pers_DocDate,
                        //Person->Location
                        obj.Pers_Loc_ID_Address,
                        obj.Pers_Loc_Content,
                        obj.Pers_Loc_CountryCode,
                        obj.Pers_Loc_CountryName,
                        obj.Pers_Loc_RegionCode,
                        obj.Pers_Loc_RegionName,
                        obj.Pers_Loc_Code_OKATO,
                        obj.Pers_Loc_Code_KLADR,
                        obj.Pers_Loc_DistrictName,
                        obj.Pers_Loc_Urban_DistrictName,
                        obj.Pers_Loc_LocalityName,
                        obj.Pers_Loc_StreetName,
                        obj.Pers_Loc_Level1Name,
                        obj.Pers_Loc_Level2Name,
                        obj.Pers_Loc_Level3Name,
                        obj.Pers_Loc_ApartmentName,
                        obj.Pers_Loc_Other,
                        //Person->FactLocation
                        obj.Pers_Floc_ID_Address,
                        obj.Pers_Floc_Content,
                        obj.Pers_Floc_CountryCode,
                        obj.Pers_Floc_CountryName,
                        obj.Pers_Floc_RegionCode,
                        obj.Pers_Floc_RegionName,
                        obj.Pers_Floc_Code_OKATO,
                        obj.Pers_Floc_Code_KLADR,
                        obj.Pers_Floc_DistrictName,
                        obj.Pers_Floc_Urban_DistrictName,
                        obj.Pers_Floc_FlocalityName,
                        obj.Pers_Floc_StreetName,
                        obj.Pers_Floc_Level1Name,
                        obj.Pers_Floc_Level2Name,
                        obj.Pers_Floc_Level3Name,
                        obj.Pers_Floc_ApartmentName,
                        obj.Pers_Floc_Other
                    });
            }
            return new BaseDataResult();
        }
    }
}
