namespace Bars.GisIntegration.Base.Tasks.PrepareData.HouseManagement
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.GisIntegration.Base.Enums;
    using Entities.HouseManagement;
    using HouseManagementAsync;
    using B4.Utils;

    /// <summary>
    /// Задача подготовки данных по домам для управляющих организаций
    /// Методы создания запроса по многоквартирным домам
    /// </summary>
    public partial class HouseUOPrepareDataTask
    {
        private importHouseUORequestApartmentHouse CreateApartmentHouseRequest(
            RisHouse house,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(RisHouse)))
            {
                transportGuidDictionary.Add(typeof(RisHouse), new Dictionary<string, long>());
            }

            var houseTransportGuid = Guid.NewGuid().ToString();
            object houseData;

            if (house.Operation == RisEntityOperation.Create)
            {
                houseData = new importHouseUORequestApartmentHouseApartmentHouseToCreate
                {
                    BasicCharacteristicts = this.GetBasicCharacteristictsToCreate<ApartmentHouseUOTypeBasicCharacteristicts>(house),

                    //BuiltUpArea = Decimal.Round(house.BuiltUpArea.GetValueOrDefault(), 2),
                    UndergroundFloorCount = house.UndergroundFloorCount,
                    MinFloorCount = (sbyte) house.MinFloorCount.GetValueOrDefault(),
                    MinFloorCountSpecified = house.MinFloorCount != null,

                    //OverhaulYear = house.OverhaulYear.GetValueOrDefault(),
                    //OverhaulFormingKind = !house.OverhaulFormingKindCode.IsEmpty() && !house.OverhaulFormingKindGuid.IsEmpty() ?
                    //    new nsiRef
                    //    {
                    //        Code = house.OverhaulFormingKindCode,
                    //        GUID = house.OverhaulFormingKindGuid
                    //    } : null,
                    //NonResidentialSquare = Decimal.Round(house.NonResidentialSquare, 2),
                    TransportGUID = houseTransportGuid
                };
            }
            else
            {
                houseData = new importHouseUORequestApartmentHouseApartmentHouseToUpdate
                {
                    BasicCharacteristicts = this.GetBasicCharacteristictsToUpdate(house),

                    //BuiltUpArea = Decimal.Round(house.BuiltUpArea.GetValueOrDefault(), 2),
                    UndergroundFloorCount = house.UndergroundFloorCount,
                    MinFloorCount = (sbyte) house.MinFloorCount.GetValueOrDefault(),
                    MinFloorCountSpecified = house.MinFloorCount != null,

                    //OverhaulYear = house.OverhaulYear.GetValueOrDefault(),
                    //OverhaulFormingKind = !house.OverhaulFormingKindCode.IsEmpty() && !house.OverhaulFormingKindGuid.IsEmpty() ?
                    //    new nsiRef
                    //    {
                    //        Code = house.OverhaulFormingKindCode,
                    //        GUID = house.OverhaulFormingKindGuid
                    //    } : null,
                    //NonResidentialSquare = Decimal.Round(house.NonResidentialSquare, 2),
                    TransportGUID = houseTransportGuid
                };
            }

            transportGuidDictionary[typeof(RisHouse)].Add(houseTransportGuid, house.Id);

            return new importHouseUORequestApartmentHouse
            {
                Item = houseData,
                NonResidentialPremiseToCreate =
                    this.CreateApartmentHouseNonResidentialPremiseToCreateRequests(house, transportGuidDictionary).ToArray(),
                NonResidentialPremiseToUpdate =
                    this.CreateApartmentHouseNonResidentialPremiseToUpdateRequests(house, transportGuidDictionary).ToArray(),
                EntranceToCreate = this.CreateApartmentHouseEntranceToCreateRequests(house, transportGuidDictionary).ToArray(),
                EntranceToUpdate = this.CreateApartmentHouseEntranceToUpdateRequests(house, transportGuidDictionary).ToArray(),
                ResidentialPremises = this.CreateApartmentHouseResidentialPremisesRequests(house, transportGuidDictionary).ToArray(),
                LiftToCreate = this.CreateApartmentHouseLiftToCreateRequests(house, transportGuidDictionary).ToArray(),
                LiftToUpdate = this.CreateApartmentHouseLiftToUpdateRequests(house, transportGuidDictionary).ToArray()
            };

        }

        
        /// <summary>
        /// Получить раздел NonResidentialPremiseToCreate
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел NonResidentialPremiseToCreate</returns>
        private List<importHouseUORequestApartmentHouseNonResidentialPremiseToCreate> CreateApartmentHouseNonResidentialPremiseToCreateRequests(
            RisHouse house,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(NonResidentialPremises)))
            {
                transportGuidDictionary.Add(typeof(NonResidentialPremises), new Dictionary<string, long>());
            }

            var premisesToCreate = this.NonResidentialPremisesList
                .Where(
                    x => (x.ApartmentHouse.Id == house.Id)
                        && (x.Operation == RisEntityOperation.Create || string.IsNullOrEmpty(x.Guid)))
                .ToList();

            var result = new List<importHouseUORequestApartmentHouseNonResidentialPremiseToCreate>();

            foreach (var premise in premisesToCreate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var items = this.GetBasicCharacteristictsItem(premise.CadastralNumber);
                result.Add(
                    new importHouseUORequestApartmentHouseNonResidentialPremiseToCreate
                    {
                        Items = items,
                        ItemsElementName = this.GetBasicCharacteristictsItemElementName(items),
                        PremisesNum = premise.PremisesNum,

                        //Purpose = new nsiRef
                        //{
                        //    Code = premise.PurposeCode,
                        //    GUID = premise.PurposeGuid
                        //},
                        //Position = new nsiRef
                        //{
                        //    Code = premise.PositionCode,
                        //    GUID = premise.PositionGuid
                        //},
                        TotalArea = decimal.Round(premise.TotalArea.GetValueOrDefault(), 2),
                        IsCommonProperty = premise.IsCommonProperty,
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
        private List<importHouseUORequestApartmentHouseNonResidentialPremiseToUpdate> CreateApartmentHouseNonResidentialPremiseToUpdateRequests(
            RisHouse house,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(NonResidentialPremises)))
            {
                transportGuidDictionary.Add(typeof(NonResidentialPremises), new Dictionary<string, long>());
            }

            var premisesToUpdate = this.NonResidentialPremisesList
                .Where(
                    x => (x.ApartmentHouse.Id == house.Id)
                        && (x.Operation == RisEntityOperation.Update && !string.IsNullOrEmpty(x.Guid)))
                .ToList();

            var result = new List<importHouseUORequestApartmentHouseNonResidentialPremiseToUpdate>();

            foreach (var premise in premisesToUpdate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var items = this.GetBasicCharacteristictsItem(premise.CadastralNumber);

                result.Add(
                    new importHouseUORequestApartmentHouseNonResidentialPremiseToUpdate
                    {
                        Items = items,
                        ItemsElementName = this.GetBasicCharacteristictsItemElementName(items),
                        PremisesNum = premise.PremisesNum,
                        TerminationDate = DateTime.Now, //premise.TerminationDate.GetValueOrDefault(),
                        TerminationDateSpecified = true, //premise.TerminationDate != null,
                        //Purpose = new nsiRef
                        //{
                        //    Code = premise.PurposeCode,
                        //    GUID = premise.PurposeGuid
                        //},
                        //Position = new nsiRef
                        //{
                        //    Code = premise.PositionCode,
                        //    GUID = premise.PositionGuid
                        //},
                        TotalArea = decimal.Round(premise.TotalArea.GetValueOrDefault(), 2),
                        TotalAreaSpecified = premise.TotalArea != null,
                        IsCommonProperty = premise.IsCommonProperty,
                        IsCommonPropertySpecified = true,
                        TransportGUID = transportGuid,
                        PremisesGUID = premise.Guid
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
        private List<importHouseUORequestApartmentHouseEntranceToCreate> CreateApartmentHouseEntranceToCreateRequests(
            RisHouse house,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(RisEntrance)))
            {
                transportGuidDictionary.Add(typeof(RisEntrance), new Dictionary<string, long>());
            }

            var entrancesToCreate = this.EntranceList.Where(x => x.ApartmentHouse.Id == house.Id && x.Operation == RisEntityOperation.Create);
            var result = new List<importHouseUORequestApartmentHouseEntranceToCreate>();

            foreach (var entrance in entrancesToCreate)
            {
                var transportGuid = Guid.NewGuid().ToString();

                result.Add(
                    new importHouseUORequestApartmentHouseEntranceToCreate
                    {
                        EntranceNum = entrance.EntranceNum?.ToString(),
                        StoreysCount = (sbyte) (entrance.StoreysCount ?? 0),
                        CreationYear = (short)entrance.CreationDate.GetValueOrDefault().Year,
                        CreationYearSpecified = entrance.CreationDate != null,
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
        private List<importHouseUORequestApartmentHouseEntranceToUpdate> CreateApartmentHouseEntranceToUpdateRequests(
            RisHouse house,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(RisEntrance)))
            {
                transportGuidDictionary.Add(typeof(RisEntrance), new Dictionary<string, long>());
            }

            var entrancesToUpdate = this.EntranceList.Where(x => x.ApartmentHouse.Id == house.Id && x.Operation == RisEntityOperation.Update);
            var result = new List<importHouseUORequestApartmentHouseEntranceToUpdate>();

            foreach (var entrance in entrancesToUpdate)
            {
                var transportGuid = Guid.NewGuid().ToString();

                result.Add(
                    new importHouseUORequestApartmentHouseEntranceToUpdate
                    {
                        EntranceNum = entrance.EntranceNum?.ToString(),
                        StoreysCount = (sbyte) (entrance.StoreysCount ?? 0),
                        StoreysCountSpecified = entrance.StoreysCount != null,
                        CreationYear = (short)entrance.CreationDate.GetValueOrDefault().Year,
                        CreationYearSpecified = entrance.CreationDate != null,
                        TerminationDate = entrance.TerminationDate.GetValueOrDefault(),
                        TerminationDateSpecified = entrance.TerminationDate != null,
                        TransportGUID = transportGuid,
                        EntranceGUID = entrance.Guid
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
        private List<importHouseUORequestApartmentHouseResidentialPremises> CreateApartmentHouseResidentialPremisesRequests(
            RisHouse house,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var residentialPremises = this.ResidentialPremisesList.Where(x => x.ApartmentHouse == house).ToList();
            var result = new List<importHouseUORequestApartmentHouseResidentialPremises>();

            if (!transportGuidDictionary.ContainsKey(typeof(ResidentialPremises)))
            {
                transportGuidDictionary.Add(typeof(ResidentialPremises), new Dictionary<string, long>());
            }

            foreach (var residentialPremise in residentialPremises)
            {
                GKN_EGRP_KeyType item;

                if (residentialPremise.Operation == RisEntityOperation.Create)
                {
                    item = this.CreateApartmentHouseResidentialPremisesToCreateRequest(residentialPremise, transportGuidDictionary);
                }
                else
                {
                    item = this.CreateApartmentHouseResidentialPremisesToUpdateRequest(residentialPremise, transportGuidDictionary);
                }

                result.Add(
                    new importHouseUORequestApartmentHouseResidentialPremises
                    {
                        Item = item,
                        LivingRoomToCreate =
                            this.CreateApartmentHouseResidentialPremisesLivingRoomToCreateRequest(residentialPremise, transportGuidDictionary).ToArray(),
                        LivingRoomToUpdate =
                            this.CreateApartmentHouseResidentialPremisesLivingRoomToUpdateRequest(residentialPremise, transportGuidDictionary).ToArray()
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
        private importHouseUORequestApartmentHouseResidentialPremisesResidentialPremisesToCreate CreateApartmentHouseResidentialPremisesToCreateRequest(
            ResidentialPremises residentialPremise,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();

            transportGuidDictionary[typeof(ResidentialPremises)].Add(transportGuid, residentialPremise.Id);
            var items = this.GetBasicCharacteristictsItem(residentialPremise.CadastralNumber);

            object entrance;

            if (residentialPremise.EntranceNum != null)
            {
                entrance = residentialPremise.EntranceNum.ToString();
            }
            else
            {
                entrance = false;
			}

			return new importHouseUORequestApartmentHouseResidentialPremisesResidentialPremisesToCreate
            {
                Item = entrance,
                Items = items,
                ItemsElementName = this.GetBasicCharacteristictsItemElementName(items),
				Item1 = residentialPremise.GrossArea ?? (object)false,
				PremisesNum = residentialPremise.PremisesNum,
                PremisesCharacteristic =
                    new nsiRef
                    {
                        Code = residentialPremise.PremisesCharacteristicCode,
                        GUID = residentialPremise.PremisesCharacteristicGuid
                    },

                //RoomsNum =
                //        new nsiRef
                //        {
                //            Code = residentialPremise.RoomsNumCode,
                //            GUID = residentialPremise.RoomsNumGuid
                //        },
                TotalArea = decimal.Round(residentialPremise.TotalArea.GetValueOrDefault(), 2),

                //ResidentialHouseType = !string.IsNullOrEmpty(residentialPremise.ResidentialHouseTypeCode) || !string.IsNullOrEmpty(residentialPremise.ResidentialHouseTypeGuid) ?
                //                                            new nsiRef
                //                                            {
                //                                                Code = residentialPremise.ResidentialHouseTypeCode,
                //                                                GUID = residentialPremise.ResidentialHouseTypeGuid
                //                                            } : null,
                TransportGUID = transportGuid
            };
        }

        /// <summary>
        /// Получить раздел Item для обновляемых комнат
        /// </summary>
        /// <param name="residentialPremise">Жилое помещение</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел Item для обновляемых комнат</returns>
        private importHouseUORequestApartmentHouseResidentialPremisesResidentialPremisesToUpdate CreateApartmentHouseResidentialPremisesToUpdateRequest(
            ResidentialPremises residentialPremise,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            var transportGuid = Guid.NewGuid().ToString();

            transportGuidDictionary[typeof(ResidentialPremises)].Add(transportGuid, residentialPremise.Id);
            var items = this.GetBasicCharacteristictsItem(residentialPremise.CadastralNumber);

            object entrance;

            if (residentialPremise.EntranceNum != null)
            {
                entrance = residentialPremise.EntranceNum.ToString();
            }
            else
            {
                entrance = false;
			}

			return new importHouseUORequestApartmentHouseResidentialPremisesResidentialPremisesToUpdate
            {
                Item = entrance,
                Items = items,
                ItemsElementName = this.GetBasicCharacteristictsItemElementName(items),
				Item1 = residentialPremise.GrossArea ?? (object)false,
				PremisesNum = residentialPremise.PremisesNum,
                TerminationDate = DateTime.Now, //residentialPremise.TerminationDate.GetValueOrDefault(),
                TerminationDateSpecified = true, //residentialPremise.TerminationDate != null,
                PremisesCharacteristic =
                    !residentialPremise.PremisesCharacteristicCode.IsEmpty() && !residentialPremise.PremisesCharacteristicGuid.IsEmpty()
                        ? new nsiRef
                        {
                            Code = residentialPremise.PremisesCharacteristicCode,
                            GUID = residentialPremise.PremisesCharacteristicGuid
                        }
                        : null,

                //RoomsNum =
                //        new nsiRef
                //        {
                //            Code = residentialPremise.RoomsNumCode,
                //            GUID = residentialPremise.RoomsNumGuid
                //        },
                
                TotalArea = decimal.Round(residentialPremise.TotalArea.GetValueOrDefault(), 2),
                TotalAreaSpecified = residentialPremise.TotalArea != null,
				
                //ResidentialHouseType = !string.IsNullOrEmpty(residentialPremise.ResidentialHouseTypeCode) || !string.IsNullOrEmpty(residentialPremise.ResidentialHouseTypeGuid) ?
                //                                            new nsiRef
                //                                            {
                //                                                Code = residentialPremise.ResidentialHouseTypeCode,
                //                                                GUID = residentialPremise.ResidentialHouseTypeGuid
                //                                            } : null,
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
        private List<importHouseUORequestApartmentHouseResidentialPremisesLivingRoomToCreate>
            CreateApartmentHouseResidentialPremisesLivingRoomToCreateRequest(
            ResidentialPremises premise,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(LivingRoom)))
            {
                transportGuidDictionary.Add(typeof(LivingRoom), new Dictionary<string, long>());
            }

            var livingRoomsToCreate =
                this.ResidentialPremiseLivingRooms.Where(x => x.ResidentialPremises == premise && x.Operation == RisEntityOperation.Create);
            var result = new List<importHouseUORequestApartmentHouseResidentialPremisesLivingRoomToCreate>();

            foreach (var livingRoom in livingRoomsToCreate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var items = this.GetBasicCharacteristictsItem(livingRoom.CadastralNumber);
                result.Add(
                    new importHouseUORequestApartmentHouseResidentialPremisesLivingRoomToCreate
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
        /// <param name="premise">Жилое помещение</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел LivingRoomToUpdate</returns>
        private List<importHouseUORequestApartmentHouseResidentialPremisesLivingRoomToUpdate>
            CreateApartmentHouseResidentialPremisesLivingRoomToUpdateRequest(
            ResidentialPremises premise,
            Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(LivingRoom)))
            {
                transportGuidDictionary.Add(typeof(LivingRoom), new Dictionary<string, long>());
            }

            var livingRoomsToUpdate =
                this.ResidentialPremiseLivingRooms.Where(x => x.ResidentialPremises == premise && x.Operation == RisEntityOperation.Update);
            var result = new List<importHouseUORequestApartmentHouseResidentialPremisesLivingRoomToUpdate>();

            foreach (var livingRoom in livingRoomsToUpdate)
            {
                var transportGuid = Guid.NewGuid().ToString();
                var items = this.GetBasicCharacteristictsItem(livingRoom.CadastralNumber);

                result.Add(
                    new importHouseUORequestApartmentHouseResidentialPremisesLivingRoomToUpdate
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

        /// <summary>
        /// Получить раздел LiftToCreate
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел LiftToCreate</returns>
        private List<importHouseUORequestApartmentHouseLiftToCreate> CreateApartmentHouseLiftToCreateRequests(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(RisLift)))
            {
                transportGuidDictionary.Add(typeof(RisLift), new Dictionary<string, long>());
            }

            var liftToCreate = this.LiftList.Where(x => x.ApartmentHouse.Id == house.Id && x.Operation == RisEntityOperation.Create);
            var result = new List<importHouseUORequestApartmentHouseLiftToCreate>();

            foreach (var lift in liftToCreate)
            {
                var transportGuid = Guid.NewGuid().ToString();

                result.Add(
                    new importHouseUORequestApartmentHouseLiftToCreate
                    {
                        EntranceNum = lift.EntranceNum,
                        FactoryNum = lift.FactoryNum,
                        Type = !lift.TypeCode.IsEmpty() && !lift.TypeGuid.IsEmpty() ?
                    new nsiRef
                    {
                        Code = lift.TypeCode,
                        GUID = lift.TypeGuid
                    } : null,
                        OperatingLimit = lift.OperatingLimit,
                        TransportGUID = transportGuid
                    });
                transportGuidDictionary[typeof(RisLift)].Add(transportGuid, lift.Id);
            }
            return result;
        }

        /// <summary>
        /// Получить раздел LiftToUpdate
        /// </summary>
        /// <param name="house">Дом</param>
        /// <param name="transportGuidDictionary">Словарь транспортных идентификаторов</param>
        /// <returns>Раздел LiftToUpdate</returns>
        private List<importHouseUORequestApartmentHouseLiftToUpdate> CreateApartmentHouseLiftToUpdateRequests(RisHouse house, Dictionary<Type, Dictionary<string, long>> transportGuidDictionary)
        {
            if (!transportGuidDictionary.ContainsKey(typeof(RisLift)))
            {
                transportGuidDictionary.Add(typeof(RisLift), new Dictionary<string, long>());
            }

            var liftToCreate = this.LiftList.Where(x => x.ApartmentHouse.Id == house.Id && x.Operation == RisEntityOperation.Update);
            var result = new List<importHouseUORequestApartmentHouseLiftToUpdate>();

            foreach (var lift in liftToCreate)
            {
                var transportGuid = Guid.NewGuid().ToString();

                result.Add(
                    new importHouseUORequestApartmentHouseLiftToUpdate
                    {
                        EntranceNum = lift.EntranceNum,
                        FactoryNum = lift.FactoryNum,
                        Type = !lift.TypeCode.IsEmpty() && !lift.TypeGuid.IsEmpty() ?
                    new nsiRef
                    {
                        Code = lift.TypeCode,
                        GUID = lift.TypeGuid
                    } : null,
                        OperatingLimit = lift.OperatingLimit,
                        TransportGUID = transportGuid,
                        TerminationDate = lift.TerminationDate.GetValueOrDefault()
                    });
                transportGuidDictionary[typeof(RisLift)].Add(transportGuid, lift.Id);
            }
            return result;
        }
    }
}
