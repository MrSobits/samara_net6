namespace Sobits.RosReg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.RosReg.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Entities.RosRegExtractEgrnOwner"</summary>
    public class ExtractEgrnRightIndMap : PersistentObjectMap<ExtractEgrnRightInd>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExtractEgrnRightIndMap()
            : base("Sobits.RosReg.Entities", ExtractEgrnRightIndMap.TableName)
        {
        }

        public static string TableName => "ExtractEgrnRightInd";

        public static string SchemaName => "RosReg";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.Surname, "Фамилия").Column(nameof(ExtractEgrnRightInd.Surname).ToLower());
            this.Property(x => x.FirstName, "Имя").Column(nameof(ExtractEgrnRightInd.FirstName).ToLower());
            this.Property(x => x.Patronymic, "Отчество").Column(nameof(ExtractEgrnRightInd.Patronymic).ToLower());
            this.Property(x => x.BirthDate, "Дата рождения").Column(nameof(ExtractEgrnRightInd.BirthDate).ToLower());
            this.Property(x => x.BirthPlace, "Место рождения").Column(nameof(ExtractEgrnRightInd.BirthPlace).ToLower());
            this.Property(x => x.Snils, "СНИЛС").Column(nameof(ExtractEgrnRightInd.Snils).ToLower());
            this.Reference(x => x.RightId, "Право собственности").Column(nameof(ExtractEgrnRightInd.RightId).ToLower()).Fetch();
            this.Property(x => x.DocIndCode, "Код документа").Column(nameof(ExtractEgrnRightInd.DocIndCode).ToLower());
            this.Property(x => x.DocIndName, "Название документа").Column(nameof(ExtractEgrnRightInd.DocIndName).ToLower());
            this.Property(x => x.DocIndSerial, "Серия документа").Column(nameof(ExtractEgrnRightInd.DocIndSerial).ToLower());
            this.Property(x => x.DocIndNumber, "Номер документа").Column(nameof(ExtractEgrnRightInd.DocIndNumber).ToLower());
            this.Property(x => x.DocIndDate, "Дата документа").Column(nameof(ExtractEgrnRightInd.DocIndDate).ToLower());
            this.Property(x => x.DocIndIssue, "Организация, выдавшая документ").Column(nameof(ExtractEgrnRightInd.DocIndIssue).ToLower());

        }
    }

    // ReSharper disable once UnusedMember.Global
    public class ExtractEgrnRightIndNhMapping : ClassMapping<ExtractEgrnRightInd>
    {
        public ExtractEgrnRightIndNhMapping()
        {
            this.Schema(ExtractEgrnRightIndMap.SchemaName);
        }
    }
}