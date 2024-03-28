namespace Bars.GkhGji.Migrations.Version_2015111300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015111300
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015111300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2015110500.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            if (!Database.ColumnExists("GJI_DICT_PLANINSCHECK", "DATE_APPROVAL"))
            {
                Database.AddColumn("GJI_DICT_PLANINSCHECK", new Column("DATE_APPROVAL", DbType.DateTime));
            }
            if (!Database.ColumnExists("GJI_INSPECTION_INSCHECK", "COUNT_DAYS"))
            {
                Database.AddColumn("GJI_INSPECTION_INSCHECK", new Column("COUNT_DAYS", DbType.Int32));
            }
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GJI_DICT_PLANINSCHECK", "DATE_APPROVAL");
            Database.RemoveColumn("GJI_INSPECTION_INSCHECK", "COUNT_DAYS");
        }
    }
}