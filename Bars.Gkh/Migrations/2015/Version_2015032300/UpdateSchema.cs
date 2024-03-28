namespace Bars.Gkh.Migration.Version_2015032300
{
    using System.Data;
    using global::Bars.B4.Modules.Ecm7.Framework;

    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2015032300")]
    [global::Bars.B4.Modules.Ecm7.Framework.MigrationDependsOn(typeof(global::Bars.Gkh.Migration.Version_2015031600.UpdateSchema))]
    public class UpdateSchema : global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        /// <summary>
        /// Поле ADDRESS_CODE перемещается из сущности Room в RealityObject
        /// </summary>
        public override void Up()
        {
            Database.RemoveColumn("GKH_ROOM", "ADDRESS_CODE");
            Database.AddColumn("GKH_REALITY_OBJECT", new Column("ADDRESS_CODE", DbType.String, 50, ColumnProperty.Null));
        }

        public override void Down()
        {
            Database.AddColumn("GKH_ROOM", new Column("ADDRESS_CODE", DbType.String, 50, ColumnProperty.Null));
            Database.RemoveColumn("GKH_REALITY_OBJECT", "ADDRESS_CODE");
        }
    }
}