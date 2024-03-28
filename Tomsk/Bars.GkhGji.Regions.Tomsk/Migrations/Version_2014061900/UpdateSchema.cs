namespace Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014061900
{
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014061900")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Regions.Tomsk.Migrations.Version_2014061700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            /*
             Был небольшой косяк с замено сущности доабвлялись но в таблицу расширенную не попадали
             */
            Database.ExecuteNonQuery(@"insert into GJI_TOMSK_RESOLUTION (id)
                                     select id from GJI_RESOLUTION where id not in (select id from GJI_TOMSK_RESOLUTION)");

            Database.ExecuteNonQuery(@"insert into GJI_TOMSK_PROTOCOL_DEF (id)
                                     select id from GJI_PROTOCOL_DEFINITION where id not in (select id from GJI_TOMSK_PROTOCOL_DEF)");

        }

        public override void Down()
        {
            
        }
    }
}