namespace Bars.Gkh.Overhaul.Nso.DomainService
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.Gkh.Overhaul.Nso.Entities;

    public class ProgramVersionDomainService : BaseDomainService<ProgramVersion>
    {

        /// <summary>
        /// Данный метод удаления перекрыл потому что перед кдалением надо удалить большие объекмы данных
        /// В связи с чем необходимо работать с Session и выполнят ьпрямые запросы что нельзя сделать
        /// через Интерцепторы
        /// </summary>
        public virtual void Delete(object id)
        {
            // Удаляем огромное количество записей относящиеся к версии
            DeleteChildrenRecords((long)id);
        }


        /// <summary>
        /// Данный метод удаления перекрыл потому что перед кдалением надо удалить большие объекмы данных
        /// В связи с чем необходимо работать с Session и выполнят ьпрямые запросы что нельзя сделать
        /// через Интерцепторы
        /// </summary>
        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = baseParams.Params.GetAs<long[]>("records");

            foreach (var id in ids)
            {
                // Удаляем огромное количество записей относящиеся к версии
                DeleteChildrenRecords(id);
            }

            return new BaseDataResult(ids);
        }

        private void DeleteChildrenRecords(long programId)
        {
            //Поскольку много записей приходится удалять то делаю так
            var sessionProvider = Container.Resolve<ISessionProvider>();
            var session = sessionProvider.GetCurrentSession();

            try
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        // удаляем Субсидирвоание по версии
                        session.CreateSQLQuery(string.Format("delete from ovrhl_subsidy_rec_version where version_id = {0}", programId)).ExecuteUpdate();
                        session.CreateSQLQuery(string.Format("delete from ovrhl_sm_record_version where version_id = {0}", programId)).ExecuteUpdate();

                        // удаляем записи краткосрочной программы
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_short_prog_rec where stage2_id in ( 
                                                select st2.id from ovrhl_stage2_version st2 
	                                            join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {0} )",
                                                            programId)).ExecuteUpdate();

                        // удаляем Корректировки по версии
                        session.CreateSQLQuery(string.Format(@"delete from ovhl_dpkr_correct_st2 where st2_version_id in ( 
                                                select st2.id from ovrhl_stage2_version st2 
	                                            join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {0} )",
                                                            programId)).ExecuteUpdate();

                        // удаляем связь с видами работ
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_type_work_cr_st1  where st1_id  in ( 
                                                select st1.id from ovrhl_stage1_version st1
                                                join ovrhl_stage2_version st2 on st2.id = st1.stage2_version_id
	                                            join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {0} )",
                                                            programId)).ExecuteUpdate();


                        // удаляем версию 1 этапа
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage1_version where stage2_version_id in ( 
                                                select st2.id from ovrhl_stage2_version st2 
	                                            join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {0} )",
                                                            programId)).ExecuteUpdate();

                        // удаляем запись опубликованной программы
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_publish_prg_rec where stage2_id in ( 
                                                select st2.id from ovrhl_stage2_version st2 
                                                join ovrhl_version_rec st3 on st3.id = st2.st3_version_id and st3.version_id = {0} )",
                                                            programId)).ExecuteUpdate();

                        // удаляем версию 2 этапа
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_stage2_version where st3_version_id in ( 
                                                select st3.id from ovrhl_version_rec st3 where st3.version_id = {0} )",
                                                            programId)).ExecuteUpdate();

                        // удаляем версию 3 этапа
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_version_rec where version_id = {0}",
                                                            programId)).ExecuteUpdate();

                        // удаляем Версию дифицитов МО
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_short_prog_difitsit where version_id = {0}",
                                                            programId)).ExecuteUpdate();

                        // удаляем Версию дифицитов МО
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_version_prm where version_id = {0}",
                                                            programId)).ExecuteUpdate();

                        // удаляем Опубликованные программы
                        session.CreateSQLQuery(string.Format(@"delete from ovrhl_publish_prg where version_id = {0}",
                                                            programId)).ExecuteUpdate();

                        // удаляем Опубликованные программы
                        session.CreateSQLQuery(string.Format(@"delete from OVRHL_PRG_VERSION where id = {0}",
                                                            programId)).ExecuteUpdate();

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
                sessionProvider.CloseCurrentSession();
            }
            
        }
    }
}
