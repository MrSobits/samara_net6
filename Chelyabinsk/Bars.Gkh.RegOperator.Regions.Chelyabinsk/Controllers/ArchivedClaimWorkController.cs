using Bars.B4;
using Bars.B4.DataAccess;
using Bars.Gkh.Domain;
using Bars.Gkh.Modules.ClaimWork.Entities;
using Bars.Gkh.Modules.ClaimWork.Enums;
using Bars.Gkh.RegOperator.Entities;
using Bars.Gkh.RegOperator.Entities.Owner;
using Castle.Core.Internal;
using Dapper;

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;

// ReSharper disable once CheckNamespace
namespace Bars.Gkh.RegOperator.Regions.Chelyabinsk.Controllers
{  

    public class ArchivedClaimWorkController : BaseController
    {
        public IDomainService<LawsuitIndividualOwnerInfo> LawsuitIndividualOwnerInfoDomain { get; set; }

        public IDomainService<BasePersonalAccount> BasePersonalAccountDomain { get; set; }

        public IDomainService<IndividualClaimWork> IndividualClaimWorkDomain { get; set; }

        public IDomainService<FlattenedClaimWork> FlattenedClaimWorkDomain { get; set; }

        public ActionResult CreateArchive(BaseParams baseParams)
        {
            var clwId = baseParams.Params.GetAs<long>("ClwId"); 
            var tmpIds = baseParams.Params.GetAs<string>("RloiId"); 
            List<long> rloiIds = new List<long>();
            try
            {
                rloiIds = Array.ConvertAll(tmpIds.Split(','), Convert.ToInt64).ToList();
            }
            catch
            {
                // ignored
            }

            if (rloiIds.IsNullOrEmpty())
            {
                return new BaseDataResult()
                {
                    Message = "Не выбрано ни одной записи",
                    Success = false
                }.ToJsonResult();
            }
            var clw = IndividualClaimWorkDomain.Get(clwId);

            if (clw == null)
            {
                return new BaseDataResult()
                {
                    Message = "Не удалось получить ПИР",
                    Success = false
                }.ToJsonResult();
            }

            var owners = LawsuitIndividualOwnerInfoDomain.GetAll()
                .Where(x => rloiIds.Contains(x.Id)).ToList();

            OwnerPersAccAdressProxy roominfo = GetOwnerPersAccAdressProxy(clw);

            owners.ForEach(y =>
            {               

                FlattenedClaimWork ftw = new FlattenedClaimWork
                {
                    Archived = false,
                    DebtorDebtAmount = y.Lawsuit.DebtBaseTariffSum*y.AreaShareNumerator/y.AreaShareDenominator,
                    DebtorFullname = y.Surname + " " + y.FirstName + " " + y.SecondName,
                    DebtorJurInstType = y.Lawsuit.WhoConsidered == LawsuitConsiderationType.WorldCourt? "Мировой суд" : y.Lawsuit.WhoConsidered == LawsuitConsiderationType.ArbitrationCourt? "Арбитражный суд" : "Районный суд",
                    DebtorRoomAddress = y.PersonalAccount.Room.RealityObject.Address,
                    DebtorRoomNumber = y.PersonalAccount.Room.RoomNum,
                    DebtorRoomType = y.PersonalAccount.Room.Type == Gkh.Enums.RoomType.Living? "Квартира": "Нежилое помещение",
                    DebtorDebtPeriod = y.Lawsuit.DebtStartDate.HasValue? y.Lawsuit.DebtStartDate.Value.ToShortDateString() + "-" + y.Lawsuit.DebtEndDate.Value.ToShortDateString() : "",
                    Num = y.ClaimNumber,
                    DebtorClaimDeliveryDate = y.Lawsuit.BidDate

                };
                var existsFCW = FlattenedClaimWorkDomain.GetAll()
                .Where(x => x.DebtorFullname == y.Surname + " " + y.FirstName + " " + y.SecondName && x.DebtorRoomAddress == y.PersonalAccount.Room.RealityObject.Address && x.DebtorRoomNumber == y.PersonalAccount.Room.RoomNum).ToList();
                if (existsFCW.Count == 0)
                {
                    FlattenedClaimWorkDomain.Save(ftw);
                }

            });

            return new BaseDataResult()
            {
                Message = "Выгрузка в архив успешна",
                Success = true
            }.ToJsonResult();


        }

        public OwnerPersAccAdressProxy GetOwnerPersAccAdressProxy(IndividualClaimWork clw)
        {
            return new OwnerPersAccAdressProxy();
        }

    }
    public class OwnerPersAccAdressProxy
    {
        public string DebtorRoomAddress { get; set; }
        public string DebtorRoomNumber { get; set; }
        public string DebtorRoomType { get; set; }
    }
}