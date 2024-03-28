namespace Bars.GkhGji.Migrations._2024.Version_2024030113
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2024030113")]
    [MigrationDependsOn(typeof(Version_2024030112.UpdateSchema))]
    /// Является Version_2020032300 из ядра
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("GJI_DOCUMENT_INSPECTOR",
                new Column("ERP_GUID", DbType.String.WithSize(36), ColumnProperty.Null));

            if (this.Database.TableExists(new SchemaQualifiedObjectName { Name = "gji_tat_disposal" }))
            {
                this.Database.ExecuteNonQuery(@"
                drop table if exists tmp1;
                create temp table tmp1 as
                select gdi.id
                from gji_document_inspector gdi
                    join gji_tat_disposal disposal on gdi.document_id = disposal.id
                    join gji_document doc on disposal.id = doc.id
                    join gkh_dict_inspector inspector on gdi.inspector_id = inspector.id
                where disposal.registration_number_erp notnull
                    and disposal.registration_number_erp != ''
                    and inspector.erp_guid notnull
                union
                select gdi.id
                from gji_document_inspector gdi
                    join gji_document_children children_doc on gdi.document_id = children_doc.children_id
                    join gji_tat_disposal disposal on disposal.id = children_doc.parent_id
                    join gkh_dict_inspector inspector on gdi.inspector_id = inspector.id
                    join gji_document doc on children_doc.children_id = doc.id
                where disposal.registration_number_erp notnull
                  and disposal.registration_number_erp != ''
                  and doc.type_document = 20
                  and inspector.erp_guid notnull;

                update gji_document_inspector gdi
                set erp_guid =
                    (select erp_guid
                    from gkh_dict_inspector di
                    where di.id = gdi.inspector_id)
                where gdi.id in (select id from tmp1);");
            }

            this.Database.RemoveColumn("GKH_DICT_INSPECTOR", "ERP_GUID");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_DOCUMENT_INSPECTOR", "ERP_GUID");

            this.Database.AddColumn("GKH_DICT_INSPECTOR",
                new Column("ERP_GUID", DbType.String.WithSize(36), ColumnProperty.Null));
        }
    }
}