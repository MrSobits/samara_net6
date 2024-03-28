namespace Bars.Gkh.Regions.Voronezh.ViewModel
{
    using System.Linq;
    using System;
    using Bars.B4;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Regions.Voronezh.Entities.Dicts;
    using Entities;
    using Gkh.Entities;

    public class RosRegExtractDescViewModel : BaseViewModel<RosRegExtractDesc>
    {
        public override IDataResult List(IDomainService<RosRegExtractDesc> domain, BaseParams baseParams)
        {
          
            var loadParams = GetLoadParam(baseParams);

            var data = domain.GetAll()
            .Select(x => new
            {
                x.Id,
                x.Desc_ID_Object,
                x.Desc_CadastralNumber,
                x.Desc_ObjectType,
                x.Desc_ObjectTypeText,
                x.Desc_ObjectTypeName,
                x.Desc_AssignationCode,
                x.Desc_AssignationCodeText,
                x.Desc_Area,
                x.Desc_AreaText,
                x.Desc_AreaUnit,
                x.Desc_Floor,
                x.Desc_ID_Address,
                x.Desc_AddressContent,
                x.Desc_RegionCode,
                x.Desc_RegionName,
                x.Desc_OKATO,
                x.Desc_KLADR,
                x.Desc_CityName,
                x.Desc_Urban_District,
                x.Desc_Locality,
                x.Desc_StreetName,
                x.Desc_Level1Name,
                x.Desc_Level2Name,
                x.Desc_ApartmentName,
                x.YesNoNotSet
               // Reg_RegDate = x.right_id.reg_id.Reg_RegDate, 
               // x.right_id.reg_id.Reg_RegNumber
            })
             .Filter(loadParams, Container);

            //var data = domain.GetAll()
            //   .Select(x => new
            //   {
            //       x.Id,
            //       x.Desc_ID_Object,
            //       x.Desc_CadastralNumber,
            //       x.Desc_ObjectType,
            //       x.Desc_ObjectTypeText,
            //       x.Desc_ObjectTypeName,
            //       x.Desc_AssignationCode,
            //       x.Desc_AssignationCodeText,
            //       x.Desc_Area,
            //       x.Desc_AreaText,
            //       x.Desc_AreaUnit,
            //       x.Desc_Floor,
            //       x.Desc_ID_Address,
            //       x.Desc_AddressContent,
            //       x.Desc_RegionCode,
            //       x.Desc_RegionName,
            //       x.Desc_OKATO,
            //       x.Desc_KLADR,
            //       x.Desc_CityName,
            //       x.Desc_Urban_District,
            //       x.Desc_Locality,
            //       x.Desc_StreetName,
            //       x.Desc_Level1Name,
            //       x.Desc_Level2Name,
            //       x.Desc_ApartmentName
            //   })
            //    .Filter(loadParams, Container);

            return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), data.Count());
        }

        /// <summary>
        /// Получить объект
        /// </summary>
        /// <param name="domainService">Домен</param><param name="baseParams">Базовые параметры</param>
        /// <returns>
        /// Результат выполнения запроса
        /// </returns>
        public override IDataResult Get(IDomainService<RosRegExtractDesc> domainService, BaseParams baseParams)
        {
            var roomDomain = this.Container.Resolve<IDomainService<Room>>();
            var id = baseParams.Params.GetAs<long>("id");

            var obj = domainService.Get(id);

            if (obj != null)
            {
                return new BaseDataResult(
                    new
                    {
                        obj.Id,
                        obj.Desc_ID_Object,
                        obj.Desc_CadastralNumber,
                        obj.Desc_ObjectType,
                        obj.Desc_ObjectTypeText,
                        obj.Desc_ObjectTypeName,
                        obj.Desc_AssignationCode,
                        obj.Desc_AssignationCodeText,
                        obj.Desc_Area,
                        obj.Desc_AreaText,
                        obj.Desc_AreaUnit,
                        obj.Desc_Floor,
                        obj.Desc_ID_Address,
                        obj.Desc_AddressContent,
                        obj.Desc_RegionCode,
                        obj.Desc_RegionName,
                        obj.Desc_OKATO,
                        obj.Desc_KLADR,
                        obj.Desc_CityName,
                        obj.Desc_Urban_District,
                        obj.Desc_Locality,
                        obj.Desc_StreetName,
                        obj.Desc_Level1Name,
                        obj.Desc_Level2Name,
                        obj.Desc_ApartmentName,
                        Room_id = obj.Room_id>0? new { Id = obj.Room_id, Address = roomDomain.Get(obj.Room_id).RealityObject.Address + ", пом. " + roomDomain.Get(obj.Room_id).RoomNum } : null
                        //   Reg_RegDate = obj.right_id.reg_id.Reg_RegDate,
                        //    obj.right_id.reg_id.Reg_RegNumber
                    });
            }
            return new BaseDataResult();
        }
    }
}
