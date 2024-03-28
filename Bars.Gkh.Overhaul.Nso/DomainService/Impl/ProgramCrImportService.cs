namespace Bars.Gkh.Overhaul.Nso.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Nso.Entities;
    using Bars.Gkh.Utils;
    using Bars.GkhCr.DomainService;
    using Bars.GkhCr.Entities;
    using Castle.Windsor;
    using NHibernate;

    public class ProgramCrImportService : IProgramCrImportService
    {
        public IWindsorContainer Container { get; set; }
        public IDomainService<TypeWorkCrVersionStage1> TypeWorkCrVersStage1Domain { get; set; }
        public IDomainService<VersionRecordStage1> VersStage1Domain { get; set; }
        public IDomainService<StructuralElementWork> StructuralElementWorkDomain { get; set; }

        public void SaveDpkrLink(ILogImport logImport, Dictionary<TypeWorkCr, List<int>> typeWorkYears)
        {
            var recs = VersStage1Domain.GetAll()
                .Join(StructuralElementWorkDomain.GetAll(),
                    x => x.StructuralElement.StructuralElement.Id,
                    y => y.StructuralElement.Id,
                    (x, y) => new {VersSt1 = x, Work = y})
                .Where(x => x.VersSt1.Stage2Version.Stage3Version.ProgramVersion.IsMain)
                .Select(x => new
                {
                    St1Id = x.VersSt1.Id,
                    x.VersSt1.Sum,
                    x.VersSt1.Volume,
                    x.VersSt1.Stage2Version.Stage3Version.Year,
                    RoId = x.VersSt1.Stage2Version.Stage3Version.RealityObject.Id,
                    WorkId = x.Work.Job.Work.Id
                })
                .ToList()
                .GroupBy(x => "{0}_{1}_{2}".FormatUsing(x.RoId, x.WorkId, x.Year))
                .ToDictionary(x => x.Key, y => y.Select(x => new
                {
                    x.Volume,
                    x.St1Id,
                    x.Sum
                }).ToList());

            var listToSave = new List<TypeWorkCrVersionStage1>();
            foreach (var typeWorkInfo in typeWorkYears)
            {
                var typeWork = typeWorkInfo.Key;

                foreach (var year in typeWorkInfo.Value)
                {
                    var records = recs.Get("{0}_{1}_{2}".FormatUsing(typeWork.ObjectCr.RealityObject.Id, typeWork.Work.Id, year));

                    if (records != null)
                    {
                        records.ForEach(x => listToSave.Add(new TypeWorkCrVersionStage1
                        {
                            TypeWorkCr = typeWork,
                            Stage1Version = VersStage1Domain.Load(x.St1Id),
                            Volume = x.Volume,
                            Sum = x.Sum,
                            UnitMeasure = typeWork.Work.UnitMeasure
                        }));
                    }
                }

            }

            TransactionHelper.InsertInManyTransactions(Container, listToSave, 10000, true, true);
        }

        public void DeleteDpkrLink(ISession session, IEnumerable<long> typeWorkIds)
        {
            session.CreateSQLQuery(string.Format(@"delete from CR_OBJ_TYPE_WORK_HIST where TYPE_WORK_ID in({0}) "
              , typeWorkIds.Select(x => x.ToStr()).AggregateWithSeparator(", "))).ExecuteUpdate();
        }

        public void DeleteDpkrLink(IStatelessSession session, IEnumerable<long> typeWorkIds)
        {
        }
        
        public void DeleteTypeWorkCrVersionStage1(IStatelessSession session, IEnumerable<long> typeWorkIds)
        {
        }
    }
}