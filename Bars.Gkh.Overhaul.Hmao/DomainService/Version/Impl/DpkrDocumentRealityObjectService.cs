namespace Bars.Gkh.Overhaul.Hmao.DomainService.Version.Impl
{
    using System;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Overhaul.Hmao.Entities;

    using Castle.Windsor;

    /// <summary>
    /// Service for <see cref="DpkrDocumentRealityObject"/>
    /// </summary>
    public class DpkrDocumentRealityObjectService : IDpkrDocumentRealityObjectService
    {
        /// <summary>
        /// IoC контейнер
        /// </summary>
        public IWindsorContainer Container { get; set; }

        /// <inheritdoc />
        public IDataResult AddRealityObjects(BaseParams baseParams)
        {
            var dpkrDocumentId = baseParams.Params.GetAsId("dpkrDocumentId");

            if (dpkrDocumentId == 0)
            {
                return new BaseDataResult(false, "Не удалось определить документ ДПКР");
            }

            var sessionProvider = this.Container.Resolve<ISessionProvider>();
            var session = sessionProvider.GetCurrentSession();

            try
            {
                /*
                    Включенные дома
                    -- Дом в опубликованной программе версии текущего документа
                    -- Дома нет среди включенных у другого документа

                    Исключенные дома
                    -- Дома нет в опубликованной программе версии текущего документа
                    -- Дом среди включенных другого документа (МО дома среди МО версий ДПКР текущего документа)
                    -- Дома нет среди исключенных у другого документа
                 */

                session.CreateSQLQuery(@"
                    DROP TABLE IF EXISTS tmp_table_included;
                    CREATE TEMP TABLE tmp_table_included AS
                    SELECT DISTINCT oppr.ro_id
                    FROM ovrhl_publish_prg_rec oppr
                    JOIN ovrhl_publish_prg opp ON opp.id = oppr.publish_prg_id
                    WHERE EXISTS (SELECT FROM ovrhl_dpkr_document_prg_version oddpv
                                        WHERE oddpv.dpkr_document_id = :docId AND oddpv.prg_version_id = opp.version_id)
	                    AND NOT EXISTS (SELECT FROM ovrhl_dpkr_document_real_obj oddro
                                        WHERE NOT oddro.dpkr_document_id = :docId AND oddro.reality_object_id = oppr.ro_id AND NOT oddro.is_excluded);

                    DROP TABLE IF EXISTS tmp_table_program_ro;
                    CREATE TEMP TABLE tmp_table_program_ro AS
                    SELECT DISTINCT opv.mu_id, oppr.ro_id
                    FROM ovrhl_dpkr_document_prg_version oddpv
                    JOIN ovrhl_prg_version opv ON opv.id = oddpv.prg_version_id
                    JOIN ovrhl_publish_prg opp ON opp.version_id = opv.id
                    JOIN ovrhl_publish_prg_rec oppr ON oppr.publish_prg_id = opp.id
                    WHERE oddpv.dpkr_document_id = :docId;

                    CREATE INDEX ON tmp_table_program_ro (mu_id);
                    CREATE INDEX ON tmp_table_program_ro (ro_id);
                    ANALYZE tmp_table_program_ro;

                    DROP TABLE IF EXISTS tmp_table_excluded;
                    CREATE TEMP TABLE tmp_table_excluded AS
                    SELECT oddro.reality_object_id AS ro_id
                    FROM ovrhl_dpkr_document_real_obj oddro
                    JOIN gkh_reality_object gro ON gro.id = oddro.reality_object_id
                    WHERE NOT oddro.dpkr_document_id = :docId
	                    AND EXISTS (SELECT FROM tmp_table_program_ro t1 WHERE t1.mu_id = gro.municipality_id)
	                    AND NOT EXISTS (SELECT FROM tmp_table_program_ro t2 WHERE t2.ro_id = oddro.reality_object_id)
                    GROUP BY oddro.reality_object_id
                    HAVING FALSE = ALL(array_agg(oddro.is_excluded));

                    CREATE INDEX ON tmp_table_included (ro_id);
                    CREATE INDEX ON tmp_table_excluded (ro_id);
                    ANALYZE tmp_table_included;
                    ANALYZE tmp_table_excluded;

                    DELETE FROM ovrhl_dpkr_document_real_obj oddro
                        WHERE oddro.dpkr_document_id = :docId
                            AND NOT oddro.is_excluded
                            AND NOT EXISTS (SELECT FROM tmp_table_included t WHERE t.ro_id = oddro.reality_object_id);

                    DELETE FROM ovrhl_dpkr_document_real_obj oddro
                        WHERE oddro.dpkr_document_id = :docId
                            AND oddro.is_excluded
                            AND NOT EXISTS (SELECT FROM tmp_table_excluded t WHERE t.ro_id = oddro.reality_object_id);

                    WITH not_exists_recs AS (
	                    SELECT t.ro_id
	                    FROM tmp_table_included t
	                    WHERE NOT EXISTS (SELECT FROM ovrhl_dpkr_document_real_obj oddro WHERE oddro.dpkr_document_id = :docId AND NOT oddro.is_excluded AND oddro.reality_object_id = t.ro_id)
                    )
                    INSERT INTO ovrhl_dpkr_document_real_obj (object_version,object_create_date,object_edit_date,dpkr_document_id,reality_object_id,is_excluded)
                    SELECT 0, now(), now(), :docId, ner.ro_id, FALSE
                    FROM not_exists_recs ner;

                    WITH not_exists_recs AS (
	                    SELECT t.ro_id
	                    FROM tmp_table_excluded t
	                    WHERE NOT EXISTS (SELECT FROM ovrhl_dpkr_document_real_obj oddro WHERE oddro.dpkr_document_id = :docId AND oddro.is_excluded AND oddro.reality_object_id = t.ro_id)
                    )
                    INSERT INTO ovrhl_dpkr_document_real_obj (object_version,object_create_date,object_edit_date,dpkr_document_id,reality_object_id,is_excluded)
                    SELECT 0, now(), now(), :docId, ner.ro_id, TRUE
                    FROM not_exists_recs ner;

                    DROP TABLE IF EXISTS tmp_table_included;
                    DROP TABLE IF EXISTS tmp_table_excluded;
                    DROP TABLE IF EXISTS tmp_table_program_ro;
                ")
                .SetParameter("docId", dpkrDocumentId)
                .ExecuteUpdate();
            }
            catch (Exception e)
            {
                throw new Exception("При выполнении запроса для формирования перечня домов произошла ошибка");
            }

            return new BaseDataResult();
        }
    }
}