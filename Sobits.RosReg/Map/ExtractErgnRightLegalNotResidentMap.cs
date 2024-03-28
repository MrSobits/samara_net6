namespace Sobits.RosReg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.RosReg.Entities;

    /// <summary>Маппинг для "Bars.Gkh.Entities.ExtractEgrnRightLegalResident"</summary>
    public class ExtractEgrnRightLegalNotResidentMap : JoinedSubClassMap<ExtractEgrnRightLegalNotResident>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExtractEgrnRightLegalNotResidentMap()
            : base("Sobits.RosReg.Entities", ExtractEgrnRightLegalNotResidentMap.TableName)
        {
        }

        public static string TableName => "ExtractEgrnRightLegalNotResident";

        public static string SchemaName => "RosReg";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.IncorporationCountry, "Страна региcтстрации").Column(nameof(ExtractEgrnRightLegalNotResident.IncorporationCountry).ToLower());
            this.Property(x => x.RegistrationNumber, "Регистрационный номер").Column(nameof(ExtractEgrnRightLegalNotResident.RegistrationNumber).ToLower());
            this.Property(x => x.DateStateReg, "Дата государственной регистрации").Column(nameof(ExtractEgrnRightLegalNotResident.DateStateReg).ToLower());
            this.Property(x => x.RegistrationOrgan, "Наименование регистрирующего органа").Column(nameof(ExtractEgrnRightLegalNotResident.RegistrationOrgan).ToLower());
            this.Property(x => x.RegAddresSubject, "Адрес местонахождения").Column(nameof(ExtractEgrnRightLegalNotResident.RegAddresSubject).ToLower());

        }
        
        public class ExtractEgrnRightLegalNotResidentNhMapping : JoinedSubclassMapping<ExtractEgrnRightLegalNotResident>
        {
            public ExtractEgrnRightLegalNotResidentNhMapping()
            {
                this.Schema(ExtractEgrnRightLegalNotResidentMap.SchemaName);
            }
        }
    }
}