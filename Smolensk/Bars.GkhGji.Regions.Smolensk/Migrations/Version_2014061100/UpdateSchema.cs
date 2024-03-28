namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014061100
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061100")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014060700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_PROTOCOL_SMOL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("NOTICE_NUMBER", DbType.String, 100),
                new Column("NOTICE_DATE", DbType.Date));

            Database.AddForeignKey("FK_GJI_PROTOCOL_SMOL_ID", "GJI_PROTOCOL_SMOL", "ID", "GJI_PROTOCOL", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_PROTOCOL_SMOL (id)
                                     select id from GJI_PROTOCOL");

            Database.AddColumn("GJI_PROTOCOL_DEF_SMOL", "EXTEND_UNTIL", DbType.Date);
        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_PROTOCOL_SMOL", "FK_GJI_PROTOCOL_SMOL_ID");
            Database.RemoveTable("GJI_PROTOCOL_SMOL");

            Database.RemoveColumn("GJI_PROTOCOL_DEF_SMOL", "EXTEND_UNTIL");
        }
    }
}