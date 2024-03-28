using global::Bars.B4.Modules.Ecm7.Framework;

namespace Bars.Gkh.Integration.Embir.Migration.Version_2014072300
{
    [global::Bars.B4.Modules.Ecm7.Framework.Migration("2014072300")]
    public class UpdateSchema: global::Bars.B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            Database.ExecuteNonQuery("update gkh_dict_wall_material set external_id = ''");
        }

        public override void Down()
        {
        }
    }
}