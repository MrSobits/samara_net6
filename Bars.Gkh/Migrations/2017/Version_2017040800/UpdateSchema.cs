namespace Bars.Gkh.Migrations._2017.Version_2017040800
{
    using Bars.B4.Modules.Ecm7.Framework;

    /// <inheritdoc />
    [Migration("2017040800")]
    [MigrationDependsOn(typeof(Version_2017021700.UpdateSchema))]
    [MigrationDependsOn(typeof(Version_2017032800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private const string Query = @"update gkh_room set CHAMBER_NUM = '' where CHAMBER_NUM is null;";

        /// <inheritdoc />
        public override void Up()
        {
            var oldTimeout = this.Database.CommandTimeout;
            this.Database.CommandTimeout = 2 * 60 * 60;
            this.Database.ExecuteNonQuery(UpdateSchema.Query);
            this.Database.CommandTimeout = oldTimeout;
        }
    }
}
