namespace Bars.GkhGji.Regions.Chelyabinsk.Migrations.Version_2017110600
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2017110600")]
    [MigrationDependsOn(typeof(Version_2017110500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            this.Database.AddColumn("GJI_CH_SMEV_EGRIP", new Column("INN_OGRN", DbType.Int32, 4, ColumnProperty.None, 10));
            this.Database.AddColumn("GJI_CH_SMEV_EGRUL", new Column("INN_OGRN", DbType.Int32, 4, ColumnProperty.None, 10));
        }
        public override void Down()
        {
            this.Database.RemoveColumn("GJI_CH_SMEV_EGRUL", "INN_OGRN");
            this.Database.RemoveColumn("GJI_CH_SMEV_EGRIP", "INN_OGRN");          
        }
    }
}