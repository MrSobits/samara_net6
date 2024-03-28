namespace Bars.Gkh1468.Wcf
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Enums;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Modules.Gkh1468.Entities;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh1468.Entities;
    using Bars.Gkh1468.Entities.Passport;

    public partial class PassportService
    {
        public string CreateHousePassports(string year, string month)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            var housePassportService = Container.Resolve<IDomainService<HouseProviderPassport>>();
            var housePasServ = Container.Resolve<IHousePassportService>();

            var currentUser = Container.Resolve<IUserIdentity>();

            if (currentUser is AnonymousUserIdentity)
            {
                return null;
            }

            var curOp =
                Container.Resolve<IDomainService<Operator>>().GetAll()
                    .FirstOrDefault(x => x.User.Id == currentUser.UserId);

            if (curOp == null)
            {
                return null;
            }

            var operators =
                Container.Resolve<IDomainService<Operator>>()
                    .GetAll();

            var existPassports =
                Container.Resolve<IDomainService<HousePassport>>().GetAll()
                         .Where(x => x.ReportYear == year.ToInt() && x.ReportMonth == month.ToInt())
                         .Select(x => x.RealityObject.Id);

            var realObjs = Container.Resolve<IDomainService<PublicServiceOrgContract>>()
                .GetAll()
                .Where(x => operators.Where(y => y.ContragentType == ContragentType.Pku)
                    .Select(z => z.Contragent.Id).Contains(x.PublicServiceOrg.Contragent.Id)
                    && !existPassports.Contains(x.RealityObject.Id))
                .Select(x => x.RealityObject)
                .ToList();

            realObjs.AddRange(Container.Resolve<IDomainService<RealityObjectResOrg>>()
                .GetAll()
                .Where(x => operators.Where(y => y.ContragentType == ContragentType.Pku)
                    .Select(z => z.Contragent.Id).Contains(x.ResourceOrg.Contragent.Id)
                    && !existPassports.Contains(x.RealityObject.Id))
                .Select(x => x.RealityObject)
                .ToList());

            realObjs.AddRange(Container.Resolve<IDomainService<ManagingOrgRealityObject>>()
               .GetAll()
                .Where(x => operators.Where(y => y.ContragentType == ContragentType.Pku)
                   .Select(z => z.Contragent.Id).Contains(x.ManagingOrganization.Contragent.Id)
                   && !existPassports.Contains(x.RealityObject.Id))
               .Select(x => x.RealityObject)
               .ToList());

            foreach (var realObj in realObjs.Distinct())
            {
                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        var entity = new HouseProviderPassport
                        {
                            HousePassport = housePasServ.GetPassport(realObj, year.ToInt(), month.ToInt()).Data as HousePassport,
                            Contragent = curOp.Contragent,
                            ContragentType = curOp.ContragentType,
                            RealityObject = realObj,
                            ReportYear = year.ToInt(),
                            ReportMonth = month.ToInt()
                        };
                        housePasServ.GetPassport(realObj, year.ToInt(), month.ToInt());
                        stateProvider.SetDefaultState(entity);

                        housePassportService.Save(entity);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    transaction.Commit();
                }
            }

            return string.Format("Сохранено {0} паспортов", realObjs.Distinct().Count());
        }
    }
}