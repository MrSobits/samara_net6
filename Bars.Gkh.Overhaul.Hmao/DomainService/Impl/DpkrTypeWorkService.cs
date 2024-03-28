namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using B4.Utils;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;
    using Entities;
    using Gkh.Domain;

    /// <summary>
    /// Сервис по работам КР (ХМАО)
    /// </summary>
    public class DpkrTypeWorkService : IDpkrTypeWorkService
    {
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public Dictionary<long, int> GetTypeWorkDpkrYear(IQueryable<TypeWorkCr> typeWorkQuery)
        {
            var typeWorkCrVersSt1Domain = this.Container.ResolveDomain<TypeWorkCrVersionStage1>();

            try
            {
                return typeWorkCrVersSt1Domain.GetAll()
                    .Where(x => typeWorkQuery.Any(y => y.Id == x.TypeWorkCr.Id))
                    .Select(x => new
                    {
                        x.TypeWorkCr.Id,
                        x.Stage1Version.Stage2Version.Stage3Version.Year
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.Id)
                    .ToDictionary(x => x.Key, y => y.SafeMin(x => x.Year));

            }
            finally
            {
                this.Container.Release(typeWorkCrVersSt1Domain);
            }
        }

        /// <inheritdoc />
        public IDataResult SplitStructElementInTypeWork(BaseParams baseParams)
        {
            var typeWorkId = baseParams.Params.GetAsId("typeWorkId");
            var st1Id = baseParams.Params.GetAsId("st1Id");

            var typeWorkDomain = this.Container.ResolveRepository<TypeWorkCr>();
            var typeWorkSt1Domain = this.Container.ResolveRepository<TypeWorkCrVersionStage1>();

            try
            {
                var typeWork = typeWorkDomain.Get(typeWorkId);

                if (typeWork == null)
                {
                    return new BaseDataResult(false, "Не удалось определить вид работ!");
                }

                var st1Count = typeWorkSt1Domain.GetAll().Count(x => x.TypeWorkCr.Id == typeWork.Id);

                if (st1Count <= 1)
                {
                    return new BaseDataResult(new { TypeWork = typeWork.Id });
                }

                var typeWorkSt1 = typeWorkSt1Domain.GetAll().FirstOrDefault(x => x.TypeWorkCr.Id == typeWork.Id && x.Id == st1Id);

                if (typeWorkSt1 == null)
                {
                    return new BaseDataResult(false, "Не удалось определить конструктивный элемент");
                }

                var newTypeWork = new TypeWorkCr
                {
                    ObjectCr = typeWork.ObjectCr,
                    FinanceSource = typeWork.FinanceSource,
                    Work = typeWork.Work,
                    StageWorkCr = typeWork.StageWorkCr,
                    HasPsd = typeWork.HasPsd,
                    YearRepair = typeWork.YearRepair,
                    IsDpkrCreated = typeWork.IsDpkrCreated,
                    State = typeWork.State
                };

                this.Container.InTransaction(() =>
                {
                    typeWorkDomain.Save(newTypeWork);

                    typeWorkSt1.TypeWorkCr = newTypeWork;

                    typeWorkSt1Domain.Update(typeWorkSt1);
                });

                return new BaseDataResult(new { TypeWork = newTypeWork.Id });
            }
            finally
            {
                this.Container.Release(typeWorkDomain);
                this.Container.Release(typeWorkSt1Domain);
            }
        }

        /// <inheritdoc />
        public void MergeStructElementInTypeWork(TypeWorkCr oldTypeWork, TypeWorkCr newTypeWork)
        {
            var typeWorkSt1Domain = this.Container.ResolveRepository<TypeWorkCrVersionStage1>();

            try
            {
                this.Container.InTransaction(() =>
                {
                    typeWorkSt1Domain.GetAll()
                        .Where(x => x.TypeWorkCr.Id == oldTypeWork.Id)
                        .ToArray()
                        .ForEach(x =>
                        {
                            x.TypeWorkCr = newTypeWork;

                            typeWorkSt1Domain.Update(x);
                        });
                });
            }
            finally
            {
                this.Container.Release(typeWorkSt1Domain);
            }
        }

        /// <inheritdoc />
        public bool HasTypeWorkReferenceInDpkr(TypeWorkCr typeWork)
        {
            var typeWorkSt1Domain = this.Container.ResolveRepository<TypeWorkCrVersionStage1>();

            try
            {
                return typeWorkSt1Domain.GetAll().Any(x => x.TypeWorkCr.Id == typeWork.Id);
            }
            finally
            {
                this.Container.Release(typeWorkSt1Domain);
            }
        }

        /// <inheritdoc />
        public Dictionary<long, DpkrTypeWorkDto> GetWorksByObjectCr(long objectCrId)
        {
            return this.GetWorksByObjectCr(new[] {objectCrId});
        }

        /// <inheritdoc />
        public Dictionary<long, DpkrTypeWorkDto> GetWorksByObjectCr(long[] objectCrIds)
        {
            var workCrStage1Domain = this.Container.ResolveRepository<TypeWorkCrVersionStage1>();

            try
            {
                var dpkrType = this.Container.GetGkhConfig<GkhCrConfig>().DpkrConfig.DisplayDataByDpkr;

                return workCrStage1Domain.GetAll()
                    .Where(x => objectCrIds.Contains(x.TypeWorkCr.ObjectCr.Id))
                    .GroupBy(x => x.TypeWorkCr.Id)
                    .ToDictionary(x => x.Key, y => new DpkrTypeWorkDto
                    {
                        CalcBy = y.First().CalcBy,
                        UnitMeasure = y.First().UnitMeasure.Name,
                        Volume = dpkrType == DisplayDataByDpkr.Previous ? y.Sum(x => x.Volume) : y.Sum(x => x.Stage1Version.Volume),
                        Sum = dpkrType == DisplayDataByDpkr.Previous ? y.Sum(x => x.Sum) : y.Sum(x => x.Stage1Version.Sum)
                    });
            }
            finally
            {
                this.Container.Release(workCrStage1Domain);
            }
        }
    }
}