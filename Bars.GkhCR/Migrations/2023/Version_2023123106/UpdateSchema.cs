using Bars.B4.Modules.Ecm7.Framework;
using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
using Bars.Gkh;
using System.Data;

namespace Bars.GkhCr.Migrations._2023.Version_2023123106
{
    [Migration("2023123106")]
    [MigrationDependsOn(typeof(_2023.Version_2023123105.UpdateSchema))]
    // Является Version_2018092700 из ядра
    public class UpdateSchema : B4.Modules.Ecm7.Framework.Migration
    {
        public override void Up()
        {
            //ViewManager.Drop(this.Database, "GkhCr", "DeleteViewCrObject");
            //ViewManager.Create(this.Database, "GkhCr", "CreateViewCrObject");
        }
    }
}