namespace Bars.GkhGji.Regions.Nnovgorod.Migrations.Version_2014020600
{
    using System.Data;
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014020600")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Nnovgorod.Migrations.Version_1.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GJI_NNOV_INSP_VIOL_WORD",
                new Column("WORDING", DbType.String),
                new RefColumn("INSPECTION_VIOL_ID", ColumnProperty.NotNull, "GJI_NNOV_INSP_VIOL_WORD", "GJI_INSPECTION_VIOLATION", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GJI_NNOV_INSP_VIOL_WORD");
        }
    }
}