namespace Bars.GisIntegration.RegOp.DataExtractors.HouseData
{
    using System.Collections.Generic;
    using System.Linq;
    using B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.DataExtractors;
    using Bars.GisIntegration.Base.Dictionaries;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.RegOp.Enums.HouseManagement;
    using Gkh.Entities;
    using Gkh.Enums;

    /// <summary>
    /// Экстрактор данных по жилым помещениям
    /// </summary>
    public class ResidentialPremisesDataExtractor : BaseDataExtractor<ResidentialPremises, Room>
    {
        private List<RisHouse> houses;
        private Dictionary<long, RisHouse> housesById;
        private IDictionary premisesCharacteristicDict;
       // private Dictionary<long, nsiRef> roomsNumDict;

        public IDictionaryManager DictionaryManager { get; set; }

        /// <summary>
        /// Получить сущности сторонней системы - жилые помещения
        /// </summary>
        /// <param name="parameters">Параметры сбора данных</param>
        /// <returns>Сущности сторонней системы - жилые помещения</returns>
        public override List<Room> GetExternalEntities(DynamicDictionary parameters)
        {
            var houseIds = this.houses?.Select(x => x.ExternalSystemEntityId).ToArray() ?? new long[0];
            var roomDomain = this.Container.ResolveDomain<Room>();

            try
            {
                return roomDomain.GetAll()
                    .WhereIf(this.houses != null, x => houseIds.Contains(x.RealityObject.Id))
                    .Where(x => x.Type == RoomType.Living)
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

            this.premisesCharacteristicDict = this.DictionaryManager.GetDictionary("PremisesCharacteristicDictionary");

            //this.roomsNumDict =
            //    gisDictRefDomain.GetAll()
            //        .Where(x => x.Dict.ActionCode == "Количество комнат")
            //        .Select(x => new { x.GkhId, x.GisId, x.GisGuid })
            //        .ToList()
            //        .GroupBy(x => x.GkhId)
            //        .ToDictionary(
            //            x => x.Key,
            //            x => x.Select(y => new nsiRef { Code = y.GisId, GUID = y.GisGuid }).First());

        }

        /// <summary>
        /// Обновить значения атрибутов Ris сущности
        /// </summary>
        /// <param name="room">Сущность внешней системы</param>
        /// <param name="residentialPremise">Ris сущность</param>
        protected override void UpdateRisEntity(Room room, ResidentialPremises residentialPremise)
        {
            var premisesCharacteristicEnum = this.GetPremisesCharacteristic(room);
            var premisesCharacteristic = premisesCharacteristicEnum != null ? this.premisesCharacteristicDict.GetDictionaryRecord((long)premisesCharacteristicEnum) : null;
           // var roomsNum = this.GetRoomsNum(room);

            residentialPremise.ExternalSystemEntityId = room.Id;
            residentialPremise.ExternalSystemName = "gkh";
            residentialPremise.CadastralNumber = room.CadastralNumber;
            residentialPremise.PremisesNum = room.RoomNum;
            //residentialPremise.Floor = room.Floor.HasValue ? room.Floor.Value.ToString() : null;
            residentialPremise.EntranceNum = room.Entrance != null ? (short?)room.Entrance.Number : null;
            residentialPremise.PremisesCharacteristicCode = premisesCharacteristic?.GisCode;
            residentialPremise.PremisesCharacteristicGuid = premisesCharacteristic?.GisGuid;
            //residentialPremise.RoomsNumCode = roomsNum.Code;
            //residentialPremise.RoomsNumGuid = roomsNum.GUID;
            residentialPremise.TotalArea = room.Area;
            residentialPremise.GrossArea = room.LivingArea;
            //residentialPremise.ResidentialHouseTypeCode = 
            //residentialPremise.ResidentialHouseTypeGuid = 
            residentialPremise.TerminationDate = room.RealityObject.DateDemolition;
            residentialPremise.ApartmentHouse = this.housesById?.Get(room.RealityObject.Id);
        }

        /// <summary>
        /// Получить характеристику помещения по комнате
        /// </summary>
        /// <param name="room">Комната</param>
        /// <returns>Характеристика помещения</returns>
        private PremisesCharacteristic? GetPremisesCharacteristic(Room room)
        {
            PremisesCharacteristic? result = null;

            if (room.RealityObject != null)
            {
                if (room.RealityObject.TypeHouse == TypeHouse.ManyApartments)
                {
                    result = PremisesCharacteristic.CertainApartment;
                }
                else if(room.RealityObject.TypeHouse == TypeHouse.SocialBehavior)
                {
                    result = PremisesCharacteristic.Hostel;
                }
            }

            return result;
        }

        //private nsiRef GetRoomsNum(Room room)
        //{
        //    var gkhRoomsNum = room.RoomsCount.GetValueOrDefault();

        //    if (gkhRoomsNum <= 1)
        //    {
        //        return this.roomsNumDict[(int)RoomsNum.OneRoom];
        //    }

        //    if (gkhRoomsNum <= 7)
        //    {
        //        var risRoomsNum = (RoomsNum)gkhRoomsNum;
        //        return this.roomsNumDict[(int)risRoomsNum];
        //    }

        //    return this.roomsNumDict[(int)RoomsNum.SevenAndMoreRoom];
        //}
    }
}
