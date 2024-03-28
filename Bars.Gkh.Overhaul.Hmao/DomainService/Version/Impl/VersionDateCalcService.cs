namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Сервис для получения расчетных дат
    /// </summary>
    public class VersionDateCalcService : IVersionDateCalcService
    {

        public IWindsorContainer Container { get; set; }

        public IDomainService<RealityObjectStructuralElementInProgrammStage3> st3Domain { get; set; }

        public IDomainService<PublishedProgramRecord> publishedDomain { get; set; }

        public IDomainService<DpkrCorrectionStage2> correctionDomain { get; set; }

        public IDomainService<SubsidyRecordVersion> subsidyDomain { get; set; }

        /// <summary>
        /// Получить дату расчета ДПКР по МО
        /// </summary>
        public IDataResult GetDateCalcDpkr(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAs<long>("muId");

            var query = st3Domain.GetAll().Where(x => x.RealityObject.Municipality.Id == muId).Select(x => x.ObjectCreateDate);

            var calcDate = query.Any() ? query.Max() : DateTime.MinValue;

            return new BaseDataResult(calcDate > DateTime.MinValue ? 
                                        new 
                                        {
                                            date = calcDate,
                                            dateStr = calcDate.ToString("dd.MM.yyyy HH:mm")
                                        }
                                        : null);
        }

        /// <summary>
        /// Получить дату расчета показателей собираемости
        /// </summary>
        public IDataResult GetDateCalcOwnerCollection(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAs<long>("muId");

            var query = subsidyDomain.GetAll()
                    .Where(x => x.Version.Municipality.Id == muId && x.Version.IsMain)
                    .Select(x => new { x.ObjectEditDate, x.DateCalcOwnerCollection});

            var calcDate = query.Any(x => x.DateCalcOwnerCollection.HasValue) ? query.Where(x => x.DateCalcOwnerCollection.HasValue)
                                                                                .Select(x => x.DateCalcOwnerCollection.Value)
                                                                                .Max() 
                                                                              : (query.Any() ? query.Max(x => x.ObjectEditDate) : DateTime.MinValue);

            return new BaseDataResult(calcDate > DateTime.MinValue ?
                                        new
                                        {
                                            date = calcDate,
                                            dateStr = calcDate.ToString("dd.MM.yyyy HH:mm")
                                        }
                                        : null);
        }

        /// <summary>
        /// Получить дату расчета корректировки по МО
        /// </summary>
        public IDataResult GetDateCalcCorrection(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAs<long>("muId");

            var query =
                correctionDomain.GetAll()
                                .Where(
                                    x =>
                                    x.Stage2.Stage3Version.ProgramVersion.IsMain
                                    && x.Stage2.Stage3Version.ProgramVersion.Municipality.Id == muId)
                                .Select(x => x.ObjectCreateDate);
            var calcDate = query.Any() ? query.Max() : DateTime.MinValue;

            return new BaseDataResult(calcDate > DateTime.MinValue ?
                                        new
                                        {
                                            date = calcDate,
                                            dateStr = calcDate.ToString("dd.MM.yyyy HH:mm")
                                        }
                                        : null);
        }

        public DateTime GetDateCalcPublished(List<long> muIds)
        {
            var query =
                publishedDomain.GetAll()
                               .Where(x => x.PublishedProgram.ProgramVersion.IsMain
                                   && muIds.Contains(x.PublishedProgram.ProgramVersion.Municipality.Id))
                               .Select(x => x.ObjectCreateDate);

            return query.Any() ? query.Max() : DateTime.MinValue;
        }


        /// <summary>
        /// Получить дату опубликования по МО
        /// </summary>
        public IDataResult GetDateCalcPublished(BaseParams baseParams)
        {
            var muId = baseParams.Params.GetAs<long>("muId");

            var listIds = new List<long> { muId };

            var calcDate = GetDateCalcPublished(listIds);

            return new BaseDataResult(calcDate > DateTime.MinValue ?
                                        new
                                        {
                                            date = calcDate,
                                            dateStr = calcDate.ToString("dd.MM.yyyy HH:mm")
                                        }
                                        : null);
        }
    }
}