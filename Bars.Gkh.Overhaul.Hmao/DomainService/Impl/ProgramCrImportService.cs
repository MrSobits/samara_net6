namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    using System.Collections.Generic;
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Import;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;
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
            var dictTypeWorkToUpdate = new Dictionary<long, TypeWorkCr>();

            foreach (var typeWorkInfo in typeWorkYears)
            {
                var typeWork = typeWorkInfo.Key;

                foreach (var year in typeWorkInfo.Value)
                {
                    var records = recs.Get("{0}_{1}_{2}".FormatUsing(typeWork.ObjectCr.RealityObject.Id, typeWork.Work.Id, year));

                    if (records != null)
                    {
                        records.ForEach(x =>
                        {
                            // Поскольку проставляется ссылка на ДПКР, то у работы необходимо выставит ьпризнак что она создана из ДПКР
                            // посколкьу в Капремонте ест ьместа где ест ьпровеки этог опризнака
                            if (!dictTypeWorkToUpdate.ContainsKey(typeWork.Id))
                            {
                                typeWork.IsDpkrCreated = true;
                                dictTypeWorkToUpdate.Add(typeWork.Id, typeWork);
                            }

                            listToSave.Add(new TypeWorkCrVersionStage1
                            {
                                TypeWorkCr = typeWork,
                                Stage1Version = VersStage1Domain.Load(x.St1Id),
                                Volume = x.Volume,
                                Sum = x.Sum,
                                UnitMeasure = typeWork.Work.UnitMeasure
                            });

                        });
                    }
                }
            }

            if (dictTypeWorkToUpdate.Any())
            {
                var listTypeWorkToSave = dictTypeWorkToUpdate.Values.ToList();
                TransactionHelper.InsertInManyTransactions(Container, listTypeWorkToSave, 10000, true, true);
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
            session.CreateSQLQuery(
                string.Format(
                    @"delete from CR_OBJ_TYPE_WORK_HIST where TYPE_WORK_ID in({0}) ",
                    typeWorkIds.Select(x => x.ToStr()).AggregateWithSeparator(", "))).ExecuteUpdate();
        }

        /// <summary>
        /// Удалить связь записи первого этапа версии и видов работ
        /// </summary>
        /// <param name="session">Сессия</param>
        /// <param name="typeWorkIds">Типы работ</param>
        public void DeleteTypeWorkCrVersionStage1(IStatelessSession session, IEnumerable<long> typeWorkIds)
        {
            var query = string.Format(
                @"delete from OVRHL_TYPE_WORK_CR_ST1 where TYPE_WORK_CR_ID in({0}) ",
                typeWorkIds.Select(x => x.ToStr()).AggregateWithSeparator(", "));
            
            session.CreateSQLQuery(query).ExecuteUpdate();
        }
    }
}