namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014061700
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014042500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {

            /*
               Поскольку в регионе Томск сущности ОпределениеПротокола и Постановление были расширены через subclass
               то тогда 
             */
            Database.AddTable("GJI_TOMSK_PROTOCOL_DEF",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("PLACE_REVIEW", DbType.String, 2000));
            Database.AddForeignKey("FK_GJI_TOMSK_PROTOCOL_DEF_ID", "GJI_TOMSK_PROTOCOL_DEF", "ID", "GJI_PROTOCOL_DEFINITION", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_TOMSK_PROTOCOL_DEF (id)
                                     select id from GJI_PROTOCOL_DEFINITION");

            Database.AddTable("GJI_TOMSK_RESOLUTION",
                new Column("ID", DbType.Int64, ColumnProperty.NotNull | ColumnProperty.Unique),
                new Column("PHYSICAL_GENDER", DbType.Int16, ColumnProperty.NotNull, 0));
            Database.AddForeignKey("FK_GJI_TOMSK_RESOLUTION_ID", "GJI_TOMSK_RESOLUTION", "ID", "GJI_RESOLUTION", "ID");

            Database.ExecuteNonQuery(@"insert into GJI_TOMSK_RESOLUTION (id)
                                     select id from GJI_RESOLUTION");

        }

        public override void Down()
        {
            Database.RemoveConstraint("GJI_TOMSK_PROTOCOL_DEF", "FK_GJI_TOMSK_PROTOCOL_DEF_ID");
            Database.RemoveTable("GJI_TOMSK_PROTOCOL_DEF");

            Database.RemoveConstraint("GJI_TOMSK_RESOLUTION", "FK_GJI_TOMSK_RESOLUTION_ID");
            Database.RemoveTable("GJI_TOMSK_RESOLUTION");
        }
    }
}