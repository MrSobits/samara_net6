namespace Bars.GkhGji.Regions.Tatarstan.Migration._2022.Version_2022113000
{
    using System;

    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Utils;

    [Migration("2022113000")]
    [MigrationDependsOn(typeof(Version_2022111400.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        private readonly Tuple<SchemaQualifiedObjectName, SchemaQualifiedObjectName>[] tableNameMatchArray = {
            new Tuple<SchemaQualifiedObjectName, SchemaQualifiedObjectName>(
                new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ACTIONISOLATED" },
                new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION"}
            ),
            new Tuple<SchemaQualifiedObjectName, SchemaQualifiedObjectName>(
                new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ACTIONISOLATED_ROBJECT" },
                new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ROBJECT" }
            ),
            new Tuple<SchemaQualifiedObjectName, SchemaQualifiedObjectName>(
                new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ACTIONISOLATED_VIOLATION" },
                new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_VIOLATION" }
            )
            ,
            new Tuple<SchemaQualifiedObjectName, SchemaQualifiedObjectName>(
                new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ACTIONISOLATED_ANNEX" },
                new SchemaQualifiedObjectName { Name = "GJI_MOTIVATED_PRESENTATION_ANNEX" }
            )
        };
        
        /// <inheritdoc />
        public override void Up()
        {
            tableNameMatchArray.ForEach(x => this.Database.RenameTable(x.Item1, x.Item2.Name));

            this.Database.ExecuteNonQuery(@"
                UPDATE B4_STATE
                SET type_id = 'gji_document_motivatedpresentation'
                WHERE type_id = 'gji_document_motivatedpresentation_actionisolated';

                UPDATE B4_STATE_TRANSFER
                SET type_id = 'gji_document_motivatedpresentation'
                WHERE type_id = 'gji_document_motivatedpresentation_actionisolated';
            ");
        }

        /// <inheritdoc />
        public override void Down()
        {
            tableNameMatchArray.ForEach(x => this.Database.RenameTable(x.Item2, x.Item1.Name));
            
            this.Database.ExecuteNonQuery(@"
                UPDATE B4_STATE
                SET type_id = 'gji_document_motivatedpresentation_actionisolated'
                WHERE type_id = 'gji_document_motivatedpresentation';

                UPDATE B4_STATE_TRANSFER
                SET type_id = 'gji_document_motivatedpresentation_actionisolated'
                WHERE type_id = 'gji_document_motivatedpresentation';
            ");
        }
    }
}