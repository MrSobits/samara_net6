namespace Bars.Gkh.Overhaul.Migration.Version_2017011100
{
    using System.Data;

    using global::Bars.B4.Modules.Ecm7.Framework;

    /// <summary>
    /// Миграция 2017011100
    /// </summary>
    [Migration("2017011100")]
    [MigrationDependsOn(typeof(global::Bars.Gkh.Overhaul.Migration.Version_2016051000.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn("OVRHL_RO_STRUCT_EL", new Column("SYSTEM_TYPE", DbType.Int32, ColumnProperty.None, 0));
            this.Database.AddColumn("OVRHL_RO_STRUCT_EL", new Column("NETWORK_LENGTH", DbType.String, 50));
            this.Database.AddColumn("OVRHL_RO_STRUCT_EL", new Column("NETWORK_POWER", DbType.String, 50));
            this.Database.ExecuteNonQuery("UPDATE OVRHL_RO_STRUCT_EL SET SYSTEM_TYPE = 0;");
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "SYSTEM_TYPE");
            this.Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "NETWORK_LENGTH");
            this.Database.RemoveColumn("OVRHL_RO_STRUCT_EL", "NETWORK_POWER");
        }
    }
}
