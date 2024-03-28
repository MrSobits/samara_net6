namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using B4.Utils;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.HouseManagementAsync;

    /// <summary>
    /// Задача подготовки данных по домам для ресурсоснабжающих организаций
    /// Методы создания запроса по жилым домам
    /// </summary>
    public partial class HouseRSOPrepareDataTask
    {
        /// <summary>
        /// Получить объект importHouseRSORequestLivingHouse
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект importHouseRSORequestLivingHouse</returns>
        private importHouseRSORequestLivingHouse CreateLivingHouseRequest(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            return new importHouseRSORequestLivingHouse
            {
                Item = this.GetLivingHouseItem(house, transportGuidDictionary),
                Items = this.GetLivingHouseItems(house, transportGuidDictionary)
            };
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

            if (house.Operation == RisEntityOperation.Create || string.IsNullOrEmpty(house.Guid))
            {
                result = new importHouseRSORequestLivingHouseLivingHouseToCreate
                {
                    BasicCharacteristicts = this.GetBasicCharacteristictsToCreate(house),
                    TransportGUID = houseTransportGuid
                };
            }
            else
            {
                result = new importHouseRSORequestLivingHouseLivingHouseToUpdate
                {
                    BasicCharacteristicts = this.GetBasicCharacteristictsToUpdate(house),
                    TransportGUID = houseTransportGuid
                };
            }

            transportGuidDictionary[typeof(RisHouse)].Add(houseTransportGuid, house.Id);

            return result;
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
        /// Получить раздел LivingRoomToCreate
        /// </summary>
        /// <param name="house">Жилой дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел LivingRoomToCreate</returns>
        private List<importHouseRSORequestLivingHouseLivingRoomToCreate> CreateLivingHouseLivingRoomToCreateRequest(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(LivingRoom)))
            {
                transportGuidDictionary.Add(typeof(LivingRoom), new Dictionary<string, long>());
            }

            var livingRoomsToCreate = this.LivingHouseLivingRooms.Where(x => x.House == house && x.Operation == RisEntityOperation.Create);
            var result = new List<importHouseRSORequestLivingHouseLivingRoomToCreate>();

            foreach (var livingRoom in livingRoomsToCreate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var itemsField = new List<object>();
                var itemsElementNameField = new List<ItemsChoiceType10>();

                if (livingRoom.CadastralNumber.IsNotEmpty())
                {
                    itemsElementNameField.Add(ItemsChoiceType10.CadastralNumber);
                    itemsField.Add(livingRoom.CadastralNumber);
                }
                else
                {
                    itemsElementNameField.Add(ItemsChoiceType10.No_RSO_GKN_EGRP_Registered);
                    itemsField.Add(true);
                }

                result.Add(new importHouseRSORequestLivingHouseLivingRoomToCreate
                {
                    Items = itemsField.ToArray(),
                    ItemsElementName = itemsElementNameField.ToArray(),
                    RoomNumber = livingRoom.RoomNumber,
                    Square = livingRoom.Square.GetValueOrDefault(),
                    SquareSpecified = livingRoom.Square != null,
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
        private List<importHouseRSORequestLivingHouseLivingRoomToUpdate> CreateLivingHouseLivingRoomToUpdateRequest(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(LivingRoom)))
            {
                transportGuidDictionary.Add(typeof(LivingRoom), new Dictionary<string, long>());
            }

            var livingRoomsToUpdate = this.LivingHouseLivingRooms.Where(x => x.House == house && x.Operation == RisEntityOperation.Update);
            var result = new List<importHouseRSORequestLivingHouseLivingRoomToUpdate>();

            foreach (var livingRoom in livingRoomsToUpdate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var itemsField = new List<object>();
                var itemsElementNameField = new List<ItemsChoiceType10>();

                if (livingRoom.CadastralNumber.IsNotEmpty())
                {
                    itemsElementNameField.Add(ItemsChoiceType10.CadastralNumber);
                    itemsField.Add(livingRoom.CadastralNumber);
                }
                else
                {
                    itemsElementNameField.Add(ItemsChoiceType10.No_RSO_GKN_EGRP_Registered);
                    itemsField.Add(true);
                }

                result.Add(new importHouseRSORequestLivingHouseLivingRoomToUpdate
                {
                    Items = itemsField.ToArray(),
                    ItemsElementName = itemsElementNameField.ToArray(),
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