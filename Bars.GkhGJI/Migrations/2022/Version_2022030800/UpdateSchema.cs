namespace Bars.GkhGji.Migrations._2022.Version_2022030800
{
    using System.Data;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2022030800")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(Version_2022022100.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.AddColumn("GJI_APPEAL_CITIZENS", new Column("CORR_AGE", DbType.Int32, ColumnProperty.None));
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
	    public override void Down()
        {
            Database.RemoveColumn("GJI_APPEAL_CITIZENS", "CORR_AGE");
        }
    }
}