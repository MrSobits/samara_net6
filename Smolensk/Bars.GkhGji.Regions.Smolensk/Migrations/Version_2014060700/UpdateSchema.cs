namespace Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014060700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014060700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Smolensk.Migrations.Version_2014060600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddTable("GJI_RESOLUTION_DEF_SMOL",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("DEF_RESULT", DbType.String, 2000),
                new Column("DESCRIPTION_SET", DbType.String, 2000));

            Database.AddForeignKey("FK_GJI_RESOLUTION_DEF_SMOL_ID", "GJI_RESOLUTION_DEF_SMOL", "ID", "GJI_RESOLUTION_DEFINITION", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_RESOLUTION_DEF_SMOL (id)
                                     select id from GJI_RESOLUTION_DEFINITION");

            // поскольку в предыдущих миграциях создавали такблицы Subclass но не сделали скрипты перенсоа записей в новую таблицу
            Database.ExecuteNonQuery(@"insert into GJI_PRESCR_CANCEL_SMOL (id)
                                     (select id from GJI_PRESCRIPTION_CANCEL where id not in (select id from GJI_PRESCR_CANCEL_SMOL))");

            Database.ExecuteNonQuery(@"insert into GJI_PROTOCOL_DEF_SMOL (id)
                                     (select id from GJI_PROTOCOL_DEFINITION where id not in (select id from GJI_PROTOCOL_DEF_SMOL))");

        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_RESOLUTION_DEF_SMOL", "FK_GJI_RESOLUTION_DEF_SMOL_ID");

            Database.RemoveTable("GJI_RESOLUTION_DEF_SMOL");
        }
    }
}