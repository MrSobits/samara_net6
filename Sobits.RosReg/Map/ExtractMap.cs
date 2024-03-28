namespace Sobits.RosReg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.RosReg.Entities;

    /// <summary>Маппинг для "Sobits.RosReg.Extract"</summary>
    public class ExtractMap : PersistentObjectMap<Extract>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExtractMap()
            : base("Sobits.RosReg.Entities", ExtractMap.TableName)
        {
        }

        public static string TableName => "Extract";

        public static string SchemaName => "RosReg";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.CreateDate, "Дата создания").Column(nameof(Extract.CreateDate).ToLower());
            this.Property(x => x.Type, "Тип").Column(nameof(Extract.Type).ToLower());

            this.Property(x => x.Xml, "Xml-файл").Column("xml");
            this.Property(x => x.IsParsed, "Выписка обработана?").Column(nameof(Extract.IsParsed).ToLower());
            this.Property(x => x.IsActive, "Выписки действительна?").Column(nameof(Extract.IsActive).ToLower());
            this.Property(x => x.File, "Файл выписки для печати").Column(nameof(Extract.File).ToLower());
            this.Property(x => x.Comment, "Коментарии").Column(nameof(Extract.Comment).ToLower());
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class ExtractNhMapping : ClassMapping<Extract>
    {
        public ExtractNhMapping()
        {
            this.Schema(ExtractMap.SchemaName);
        }
    }
}