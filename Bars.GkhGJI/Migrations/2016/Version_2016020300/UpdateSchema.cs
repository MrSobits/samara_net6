namespace Bars.GkhGji.Migrations.Version_2016020300
{
    using System.Data;

    using Bars.Gkh;

    using global::Bars.B4.Modules.Ecm7.Framework;

	/// <summary>
	/// Миграция 2016020300
	/// </summary>
	[global::Bars.B4.Modules.Ecm7.Framework.Migration("2016020300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.GkhGji.Migrations.Version_2016011900.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Накатить
        /// </summary>
        public override void Up()
        {
        }

        /// <summary>
        /// Откатить
        /// </summary>
        public override void Down()
        {
        }
    }
}