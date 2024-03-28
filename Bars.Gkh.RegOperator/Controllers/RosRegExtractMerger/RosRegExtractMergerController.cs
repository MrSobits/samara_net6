namespace Bars.Gkh.RegOperator.Controllers
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;

    using Gkh.Domain;

    using Sobits.RosReg.Entities;

    using RegOperator.DomainService.PersonalAccount;

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text.RegularExpressions;
    using Microsoft.AspNetCore.Mvc;

    using Bars.B4.Modules.FIAS;
    using Bars.B4.Modules.Tasks.Common.Service;
    using Bars.Gkh.RegOperator.Tasks.ExtractMerge;

    public class DRosRegExtractMergerController : BaseController
    {
        #region Fields
        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        public IDomainService<ExtractEgrn> ExtractEgrnDomain { get; set; }

        public IDomainService<FiasAddress> FiasAddressDomain { get; set; }

        public IDomainService<Fias> FiasDomain { get; set; }

        public IDomainService<Room> RoomDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        /// <summary>
        /// Сервис лицевых счетов
        /// </summary>
        public IPersonalAccountService PersonalAccountService { get; set; }

        public ISessionProvider SessionProvider { get; set; }
        #endregion

        #region Public methods
        public ActionResult ListAccountsForComparsion(BaseParams baseParams)
        {
            // var rosRegExtractDomain = Container.Resolve<IDomainService<RosRegExtract>>();
            // var listRRE = rosRegExtractDomain.GetAll().Take(10);

            var loadParams = baseParams.GetLoadParam();

            var parentId = baseParams.Params.ContainsKey("parentId")
                ? baseParams.Params.GetAs<long>("parentId")
                : 0;
            var selectedRo = baseParams.Params.ContainsKey("selectedRo")
             ? baseParams.Params.GetAs<long>("selectedRo")
             : 0;
            if (parentId > 0 && selectedRo > 0)
            {
                var rosregExtract = ExtractEgrnDomain.Get(parentId);
                List<long> roomsList = new List<long>();

                if (rosregExtract.Purpose.ToLower().Contains("нежил") || rosregExtract.Purpose.ToLower().Contains("xозяйственн"))
                {
                    roomsList = RoomDomain.GetAll()
                        .Where(x => x.RealityObject.Id == selectedRo)
                        .Where(x => x.Type == Gkh.Enums.RoomType.NonLiving).Select(x => x.Id).ToList();
                }
                else
                {
                    roomsList = RoomDomain.GetAll()
                        .Where(x => x.RealityObject.Id == selectedRo).Select(x => x.Id).ToList();
                }

                if (roomsList.Count > 0)
                {
                    var persaccList = BasePersonalAccountDomain.GetAll()
                        .Where(x => roomsList.Contains(x.Room.Id))
                        .Select(x => new
                        {
                            x.Id,
                            RoomId = x.Room.Id,
                            Address = x.Room.RealityObject.Address + ", пом. " + x.Room.RoomNum,
                            CnumRoom = x.Room.RoomNum,
                            x.PersonalAccountNum,
                            Municipality = x.Room.RealityObject.Municipality.Name,
                            OwnerName = x.AccountOwner.Name,
                            AreaRoom = x.Room.Area,
                            AreaShare = x.AreaShare
                        });
                    Dictionary<long, string> accDict = new Dictionary<long, string>();

                    foreach (var acc in persaccList)
                    {
                        if (!accDict.ContainsKey(acc.RoomId))
                        {
                            accDict.Add(acc.RoomId, acc.PersonalAccountNum + "," + acc.OwnerName);
                        }
                        else
                        {
                            accDict[acc.RoomId] += "; " + acc.PersonalAccountNum + "," + acc.OwnerName;
                        }
                    }

                    var roomList = RoomDomain.GetAll()
                        .Where(x => roomsList.Contains(x.Id))
                        .AsEnumerable()
                        .Select(x => new
                        {
                            x.Id,
                            Address = x.RealityObject.Address,
                            CnumRoom = x.RoomNum,
                            Municipality = x.RealityObject.Municipality.Name,
                            OwnerName = accDict.ContainsKey(x.Id) ? accDict[x.Id] : "У помещения отсутствует лицевой счет",
                            AreaRoom = x.Area
                        }).AsQueryable();

                    //.Select(x => new
                    //{
                    //    x.Id,
                    //    x.Address,
                    //    x.CnumRoom,
                    //    x.Municipality,
                    //    OwnerName = accDict.ContainsKey(x.Id) ? accDict[x.Id] : "У помещения отсутствует лицевой счет",
                    //    x.AreaRoom
                    //});

                    var data = roomList
                        .Select(x => new
                        {
                            x.Id,
                            x.Address,
                            x.CnumRoom,
                            x.Municipality,
                            x.OwnerName,
                            x.AreaRoom
                        }).Filter(loadParams, this.Container).Order(loadParams).Paging(loadParams).ToList();


                    int totalCount = data.Count;
                    ListDataResult ldr = new ListDataResult(data, totalCount);
                    return ldr.ToJsonResult();
                }
                return null;

            }
            else if (parentId > 0)
            {
                try
                {
                    var rosregExtract = ExtractEgrnDomain.Get(parentId);


                    string fulladdress = rosregExtract.Address;
                    string[] splitAddress = fulladdress.Split(',');

                    string house = "";
                    string korp = "";
                    string pattern = @" корп\.? .*,";
                    Match match = Regex.Match(fulladdress, pattern, RegexOptions.IgnoreCase);

                    if (match.Success)
                    {
                        korp = match.Value.Replace("корп", "").Replace(".", "").Replace(",", "").Trim();
                    }

                    if (splitAddress.Any())
                    {
                        house = match.Success ? splitAddress[splitAddress.Count() - 3].Trim() : splitAddress[splitAddress.Count() - 2].Trim();
                    }

                    if (house.Length > 1 && house.Substring(0, 2) == "д ")
                    {
                        house = house.Substring(2).Trim();
                    }

                    house = house.Contains(".") ? house.Split('.')[1].Trim().ToLower() : house;
                    house = house.Replace("дом", "").Trim();
                    house = house.Replace(")", "").Trim();
                    house = house.Replace("(", "").Trim();
                    house = house.Replace("№", "").Trim();
                    house = house.Replace(",", "").Trim();
                    house = house.Replace(".", "").Trim();

                    //добавляем в сопоставление корпус
                    //RealityObject realityObj = RealityObjectDomain.GetAll()
                    //    .Where(x => fulladdress.ToLower().Contains(x.FiasAddress.PlaceName.ToLower()) || fulladdress.ToLower().Contains(x.FiasAddress.PlaceName.ToLower().Replace(".", "")))
                    //    .Where(x => fulladdress.ToLower().Contains(x.FiasAddress.StreetName.ToLower()) || fulladdress.Trim().ToLower().Contains(x.FiasAddress.StreetName.ToLower().Replace(".", "")))
                    //    .Where(x => x.FiasAddress.House.ToLower() == house).FirstOrDefault();
                    //if (realityObj == null)
                    //{
                    //    realityObj = RealityObjectDomain.GetAll()
                    //        .Where(x => x.Address.ToLower() == fulladdress.ToLower()).FirstOrDefault();
                    //}

                    RealityObject realityObj = RealityObjectDomain
                        .GetAll()
                        .Where(x => fulladdress.ToLower().Replace('ё', 'е').Contains(x.FiasAddress.PlaceName.ToLower().Replace('ё', 'е'))
                            || fulladdress.ToLower().Replace('ё', 'е').Contains(x.FiasAddress.PlaceName.ToLower().Replace(".", "").Replace('ё', 'е')))
                        .Where(x => fulladdress.ToLower().Replace('ё', 'е').Contains(x.FiasAddress.StreetName.ToLower().Replace('ё', 'е'))
                            || fulladdress.Trim().ToLower().Replace('ё', 'е')
                                .Contains(x.FiasAddress.StreetName.ToLower().Replace(".", "").Replace('ё', 'е')))
                        .Where(x => x.FiasAddress.House.ToLower() == house)
                        .WhereIf(string.IsNullOrEmpty(korp), x=> !x.Address.Contains("корп."))
                        .WhereIf(!string.IsNullOrEmpty(korp), x => x.FiasAddress.Housing.ToLower() == korp)
                        .FirstOrDefault();
                    if (realityObj == null)
                    {
                        var fiasPlaces = FiasDomain.GetAll()
                            .Where(x => x.ActStatus == FiasActualStatusEnum.Actual && x.AOLevel != FiasLevelEnum.Street &&
                                fulladdress.ToLower().Contains(x.FormalName.ToLower()))
                            .Select(x => x.AOGuid)
                            .ToList();

                        var fiasStreet = FiasDomain.GetAll()
                            .Where(x => x.ActStatus == FiasActualStatusEnum.Actual && x.AOLevel == FiasLevelEnum.Street &&
                                fulladdress.ToLower().Contains(x.FormalName.ToLower()))
                            .Where(x => fulladdress.ToLower().Contains(x.ShortName))
                            .Where(x => fiasPlaces.Contains(x.ParentGuid))
                            .OrderByDescending(x => x.FormalName.Length)
                            .FirstOrDefault();

                        //   .Select(x=> x.AOGuid)
                        // .ToList();

                        realityObj = RealityObjectDomain.GetAll()
                            .Where(x => x.FiasAddress.StreetGuidId == fiasStreet.AOGuid) //fiasStreet.AOGuidx.FiasAddress.StreetGuidId == fiasStreet.AOGuid
                            .Where(x => x.FiasAddress.House.ToLower() == house.ToLower())
                            .WhereIf(match.Success, x => x.FiasAddress.Housing == korp)
                             .WhereIf(string.IsNullOrEmpty(korp), x => !x.Address.Contains("корп."))
                            .FirstOrDefault();
                    }

                    if (realityObj == null)
                    {
                        realityObj = RealityObjectDomain
                            .GetAll().Where(x => (x.Address.ToLower() == fulladdress.ToLower() ||
                                    fulladdress
                                        .Replace(" дом ", "д.")
                                        .ToLower()
                                        .Replace('ё', 'е')
                                        .Replace(" ", "")
                                        .Contains(x.Address.ToLower()
                                            .Replace('ё', 'е')
                                            .Replace(" ", ""))
                                )
                                && x.Address != "").OrderByDescending(x => x.Address.Length).FirstOrDefault();
                    }

                    List<long> roomsList = new List<long>();

                    if (rosregExtract.Purpose.ToLower().Contains("нежил") || rosregExtract.Purpose.ToLower().Contains("xозяйственн"))
                    {
                        roomsList = RoomDomain.GetAll()
                            .Where(x => x.RealityObject.Id == realityObj.Id)
                            .Where(x => x.Type == Gkh.Enums.RoomType.NonLiving).Select(x => x.Id).ToList();
                    }
                    else
                    {
                        roomsList = RoomDomain.GetAll()
                            .Where(x => x.RealityObject.Id == realityObj.Id).Select(x => x.Id).ToList();
                    }

                    bool hasResult = false;


                    if (roomsList.Count > 0)
                    {
                        var persaccList = BasePersonalAccountDomain.GetAll()
                            .Where(x => roomsList.Contains(x.Room.Id))
                            .Select(x => new
                            {
                                x.Id,
                                RoomId = x.Room.Id,
                                Address = x.Room.RealityObject.Address + ", пом. " + x.Room.RoomNum,
                                CnumRoom = x.Room.RoomNum,
                                x.PersonalAccountNum,
                                Municipality = x.Room.RealityObject.Municipality.Name,
                                OwnerName = x.AccountOwner.Name,
                                AreaRoom = x.Room.Area,
                                AreaShare = x.AreaShare
                            });
                        Dictionary<long, string> accDict = new Dictionary<long, string>();

                        foreach (var acc in persaccList)
                        {
                            if (!accDict.ContainsKey(acc.RoomId))
                            {
                                accDict.Add(acc.RoomId, acc.PersonalAccountNum + "," + acc.OwnerName);
                            }
                            else
                            {
                                accDict[acc.RoomId] += "; " + acc.PersonalAccountNum + "," + acc.OwnerName;
                            }
                        }

                        var roomList = RoomDomain.GetAll()
                            .Where(x => roomsList.Contains(x.Id))
                            .AsEnumerable()
                            .Select(x => new
                            {
                                x.Id,
                                Address = x.RealityObject.Address,
                                CnumRoom = x.RoomNum,
                                Municipality = x.RealityObject.Municipality.Name,
                                OwnerName = accDict.ContainsKey(x.Id) ? accDict[x.Id] : "У помещения отсутствует лицевой счет",
                                AreaRoom = x.Area
                            }).AsQueryable();

                        //.Select(x => new
                        //{
                        //    x.Id,
                        //    x.Address,
                        //    x.CnumRoom,
                        //    x.Municipality,
                        //    OwnerName = accDict.ContainsKey(x.Id) ? accDict[x.Id] : "У помещения отсутствует лицевой счет",
                        //    x.AreaRoom
                        //});

                        var data = roomList
                            .Select(x => new
                            {
                                x.Id,
                                x.Address,
                                x.CnumRoom,
                                x.Municipality,
                                x.OwnerName,
                                x.AreaRoom
                            }).Filter(loadParams, this.Container).Order(loadParams).Paging(loadParams).ToList();


                        int totalCount = data.Count;
                        ListDataResult ldr = new ListDataResult(data, totalCount);
                        return ldr.ToJsonResult();
                    }
                }
                catch (Exception e)
                {
                    return JsFailure(e.Message);
                }

                //return new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
                return null;
            }
            else
            {
                return null;
            }
        }

        public void MergeExtract()
        {
            var extractEgrnDomain = this.Container.Resolve<IDomainService<ExtractEgrn>>();
            var roomDomain = this.Container.Resolve<IDomainService<Room>>();
            var fiasDomain = this.Container.Resolve<IDomainService<Fias>>();
            var realityObjectDomain = this.Container.Resolve<IDomainService<RealityObject>>();

            var extractList = extractEgrnDomain.GetAll()
                .Where(x => x.IsMerged == Gkh.Enums.YesNoNotSet.No);

            extractList.ForEach(
                rec =>
                {
                    try
                    {
                        string fulladdress = rec.Address;
                        string[] splitAddress = fulladdress.Split(',');
                        foreach (var spl in splitAddress)
                        {
                            if (Regex.Match(spl, ".*район.*").Success)
                            {
                                fulladdress = fulladdress.Replace(spl, "");
                            }
                        }
                        foreach (var spl in splitAddress)
                        {
                            if (Regex.Match(spl, ".*область.*").Success)
                            {
                                fulladdress = fulladdress.Replace(spl, "");
                            }
                        }


                        string house = "";
                        string korp = "";
                        int indKorp = fulladdress.IndexOf(" корп.");
                        if (indKorp > 0)
                        {
                            korp = fulladdress.Substring(fulladdress.IndexOf(" корп."));
                            korp = korp.Substring(0, korp.IndexOf(',')).Trim();
                            korp = korp.Replace("корп.", "").Trim();
                        }

                        if (splitAddress.Count() > 0)
                        {
                            if (korp != "")
                            {
                                house = splitAddress[splitAddress.Count() - 3].Trim();
                            }
                            else
                            {
                                house = splitAddress[splitAddress.Count() - 2].Trim();
                            }
                        }

                        if (house.Length > 1 && house.Substring(0, 2) == "д ")
                        {
                            house = house.Substring(2).Trim();
                        }

                        house = house.Contains(".") ? house.Split('.')[1].Trim().ToLower() : house;
                        house = house.Replace("дом", "").Trim();
                        house = house.Replace(")", "").Trim();
                        house = house.Replace("(", "").Trim();
                        house = house.Replace("№", "").Trim();
                        house = house.Replace(",", "").Trim();
                        house = house.Replace(".", "").Trim();

                        var fulladdr = fulladdress.ToLower().Replace('ё', 'е').Replace(".", "");
                        RealityObject realityObj = realityObjectDomain
                            .GetAll()
                            .Where(x => fulladdr.Contains(x.FiasAddress.PlaceName.ToLower().Replace('ё', 'е'))
                                || fulladdr.Contains(x.FiasAddress.PlaceName.ToLower().Replace(".", "").Replace('ё', 'е')))
                            .Where(x => fulladdr.Contains(x.FiasAddress.StreetName.ToLower().Replace('ё', 'е'))
                                || fulladdr.Contains(x.FiasAddress.StreetName.ToLower().Replace(".", "").Replace('ё', 'е')))
                            .Where(x => x.FiasAddress.House.ToLower() == house)
                            .FirstOrDefault(x => x.FiasAddress.Housing.ToLower() == korp);


                        /*
                        RealityObject realityObj = realityObjectDomain
                            .GetAll()
                            .Where(x => fulladdress.ToLower().Replace('ё', 'е').Contains(x.FiasAddress.PlaceName.ToLower().Replace('ё', 'е'))
                                || fulladdress.ToLower().Replace('ё', 'е').Contains(x.FiasAddress.PlaceName.ToLower().Replace(".", "").Replace('ё', 'е')))
                            .Where(x => fulladdress.ToLower().Replace('ё', 'е').Contains(x.FiasAddress.StreetName.ToLower().Replace('ё', 'е'))
                                || fulladdress.Trim().ToLower().Replace('ё', 'е')
                                    .Contains(x.FiasAddress.StreetName.ToLower().Replace(".", "").Replace('ё', 'е')))
                            .Where(x => x.FiasAddress.House.ToLower() == house)
                            .FirstOrDefault(x => x.FiasAddress.Housing.ToLower() == korp);
                            */

                        if (realityObj == null)
                        {
                            var fiasPlaces = fiasDomain.GetAll()
                                .Where(x => x.AOGuid != null && x.ActStatus != null && x.ActStatus == FiasActualStatusEnum.Actual && x.AOLevel != null &&
                                    x.AOLevel != FiasLevelEnum.Street && fulladdress.ToLower().Contains(x.FormalName.ToLower()))
                                .Select(x => x.AOGuid)
                                .ToList();

                            var fiasStreet = fiasDomain.GetAll()
                                .Where(x => x.ActStatus == FiasActualStatusEnum.Actual && x.AOLevel == FiasLevelEnum.Street &&
                                    fulladdress.ToLower().Contains(x.FormalName.ToLower()))
                                .Where(x => fulladdress.ToLower().Contains(x.ShortName))
                                .Where(x => fiasPlaces.Contains(x.ParentGuid))
                                .OrderByDescending(x => x.FormalName.Length)
                                .FirstOrDefault();

                            //   .Select(x=> x.AOGuid)
                            // .ToList();

                            realityObj = realityObjectDomain.GetAll()
                                .Where(x => fiasStreet.AOGuid == x.FiasAddress.StreetGuidId)
                                .Where(x => x.FiasAddress.House.ToLower() == house.ToLower()).FirstOrDefault();
                        }


                        if (realityObj != null)
                        {
                            var roomnum = splitAddress[splitAddress.Count() - 1].Trim();

                            char[] charsS3 = roomnum.ToCharArray();
                            var length = roomnum.Length;
                            for (int i = 0; i < length; i++)
                            {
                                if (!char.IsDigit(charsS3[i]))
                                {
                                    roomnum = roomnum.Replace(charsS3[i].ToString(), "");
                                }
                            }

                            var roomsMerged = roomDomain.GetAll()
                                .Where(x => x.RealityObject.Id == realityObj.Id)
                                .Where(x => x.RoomNum == roomnum).ToList();

                            if (roomsMerged.Count == 1)
                            {
                                rec.RoomId = roomsMerged[0];
                                rec.IsMerged = Gkh.Enums.YesNoNotSet.Yes;
                                rec.FullAddress = roomsMerged[0].RealityObject.Address + ", кв." + roomsMerged[0].RoomNum;
                                extractEgrnDomain.Update(rec);
                            }
                        }
                    }
                    catch (Exception e)
                    {
                    }
                }
            );
        }

        public ActionResult RunTask(BaseParams baseParams)
        {
            // var rosRegExtractDomain = Container.Resolve<IDomainService<RosRegExtract>>();
            // var listRRE = rosRegExtractDomain.GetAll().Take(10);
            var taskManager = this.Container.Resolve<ITaskManager>();

            try
            {
                taskManager.CreateTasks(new ExtractMergeTaskProvider(), new BaseParams());
            }
            catch
            {
                return JsFailure("Ошибка постановки задачи");
            }
            finally
            {
                this.Container.Release(taskManager);
            }

            return JsSuccess();
        }
        #endregion
    }
}