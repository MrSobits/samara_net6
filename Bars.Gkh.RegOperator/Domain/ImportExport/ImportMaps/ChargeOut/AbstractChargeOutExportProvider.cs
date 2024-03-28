namespace Bars.Gkh.RegOperator.Domain.ImportExport.ImportMaps
{
    using System;
    using System.Linq;

    using B4;
    using B4.DataAccess;
    using B4.IoC;
    using B4.Utils;

    using Bars.Gkh.Entities;
    using Bars.Gkh.RegOperator.DomainModelServices;
    using Bars.Gkh.Repositories.ChargePeriod;

    using Gkh.Domain;
    using DataProviders.Export;
    using Mapping;
    using ProxyEntity;
    using DomainService.PersonalAccount;
    using Entities;
    using Repository;

    public abstract class AbstractChargeOutExportProvider : AbstractImportMap<ChargeOutProxy>, IExportDataProvider
    {
        public ImportExportDataProvider ImportExportProvider { get; set; }

        public IImportMap Mapper
        {
            get { return this; }
        }

        protected virtual string FileName
        {
            get { return "{0}.{1}".FormatUsing(ProviderCode, Format.ToLowerInvariant()); }
        }

        public IDataResult<ExportOutput> GetData(BaseParams @params)
        {
            var persAccFilterService = Container.Resolve<IPersonalAccountFilterService>();
            var chargePeriodRepo = Container.Resolve<IChargePeriodRepository>();
            var chargeDomain = Container.ResolveDomain<PersonalAccountCharge>();
            var periodDomain = Container.ResolveDomain<ChargePeriod>();
            var persAccDomain = Container.ResolveDomain<BasePersonalAccount>();

            using (Container.Using(chargePeriodRepo, chargeDomain, periodDomain, persAccDomain, persAccFilterService))
            {
                var date = @params.Params.GetAs<DateTime>("date");

                var accountIds = @params.Params.GetAs<string>("accountIds").ToLongArray();

                // Фильтры в реестре вынесены в отдельный метод поскольку Выгрузки, и все действия делаются по отфильтрованному реестру через этот метод
                var queryByFilters = persAccFilterService.GetQueryableByFilters(@params, persAccDomain.GetAll());

                var periodId = @params.Params.GetAsId("periodId");

                ChargePeriod period;

                if (periodId > 0)
                {
                    period = periodDomain.Get(periodId);
                }
                else
                {
                    period =
                        chargePeriodRepo.GetPeriodByDate(date)
                        ?? chargePeriodRepo.GetFirstPeriod();
                }

                if (period == null)
                {
                    return new ExportOutputResult("Не удалось получить период") { Success = false };
                }

                var endDate = period.GetEndDate();

                var charges = chargeDomain.GetAll()
                    .Where(x => x.ChargeDate >= period.StartDate)
                    .Where(x => x.ChargeDate <= endDate)
                    .Where(x => queryByFilters.Any(a => a.Id == x.BasePersonalAccount.Id))
                    .WhereIf(accountIds.Any(),x => accountIds.Contains(x.BasePersonalAccount.Id))
                    .Select(x => new
                    {
                        x.BasePersonalAccount.Room,
                        x.BasePersonalAccount.Room.RealityObject,
                        x.BasePersonalAccount,
                        x.ChargeDate,
                        x.Penalty,
                        x.ChargeTariff
                    })
                    .Select(x => new ChargeOutProxy
                    {
                        //Id = x.BasePersonalAccount.Id,
                        persacc_name = x.BasePersonalAccount.AccountOwner.Name,
                        personal_acc = x.BasePersonalAccount.PersonalAccountNum,
                        persacc_extsys = x.BasePersonalAccount.PersAccNumExternalSystems,
                        mr = x.RealityObject.Municipality.Name,
                        mu = x.RealityObject.MoSettlement.Name,
                        city = x.RealityObject.FiasAddress.PlaceName,
                        street = x.RealityObject.FiasAddress.StreetName,
                        house = x.RealityObject.FiasAddress.House,
                        liter = x.RealityObject.FiasAddress.Letter,
                        housing = x.RealityObject.FiasAddress.Housing,
                        building = x.RealityObject.FiasAddress.Building,
                        room_num = x.BasePersonalAccount.Room.RoomNum,
                        month = x.ChargeDate.ToString("MMMM"),
                        year = x.ChargeDate.Year,
                        charged_sum = x.ChargeTariff,
                        charged_penalty = x.Penalty,
                        square = x.Room.Area
                    })
                    .ToList();

                var serialized = ImportExportProvider.Serialize(charges, this);

                return new ExportOutputResult
                {
                    Data = new ExportOutput
                    {
                        Data = serialized,
                        OutputName = FileName
                    },
                    Success = true
                };
            }
        }
    }
}