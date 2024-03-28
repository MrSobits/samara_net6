namespace Bars.GkhGji.Migrations.Version_2015110500
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2015110500
    /// </summary>
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015110500")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations._2015.Version_2015102700.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
            Database.AddColumn("GJI_ACT_TSJ_STATUTE", new Column("DOC_NUMBER", DbType.String, 50));
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
            Database.RemoveColumn("GJI_ACT_TSJ_STATUTE", "DOC_NUMBER");
        }
    }
}