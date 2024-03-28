namespace Bars.Gkh.Overhaul.Hmao.DomainService
{
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.B4.DomainService.BaseParams;
    using Bars.B4.IoC;
    using Bars.B4.Utils;
    using Bars.Gkh.DomainService;
    using Bars.Gkh.Overhaul.Hmao.Entities;
    using Bars.Gkh.Overhaul.Hmao.Entities.Version;

    /// <summary>
	/// Домен сервис для Версия программы
	/// </summary>
    public class ProgramVersionDomainService : GkhFileStorageDomainService<ProgramVersion>
    {
        /// <summary>
        /// Данный метод удаления перекрыл потому что перед кдалением надо удалить большие объекмы данных
        /// В связи с чем необходимо работать с Session и выполнят ьпрямые запросы что нельзя сделать
        /// через Интерцепторы
        /// </summary>
        public virtual void Delete(object id)
        {
            // Удаляем огромное количество записей относящиеся к версии
	        this.DeleteChildrenRecords((int)id);
            
            base.Delete(id);
        }


        /// <summary>
        /// Данный метод удаления перекрыл потому что перед кдалением надо удалить большие объекмы данных
        /// В связи с чем необходимо работать с Session и выполнят ьпрямые запросы что нельзя сделать
        /// через Интерцепторы
        /// </summary>
        public override IDataResult Delete(BaseParams baseParams)
        {
            var ids = Converter.ToIntArray(baseParams.Params, "records");

            foreach (var id in ids)
            {
                // Удаляем огромное количество записей относящиеся к версии
	            this.DeleteChildrenRecords(id);
            }

            return base.Delete(baseParams);
        }

        private void DeleteChildrenRecords(int programId)
        {
            //Поскольку много записей приходится удалять то делаю так
            var session = this.Container.Resolve<ISessionProvider>().GetCurrentSession();

            session.CreateSQLQuery($@"
                CREATE TEMP TABLE tmp_stage2_version (
                    id             bigint NOT NULL,
                    CONSTRAINT pk_tmp_stage2_version PRIMARY KEY (id)
                );

                CREATE TEMP TABLE tmp_stage1_version (
                    id             bigint NOT NULL,
                    CONSTRAINT pk_tmp_stage1_version PRIMARY KEY (id)
                );

                CREATE TEMP TABLE tmp_publish_program (
                    id             bigint NOT NULL,
                    CONSTRAINT pk_tmp_publish_program PRIMARY KEY (id)
                );

                CREATE TEMP TABLE tmp_version_record (
                    id             bigint NOT NULL,
                    CONSTRAINT pk_tmp_version_record PRIMARY KEY (id)
                );

                DO $code$
                DECLARE 
                    i_version_id   bigint = {programId};
                BEGIN
                    -- Идентификаторы Версионирования второго этапа
                    INSERT INTO tmp_stage2_version
                    SELECT
                        os2v.id
                    FROM public.ovrhl_stage2_version os2v 
                        INNER JOIN public.ovrhl_version_rec ovr
                            ON ovr.id = os2v.st3_version_id
                            AND ovr.version_id = i_version_id;

                    -- Идентификаторы Версионирования первого этапа
                    INSERT INTO tmp_stage1_version
                    SELECT
                        os1v.id
                    FROM public.ovrhl_stage1_version os1v
                        INNER JOIN public.ovrhl_stage2_version os2v
                            ON os2v.id = os1v.stage2_version_id
                        INNER JOIN public.ovrhl_version_rec ovr
                            ON ovr.id = os2v.st3_version_id
                            AND ovr.version_id = i_version_id;

                    -- Идентификаторы Опубликованных программ
                    INSERT INTO tmp_publish_program
                    SELECT
                        opp.id
                    FROM public.ovrhl_publish_prg opp
                    WHERE opp.version_id = i_version_id;

                    -- Идентификаторы Записей в версии программы
                    INSERT INTO tmp_version_record
                    SELECT
                        ovr.id
                    FROM public.ovrhl_version_rec ovr
                    WHERE ovr.version_id = i_version_id;

                    ANALYZE tmp_stage2_version;
                    ANALYZE tmp_stage1_version;
                    ANALYZE tmp_publish_program;
                    ANALYZE tmp_version_record;

                    -- УДАЛЕНИЕ
                    -- Субсидирование
                    DELETE FROM public.ovrhl_subsidy_rec_version d
                    WHERE d.version_id = i_version_id;

                    -- Краткосрочные программы
                    DELETE FROM public.ovrhl_short_prog_rec d
                        USING tmp_stage2_version td
                    WHERE d.stage2_id = td.id;

                    -- Корректировки
                    DELETE FROM public.ovrhl_dpkr_correct_st2 d
                        USING tmp_stage2_version td
                    WHERE d.st2_version_id = td.id;

                    -- Связи с видами работ
                    DELETE FROM public.ovrhl_type_work_cr_st1 d
                        USING tmp_stage1_version td
                    WHERE d.st1_id = td.id;

                    -- Связи с решениями собственников
                    DELETE FROM public.ovrhl_change_year_owner_decision d
                        USING tmp_stage1_version td
                    WHERE d.stage1_id = td.id;

                    -- Версии первого этапа
                    DELETE FROM public.ovrhl_stage1_version d
                        USING tmp_stage2_version td
                    WHERE d.stage2_version_id = td.id;

                    -- Записи опубликованной программы
                    DELETE FROM public.ovrhl_publish_prg_rec d
                        USING tmp_publish_program td
                    WHERE d.publish_prg_id = td.id;

                    -- Версии второго этапа
                    DELETE FROM public.ovrhl_stage2_version d
                        USING tmp_version_record td
                    WHERE d.st3_version_id = td.id;

                    -- Версии программы
                    DELETE FROM public.ovrhl_version_rec d
                    WHERE d.version_id = i_version_id;

                    -- Версии дефицитов МО
                    DELETE FROM public.ovrhl_short_prog_difitsit d
                    WHERE d.version_id = i_version_id;

                    -- Параметры версий
                    DELETE FROM public.ovrhl_version_prm d
                    WHERE d.version_id = i_version_id;

                    -- Опубликованные программы
                    DELETE FROM public.ovrhl_publish_prg d
                    WHERE d.version_id = i_version_id;

                    DROP TABLE tmp_stage2_version;
                    DROP TABLE tmp_stage1_version;
                    DROP TABLE tmp_publish_program;
                    DROP TABLE tmp_version_record;
                END;
                $code$;
            ").ExecuteUpdate();
            
            var versionActualizeLogDomain = this.Container.Resolve<IDomainService<VersionActualizeLog>>();
            var versionActualizeLogRecordDomain = this.Container.ResolveDomain<VersionActualizeLogRecord>();
            
            using (this.Container.Using(versionActualizeLogDomain, versionActualizeLogRecordDomain))
            {
                var parameters = new BaseParams();
                var data = versionActualizeLogDomain.GetAll()
                    .Where(x => x.ProgramVersion.Id == programId)
                    .Select(x => x.Id)
                    .ToArray();
                parameters.Params["records"] = data.Cast<object>().ToList();

                versionActualizeLogRecordDomain.GetAll()
                    .WhereContains(x => x.ActualizeLog.Id, data)
                    .Select(x => x.Id)
                    .ForEach(x => versionActualizeLogRecordDomain.Delete(x));
                            
                versionActualizeLogDomain.Delete(parameters);
            }
        }
    }
}