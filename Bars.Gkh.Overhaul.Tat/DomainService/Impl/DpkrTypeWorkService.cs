﻿namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using B4;
    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.ConfigSections.Cr;
    using Bars.Gkh.ConfigSections.Cr.Enums;
    using Bars.Gkh.Domain.CollectionExtensions;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;
    using Entities;

    /// <summary>
    /// Сервис по работам КР (ТАТАРСТАН)
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
            // пока необходимо только для ДПКР версии ХМАО
            return new BaseDataResult(false, "Операция не поддерживается");
        }

        /// <inheritdoc />
        public void MergeStructElementInTypeWork(TypeWorkCr oldTypeWork, TypeWorkCr newTypeWork)
        {
            // пока необходимо только для ДПКР версии ХМАО
        }

        /// <inheritdoc />
        public bool HasTypeWorkReferenceInDpkr(TypeWorkCr typeWork)
        {
            // пока необходимо только для ДПКР версии ХМАО
            return true;
        }

        /// <inheritdoc />
        public Dictionary<long, DpkrTypeWorkDto> GetWorksByObjectCr(long objectCrId)
        {
            return this.GetWorksByObjectCr(new[] { objectCrId });
        }

        /// <inheritdoc />
        public Dictionary<long, DpkrTypeWorkDto> GetWorksByObjectCr(long[] objectCrIds)
        {
            var workCrStage1Domain = this.Container.Resolve<IDomainService<TypeWorkCrVersionStage1>>();

            try
            {
                var dpkrType = this.Container.GetGkhConfig<GkhCrConfig>().DpkrConfig.DisplayDataByDpkr;

                return workCrStage1Domain.GetAll()
                    .WhereContains(x => x.TypeWorkCr.ObjectCr.Id, objectCrIds)
                    .Where(x => x.Stage1Version.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                    .Select(x => new
                    {
                        TypeWorkCrId = x.TypeWorkCr.Id,
                        x.Stage1Version.StrElement.CalculateBy,
                        UnitMeasure = x.Stage1Version.StrElement.UnitMeasure.Name,
                        x.Volume,
                        x.Sum,
                        Stage1VersionVolume = x.Stage1Version.Volume,
                        Stage1VersionSum = x.Stage1Version.Sum + x.Stage1Version.SumService
                    })
                    .AsEnumerable()
                    .GroupBy(x => x.TypeWorkCrId)
                    .ToDictionary(x => x.Key, y => new DpkrTypeWorkDto
                    {
                        CalcBy = y.Select(x => x.CalculateBy).First(),
                        UnitMeasure = y.Select(x => x.UnitMeasure).First(),
                        Volume = dpkrType == DisplayDataByDpkr.Previous ? y.Sum(x => x.Volume) : y.Sum(x => x.Stage1VersionVolume),
                        Sum = dpkrType == DisplayDataByDpkr.Previous ? y.Sum(x => x.Sum) : y.Sum(x => x.Stage1VersionSum)
                    });
            }
            finally
            {
                this.Container.Release(workCrStage1Domain);
            }
        }
    }
}