namespace Bars.Gkh.Migrations._2022.Version_2022101200
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022101200")]
    
    [MigrationDependsOn(typeof(_2022.Version_2022090500.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_MANORG_LIC_EXTENSION",
                    new RefColumn("LIC_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_EXTENSION_L", "GKH_MANORG_LICENSE", "ID"),
                    new RefColumn("FILE_ID", ColumnProperty.NotNull, "GKH_MANORG_LIC_EXTENSION_F", "B4_FILE_INFO", "ID"),
                    new Column("DOC_TYPE", DbType.Int16, 4, ColumnProperty.NotNull, 10),
                    new Column("DOC_NUMBER", DbType.String, 100),
                    new Column("DOC_DATE", DbType.DateTime));
            Database.AlterColumnSetNullable("GKH_MANORG_LIC_EXTENSION", "FILE_ID", true);
            Database.AddColumn("GKH_MANORG_LICENSE", new Column("DATE_VALIDITY", DbType.DateTime));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_MANORG_LIC_EXTENSION");
            Database.RemoveColumn("GKH_MANORG_LICENSE", "DATE_VALIDITY");
        }
    }
}