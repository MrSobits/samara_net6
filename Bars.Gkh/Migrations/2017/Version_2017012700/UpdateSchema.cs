namespace Bars.Gkh.Migrations._2017.Version_2017012700
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    /// <summary>
    /// Миграция 2017012700
    /// </summary>
    [Migration("2017012700")]
    [MigrationDependsOn(typeof(Bars.Gkh.Migrations._2017.Version_2017012502.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string Query = "update GKH_DICT_MAN_CONTRACT_SERVICE set name='';";

        /// <inheritdoc />
        public override void Up()
        {
            this.Database.RemoveColumn("GKH_DICT_ADD_CONTRACT_SERVICE", "NAME");
            this.Database.RemoveColumn("GKH_DICT_COM_CONTRACT_SERVICE", "NAME");
            this.Database.RemoveColumn("GKH_DICT_AGR_CONTRACT_SERVICE", "NAME");

            this.Database.AddColumn("GKH_DICT_MAN_CONTRACT_SERVICE", "NAME", DbType.String, 255, ColumnProperty.Null);
            this.Database.ExecuteNonQuery(UpdateSchema.Query);
            this.Database.ChangeColumnNotNullable("GKH_DICT_MAN_CONTRACT_SERVICE", "NAME", true); 
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn("GKH_DICT_MAN_CONTRACT_SERVICE", "NAME");
        }
    }
}
