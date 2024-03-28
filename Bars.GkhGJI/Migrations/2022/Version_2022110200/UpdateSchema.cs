namespace Bars.GkhGji.Migrations._2022.Version_2022110200
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022110200")]
    [MigrationDependsOn(typeof(Version_2022110100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// <summary>
        public override void Up()
        {    
            this.Database.AddRefColumn("GJI_PROTOCOL_DEFINITION", new RefColumn("SIGNER_ID", ColumnProperty.None, "GJI_PROTOCOL_DEFINITION_SIGNER", "GKH_DICT_INSPECTOR", "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_PROTOCOL_DEFINITION", "SIGNER_ID");
  

        }
    }
}