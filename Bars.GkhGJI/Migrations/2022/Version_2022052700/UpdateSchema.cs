namespace Bars.GkhGji.Migrations._2022.Version_2022052700
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022052700")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022052300.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable("GJI_DICT_TYPE_OF_FEEDBACK",
                 new Column("CODE", DbType.String, 25),
                 new Column("NAME", DbType.String, 500));

        }

        public override void Down()
        {
            this.Database.RemoveTable("GJI_DICT_TYPE_OF_FEEDBACK");
        }
    }
}