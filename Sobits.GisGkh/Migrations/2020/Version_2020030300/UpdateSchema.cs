namespace Sobits.GisGkh.Migrations._2020.Version_2020030300
{
    using Bars.B4.Modules.Ecm7.Framework;
    using Bars.B4.Modules.NH.Migrations.DatabaseExtensions;
    using Sobits.GisGkh.Entities;
    using Sobits.GisGkh.Map;
    using System.Data;

    [Migration("2020030300")]
    [MigrationDependsOn(typeof(Version_2020012800.UpdateSchema))]
    public class UpdateSchema : Migration
    {
        public override void Up()
        {
            Database.AddEntityTable(
                  GisGkhDownloadsMap.TableName,
                  new Column(nameof(GisGkhDownloads.Guid).ToLower(), DbType.String, 36),
                  new Column(nameof(GisGkhDownloads.EntityT).ToLower(), DbType.String),
                  new Column(nameof(GisGkhDownloads.RecordId).ToLower(), DbType.Int32),
                  new Column(nameof(GisGkhDownloads.FileField).ToLower(), DbType.String),
                  new Column(nameof(GisGkhDownloads.orgPpaGuid).ToLower(), DbType.String, 36)
                  );

            Database.AddEntityTable(
                 AttachmentFieldMap.TableName,
                 new RefColumn(nameof(AttachmentField.NsiItem).ToLower(), $"{ AttachmentFieldMap.TableName }_{ NsiItemMap.TableName }_ID", $"{ NsiItemMap.TableName }", nameof(NsiItem.Id).ToLower()),
                 new Column(nameof(AttachmentField.Name).ToLower(), DbType.String),
                 new Column(nameof(AttachmentField.Description).ToLower(), DbType.String),
                 new RefColumn(nameof(AttachmentField.Attachment).ToLower(), $"{ AttachmentFieldMap.TableName }_FILE_ID", "B4_FILE_INFO", "ID"),
                 new Column(nameof(AttachmentField.Hash).ToLower(), DbType.String),
                 new Column(nameof(AttachmentField.Guid).ToLower(), DbType.String, 36)
                 );

            Database.AddRefColumn(
                NsiListMap.TableName,
                new RefColumn(nameof(NsiList.Contragent).ToLower(), $"{ NsiListMap.TableName }_CONTRAGENT_ID", "GKH_CONTRAGENT", "ID")
                );
        }

        public override void Down()
        {
            Database.RemoveColumn(NsiListMap.TableName, nameof(NsiList.Contragent).ToLower());
            Database.RemoveTable(AttachmentFieldMap.TableName);
            Database.RemoveTable(GisGkhDownloadsMap.TableName);
        }
    }
}