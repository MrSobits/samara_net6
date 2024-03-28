using Bars.B4;
using Bars.B4.DataAccess;
using Bars.B4.Modules.States;
using Bars.B4.Utils;
using Bars.Gkh.ConfigSections.Overhaul.Enums;
using Bars.Gkh.Entities;
using Bars.Gkh.Overhaul.Hmao.ConfigSections;
using Bars.Gkh.Overhaul.Hmao.Entities;
using Bars.Gkh.Utils;
using Castle.Windsor;
using NHibernate;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Bars.Gkh.Overhaul.Hmao.DomainService.Impl
{
    public class FakePublicationService : IFakePublicationService
    {
        public IWindsorContainer Container { get; set; }
        public ISessionProvider sessionProvider { get; set; }

        /// <summary>
        /// Домен-сервис <see cref="State" />
        /// </summary>
        public IDomainService<State> StateDomain { get; set; }

        public IDomainService<VersionRecord> versionRecordDomain { get; set; }
        public IDomainService<VersionRecordStage2> VersionRecordStage2Domain { get; set; }
        public IDomainService<ProgramVersion> ProgramVersionDomain { get; set; }
        public IDomainService<PublishedProgramRecord> PublishedProgramRecordDomain { get; set; }
        public IDomainService<PublishedProgram> PublishedProgramDomain { get; set; }
        public IDomainService<DpkrCorrectionStage2> DpkrCorrectedDomain { get; set; }

        private OverhaulHmaoConfig overhaulHmaoConfig;
        public OverhaulHmaoConfig OverhaulHmaoConfig
        {
            get { return this.overhaulHmaoConfig ?? (this.overhaulHmaoConfig = this.Container.GetGkhConfig<OverhaulHmaoConfig>()); }
        }

        /// <summary>
        /// Создать ДПКР для публикации
        /// </summary>
        public void CreateDpkrForPublish(ProgramVersion version)
        {
            var session = sessionProvider.OpenStatelessSession();

            var parameters = new KRParameters
            {
                minYear = OverhaulHmaoConfig.ProgrammPeriodStart,
                period = OverhaulHmaoConfig.PublishProgramConfig.PublicationPeriod,
                maxYear = OverhaulHmaoConfig.ProgrammPeriodEnd,
                shortTerm = OverhaulHmaoConfig.ShortTermProgPeriod,
            };

            if (parameters.period == 0)
                throw new ApplicationException($"Не задан параметр \"Период для публикации (год)\" в единых настройках приложения");

            var useShortTerm = this.OverhaulHmaoConfig.PublishProgramConfig.UseShortProgramPeriod;
            if (useShortTerm == TypeUseShortProgramPeriod.WithOut)
                // Поскольку в настройках указано, что не используем период краткосрочки
                parameters.shortTerm = 0;
            if (parameters.shortTerm > 0)
                parameters.minYear = parameters.minYear + parameters.shortTerm;

            try
            {
                //удаляем текущие записи в опубликованной программе
                DeleteAllPublishedProgramRecords(session, version.Id);

                // Грохаем существующую запись опубликованной программы поскольку
                // у нее могли существовать уже ненужные подписи ЭЦП
                DeleteAllPublishedPrograms(version.Id);

                // Проверяем, существует ли нужный статус и если нет, то создаем новый
                var firstState = GetStartStatus();

                //Записи, полученные в результате разделения
                var versionRecordsForUpd = versionRecordDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == version.Id)
                    .Where(x => x.IsDividedRec)
                    .ToList();

                //список новых опубликованных программ для сохранения
                var listPubProgsToSave = new List<PublishedProgram>();
                //список новых опубликованных записей для сохранения
                var listRecordsToSave = new List<PublishedProgramRecord>();

                // Получаем словарь ид версии - массив огрызков и по нему создаем опубликованную программу
                var dataCorrections = GetVersionsCache(version.Id);

                //создаем программу
                MakePublishVersion(dataCorrections, version, firstState, listPubProgsToSave, listRecordsToSave, parameters);

                //сохраняем
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        listPubProgsToSave.Where(x => x.Id == 0).ForEach(x => session.Insert(x));
                        listRecordsToSave.ForEach(x => session.Insert(x));

                        versionRecordsForUpd.ForEach(
                            x =>
                            {
                                x.IsDividedRec = false;
                                x.PublishYearForDividedRec = 0;
                                session.Update(x);
                            });

                        transaction.Commit();
                    }
                    catch (Exception)
                    {
                        transaction.Rollback();
                        throw;
                    }
                }

            }
            finally
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private void MakePublishVersion(List<CacheRecord> tempCorrection,
            ProgramVersion programVersion,
            State firstState,
            List<PublishedProgram> listPubProgsToSave,
            List<PublishedProgramRecord> listRecordsToSave,
            KRParameters parameters)
        {
            var publishProg = new PublishedProgram
            {
                State = firstState,
                ProgramVersion = programVersion
            };


            listPubProgsToSave.Add(publishProg);

            if (tempCorrection != null)
            {
                foreach (var rec in tempCorrection)
                {
                    var publicationYear = rec.Year;

                    if (rec.Year >= parameters.minYear)
                    {
                        publicationYear = rec.Year + parameters.period - 1 - (rec.Year - parameters.minYear) % parameters.period;
                        publicationYear = publicationYear > parameters.maxYear ? parameters.maxYear : publicationYear;
                    }

                    var newRec = new PublishedProgramRecord
                    {
                        PublishedProgram = publishProg,
                        Stage2 = new VersionRecordStage2 { Id = rec.Stage2Id },
                        RealityObject = new RealityObject { Id = rec.RealityObjectId },
                        PublishedYear = publicationYear,
                        IndexNumber = rec.IndexNumber,
                        Locality = rec.Locality,
                        Street = rec.Street,
                        House = rec.House,
                        Housing = rec.Housing,
                        Address = rec.Address,
                        CommonEstateobject = rec.CommonEstateobject,
                        CommissioningYear = rec.CommissioningYear,
                        Sum = rec.Sum
                    };

                    listRecordsToSave.Add(newRec);
                }
            }
        }

        private List<CacheRecord> GetVersionsCache(long versionId)
        {
            //коррекция stage2.id - год
            var correctionCache = DpkrCorrectedDomain.GetAll()
            .Where(x => x.Stage2.Stage3Version.ProgramVersion.Id == versionId)
            .GroupBy(x => x.Stage2.Id)
            .ToDictionary(x => x.Key, y => y.Select(z => z.PlanYear).First());

            var cache = VersionRecordStage2Domain.GetAll()
            .Where(x => x.Stage3Version.ProgramVersion.Id == versionId)
            .Where(x => x.Stage3Version.Show)
            .Select(
             x => new CacheRecord
             {
                 Stage2Id = x.Id,
                 Year = x.Stage3Version.Year,
                 IndexNumber = x.Stage3Version.IndexNumber,
                 Locality = x.Stage3Version.RealityObject.FiasAddress.PlaceName,
                 Street = x.Stage3Version.RealityObject.FiasAddress.StreetName,
                 House = x.Stage3Version.RealityObject.FiasAddress.House,
                 Housing = x.Stage3Version.RealityObject.FiasAddress.Housing,
                 Address = x.Stage3Version.RealityObject.FiasAddress.AddressName,
                 CommonEstateobject = x.CommonEstateObject.Name,
                 CommissioningYear = x.Stage3Version.RealityObject.BuildYear.HasValue ? x.Stage3Version.RealityObject.BuildYear.Value : 0,
                 versionId = x.Stage3Version.ProgramVersion.Id,
                 Sum = x.Sum,
                 RealityObjectId = x.Stage3Version.RealityObject.Id
             })
            .ToList();

            foreach (var record in cache)
            {
                if (correctionCache.ContainsKey(record.Stage2Id))
                    record.Year = correctionCache[record.Stage2Id];
            }

            return cache;
        }

        //Кусок нормального опубликования
        //private Dictionary<long, CacheRecord[]> GetVersionsCache(List<long> versions)
        //{
        //    return DpkrCorrectedDomain.GetAll()
        //    .Where(x => x.Stage2.Stage3Version.ProgramVersion.IsMain)
        //    .Where(x => versions.Any(y => y == x.Stage2.Stage3Version.ProgramVersion.Id))
        //    .Select(
        //     x => new CacheRecord
        //     {
        //         Stage2Id = x.Stage2.Id,
        //         Year = x.PlanYear,
        //         IndexNumber = x.Stage2.Stage3Version.IndexNumber,
        //         Locality = x.RealityObject.FiasAddress.PlaceName,
        //         Street = x.RealityObject.FiasAddress.StreetName,
        //         House = x.RealityObject.FiasAddress.House,
        //         Housing = x.RealityObject.FiasAddress.Housing,
        //         Address = x.RealityObject.FiasAddress.AddressName,
        //         CommonEstateobject = x.Stage2.CommonEstateObject.Name,
        //         CommissioningYear = x.RealityObject.BuildYear.HasValue ? x.RealityObject.BuildYear.Value : 0,
        //         versionId = x.Stage2.Stage3Version.ProgramVersion.Id,
        //         Sum = x.Stage2.Sum,
        //         RealityObjectId = x.RealityObject.Id
        //     })
        //    .AsEnumerable()
        //    .GroupBy(x => x.versionId)
        //    .ToDictionary(x => x.Key, y => y.ToArray());
        //}

        private State GetStartStatus()
        {
            var startstate = StateDomain.GetAll().FirstOrDefault(x => x.TypeId == "ovrhl_published_program" && x.StartState);
            if (startstate != null)
                return startstate;

            startstate = new State
            {
                Name = "Черновик",
                Code = "Черновик",
                StartState = true,
                TypeId = "ovrhl_published_program"
            };

            StateDomain.Save(startstate);

            return startstate;
        }

        private void DeleteAllPublishedProgramRecords(IStatelessSession session, long versionId)
        {
            using (var transaction = session.BeginTransaction())
            {
                try
                {
                    // Удаляем записи опубликованной программы именно этой основной версии
                    session.CreateSQLQuery(
                        string.Format(
                            "  delete from OVRHL_PUBLISH_PRG_REC where PUBLISH_PRG_ID in ( " +
                                "       select id from OVRHL_PUBLISH_PRG where version_id = {0} ) ",
                            versionId)
                        ).ExecuteUpdate();
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        private void DeleteAllPublishedPrograms(long versionId)
        {

            var publishPrograms = PublishedProgramDomain.GetAll()
                .Where(x => x.ProgramVersion.Id == versionId)
                .ToList();

            if (publishPrograms.Any())
            {
                publishPrograms.ForEach(x => PublishedProgramDomain.Delete(x.Id));
            }
        }

        private class CacheRecord
        {
            public long Stage2Id { get; set; }
            public int Year { get; set; }
            public string Locality { get; set; }
            public string Street { get; set; }
            public string Address { get; set; }
            public string CommonEstateobject { get; set; }
            public int CommissioningYear { get; set; }
            public long versionId { get; set; }
            public long RealityObjectId { get; set; }
            public int IndexNumber { get; internal set; }
            public string House { get; internal set; }
            public string Housing { get; internal set; }
            public decimal Sum { get; internal set; }
        }

        //настройки
        private class KRParameters
        {
            //Период долгосрочной программы с
            public int minYear { get; set; }
            //Период для публикации (год)
            public int period { get; internal set; }
            //Период долгосрочной программы по
            public int maxYear { get; internal set; }
            //Период планирования бюджета (год)
            public int shortTerm { get; internal set; }
        }
    }
}
