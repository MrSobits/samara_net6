namespace Bars.GkhGji
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.Gkh;

    public class GkhEdoIntegViewCollection : BaseGkhViewCollection
    {
        public override int Number => 1;

        public override List<string> GetDropAll(DbmsKind dbmsKind)
        {
            var deleteView = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("DeleteView")).ToList();

            var queries = new List<string>();

            foreach (var method in deleteView)
            {
                var str = (string)method.Invoke(this, new object[] { dbmsKind });
                if (!string.IsNullOrEmpty(str))
                {
                    queries.Add(str);
                }
            }

            var deleteFunc = this.GetType().GetMethods(BindingFlags.Instance | BindingFlags.NonPublic).Where(x => x.Name.Contains("DeleteFunction")).ToList();
            foreach (var method in deleteFunc)
            {
                var str = (string)method.Invoke(this, new object[] { dbmsKind });
                if (!string.IsNullOrEmpty(str))
                {
                    queries.Add(str);
                }
            }

            return queries;
        }

        public override List<string> GetCreateAll(DbmsKind dbmsKind)
        {
            var queries = new List<string>();
            queries.AddRange(base.GetCreateAll(dbmsKind));
            return queries;
        }

        #region Вьюхи
        #region Create

        /// <summary>
        /// Вьюха обращений граждан
        /// </summary>
        /// <param name="dbmsKind"></param>
        /// <returns></returns>
        private string CreateViewAppealCits(DbmsKind dbmsKind) =>
            @"
            CREATE OR REPLACE VIEW view_gji_appeal_cits_edo AS
            SELECT gac.id,
                   gac.document_number,
                   gac.gji_number,
                   gac.date_from,
                   gac.check_time,
                   gac.questions_count,
                   COUNT(DISTINCT garo.reality_object_id)                 AS count_ro,
                   (ARRAY_AGG(gdm.name))[1]                               AS municipality,
                   c.id                                                   AS contragent_id,
                   gac.state_id,
                   CASE
            	       WHEN edo.is_edo IS NULL
            		   THEN FALSE
            	       ELSE edo.is_edo
            	   END                                                    AS is_edo,
                   gac.executant_id,
                   gac.tester_id,
                   gac.surety_resolve_id,
                   gac.execute_date,
                   gac.zonainsp_id,
                   ARRAY_TO_STRING(ARRAY_AGG(DISTINCT gro.address), '; ') AS ro_adr,
                   gac.correspondent,
                   (ARRAY_AGG(gdm.id))[1]                                 AS municipality_id,
                   edo.address_edo,
                   COUNT(acs.id)                                          AS count_subject
            FROM gji_appeal_citizens gac
            	     LEFT JOIN gkh_managing_organization mo ON mo.id = gac.managing_org_id
            	     LEFT JOIN gkh_contragent c ON c.id = mo.contragent_id
            	     LEFT JOIN intgedo_appcits edo ON edo.appeal_cits_id = gac.id
            	     LEFT JOIN gji_appcit_statsubj acs ON acs.appcit_id = gac.id
            	     LEFT JOIN (gji_appcit_ro garo
            	        JOIN gkh_reality_object gro ON gro.id = garo.reality_object_id
            	        JOIN gkh_dict_municipality gdm ON gdm.id = gro.municipality_id)
            	            ON garo.appcit_id = gac.id
            GROUP BY gac.id, edo.id, c.id";

        #endregion Create
        #region Delete

        private string DeleteViewAppealCits(DbmsKind dbmsKind)
        {
            var viewName = "view_gji_appeal_cits_edo";
            if (dbmsKind == DbmsKind.Oracle)
            {
                return DropViewOracleQuery(viewName);
            }

            return DropViewPostgreQuery(viewName);
        }


        #endregion Delete
        #endregion Вьюхи
    }
}