namespace Bars.Gkh1468.Domain.PassportImport.Impl.DataProcessor
{
    using System.Collections.Generic;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using Castle.Windsor;
    using Entities;
    using Interfaces;
    using ProxyEntity;
    using Bars.B4.Modules.States;

    public class OnlineServiceDataProcessor : IDataProcessor
    {
        public IDomainService<HouseProviderPassport> PassportDomain { get; set; }
        public IDomainService<HouseProviderPassportRow> RowDomain { get; set; }
        public IWindsorContainer _container { get; set; }

        private IDynamicDataProvider _dataProvider;

        public OnlineServiceDataProcessor(IDynamicDataProvider dataProvider, IWindsorContainer container)
        {
            _dataProvider = dataProvider;
            _container = container;
            PassportDomain = container.Resolve<IDomainService<HouseProviderPassport>>();
            RowDomain = container.Resolve<IDomainService<HouseProviderPassportRow>>();
        }

        public void ProcessData()
        {
            var dataToSave = _dataProvider.GetData() as List<PassportWithAttributes>;
            var stateProvider = _container.Resolve<IStateProvider>();
            var state =
                _container.ResolveDomain<State>()
                    .GetAll()
                    .Where(x => x.TypeId == "houseproviderpassport")
                    .ToList()
                    .FirstOrDefault(x => x.Code.Trim().ToUpper() == "ПОДПИСАНО");

            if (dataToSave == null)
            {
                return;
            }

            _container.UsingForResolved<IDataTransaction>((c, tr) =>
            {
                try
                {
                    foreach (var passportAndAttributes in dataToSave)
                    {
                        var passport = passportAndAttributes.Passport;
                        if (passport.Id > 0)
                        {
                            PassportDomain.Update(passport);
                        }
                        else
                        {
                            if (state != null)
                            {
                                passport.State = state;
                            }
                            else
                            {
                                stateProvider.SetDefaultState(passport);
                            }

                            PassportDomain.Save(passport);
                        }

                        foreach (var row in passportAndAttributes.Rows)
                        {
                            if (row.Id > 0)
                            {
                                RowDomain.Update(row);
                            }
                            else
                            {
                                RowDomain.Save(row);
                            }
                        }
                    }

                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();
                }
            });
        }
    }
}