namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4.Utils;
    using Bars.GisIntegration.Base.Entities.HouseManagement;
    using Bars.GisIntegration.Base.Enums;
    using Bars.GisIntegration.Base.HouseManagementAsync;

    /// <summary>
    /// Задача подготовки данных по домам для ресурсоснабжающих организаций
    /// Методы создания запроса по многоквартирным домам
    /// </summary>
    public partial class HouseRSOPrepareDataTask
    {
        /// <summary>
        /// Получить объект importHouseRSORequestApartmentHouse
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Объект importHouseRSORequestApartmentHouse</returns>
        private importHouseRSORequestApartmentHouse CreateApartmentHouseRequest(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(RisHouse)))
            {
                transportGuidDictionary.Add(typeof(RisHouse), new Dictionary<string, long>());
            }

            var houseTransportGuid = Guid.NewGuid().ToString();
            object houseData;

            if (house.Operation == RisEntityOperation.Create)
            {
                houseData = new importHouseRSORequestApartmentHouseApartmentHouseToCreate
                {
                    BasicCharacteristicts = this.GetBasicCharacteristictsToCreate(house),
                    TransportGUID = houseTransportGuid
                };
            }
            else
            {
                houseData = new importHouseRSORequestApartmentHouseApartmentHouseToUpdate
                {
                    BasicCharacteristicts = this.GetBasicCharacteristictsToUpdate(house),
                    TransportGUID = houseTransportGuid
                };
            }

            transportGuidDictionary[typeof(RisHouse)].Add(houseTransportGuid, house.Id);

            return new importHouseRSORequestApartmentHouse
            {
                Item = houseData,
                NonResidentialPremiseToCreate = this.CreateApartmentHouseNonResidentialPremiseToCreateRequests(house, transportGuidDictionary).ToArray(),
                NonResidentialPremiseToUpdate = this.CreateApartmentHouseNonResidentialPremiseToUpdateRequests(house, transportGuidDictionary).ToArray(),
                EntranceToCreate = this.CreateApartmentHouseEntranceToCreateRequests(house, transportGuidDictionary).ToArray(),
                EntranceToUpdate = this.CreateApartmentHouseEntranceToUpdateRequests(house, transportGuidDictionary).ToArray(),
                ResidentialPremises = this.CreateApartmentHouseResidentialPremisesRequests(house, transportGuidDictionary).ToArray()
            };
        }

        /// <summary>
        /// Получить раздел NonResidentialPremiseToCreate
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел NonResidentialPremiseToCreate</returns>
        private List<importHouseRSORequestApartmentHouseNonResidentialPremiseToCreate> CreateApartmentHouseNonResidentialPremiseToCreateRequests(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(NonResidentialPremises)))
            {
                transportGuidDictionary.Add(typeof(NonResidentialPremises), new Dictionary<string, long>());
            }

            var premisesToCreate = this.NonResidentialPremisesList.Where(x => x.ApartmentHouse.Id == house.Id && x.Operation == RisEntityOperation.Create);
            var result = new List<importHouseRSORequestApartmentHouseNonResidentialPremiseToCreate>();

            foreach (var premise in premisesToCreate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var itemsField = new List<object>();
                var itemsElementNameField = new List<ItemsChoiceType10>();

                if (premise.CadastralNumber.IsNotEmpty())
                {
                    itemsElementNameField.Add(ItemsChoiceType10.CadastralNumber);
                    itemsField.Add(premise.CadastralNumber);
                }
                else
                {
                    itemsElementNameField.Add(ItemsChoiceType10.No_RSO_GKN_EGRP_Registered);
                    itemsField.Add(true);
                }

                result.Add(new importHouseRSORequestApartmentHouseNonResidentialPremiseToCreate
                {
                    Items = itemsField.ToArray(),
                    ItemsElementName = itemsElementNameField.ToArray(),
                    PremisesNum = premise.PremisesNum,
                    TotalArea = decimal.Round(premise.TotalArea.GetValueOrDefault(), 2),
                    TotalAreaSpecified = premise.TotalArea != null,
                    TransportGUID = transportGuid
                });

                transportGuidDictionary[typeof(NonResidentialPremises)].Add(transportGuid, premise.Id);
            }

            return result;
        }

        /// <summary>
        /// Получить раздел NonResidentialPremiseToUpdate
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел NonResidentialPremiseToUpdate</returns>
        private List<importHouseRSORequestApartmentHouseNonResidentialPremiseToUpdate> CreateApartmentHouseNonResidentialPremiseToUpdateRequests(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(NonResidentialPremises)))
            {
                transportGuidDictionary.Add(typeof(NonResidentialPremises), new Dictionary<string, long>());
            }

            var premisesToUpdate = this.NonResidentialPremisesList.Where(x => x.ApartmentHouse.Id == house.Id && x.Operation == RisEntityOperation.Update);
            var result = new List<importHouseRSORequestApartmentHouseNonResidentialPremiseToUpdate>();

            foreach (var premise in premisesToUpdate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var itemsField = new List<object>();
                var itemsElementNameField = new List<ItemsChoiceType10>();

                if (premise.CadastralNumber.IsNotEmpty())
                {
                    itemsElementNameField.Add(ItemsChoiceType10.CadastralNumber);
                    itemsField.Add(premise.CadastralNumber);
                }
                else
                {
                    itemsElementNameField.Add(ItemsChoiceType10.No_RSO_GKN_EGRP_Registered);
                    itemsField.Add(true);
                }

                result.Add(new importHouseRSORequestApartmentHouseNonResidentialPremiseToUpdate
                {
                    Items = itemsField.ToArray(),
                    ItemsElementName = itemsElementNameField.ToArray(),
                    PremisesNum = premise.PremisesNum,                  
                    TotalArea = decimal.Round(premise.TotalArea.GetValueOrDefault(), 2),
                    TotalAreaSpecified = premise.TotalArea != null,
                    TerminationDate = DateTime.Now,//premise.TerminationDate.GetValueOrDefault(),
                    TerminationDateSpecified = true, //premise.TerminationDate != null,
                    PremisesGUID = premise.Guid,
                    TransportGUID = transportGuid
                });

                transportGuidDictionary[typeof(NonResidentialPremises)].Add(transportGuid, premise.Id);
            }

            return result;
        }

        /// <summary>
        /// Получить раздел EntranceToCreate
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел EntranceToCreate</returns>
        private List<importHouseRSORequestApartmentHouseEntranceToCreate> CreateApartmentHouseEntranceToCreateRequests(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(RisEntrance)))
            {
                transportGuidDictionary.Add(typeof(RisEntrance), new Dictionary<string, long>());
            }

            var entrancesToCreate = this.EntranceList.Where(x => x.ApartmentHouse.Id == house.Id && x.Operation == RisEntityOperation.Create);
            var result = new List<importHouseRSORequestApartmentHouseEntranceToCreate>();

            foreach (var entrance in entrancesToCreate)
            {
                var transportGuid = Guid.NewGuid().ToString();

                result.Add(new importHouseRSORequestApartmentHouseEntranceToCreate
                {
                    EntranceNum = entrance.EntranceNum?.ToString(),
                    TransportGUID = transportGuid
                });

                transportGuidDictionary[typeof(RisEntrance)].Add(transportGuid, entrance.Id);
            }

            return result;
        }

        /// <summary>
        /// Получить раздел EntranceToUpdate
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел EntranceToUpdate</returns>
        private List<importHouseRSORequestApartmentHouseEntranceToUpdate> CreateApartmentHouseEntranceToUpdateRequests(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(RisEntrance)))
            {
                transportGuidDictionary.Add(typeof(RisEntrance), new Dictionary<string, long>());
            }

            var entrancesToUpdate = this.EntranceList.Where(x => x.ApartmentHouse.Id == house.Id && x.Operation == RisEntityOperation.Update);
            var result = new List<importHouseRSORequestApartmentHouseEntranceToUpdate>();

            foreach (var entrance in entrancesToUpdate)
            {
                var transportGuid = Guid.NewGuid().ToString();

                result.Add(new importHouseRSORequestApartmentHouseEntranceToUpdate
                {
                    EntranceNum = entrance.EntranceNum?.ToString(),
                    TerminationDate = entrance.TerminationDate.GetValueOrDefault(),
                    TerminationDateSpecified = entrance.TerminationDate != null,
                    EntranceGUID = entrance.Guid,
                    TransportGUID = transportGuid
                });

                transportGuidDictionary[typeof(RisEntrance)].Add(transportGuid, entrance.Id);
            }

            return result;
        }

        /// <summary>
        /// Получить раздел ResidentialPremises
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел ResidentialPremises</returns>
        private List<importHouseRSORequestApartmentHouseResidentialPremises> CreateApartmentHouseResidentialPremisesRequests(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var residentialPremises = this.ResidentialPremisesList.Where(x => x.ApartmentHouse == house);
            var result = new List<importHouseRSORequestApartmentHouseResidentialPremises>();

            if (!transportGuidDictionary.ContainsKey(typeof(ResidentialPremises)))
            {
                transportGuidDictionary.Add(typeof(ResidentialPremises), new Dictionary<string, long>());
            }

            foreach (var residentialPremise in residentialPremises)
            {
                GKN_EGRP_KeyRSOType item;

                if (residentialPremise.Operation == RisEntityOperation.Create)
                {
                    item = this.CreateApartmentHouseResidentialPremisesToCreateRequest(residentialPremise, transportGuidDictionary);
                }
                else
                {
                    item = this.CreateApartmentHouseResidentialPremisesToUpdateRequest(residentialPremise, transportGuidDictionary);
                }

                result.Add(new importHouseRSORequestApartmentHouseResidentialPremises
                {
                    Item = item,
                    LivingRoomToCreate = this.CreateApartmentHouseResidentialPremisesLivingRoomToCreateRequest(residentialPremise, transportGuidDictionary).ToArray(),
                    LivingRoomToUpdate = this.CreateApartmentHouseResidentialPremisesLivingRoomToUpdateRequest(residentialPremise, transportGuidDictionary).ToArray()
                });
            }

            return result;
        }

        /// <summary>
        /// Получить раздел Item для новых комнат
        /// </summary>
        /// <param name="residentialPremise">Жилое помещение</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел Item для новых комнат</returns>
        private importHouseRSORequestApartmentHouseResidentialPremisesResidentialPremisesToCreate CreateApartmentHouseResidentialPremisesToCreateRequest(ResidentialPremises residentialPremise, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();

            transportGuidDictionary[typeof(ResidentialPremises)].Add(transportGuid, residentialPremise.Id);

            object entrance;

            if (residentialPremise.EntranceNum != null)
            {
                entrance = residentialPremise.EntranceNum.ToString();
            }
            else
            {
                entrance = false;
            }

            var itemsField = new List<object>();
            var itemsElementNameField = new List<ItemsChoiceType10>();

            if (residentialPremise.CadastralNumber.IsNotEmpty())
            {
                itemsElementNameField.Add(ItemsChoiceType10.CadastralNumber);
                itemsField.Add(residentialPremise.CadastralNumber);
            }
            else
            {
                itemsElementNameField.Add(ItemsChoiceType10.No_RSO_GKN_EGRP_Registered);
                itemsField.Add(true);
            }

            return new importHouseRSORequestApartmentHouseResidentialPremisesResidentialPremisesToCreate
            {
                TransportGUID = transportGuid,
                Item = entrance,
                Items = itemsField.ToArray(),
                ItemsElementName = itemsElementNameField.ToArray(),
                PremisesNum = residentialPremise.PremisesNum,
                PremisesCharacteristic = !residentialPremise.PremisesCharacteristicCode.IsEmpty() && !residentialPremise.PremisesCharacteristicGuid.IsEmpty()? new nsiRef
                {
                    Code = residentialPremise.PremisesCharacteristicCode,
                    GUID = residentialPremise.PremisesCharacteristicGuid
                }: null,
                TotalArea = decimal.Round(residentialPremise.TotalArea.GetValueOrDefault(), 2),
                TotalAreaSpecified = residentialPremise.TotalArea != null
            };
        }

        /// <summary>
        /// Получить раздел Item для обновляемых комнат
        /// </summary>
        /// <param name="residentialPremise">Жилое помещение</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел Item для обновляемых комнат</returns>
        private importHouseRSORequestApartmentHouseResidentialPremisesResidentialPremisesToUpdate CreateApartmentHouseResidentialPremisesToUpdateRequest(ResidentialPremises residentialPremise, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();
            var itemsField = new List<object>();
            var itemsElementNameField = new List<ItemsChoiceType10>();

            if (residentialPremise.CadastralNumber.IsNotEmpty())
            {
                itemsElementNameField.Add(ItemsChoiceType10.CadastralNumber);
                itemsField.Add(residentialPremise.CadastralNumber);
            }
            else
            {
                itemsElementNameField.Add(ItemsChoiceType10.No_RSO_GKN_EGRP_Registered);
                itemsField.Add(true);
            }

            transportGuidDictionary[typeof(ResidentialPremises)].Add(transportGuid, residentialPremise.Id);

            object entrance;

            if (residentialPremise.EntranceNum != null)
            {
                entrance = residentialPremise.EntranceNum.ToString();
            }
            else
            {
                entrance = false;
            }

            return new importHouseRSORequestApartmentHouseResidentialPremisesResidentialPremisesToUpdate
            {
                Items = itemsField.ToArray(),
                ItemsElementName = itemsElementNameField.ToArray(),
                PremisesNum = residentialPremise.PremisesNum,
                TerminationDate = residentialPremise.TerminationDate.GetValueOrDefault(),
                TerminationDateSpecified = residentialPremise.TerminationDate != null,
                Item = entrance,
                PremisesCharacteristic = !residentialPremise.PremisesCharacteristicCode.IsEmpty() && !residentialPremise.PremisesCharacteristicGuid.IsEmpty() ? new nsiRef
                {
                    Code = residentialPremise.PremisesCharacteristicCode,
                    GUID = residentialPremise.PremisesCharacteristicGuid
                } : null,
                TotalArea = decimal.Round(residentialPremise.TotalArea.GetValueOrDefault(), 2),
                TotalAreaSpecified = residentialPremise.TotalArea != null,
                TransportGUID = transportGuid,
                PremisesGUID = residentialPremise.Guid
            };
        }

        /// <summary>
        /// Получить раздел LivingRoomToCreate
        /// </summary>
        /// <param name="premise">Жилое помещение</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел LivingRoomToCreate</returns>
        private List<importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToCreate> CreateApartmentHouseResidentialPremisesLivingRoomToCreateRequest(ResidentialPremises premise, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(LivingRoom)))
            {
                transportGuidDictionary.Add(typeof(LivingRoom), new Dictionary<string, long>());
            }

            var livingRoomsToCreate = this.ResidentialPremiseLivingRooms.Where(x => x.ResidentialPremises == premise && x.Operation == RisEntityOperation.Create);
            var result = new List<importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToCreate>();

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

                result.Add(new importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToCreate
                {
                    Items = itemsField.ToArray(),
                    ItemsElementName = itemsElementNameField.ToArray(),
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
        /// <param name="premise">Жилое помещение</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел LivingRoomToUpdate</returns>
        private List<importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToUpdate>CreateApartmentHouseResidentialPremisesLivingRoomToUpdateRequest(ResidentialPremises premise, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(LivingRoom)))
            {
                transportGuidDictionary.Add(typeof(LivingRoom), new Dictionary<string, long>());
            }

            var livingRoomsToUpdate = this.ResidentialPremiseLivingRooms.Where(x => x.ResidentialPremises == premise && x.Operation == RisEntityOperation.Update);
            var result = new List<importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToUpdate>();

            var itemsField = new List<object>();
            var itemsElementNameField = new List<ItemsChoiceType10>();

            if (premise.CadastralNumber.IsNotEmpty())
            {
                itemsElementNameField.Add(ItemsChoiceType10.CadastralNumber);
                itemsField.Add(premise.CadastralNumber);
            }
            else
            {
                itemsElementNameField.Add(ItemsChoiceType10.No_RSO_GKN_EGRP_Registered);
                itemsField.Add(true);
            }

            foreach (var livingRoom in livingRoomsToUpdate)
            {
                var transportGuid = Guid.NewGuid().ToString();

                result.Add(new importHouseRSORequestApartmentHouseResidentialPremisesLivingRoomToUpdate
                {
                    Items = itemsField.ToArray(),
                    ItemsElementName = itemsElementNameField.ToArray(),
                    RoomNumber = livingRoom.RoomNumber,
                    Square = livingRoom.Square.GetValueOrDefault(),
                    SquareSpecified = livingRoom.Square != null,
                    TerminationDate = DateTime.Now,//livingRoom.TerminationDate.GetValueOrDefault(),
                    TerminationDateSpecified = true,//livingRoom.TerminationDate != null,
                    TransportGUID = transportGuid,
                    LivingRoomGUID = livingRoom.Guid
                });

                transportGuidDictionary[typeof(LivingRoom)].Add(transportGuid, livingRoom.Id);
            }

            return result;
        }
    }
}
