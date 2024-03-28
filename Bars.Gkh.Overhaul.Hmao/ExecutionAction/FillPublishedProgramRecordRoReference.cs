namespace Bars.Gkh.Overhaul.Hmao.ExecutionAction
{
    using System;
    using System.Reflection;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.ExecutionAction;
    using Bars.Gkh.ExecutionAction.Impl;

    using NHibernate;

    /// <summary>
    /// Заполнение ссылки на Жилой дом в записях опубликованной программы
    /// </summary>
    public class FillPublishedProgramRecordRoReference : BaseExecutionAction
    { 
        /// <summary>
        /// Описание действия
        /// </summary>
        public override string Description => "Заполнение ссылки на Жилой дом в записях опубликованной программы";

        /// <summary>
        /// Название для отображения
        /// </summary>
        public override string Name => "ДПКР - Заполнение ссылки на Жилой дом в записях опубликованной программы";

        /// <summary>
        /// Действие
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        private BaseDataResult Execute()
        {
            var checkColumnsQuery = @"select exists(
           select 1
           from information_schema.columns
           where table_name = 'ovrhl_publish_prg_rec'
                 and column_name = 'ro_id'
       )
       and exists(
           select 1
           from information_schema.columns
           where table_name = 'ovrhl_publish_prg'
                 and column_name = 'total_ro_count'
       )
       and exists(
           select 1
           from information_schema.columns
           where table_name = 'ovrhl_publish_prg'
                 and column_name = 'included_ro_count'
       )
       and exists(
           select 1
           from information_schema.columns
           where table_name = 'ovrhl_publish_prg'
                 and column_name = 'excluded_ro_count'
       ) as check;";

            var fillRoIdQuery = @"drop table if exists ro_id_temp;

create temp table ro_id_temp as
  (
    select
      r.id,
      st3.ro_id
    from ovrhl_publish_prg_rec r
      join ovrhl_stage2_version st2 on r.stage2_id = st2.id
      join ovrhl_version_rec st3 on st2.st3_version_id = st3.id
  );

create index index_id_ro_id_temp
  on ro_id_temp using btree (id);

analyze ro_id_temp;

drop index ind_ovrhl_publish_prg_rec_ro_id;
drop index ind_ovrhl_publish_prg_rec_pp;
drop index ind_ovrhl_publish_prg_rec_st2;

update ovrhl_publish_prg_rec r
set ro_id = ro_id_temp.ro_id
from ro_id_temp
where r.id = ro_id_temp.id;

create index ind_ovrhl_publish_prg_rec_ro_id
  on ovrhl_publish_prg_rec using btree (ro_id);
create index ind_ovrhl_publish_prg_rec_pp
  on ovrhl_publish_prg_rec using btree (publish_prg_id);
create index ind_ovrhl_publish_prg_rec_st2
  on ovrhl_publish_prg_rec using btree (stage2_id);

analyze ovrhl_publish_prg_rec;

drop table if exists ro_id_temp;";

            var fillRoCountQuery = @"drop table if exists ro_count_temp;

create temp table ro_count_temp as
  (
    select
      p.id,
      count(distinct r.ro_id) ro_count
    from ovrhl_publish_prg p
      join ovrhl_publish_prg_rec r on p.id = r.publish_prg_id
    group by p.id
  );

create index index_id_ro_count_temp
  on ro_count_temp using btree (id);

analyze ro_count_temp;

drop index ind_ovrhl_publish_prg_v;
drop index ind_ovrhl_publish_prg_st;
drop index ind_ovrhl_publish_prg_fxml;
drop index ind_ovrhl_publish_prg_fpdf;
drop index ind_ovrhl_publish_prg_fcer;
drop index ind_ovrhl_publish_prg_f;

update ovrhl_publish_prg p
set total_ro_count = ro_count_temp.ro_count
from ro_count_temp
where p.id = ro_count_temp.id;

create index ind_ovrhl_publish_prg_v
  on ovrhl_publish_prg using btree (version_id);
create index ind_ovrhl_publish_prg_st
  on ovrhl_publish_prg using btree (state_id);
create index ind_ovrhl_publish_prg_fxml
  on ovrhl_publish_prg using btree (file_xml_id);
create index ind_ovrhl_publish_prg_fpdf
  on ovrhl_publish_prg using btree (file_pdf_id);
create index ind_ovrhl_publish_prg_fcer
  on ovrhl_publish_prg using btree (file_cer_id);
create index ind_ovrhl_publish_prg_f
  on ovrhl_publish_prg using btree (file_id);

analyze ovrhl_publish_prg;

drop table if exists ro_id_temp;";

            var sessionProvider = this.Container.Resolve<ISessionProvider>();

            using (var session = sessionProvider.OpenStatelessSession())
            {
                using (var transaction = session.BeginTransaction())
                {
                    try
                    {
                        var check = session.CreateSQLQuery(checkColumnsQuery)
                            .AddScalar("check", NHibernateUtil.Boolean)
                            .UniqueResult<bool>();

                        if (!check)
                        {
                            throw new ValidationException("Отсутсвуют необходимые для проведения действия столбцы");
                        }

                        session.CreateSQLQuery(fillRoIdQuery).SetTimeout(1 * 60 * 60).ExecuteUpdate();

                        session.CreateSQLQuery(fillRoCountQuery).SetTimeout(30 * 60).ExecuteUpdate();
                    }
                    catch
                    {
                        transaction.Rollback();
                        throw;
                    }

                    transaction.Commit();
                }
            }

            return new BaseDataResult();
        }
    }
}