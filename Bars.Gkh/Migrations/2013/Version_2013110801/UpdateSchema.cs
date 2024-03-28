namespace Bars.Gkh.Migrations.Version_2013110801
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2013110801")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migrations.Version_2013110800.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GKH_REALITY_OBJECT", "HAS_PRIV_FLATS", DbType.Boolean, ColumnProperty.NotNull, false);

            if (Database.DatabaseKind == DbmsKind.Oracle)
            {
                Database.ExecuteNonQuery(@"update GKH_REALITY_OBJECT set HAS_PRIV_FLATS = 1 where PRIV_DATE_FAPARTMENT is not null");
            }

            if (Database.DatabaseKind == DbmsKind.PostgreSql)
            {
                Database.ExecuteNonQuery(@"update GKH_REALITY_OBJECT set HAS_PRIV_FLATS = TRUE where PRIV_DATE_FAPARTMENT is not null");
            }
        }

        public override void Down()
        {
            Database.RemoveColumn("GKH_REALITY_OBJECT", "HAS_PRIV_FLATS");
        }
    }
}