namespace Bars.GkhGji.Migrations._2022.Version_2022110201
{
    using B4.Modules.Ecm7.Framework;
    using B4.Modules.NH.Migrations.DatabaseExtensions;
    using Gkh;
    using System.Data;

    [Migration("2022110201")]
    [MigrationDependsOn(typeof(Version_2022110200.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// <summary>
        public override void Up()
        {    
            this.Database.AddColumn("GJI_APPCIT_DECISION", new Column("APELLANT_PLACE_WORK", DbType.String, 500));
        }

        public override void Down()
        {
            this.Database.RemoveColumn("GJI_APPCIT_DECISION", "APELLANT_PLACE_WORK");
        }
    }
}