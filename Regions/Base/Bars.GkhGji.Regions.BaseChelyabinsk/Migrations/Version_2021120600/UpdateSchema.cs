namespace Bars.GkhGji.Regions.BaseChelyabinsk.Migrations.Version_2021120600
{
    using B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using System.Data;

    [Migration("2021120600")]
    [MigrationDependsOn(typeof(Version_2021120400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_PROTOCOL197_ANNEX", new Column("MESSAGE_CHECK", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.MessageCheck.NotSet));
            Database.AddColumn("GJI_PROTOCOL197_ANNEX", new Column("TYPE_ANNEX", DbType.Int16, ColumnProperty.NotNull, (int)Bars.GkhGji.Enums.TypeAnnex.Protocol));

        }
        public override void Down()
        {
            Database.RemoveColumn("GJI_PROTOCOL197_ANNEX", "TYPE_ANNEX");
            Database.RemoveColumn("GJI_PROTOCOL197_ANNEX", "MESSAGE_CHECK");          

        }

    }
}