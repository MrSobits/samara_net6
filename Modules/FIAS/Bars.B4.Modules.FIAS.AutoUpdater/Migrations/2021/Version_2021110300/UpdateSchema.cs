namespace Bars.B4.Modules.FIAS.AutoUpdater.Migrations._2021.Version_2021110300
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;

    [Migration("2021110300")]
    public class UpdateSchema : Migration
    {
        private readonly SchemaQualifiedObjectName fiasTable = new SchemaQualifiedObjectName() { Name = "b4_fias" };
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.ChangeColumn(this.fiasTable, new Column("formalname", DbType.String.WithSize(500)));
            this.Database.ChangeColumn(this.fiasTable, new Column("offname", DbType.String.WithSize(500)));
        }
        
        /// <inheritdoc />
        public override void Down()
        {
            this.Database.ChangeColumn(this.fiasTable, new Column("formalname", DbType.String.WithSize(120)));
            this.Database.ChangeColumn(this.fiasTable, new Column("offname", DbType.String.WithSize(120)));
        }
    }
}