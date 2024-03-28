namespace Bars.GkhGji.Migrations._2016.Version_2016101300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using System.Data;

    /// <summary>
    /// Миграция 2016101300
    /// </summary>
    [Migration("2016101300")]
    [MigrationDependsOn(typeof(Version_2016092301.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        /// <summary>
        /// Накатить миграцию
        /// </summary>
	    public override void Up()
	    {
	        if (!this.Database.ColumnExists("GJI_DICT_PLANJURPERSON", "URI_REGISTRATION_NUMBER"))
	        {
                this.Database.AddColumn("GJI_DICT_PLANJURPERSON", new Column("URI_REGISTRATION_NUMBER", DbType.Int32));
	        }

            if (!this.Database.ColumnExists("GJI_INSPECTION_JURPERSON", "URI_REGISTRATION_NUMBER"))
            {
                this.Database.AddColumn("GJI_INSPECTION_JURPERSON", new Column("URI_REGISTRATION_NUMBER", DbType.Int32));
            }

            if (!this.Database.ColumnExists("GJI_INSPECTION_JURPERSON", "URI_REGISTRATION_DATE"))
            {
                this.Database.AddColumn("GJI_INSPECTION_JURPERSON", new Column("URI_REGISTRATION_DATE", DbType.DateTime));
            }
        }

        /// <summary>
        /// Откатить миграцию
        /// </summary>
	    public override void Down()
        {
	        if (this.Database.ColumnExists("GJI_DICT_PLANJURPERSON", "URI_REGISTRATION_NUMBER"))
	        {
	            this.Database.RemoveColumn("GJI_DICT_PLANJURPERSON", "URI_REGISTRATION_NUMBER");
	        }

            if (this.Database.ColumnExists("GJI_INSPECTION_JURPERSON", "URI_REGISTRATION_NUMBER"))
            {
                this.Database.RemoveColumn("GJI_INSPECTION_JURPERSON", "URI_REGISTRATION_NUMBER");
            }

            if (this.Database.ColumnExists("GJI_INSPECTION_JURPERSON", "URI_REGISTRATION_DATE"))
            {
                this.Database.RemoveColumn("GJI_INSPECTION_JURPERSON", "URI_REGISTRATION_DATE");
            }
        }
    }
}