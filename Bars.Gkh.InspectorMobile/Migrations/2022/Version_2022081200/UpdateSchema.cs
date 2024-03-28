namespace Bars.Gkh.InspectorMobile.Migrations._2022.Version_2022081200
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    [Migration("2022081200")]
    public class UpdateSchema : Migration
    {
        private const string Table = "MP_ROLE";

        public override void Up()
        {
            this.Database.AddEntityTable(Table, new RefColumn("ROLE_ID", "FK_MP_ROLE_B4_ROLE", "B4_ROLE", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable(Table);
        }
    }
}
