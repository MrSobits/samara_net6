namespace Bars.Gkh.Regions.Voronezh.Controllers
{
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.Entities;
    using Bars.Gkh.RegOperator.Entities.Owner;
    using Domain;
    using Entities;
    using Gkh.Domain;
    using Microsoft.AspNetCore.Mvc;
    using RegOperator.DomainService.PersonalAccount;
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class DataAreaOwnerMergerController : BaseController
    {
        #region Fields

        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        public IDomainService<Room> RoomDomain { get; set; }

        public IDomainService<LawsuitOwnerInfo> LawSuitOwnerInfoDomain { get; set; }

        public IDomainService<RealityObject> RealityObjectDomain { get; set; }

        public IDomainService<RosRegExtractDesc> RosRegExtractDescDomain { get; set; }

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
            if (parentId > 0)
            {
                try
                {
                    var rosregExtract = RosRegExtractDescDomain.Get(parentId);
                    string street = rosregExtract.Desc_StreetName;
                    street = street.Contains(".") ? street.Split('.')[1].Trim().ToLower() : street;
                    string kladrcode = rosregExtract.Desc_KLADR.Trim();
                    string municipality = rosregExtract.Desc_CityName.Trim().ToLower();
                    municipality = municipality.Contains(".") ? municipality.Split('.')[1].Trim().ToLower() : municipality;
                    string house = rosregExtract.Desc_Level1Name.Trim().ToLower();
                    house = house.Contains(".") ? house.Split('.')[1].Trim().ToLower() : house;
                    house = house.Replace(")", "").Trim();
                    house = house.Replace("(", "").Trim();
                    house = house.Replace("№", "").Trim();
                    house = house.Replace(",", "").Trim();
                    house = house.Replace(".", "").Trim();
                    string mo = rosregExtract.Desc_CityName.Trim().ToLower();
                    mo = mo.Contains(".") ? mo.Split('.')[1].Trim().ToLower() : mo;

                    bool hasResult = false;

                    if (!string.IsNullOrEmpty(kladrcode))
                    {
                        string cmdQuery = @"select id from gkh_room where ro_id in
(
select id from gkh_reality_object where fias_address_id in 
(select id from b4_fias_address where street_code = (select coderecord from b4_fias where kladrcode = '" + kladrcode + @"' limit 1) and lower(house) = '" + house + @"'))";

                        var session = this.SessionProvider.GetCurrentSession();
                        var query = session.CreateSQLQuery(cmdQuery);
                        var list = query.List();
                        if (list.Count > 0)
                        {
                            hasResult = true;
                        }

                        var persaccList = BasePersonalAccountDomain.GetAll()
                            .Where(x => list.Contains(x.Room.Id))
                            .Select(x => new
                            {
                                x.Id,
                                Room_Id = x.Room.Id,
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
                            if (!accDict.ContainsKey(acc.Room_Id))
                            {
                                accDict.Add(acc.Room_Id, acc.PersonalAccountNum + "," + acc.OwnerName);
                            }
                            else
                            {
                                accDict[acc.Room_Id] += "; " + acc.PersonalAccountNum + "," + acc.OwnerName;
                            }
                        }
                        if (hasResult)
                        {
                            var roomList = RoomDomain.GetAll()
                            .Where(x => list.Contains(x.Id))
                            .Select(x => new
                            {
                                x.Id,
                                Address = x.RealityObject.Address,
                                CnumRoom = x.RoomNum,
                                Municipality = x.RealityObject.Municipality.Name,
                                 //  OwnerName = accDict.ContainsKey(x.Id) ? accDict[x.Id] : "У помещения отсутствует лицевой счет",
                                 AreaRoom = x.Area
                            }).Filter(loadParams, this.Container).Order(loadParams).Paging(loadParams).ToList()
                            .Select(x => new
                            {
                                x.Id,
                                x.Address,
                                x.CnumRoom,
                                x.Municipality,
                                OwnerName = accDict.ContainsKey(x.Id) ? accDict[x.Id] : "У помещения отсутствует лицевой счет",
                                x.AreaRoom
                            }

                              ).ToList();



                            int totalCount = list.Count;
                            ListDataResult ldr = new ListDataResult(roomList, totalCount);
                            return ldr.ToJsonResult();
                        }

                    }
                    if (!string.IsNullOrEmpty(street) && !string.IsNullOrEmpty(house))
                    {
                        string cmdQuery = $@"select id from gkh_room where ro_id in
(
select gro.id from gkh_reality_object gro
inner join gkh_dict_municipality gdm on gdm.id = gro.municipality_id where lower(gdm.name) like '%{municipality}%' 
and lower(gro.address) like '%{street}%' and lower(gro.address) like '%{house}')";

                        var session = this.SessionProvider.GetCurrentSession();
                        var query = session.CreateSQLQuery(cmdQuery);
                        var list = query.List();


                        var persaccList = BasePersonalAccountDomain.GetAll()
                            .Where(x => list.Contains(x.Room.Id))
                            .Select(x => new
                            {
                                x.Id,
                                Room_Id = x.Room.Id,
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
                            if (!accDict.ContainsKey(acc.Room_Id))
                            {
                                accDict.Add(acc.Room_Id, acc.PersonalAccountNum + "," + acc.OwnerName);
                            }
                            else
                            {
                                accDict[acc.Room_Id] += "; " + acc.PersonalAccountNum + "," + acc.OwnerName;
                            }
                        }

                        var roomList = RoomDomain.GetAll()
                          .Where(x => list.Contains(x.Id))
                          .Select(x => new
                          {
                              x.Id,
                              Address = x.RealityObject.Address,
                              CnumRoom = x.RoomNum,
                              Municipality = x.RealityObject.Municipality.Name,
                              //  OwnerName = accDict.ContainsKey(x.Id) ? accDict[x.Id] : "У помещения отсутствует лицевой счет",
                              AreaRoom = x.Area
                          }).Filter(loadParams, this.Container).Order(loadParams).Paging(loadParams).ToList()
                          .Select(x => new
                          {
                              x.Id,
                              x.Address,
                              x.CnumRoom,
                              x.Municipality,
                              OwnerName = accDict.ContainsKey(x.Id) ? accDict[x.Id] : "У помещения отсутствует лицевой счет",
                              x.AreaRoom
                          }

                            ).ToList();





                        int totalCount = list.Count;
                        ListDataResult ldr = new ListDataResult(roomList, totalCount);
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

        /// <summary>
        /// Получить должностное лицо по контрагенту
        /// </summary>
        public ActionResult GetListOwnerByDocId(BaseParams baseParams)
        {
            var loadParams = baseParams.GetLoadParam();

            var parentId = baseParams.Params.ContainsKey("Lawsuit")
            ? baseParams.Params.GetAs<long>("Lawsuit")
            : 0;
            int totalCount;

            if(parentId>0)
            {
                var data = LawSuitOwnerInfoDomain.GetAll()
                    .Where(x => x.Lawsuit.Id == parentId)
                    .Select(x=> new
                    {
                        x.Id,
                        x.Name
                    }).AsQueryable()
                .Filter(loadParams, this.Container);

                totalCount = data.Count();

                var result = new ListDataResult(data.Order(loadParams).Paging(loadParams).ToList(), totalCount);
               

                return result.ToJsonResult();
            }
            else
                return this.JsFailure("Собственники не найдены");

         
        }

        #endregion 
    }
}
