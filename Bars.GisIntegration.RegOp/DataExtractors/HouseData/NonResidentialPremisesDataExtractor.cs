namespace Bars.GisIntegration.RegOp.DataExtractors.HouseData
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Gkh.Entities;
    using Gkh.Enums;

    /// <summary>
    /// Экстрактор данных по нежилым помещениям
    /// </summary>
    public class NonResidentialPremisesDataExtractor : BaseDataExtractor<NonResidentialPremises, Room>
    {
        private List<RisHouse> houses;
        private Dictionary<long, RisHouse> housesById;
        // private Dictionary<long, nsiRef> purposeDict;
        //  private Dictionary<long, nsiRef> positionDict;

        /// <summary>
        /// Получить сущности сторонней системы - нежилые помещения
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - нежилые помещения</returns>
        public override List<Room> GetExternalEntities(DynamicDictionary parameters)
        {
            var houseIds = this.houses?.Select(x => x.ExternalSystemEntityId).ToArray() ?? new long[0];
            var roomDomain = this.Container.ResolveDomain<Room>();

            try
            {
                return roomDomain.GetAll()
                    .WhereIf(this.houses != null, x => houseIds.Contains(x.RealityObject.Id))
                    .Where(x => x.Type == RoomType.NonLiving)
                    .Where(x => x.Area > 0)
                    .ToList();
            }
            finally
            {
                this.Container.Release(roomDomain);
            }
        }

        /// <summary>
        /// Выполнить обработку перед извлечением данных
        /// Заполнить словари
        /// </summary>
        /// <param name="parameters">Входные параметры</param>
        protected override void BeforeExtractHandle(DynamicDictionary parameters)
        {
            this.houses = parameters.GetAs<List<RisHouse>>("apartmentHouses");

            this.housesById = this.houses?
               .GroupBy(x => x.ExternalSystemEntityId)
               .ToDictionary(x => x.Key, x => x.First());

            // var gisDictRefDomain = this.Container.ResolveDomain<GisDictRef>();

            // try
            // {
            //this.purposeDict =
            //    gisDictRefDomain.GetAll()
            //        .Where(x => x.Dict.ActionCode == "Назначение помещения")
            //        .Select(x => new { x.GkhId, x.GisId, x.GisGuid })
            //        .ToList()
            //        .GroupBy(x => x.GkhId)
            //        .ToDictionary(
            //            x => x.Key,
            //            x => x.Select(y => new nsiRef { Code = y.GisId, GUID = y.GisGuid }).First());

            //this.positionDict =
            //    gisDictRefDomain.GetAll()
            //        .Where(x => x.Dict.ActionCode == "Расположение помещения")
            //        .Select(x => new { x.GkhId, x.GisId, x.GisGuid })
            //        .ToList()
            //        .GroupBy(x => x.GkhId)
            //        .ToDictionary(
            //            x => x.Key,
            //    //            x => x.Select(y => new nsiRef { Code = y.GisId, GUID = y.GisGuid }).First());
            //}
            //finally
            //{
            //    this.Container.Release(gisDictRefDomain);
            //}
        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="room">Сущность внешней системы</param>
        /// <param name="nonResidentialPremise">Ris сущность</param>
        protected override void UpdateRisEntity(Room room, NonResidentialPremises nonResidentialPremise)
        {
           // var purpose = this.purposeDict[(int)Purpose.Pharmacy];
           // var position = this.positionDict[(int)Enums.HouseManagement.Position.BuiltIn];

            nonResidentialPremise.ExternalSystemEntityId = room.Id;
            nonResidentialPremise.ExternalSystemName = "gkh";
            nonResidentialPremise.CadastralNumber = room.CadastralNumber;
            nonResidentialPremise.PremisesNum = room.RoomNum;
           // nonResidentialPremise.Floor = room.Floor.HasValue ? room.Floor.Value.ToString() : null;
          //  nonResidentialPremise.PurposeCode = purpose.Code;
           // nonResidentialPremise.PurposeGuid = purpose.GUID;
           // nonResidentialPremise.PositionCode = position.Code;
           // nonResidentialPremise.PositionGuid = position.GUID;
            nonResidentialPremise.TotalArea = room.Area;
            nonResidentialPremise.IsCommonProperty = false;
           // nonResidentialPremise.GrossArea = room.LivingArea;
            nonResidentialPremise.TerminationDate = room.RealityObject.DateDemolition;
            nonResidentialPremise.ApartmentHouse = this.housesById?.Get(room.RealityObject.Id);
        }
    }
}
