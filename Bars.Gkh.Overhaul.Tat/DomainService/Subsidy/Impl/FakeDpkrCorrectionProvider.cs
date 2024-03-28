namespace Bars.Gkh.Overhaul.Tat.DomainService.Impl
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.Gkh.Overhaul.Tat.ConfigSections;
    using Bars.Gkh.Overhaul.Tat.Entities;
    using Bars.Gkh.Overhaul.Tat.Enum;
    using Bars.Gkh.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Фейковая реализация. Заменить при реализации коррекьтировки субсидирования
    /// </summary>
    public class FakeDpkrCorrectionProvider : IDpkrCorrectionDataProvider
    {
        private readonly IDomainService<VersionRecordStage2> _stage2Domain;

        private readonly IDomainService<DpkrCorrectionStage2> _correctionDomain;

        private readonly IWindsorContainer _container;

        public FakeDpkrCorrectionProvider(IDomainService<VersionRecordStage2> stage2Domain,
            IDomainService<DpkrCorrectionStage2> correctionDomain,
            IWindsorContainer container)
        {
            _stage2Domain = stage2Domain;
            _correctionDomain = correctionDomain;
            _container = container;
        }

        public IQueryable<DpkrCorrectionStage2> GetCorrectionData(BaseParams baseParams)
        {
            var municipalityId = baseParams.Params.GetAs<long>("municipalityId");

            var config = _container.GetGkhConfig<OverhaulTatConfig>();
            var startYear = config.ProgrammPeriodStart;
            var endYear = config.ProgrammPeriodEnd;

            var corrRecords =
                _correctionDomain.GetAll()
                                 .WhereIf(
                                     municipalityId > 0,
                                     x => x.Stage2.Stage3Version.RealityObject.Municipality.Id == municipalityId)
                                 .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain);
            if (corrRecords.Any())
            {
                return corrRecords;
            }

            var rnd = new Random();

            _container.UsingForResolved<IDataTransaction>((c, tr) =>
            {

                var stage2Data =
                _stage2Domain.GetAll()
                    .WhereIf(municipalityId > 0, x => x.Stage3Version.RealityObject.Municipality.Id == municipalityId)
                    .Where(x => x.Stage3Version.ProgramVersion.IsMain)
                    .Select(x => new { Stage2 = x, RoId = x.Stage3Version.RealityObject.Id });

                foreach (var data in stage2Data.ToList())
                {
                    var year = rnd.Next(startYear, endYear);
                    var corrRecord = new DpkrCorrectionStage2
                    {
                        PlanYear = year,
                        RealityObject = new RealityObject
                        {
                            Id = data.RoId
                        },
                        Stage2 = data.Stage2,
                        TypeResult = GetResultType(year)
                    };

                    _correctionDomain.Save(corrRecord);
                }

                try
                {
                    tr.Commit();
                }
                catch
                {
                    tr.Rollback();

                    throw;
                }
            });

            return _correctionDomain.GetAll()
                                 .WhereIf(
                                     municipalityId > 0,
                                     x => x.Stage2.Stage3Version.RealityObject.Municipality.Id == municipalityId)
                                 .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain);
        }

        private TypeResultCorrectionDpkr GetResultType(int year)
        {
            if (year % (int)TypeResultCorrectionDpkr.AddInShortTerm == 0)
            {
                return TypeResultCorrectionDpkr.AddInShortTerm;
            }

            if (year % (int)TypeResultCorrectionDpkr.RemoveFromShortTerm == 0)
            {
                return TypeResultCorrectionDpkr.RemoveFromShortTerm;
            }

            if (year % (int)TypeResultCorrectionDpkr.InLongTerm == 0)
            {
                return TypeResultCorrectionDpkr.InLongTerm;
            }

            if (year % (int)TypeResultCorrectionDpkr.InShortTerm == 0)
            {
                return TypeResultCorrectionDpkr.InShortTerm;
            }

            return year % 3 == 0 ? TypeResultCorrectionDpkr.RemoveFromShortTerm : TypeResultCorrectionDpkr.New;
        }
    }
}