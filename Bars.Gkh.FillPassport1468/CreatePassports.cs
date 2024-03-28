using System;

namespace Bars.Gkh.FillPassport1468
{
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Enums;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Entities.Passport;
    using Bars.Gkh1468.Enums;
    using Castle.Windsor;

    public class CreatePassports : BaseExecutionAction
    {

        public override string Description => @"1. Создаем договора для всех домов на ПКУ ОАО РСО
2. Создаем паспорта 1468 типа МКД за ноябрь 2014";

        public override string Name => "1468 - Создание паспортов";

        public override Func<IDataResult> Action => this.CreatePassportForAll;

        public IDomainService<SupplyResourceOrg> SupplyResourceOrgService { get; set; }

        public IDomainService<SupplyResourceOrgRealtyObject> supRoService { get; set; }

        public IDomainService<SupplyResourceOrgRealtyObject> supRoDocService { get; set; }

        public IDomainService<RealityObjectResOrg> RealityObjectResOrgService { get; set; }

        public IDomainService<RealityObject> RoService { get; set; }

        public IDomainService<HouseProviderPassport> PassportProviderService { get; set; }

        public IDomainService<HousePassport> HousePassportService { get; set; }

        public IDomainService<PassportStruct> PassportStructService { get; set; }

        private BaseDataResult CreatePassportForAll()
        {
            var supplyOrg = GetSupplyResourceOrg();

            if (supplyOrg == null)
            {
                return BaseDataResult.Error("Не найдена ПКУ с названием ОАО \"РСО\"");
            }

            CreateHouseDocAndLinks(supplyOrg);

            var pasportStruct = PassportStructService.GetAll()
                .Where(x => x.PassportType == PassportType.Mkd)
                .Where(x => (x.ValidFromYear < 2014) || (x.ValidFromYear == 2014 && x.ValidFromMonth <= 11))
                .OrderByDescending(x => x.ValidFromYear)
                .ThenByDescending(x => x.ValidFromMonth)
                .FirstOrDefault();

            var currentUser = Container.Resolve<IUserIdentity>();

            if (currentUser is AnonymousUserIdentity)
            {
                return new BaseDataResult(false, "Не определен оператор");
            }

            var curOp =
                Container.Resolve<IDomainService<Operator>>()
                    .GetAll()
                    .FirstOrDefault(x => x.User.Id == currentUser.UserId);

            var paspExistRoIdList = PassportProviderService.GetAll()
                .Where(x => x.HouseType == HouseType.Mkd && x.ReportYear == 2014 && x.ReportMonth == 11)
                .Select(x => x.RealityObject.Id).ToList();

            var roList = RoService.GetAll()
                .Where(x => !paspExistRoIdList.Contains(x.Id) &&
                    (x.TypeHouse == TypeHouse.ManyApartments || x.TypeHouse == TypeHouse.SocialBehavior))
                .ToList();

            foreach (var ro in roList)
            {
                MakeNewPassport(curOp, ro, pasportStruct, supplyOrg.Contragent);
            }

            return new BaseDataResult(true);
        }


        public void MakeNewPassport(Operator curOp, RealityObject ro, PassportStruct paspStruct, Contragent contractor)
        {
            var housePasp = new HousePassport
            {
                RealityObject = ro,
                ReportMonth = 11,
                ReportYear = 2014,
                HouseType = HouseType.Mkd
            };

            var newPasp = new HouseProviderPassport
            {
                HousePassport = housePasp,
                Contragent = contractor,
                ContragentType = ContragentType.Pku,
                RealityObject = ro,
                ReportYear = 2014,
                ReportMonth = 11,
                PassportStruct = paspStruct,
                UserName = curOp.User.Name,
                HouseType = HouseType.Mkd
            };

            var stateProvider = Container.Resolve<IStateProvider>();
            stateProvider.SetDefaultState(newPasp);

            HousePassportService.Save(housePasp);
            PassportProviderService.Save(newPasp);
        }

        private void CreateHouseDocAndLinks(SupplyResourceOrg supplyOrg)
        {
            var домаДляРусурсника = @"select ro.id from gkh_reality_object ro 
where ro.id not in (select sro.realityobject_id from GKH_SUPPLY_RESORG_RO sro
where sro.supply_resorg_id = {0}) and (ro.TYPE_HOUSE = 30 or ro.TYPE_HOUSE = 40)".FormatUsing(supplyOrg.Id);

            var домаДляДоговоров = @"select ro.id from gkh_reality_object ro 
where ro.id not in (select t.reality_object_id from GKH_OBJ_RESORG t
where t.resorg_id = {0}) and (ro.TYPE_HOUSE = 30 or ro.TYPE_HOUSE = 40)".FormatUsing(supplyOrg.Id);

            List<long> roListSupl = new List<long>();
            List<long> roListDocSupl = new List<long>();
            var session = Container.Resolve<ISessionProvider>().GetCurrentSession();

            foreach (var item in session.CreateSQLQuery(домаДляРусурсника).List())
            {
                roListSupl.Add(item.ToLong());
            }

            foreach (var item in session.CreateSQLQuery(домаДляДоговоров).List())
            {
                roListDocSupl.Add(item.ToLong());
            }

            var supRoList = new List<SupplyResourceOrgRealtyObject>();

            roListSupl.ForEach(x => supRoList.Add(new SupplyResourceOrgRealtyObject
            {
                RealityObject = RoService.Load(x),
                SupplyResourceOrg = supplyOrg
            }));

            var supRoDocList = new List<RealityObjectResOrg>();
            roListDocSupl.ForEach(x => supRoDocList.Add(new RealityObjectResOrg
            {
                RealityObject = RoService.Load(x),
                ResourceOrg = supplyOrg,
                DateStart = new DateTime(2014, 11, 1)
            }));

            TransactionHelper.InsertInManyTransactions(Container, supRoList, useStatelessSession: true);

            TransactionHelper.InsertInManyTransactions(Container, supRoDocList, useStatelessSession: true);
        }

        private SupplyResourceOrg GetSupplyResourceOrg()
        {
            var supplyOrg = SupplyResourceOrgService.FirstOrDefault(x => x.Contragent != null && x.Contragent.Name == "ОАО \"РСО\"");

            return supplyOrg;
        }
    }
}
