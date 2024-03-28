namespace Bars.Gkh.Overhaul.Regions.Kamchatka.ExecutionAction
{
    using System;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.Overhaul.DomainService;
    using Bars.Gkh.Overhaul.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    public class DeleteDoubleRecordInVersionAction : BaseExecutionAction
    {
        public override string Name
        {
            get { return "Удаление дублирующих записей в версии ДПКР (КАМЧАТКА)"; }
        }

        public override string Description
        {
            get { return "Удаление дублирующих записей в версии ДПКР (КАМЧАТКА)"; }
        }

        public override Func<IDataResult> Action
        {
            get { return this.Execute; }
        }

        private BaseDataResult Execute()
        {
            var roStructElDomain = this.Container.Resolve<IDomainService<RealityObjectStructuralElement>>();
            var stage2Domain = this.Container.Resolve<IDomainService<VersionRecordStage2>>();
            var stage1Domain = this.Container.Resolve<IDomainService<VersionRecordStage1>>();
            var stage3Domain = this.Container.Resolve<IDomainService<VersionRecord>>();
            var roStrElService = this.Container.Resolve<IRealityObjectStructElementService>();

            try
            {
                var stage3Ids = stage3Domain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            VersId = x.ProgramVersion.Id,
                            x.CommonEstateObjects,
                            RoId = x.RealityObject.Id,
                            x.Year
                        })
                    .AsEnumerable()
                    .GroupBy(x => new {x.VersId, x.CommonEstateObjects, x.RoId, x.Year})
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Select(y => y.Id).OrderBy(y => y).Skip(1))
                    .SelectMany(x => x)
                    .ToList();

                var incorrectSt3Ids = stage3Domain.GetAll()
                    .Where(x => !stage1Domain.GetAll().Any(y => y.Stage2Version.Stage3Version.Id == x.Id))
                    .Select(x => x.Id)
                    .ToList();

                var stage1Ids = stage1Domain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            St2Id = x.Stage2Version.Id,
                            StElId = x.StructuralElement.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => new {x.St2Id, x.StElId})
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Select(y => y.Id).OrderBy(y => y).Skip(1))
                    .SelectMany(x => x)
                    .ToList();

                stage1Ids.AddRange(
                    stage1Domain.GetAll()
                        .Where(x => stage3Ids.Contains(x.Stage2Version.Stage3Version.Id))
                        .Select(x => x.Id)
                        .ToList());

                var stage2Ids = stage2Domain.GetAll()
                    .Select(
                        x => new
                        {
                            x.Id,
                            St3Id = x.Stage3Version.Id,
                            CeoId = x.CommonEstateObject.Id
                        })
                    .AsEnumerable()
                    .GroupBy(x => new {x.St3Id, x.CeoId})
                    .Where(x => x.Count() > 1)
                    .Select(x => x.Select(y => y.Id).OrderBy(y => y).Skip(1))
                    .SelectMany(x => x)
                    .ToList();

                stage2Ids.AddRange(
                    stage2Domain.GetAll()
                        .Where(x => stage3Ids.Contains(x.Stage3Version.Id) || !stage1Domain.GetAll().Any(y => y.Stage2Version.Id == x.Id))
                        .Select(x => x.Id)
                        .ToList());

                var sessionProvider = this.Container.Resolve<ISessionProvider>();
                var session = sessionProvider.OpenStatelessSession();

                try
                {
                    using (var transaction = session.BeginTransaction())
                    {
                        try
                        {
                            foreach (var id in stage1Ids)
                            {
                                // удаляем связь с видами работ
                                session.CreateSQLQuery(
                                    string.Format(@"delete from ovrhl_type_work_cr_st1 where st1_id = {0}", id))
                                    .ExecuteUpdate();

                                // удаляем запись 1 этапа
                                session.CreateSQLQuery(string.Format(@"delete from OVRHL_STAGE1_VERSION where id = {0}", id)).ExecuteUpdate();
                            }

                            foreach (var id in stage2Ids)
                            {
                                // удаляем записи краткосрочной программы
                                session.CreateSQLQuery(string.Format(@"delete from ovrhl_short_prog_rec where stage2_id = {0}", id)).ExecuteUpdate();

                                // удаляем Корректировки по версии
                                session.CreateSQLQuery(string.Format(@"delete from ovhl_dpkr_correct_st2 where st2_version_id  = {0}", id)).ExecuteUpdate();

                                // удаляем версию 1 этапа
                                session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage1_version where stage2_version_id = {0}", id))
                                    .ExecuteUpdate();

                                // удаляем запись опубликованной программы
                                session.CreateSQLQuery(string.Format(@"delete from ovrhl_publish_prg_rec where stage2_id = {0} ", id)).ExecuteUpdate();

                                // удаляем запись 2 этапа
                                session.CreateSQLQuery(string.Format(@"delete from OVRHL_STAGE2_VERSION where id = {0}", id)).ExecuteUpdate();
                            }

                            foreach (var id in stage3Ids)
                            {
                                // удаляем версию 2 этапа
                                session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage2_version where st3_version_id = {0}", id)).ExecuteUpdate();

                                // удаляем запись 3 этапа
                                session.CreateSQLQuery(string.Format(@"delete from OVRHL_VERSION_REC where id = {0}", id)).ExecuteUpdate();
                            }

                            foreach (var id in incorrectSt3Ids)
                            {
                                // удаляем версию 2 этапа
                                session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage2_version where st3_version_id = {0}", id)).ExecuteUpdate();

                                // удаляем запись 3 этапа
                                session.CreateSQLQuery(string.Format(@"delete from OVRHL_VERSION_REC where id = {0}", id)).ExecuteUpdate();
                            }

                            // удаляем, продублированные название ООИ в 3 этапе
                            session.CreateSQLQuery(@"UPDATE ovrhl_version_rec SET ceo_string = substring(ceo_string, 0, position(',' in ceo_string))
                                                       where id in (select id from ovrhl_version_rec where ceo_string like '%, %')").ExecuteUpdate();

                            transaction.Commit();
                        }
                        catch (Exception exc)
                        {
                            transaction.Rollback();
                            throw exc;
                        }
                    }
                }
                finally
                {
                    sessionProvider.CloseCurrentSession();
                }
            }
            finally
            {
                this.Container.Release(roStructElDomain);
                this.Container.Release(roStrElService);
                this.Container.Release(stage2Domain);
                this.Container.Release(stage1Domain);
                this.Container.Release(stage3Domain);
            }

            return new BaseDataResult();
        }
    }
}