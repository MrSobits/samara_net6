namespace Bars.B4.Modules.ESIA.Auth.Migrations._2022.Version_2022060200
{
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    
    [Migration("2022060200")]
    [MigrationDependsOn(typeof(Version_2021062900.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private SchemaQualifiedObjectName EsiaOperatorTable =
            new SchemaQualifiedObjectName { Name = "ESIA_OPERATOR" };

        private Column IsActiveColumn = new Column("IS_ACTIVE", DbType.Boolean, ColumnProperty.NotNull, true);
        
        /// <inheritdoc />
        public override void Up()
        {
            this.Database.AddColumn(this.EsiaOperatorTable, this.IsActiveColumn);

            var sql = $"UPDATE {this.EsiaOperatorTable.Name} SET username = trim(concat_ws(' ', lastname, firstname, middlename));";
            this.Database.ExecuteNonQuery(sql);
        }

        /// <inheritdoc />
        public override void Down()
        {
            this.Database.RemoveColumn(this.EsiaOperatorTable, this.IsActiveColumn.Name);
        }
    }
}