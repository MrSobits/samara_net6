namespace Bars.GkhCr.Migrations._2017.Version_2017062400
{
    using System;
    using System.Collections.Generic;
    using System.Data;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;

    using Column = Bars.B4.Modules.Ecm7.Framework.Column;

    [Migration("2017062400")]
    [MigrationDependsOn(typeof(Version_2017042400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            this.Database.AddEntityTable(
                "CR_DICT_TERMINATION_REASON",
                new Column("NAME", DbType.String, ColumnProperty.NotNull),
                new Column("CODE", DbType.String, ColumnProperty.NotNull));

            this.Database.AddColumn("CR_OBJ_BUILD_CONTRACT", "TERMINATION_DOCUMENT_NUMBER", DbType.String);
            this.Database.AddRefColumn("CR_OBJ_BUILD_CONTRACT", 
                new RefColumn("TERMINATION_REASON_ID", "CR_OBJ_BUILD_CONTRACT_TERMINATION_REASON", "CR_DICT_TERMINATION_REASON", "ID"));

            this.Database.AddColumn("CR_DICT_PROGRAM", "DOCUMENT_NUMBER", DbType.String);
            this.Database.AddColumn("CR_DICT_PROGRAM", "DOCUMENT_DATE", DbType.DateTime);
            this.Database.AddColumn("CR_DICT_PROGRAM", "DOCUMENT_DEPARTMENT", DbType.String);

            this.FillDict();
        }

        public override void Down()
        {
            this.Database.RemoveTable("CR_DICT_TERMINATION_REASON");

            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "TERMINATION_DOCUMENT_NUMBER");
            this.Database.RemoveColumn("CR_OBJ_BUILD_CONTRACT", "TERMINATION_REASON_ID");

            this.Database.RemoveColumn("CR_DICT_PROGRAM", "DOCUMENT_NUMBER");
            this.Database.RemoveColumn("CR_DICT_PROGRAM", "DOCUMENT_DATE");
            this.Database.RemoveColumn("CR_DICT_PROGRAM", "DOCUMENT_DEPARTMENT");
        }

        private void FillDict()
        {
            var reasons = new List<Tuple<string, string>>
            {
                Tuple.Create("1", "Существенное изменение обстоятельств по решению суда"),
                Tuple.Create("2", "Существенное изменение обстоятельств по иным причинам"),
                Tuple.Create("3", "Существенное нарушение договора одной из сторон"),
                Tuple.Create("4", "По взаимному согласию сторон"),
                Tuple.Create("5", "Истек срок действия договора"),
                Tuple.Create("6", "Аннулирование лицензии (только для УО)"),
                Tuple.Create("7", "Прекращение действия лицензии (только для УО)"),
                Tuple.Create("8", "По решению общего собрания собственников")
            };

            foreach (var reason in reasons)
            {
                this.InsertRecord(reason);
            }
        }

        private void InsertRecord(Tuple<string, string> record)
        {
            this.Database.ExecuteNonQuery(
                $@"insert into cr_dict_termination_reason
                    (object_version, object_create_date, object_edit_date, code, name)
                    values(0, now(), now(), '{record.Item1}', '{record.Item2}');"
                );
        }
    }
}