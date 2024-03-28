namespace Bars.GkhCr.DataProviders
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.Analytics.Data;
    using Bars.B4.Modules.Analytics.Enums;
    using Bars.B4.Modules.Analytics.Extensions;
    using Bars.B4.Utils;
    using Bars.GkhCr.Entities;
    using Bars.GkhCr.Meta;
    using Castle.Windsor;
    using NHibernate.Linq;

    public class ManyApartmentsHouses : BaseCollectionDataProvider<МКД>
    {
        public ManyApartmentsHouses(IWindsorContainer container) : base(container)
        {
        }

        protected override IQueryable<МКД> GetDataInternal(BaseParams baseParams)
        {
            var programCrId = baseParams.Params[string.Format("{0}_programCr", Key)].ToLong();

            var municipalityIdsList = baseParams.Params.GetAs(string.Format("{0}_municipalities", Key), string.Empty);
            var municipalityIds = !string.IsNullOrEmpty(municipalityIdsList)
                ? municipalityIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];

            var financeIdsList = baseParams.Params.GetAs(string.Format("{0}_financeSources", Key), string.Empty);
            var financeIds = !string.IsNullOrEmpty(financeIdsList)
                ? financeIdsList.Split(',').Select(id => id.ToLong()).ToArray()
                : new long[0];

            var reportDate = baseParams.Params[string.Format("{0}_reportDate", Key)].ToDateTime();
                // TODO: выяснить почему не используется?

            var typeWorkCrDomain = Container.Resolve<IDomainService<TypeWorkCr>>();
            var finSrcResourceDomain = Container.Resolve<IDomainService<FinanceSourceResource>>();

            var typeWorkCrs = typeWorkCrDomain.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .WhereIf(municipalityIds.Length > 0,
                    x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(financeIds.Length > 0, x => financeIds.Contains(x.FinanceSource.Id))
                .Fetch(x => x.ObjectCr)
                .ThenFetch(x => x.RealityObject)
                .ThenFetch(x => x.Municipality)
                .Fetch(x => x.FinanceSource)
                .Fetch(x => x.Work).ToList();

            var finSrcResources = finSrcResourceDomain.GetAll()
                .Where(x => x.ObjectCr.ProgramCr.Id == programCrId)
                .WhereIf(municipalityIds.Length > 0,
                    x => municipalityIds.Contains(x.ObjectCr.RealityObject.Municipality.Id))
                .WhereIf(financeIds.Length > 0, x => financeIds.Contains(x.FinanceSource.Id))
                .Fetch(x => x.FinanceSource)
                .Fetch(x => x.ObjectCr).ToList();

            var data = typeWorkCrs.LeftJoin(finSrcResources,
                cr =>
                    new
                    {
                        ObjectCrId = cr.ObjectCr.Id,
                        FinanceSourceId = cr.FinanceSource != null ? cr.FinanceSource.Id : -1
                    },
                resource => new {ObjectCrId = resource.ObjectCr.Id, FinanceSourceId = resource.FinanceSource.Id},
                (cr, resource) => new {TypeWork = cr, FinanceSource = resource}).Select(x => new
                {
                    ObjectCrId = x.TypeWork.ObjectCr.Id,
                    MunicipalityName = x.TypeWork.ObjectCr.RealityObject.Municipality.Name,
                    x.TypeWork.ObjectCr.RealityObject.Address,
                    MaintainStartDate = x.TypeWork.ObjectCr.RealityObject.DateCommissioning ?? DateTime.MinValue,
                    LastOverhaulYear = x.TypeWork.ObjectCr.RealityObject.DateLastOverhaul ?? DateTime.MinValue,
                    WallMaterial =
                        x.TypeWork.ObjectCr.RealityObject.WallMaterial != null
                            ? x.TypeWork.ObjectCr.RealityObject.WallMaterial.Name
                            : string.Empty,
                    FloorsCount = x.TypeWork.ObjectCr.RealityObject.MaximumFloors ?? 0,
                    EntrancesCount = x.TypeWork.ObjectCr.RealityObject.NumberEntrances ?? 0,
                    Area = x.TypeWork.ObjectCr.RealityObject.AreaMkd ?? 0M,
                    RoomsArea = x.TypeWork.ObjectCr.RealityObject.AreaLivingNotLivingMkd ?? 0M,
                    RoomsAreaWithPrivateOwner = x.TypeWork.ObjectCr.RealityObject.AreaLivingOwned ?? 0M,
                    InhabitantsCount = x.TypeWork.ObjectCr.RealityObject.NumberLiving ?? 0,
                    Sum = x.TypeWork.Sum ?? 0M,
                    WorkName = x.TypeWork.Work.Name,
                    FinishDate = x.TypeWork.DateEndWork ?? DateTime.MinValue,
                    SubjectBudgetCost = x.FinanceSource != null ? x.FinanceSource.BudgetSubject ?? 0M : 0M,
                    FundCost = x.FinanceSource != null ? x.FinanceSource.FundResource ?? 0M : 0M,
                    OtherBudgetCost = x.FinanceSource != null ? x.FinanceSource.OwnerResource ?? 0M : 0M,
                    LocalBudgetCost = x.FinanceSource != null ? x.FinanceSource.BudgetMu ?? 0M : 0M
                })
                .GroupBy(x => x.ObjectCrId)
                .ToDictionary(
                    x => x.Key,
                    x => new МКД
                    {
                        Муниципальное_образование = x.Select(y => y.MunicipalityName).FirstOrDefault(),
                        Адрес = x.Select(y => y.Address).FirstOrDefault(),
                        Год_ввода_в_эксплуатацию = x.Select(y => y.MaintainStartDate).FirstOrDefault(),
                        Год_последнего_кап_ремонта = x.Select(y => y.LastOverhaulYear).FirstOrDefault(),
                        Материал_стен = x.Select(y => y.WallMaterial).FirstOrDefault(),
                        Количество_этажей = x.Select(y => y.FloorsCount).FirstOrDefault(),
                        Количество_подъездов = x.Select(y => y.EntrancesCount).FirstOrDefault(),
                        Общая_площадь = x.Select(y => y.Area).FirstOrDefault(),
                        Площадь_помещений = x.Select(y => y.RoomsArea).FirstOrDefault(),
                        Площадь_помещений_в_том_числе_в_собственности_граждан = x.Select(y => y.RoomsAreaWithPrivateOwner).FirstOrDefault(),
                        Количество_зарегистрированных_жителей = x.Select(y => y.InhabitantsCount).FirstOrDefault(),
                        Стоимость_итого = x.Sum(y => y.Sum),
                        Вид_ремонта = string.Join(", ", x.Select(z => z.WorkName).Where(z => !string.IsNullOrEmpty(z))),
                        Плановая_дата_завершения_работ = x.Select(y => y.FinishDate).FirstOrDefault(),
                        Стоимость_за_счет_средств_бюджета_субъекта = x.Sum(z => z.SubjectBudgetCost.RoundDecimal(2)),
                        Стоимость_за_счет_средств_Фонда = x.Sum(z => z.FundCost.RoundDecimal(2)),
                        Стоимость_за_счет_средств_ТСЖ_и_собственников_помещений = x.Sum(z => z.OtherBudgetCost.RoundDecimal(2)),
                        Стоимость_за_счет_средств_местного_бюджета = x.Sum(z => z.LocalBudgetCost.RoundDecimal(2)),
                        Предельная_стоимость_1_кв_м = 9000,
                        Удельная_стоимость_1_кв_м =
                            x.Select(y => y.Area).FirstOrDefault() > 0
                                ? x.Sum(y => y.Sum)/x.Select(y => y.Area).FirstOrDefault()
                                : 0
                    }
                )
                .Select(x => x.Value);


            return data.AsQueryable();
        }

        public override string Name
        {
            get { return "Перечень многоквартирных домов (Объекты КР)"; }
        }

        public override string Key
        {
            get { return typeof (ManyApartmentsHouses).Name; }
        }

        public override string Description
        {
            get { return Name; }
        }

        public override IEnumerable<DataProviderParam> Params
        {
            get
            {
                return new List<DataProviderParam>
                {
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_municipalities", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Муниципальные образования",
                        Additional = "Municipality"
                    },
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_financeSources", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Разрезы финансирования",
                        Additional = "FinanceSource"
                    },
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_reportDate", Key),
                        ParamType = ParamType.Date,
                        Label = "Дата отчета"
                    },
                    new DataProviderParam
                    {
                        Name = string.Format("{0}_programCr", Key),
                        ParamType = ParamType.Catalog,
                        Label = "Программа кап.ремонта",
                        Additional = "ProgramCr",
                        Required = true
                    }
                };
            }
        }
    }
}
