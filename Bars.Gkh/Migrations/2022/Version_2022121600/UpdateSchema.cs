namespace Bars.Gkh.Migrations._2022.Version_2022121600
{
    using System.Data;
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.Gkh.Utils;

    [Migration("2022121600")]
    
    [MigrationDependsOn(typeof(Version_2022102800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable("GKH_TRANSLATE_ENTITY_HISTORY_FIELD",
                new Column("ENTITY_TYPE", DbType.String),
                new Column("ENGLISH_NAME", DbType.String),
                new Column("RUSSIAN_NAME", DbType.String));
        }

        public override void Down()
        {
            Database.RemoveTable("GKH_TRANSLATE_ENTITY_HISTORY_FIELD");
        }
    }
}