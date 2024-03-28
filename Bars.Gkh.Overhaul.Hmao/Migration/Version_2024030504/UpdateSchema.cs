namespace Bars.Gkh.Overhaul.Hmao.Migration.Version_2024030504
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030504")]
    [MigrationDependsOn(typeof(Version_2024030503.UpdateSchema))]
    // Является Version_2022071100 (Номер дублируется с существующей у нас, но содержание другое) из ядра
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName table = new SchemaQualifiedObjectName { Name = "OVRHL_VERSION_REC" };
        private readonly Column column = new Column("WORK_CODE", DbType.String);

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.table, this.column);

            this.Database.ExecuteNonQuery(@"
                WITH tmp AS (
					SELECT
						ovr.id,
						orse.ro_id,
						oceo.ceo_code,
						ROW_NUMBER() OVER (PARTITION BY orse.ro_id, oceo.id ORDER BY ovr.year, ovr.id) AS rnum
					FROM ovrhl_stage1_version osv
					JOIN ovrhl_stage2_version osv2 ON osv2.id = osv.stage2_version_id
					JOIN ovrhl_version_rec ovr ON ovr.id = osv2.st3_version_id
					JOIN ovrhl_prg_version opv ON opv.id = ovr.version_id AND opv.is_main
					JOIN ovrhl_ro_struct_el orse ON orse.id = osv.struct_el_id
					JOIN ovrhl_struct_el ose ON ose.id = orse.struct_el_id
					JOIN ovrhl_struct_el_group oseg ON oseg.id = ose.group_id
					JOIN ovrhl_common_estate_object oceo ON oceo.id = oseg.cmn_estate_obj_id
					GROUP BY orse.ro_id, oceo.id, ovr.id, osv2.id
				)
				UPDATE ovrhl_version_rec ovr SET work_code = t.ro_id || to_char(to_number(t.ceo_code, '000'), 'FM000') || to_char(t.rnum, 'FM00')
				FROM tmp t
				WHERE t.id = ovr.id");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.table, this.column.Name);
        }
    }
}