namespace Bars.Gkh.Migrations._2021.Version_2021051100
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2021051100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2021042800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GKH_REALITY_HOUSEKEEPER",
                new Column("FIO", DbType.String, ColumnProperty.None),
                new Column("IS_ACTIVE", DbType.Int32, 4, ColumnProperty.NotNull, 10),
                new Column("LOGIN", DbType.String, ColumnProperty.NotNull),
                new Column("PASSWORD", DbType.String, ColumnProperty.None),             
                new RefColumn("RO_ID", "HOUSEKEEPER_REAL_OBJECT", "GKH_REALITY_OBJECT", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable("GKH_REALITY_HOUSEKEEPER");
        }
    }
}