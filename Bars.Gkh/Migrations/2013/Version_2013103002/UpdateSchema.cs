namespace Bars.Gkh.Migrations.Version_2013103002
{
    using global::Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013103002")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013103001.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_CONTRAGENT_MUNICIPALITY",
                new RefColumn("CONTRAGENT_ID", ColumnProperty.NotNull, "CONTR_MUN_CONTR", "GKH_CONTRAGENT", "ID"),
                new RefColumn("MUNICIPALITY_ID", ColumnProperty.NotNull, "CONTR_MUN_MUN", "GKH_DICT_MUNICIPALITY", "ID"));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_CONTRAGENT_MUNICIPALITY");
        }
    }
}