namespace Bars.Gkh1468.Wcf
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Modules.States;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh1468.DomainService;
    using Bars.Gkh1468.Entities;

    public partial class PassportService
    {
        public string CreateOkiPassports(string year, string month)
        {
            var stateProvider = Container.Resolve<IStateProvider>();
            var okiPassportService = Container.Resolve<IDomainService<OkiProviderPassport>>();
            var okiPassServ = Container.Resolve<IOkiPassportService>();

            var currentUser = Container.Resolve<IUserIdentity>();

            if (currentUser is AnonymousUserIdentity)
            {
                return null;
            }

            var curOp =
                Container.Resolve<IDomainService<Operator>>()
                    .GetAll()
                    .FirstOrDefault(x => x.User.Id == currentUser.UserId);

            if (curOp == null)
            {
                return null;
            }

            var existOkiPassportsMunicipalities = Container.Resolve<IDomainService<OkiPassport>>()
                         .GetAll()
                         .Where(x => x.ReportYear == year.ToInt() && x.ReportMonth == month.ToInt())
                         .Select(x => x.Municipality.Id)
                         .ToArray();

            var muList = Container.Resolve<IDomainService<OperatorMunicipality>>()
                       .GetAll()
                       .Where(x => !existOkiPassportsMunicipalities.Contains(x.Municipality.Id))
                       .Select(x => x.Municipality)
                       .Distinct()
                       .ToList();

            foreach (var municipality in muList)
            {
                using (var transaction = this.Container.Resolve<IDataTransaction>())
                {
                    try
                    {
                        var entity = new OkiProviderPassport
                                        {
                                            OkiPassport = okiPassServ.GetPassport(municipality, year.ToInt(), month.ToInt()).Data as OkiPassport,
                                            Contragent = curOp.Contragent,
                                            ContragentType = curOp.ContragentType,
                                            Municipality = municipality,
                                            ReportYear = year.ToInt(),
                                            ReportMonth = month.ToInt()
                                        };

                        stateProvider.SetDefaultState(entity);

                        okiPassportService.Save(entity);
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }

                    transaction.Commit();
                }
            }

            return string.Format("Сохранено {0} паспортов", muList.Count);
        }
    }
}