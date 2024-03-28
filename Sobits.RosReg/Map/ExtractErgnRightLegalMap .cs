namespace Sobits.RosReg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.RosReg.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractEgrnOwner"</summary>
    public class ExtractEgrnRightLegalMap : PersistentObjectMap<ExtractEgrnRightLegal>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExtractEgrnRightLegalMap()
            : base("Sobits.RosReg.Entities", ExtractEgrnRightLegalMap.TableName)
        {
        }

        public static string TableName => "ExtractEgrnRightLegal";

        public static string SchemaName => "RosReg";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.OwnerType, "Резидент РФ или нет").Column(nameof(ExtractEgrnRightLegal.OwnerType).ToLower());
            this.Property(x => x.IncorporationForm, "Правовая форма").Column(nameof(ExtractEgrnRightLegal.IncorporationForm).ToLower());
            this.Property(x => x.Name, "Наименование").Column(nameof(ExtractEgrnRightLegal.Name).ToLower());
            this.Property(x => x.Inn, "Inn").Column(nameof(ExtractEgrnRightLegal.Inn).ToLower());
            this.Property(x => x.Email, "E-mail").Column(nameof(ExtractEgrnRightLegal.Email).ToLower());
            this.Property(x => x.MailingAddress, "Почтовый адрес").Column(nameof(ExtractEgrnRightLegal.MailingAddress).ToLower());
            this.Reference(x => x.RightId, "Право собственности").Column(nameof(ExtractEgrnRightLegal.RightId).ToLower()).Fetch();
        }
    }

    //ReSharper disable once UnusedMember.Global
    public class ExtractEgrnRightLegalNhMapping : ClassMapping<ExtractEgrnRightLegal>
    {
        public ExtractEgrnRightLegalNhMapping()
        {
            this.Schema(ExtractEgrnRightLegalMap.SchemaName);
        }
    }
}