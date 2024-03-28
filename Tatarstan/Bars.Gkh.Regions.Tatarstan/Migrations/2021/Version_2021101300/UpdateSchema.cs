namespace Bars.Gkh.Regions.Tatarstan.Migrations._2021.Version_2021101300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Bars.B4.Utils;

    [Migration("2021101300")]
    [MigrationDependsOn(typeof(Version_2021040600.UpdateSchema))]
    [MigrationDependsOn(typeof(Bars.Gkh.Gis.Migrations._2021.Version_2021100100.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName pgmuAddressTable = new SchemaQualifiedObjectName()
        {
            Schema = "PUBLIC",
            Name = "PGMU_ADDRESSES"
        };
        
        private SchemaQualifiedObjectName fsspAddressTable = new SchemaQualifiedObjectName()
        {
            Schema = "PUBLIC",
            Name = "FSSP_ADDRESS"
        };

        private SchemaQualifiedObjectName litigationTable = new SchemaQualifiedObjectName()
        {
            Schema = "PUBLIC",
            Name = "FSSP_LITIGATION"
        };
        
        public override void Up()
        {
            // поправил максимально допустимые значения колонок
            new []
            {
                new Column("HOUSE", DbType.String, 10),
                new Column("BUILDING", DbType.String, 10),
                new Column("APARTMENT", DbType.String, 10),
                new Column("ROOM", DbType.String, 10)
            }.ForEach(x =>
            {
                this.Database.ChangeColumn(this.pgmuAddressTable.Name, x);
            });
            
            this.Database.AddPersistentObjectTable(this.fsspAddressTable.Name,
                new Column("ADDRESS", DbType.String, 100),
                new RefColumn("PGMU_ADDRESS_ID", "FSSP_PGMU_ADDRESS", this.pgmuAddressTable.Name, "ID"));
            
            this.Database.AddIndex("IND_ADDRESS", false, this.fsspAddressTable.Name, "ADDRESS");

            this.Database.AddPersistentObjectTable(this.litigationTable.Name,
                new Column("JUR_INSTITUTION", DbType.String, 255),
                new Column("STATE", DbType.String, 80),
                new Column("IND_ENTR_REGISTRATION_NUMBER", DbType.String, 40),
                new Column("DEBTOR", DbType.String, 100),
                new RefColumn("DEBTOR_FSSP_ADDRESS_ID", "DEBTOR_FSSP_ADDRESS", this.fsspAddressTable.Name, "ID"));
        }

        public override void Down()
        {
            this.Database.RemoveTable(this.litigationTable);
            this.Database.RemoveTable(this.fsspAddressTable);
        }
    }
}