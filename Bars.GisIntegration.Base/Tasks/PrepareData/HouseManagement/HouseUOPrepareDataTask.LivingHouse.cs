namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.GisIntegration.Base.Enums;
    using Entities.HouseManagement;
    using HouseManagementAsync;

    /// <summary>
    /// Задача подготовки данных по домам для управляющих организаций
    /// Методы создания запроса по жилым домам
    /// </summary>
    public partial class HouseUOPrepareDataTask
    {
        /// <summary>
        /// Получить объект importHouseUORequestLivingHouse
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект importHouseUORequestLivingHouse</returns>
        private importHouseUORequestLivingHouse CreateLivingHouseRequest(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            return new importHouseUORequestLivingHouse
            {
                Item = this.GetLivingHouseItem(house, transportGuidDictionary),
                Items = this.GetLivingHouseItems(house, transportGuidDictionary)
            };
        }

        /// <summary>
        /// Получить Items для LivingHouse
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Items для LivingHouse</returns>
        private object[] GetLivingHouseItems(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var result = new List<object>();

            //if (house.HouseType == HouseType.Blocks)
            //{
            //    result = new []
            //    {
            //        new importHouseUORequestLivingHouseBlocks
            //        {
            //                нет данных
            //        }
            //    };
            //}
            //else if(house.HouseType == HouseType.Living)
            //{

            result.AddRange(this.CreateLivingHouseLivingRoomToCreateRequest(house, transportGuidDictionary));
            result.AddRange(this.CreateLivingHouseLivingRoomToUpdateRequest(house, transportGuidDictionary));
            //}

            return result.ToArray();
        }

        /// <summary>
        /// Получить Item для LivingHouse
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Item для LivingHouse</returns>
        private object GetLivingHouseItem(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(RisHouse)))
            {
                transportGuidDictionary.Add(typeof(RisHouse), new Dictionary<string, long>());
            }

            var houseTransportGuid = Guid.NewGuid().ToString();
            object result;

            if (house.Operation == RisEntityOperation.Create)
            {
                result = new importHouseUORequestLivingHouseLivingHouseToCreate
                {
                    BasicCharacteristicts = this.GetBasicCharacteristictsToCreate<HouseBasicUOType>(house),
                    //ResidentialHouseType = !string.IsNullOrEmpty(house.ResidentialHouseTypeCode) || !string.IsNullOrEmpty(house.ResidentialHouseTypeGuid) ?
                    //                                       new nsiRef
                    //                                       {
                    //                                           Code = house.ResidentialHouseTypeCode,
                    //                                           GUID = house.ResidentialHouseTypeGuid
                    //                                       } : null,
                    TransportGUID = houseTransportGuid
                };
            }
            else
            {
                result = new importHouseUORequestLivingHouseLivingHouseToUpdate
                {
                    BasicCharacteristicts = this.GetBasicCharacteristictsToUpdate(house),
                    //ResidentialHouseType = !string.IsNullOrEmpty(house.ResidentialHouseTypeCode) || !string.IsNullOrEmpty(house.ResidentialHouseTypeGuid) ?
                    //                       new nsiRef
                    //                       {
                    //                           Code = house.ResidentialHouseTypeCode,
                    //                           GUID = house.ResidentialHouseTypeGuid
                    //                       } : null,
                    TransportGUID = houseTransportGuid
                };
            }

            transportGuidDictionary[typeof(RisHouse)].Add(houseTransportGuid, house.Id);

            return result;
        }

        /// <summary>
        /// Получить раздел LivingRoomToCreate
        /// </summary>
        /// <param name="house">Жилой дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел LivingRoomToCreate</returns>
        private List<importHouseUORequestLivingHouseLivingRoomToCreate> CreateLivingHouseLivingRoomToCreateRequest(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(LivingRoom)))
            {
                transportGuidDictionary.Add(typeof(LivingRoom), new Dictionary<string, long>());
            }

            var livingRoomsToCreate = this.LivingHouseLivingRooms.Where(x => x.House == house && x.Operation == RisEntityOperation.Create);
            var result = new List<importHouseUORequestLivingHouseLivingRoomToCreate>();

            foreach (var livingRoom in livingRoomsToCreate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var items = this.GetBasicCharacteristictsItem(livingRoom.CadastralNumber);

                result.Add(new importHouseUORequestLivingHouseLivingRoomToCreate
                {
                    Items = items,
                    ItemsElementName = this.GetBasicCharacteristictsItemElementName(items),
                    RoomNumber = livingRoom.RoomNumber,
                    Square = livingRoom.Square.GetValueOrDefault(),
                    TransportGUID = transportGuid
                });

                transportGuidDictionary[typeof(LivingRoom)].Add(transportGuid, livingRoom.Id);
            }

            return result;
        }

        /// <summary>
        /// Получить раздел LivingRoomToUpdate
        /// </summary>
        /// <param name="house">Жилой дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел LivingRoomToUpdate</returns>
        private List<importHouseUORequestLivingHouseLivingRoomToUpdate> CreateLivingHouseLivingRoomToUpdateRequest(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(LivingRoom)))
            {
                transportGuidDictionary.Add(typeof(LivingRoom), new Dictionary<string, long>());
            }

            var livingRoomsToUpdate = this.LivingHouseLivingRooms.Where(x => x.House == house && x.Operation == RisEntityOperation.Update);
            var result = new List<importHouseUORequestLivingHouseLivingRoomToUpdate>();

            foreach (var livingRoom in livingRoomsToUpdate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var items = this.GetBasicCharacteristictsItem(livingRoom.CadastralNumber);
                result.Add(new importHouseUORequestLivingHouseLivingRoomToUpdate
                {
                    Items = items,
                    ItemsElementName = this.GetBasicCharacteristictsItemElementName(items),
                    RoomNumber = livingRoom.RoomNumber,
                    Square = livingRoom.Square.GetValueOrDefault(),
                    SquareSpecified = livingRoom.Square != null,
                    TerminationDate = livingRoom.TerminationDate.GetValueOrDefault(),
                    TerminationDateSpecified = livingRoom.TerminationDate != null,
                    TransportGUID = transportGuid,
                    LivingRoomGUID = livingRoom.Guid
                });

                transportGuidDictionary[typeof(LivingRoom)].Add(transportGuid, livingRoom.Id);
            }

            return result;
        }
    }
}