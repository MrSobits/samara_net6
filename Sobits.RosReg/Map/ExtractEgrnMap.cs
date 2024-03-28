namespace Sobits.RosReg.Map
{
    using Bars.B4.Modules.Mapping.Mappers;

    using NHibernate.Mapping.ByCode.Conformist;

    using Sobits.RosReg.Entities;

    /// <summary>Маппинг для "Sobits.RosReg.ExtractEgrn"</summary>
    public class ExtractEgrnMap : PersistentObjectMap<ExtractEgrn>
    {
        /// <summary>
        /// .ctor
        /// </summary>
        public ExtractEgrnMap()
            : base("Sobits.RosReg.Entities", ExtractEgrnMap.TableName)
        {
        }

        public static string TableName => "ExtractEgrn";

        public static string SchemaName => "RosReg";

        /// <summary>
        /// Мап
        /// </summary>
        protected override void Map()
        {
            this.Property(x => x.CadastralNumber, "Кадастровый номер").Column(nameof(ExtractEgrn.CadastralNumber).ToLower());
            this.Property(x => x.Address, "Адрес").Column(nameof(ExtractEgrn.Address).ToLower());
            this.Property(x => x.FullAddress, "Адрес помещения").Column(nameof(ExtractEgrn.FullAddress).ToLower());
            this.Property(x => x.Area, "Площадь").Column(nameof(ExtractEgrn.Area).ToLower());
            this.Property(x => x.Type, "Тип помещения").Column(nameof(ExtractEgrn.Type).ToLower());
            this.Property(x => x.Purpose, "Назначение помещения").Column(nameof(ExtractEgrn.Purpose).ToLower());
            this.Property(x => x.IsMerged, "Сопоставлена").Column(nameof(ExtractEgrn.IsMerged).ToLower());
            this.Property(x => x.ExtractNumber, "Номер выписки").Column("ExtractNumber".ToLower());
            this.Property(x => x.ExtractDate, "Дата формирования выписки").Column(nameof(ExtractEgrn.ExtractDate).ToLower());
            this.Reference(x => x.RoomId, "Комната").Column("ROOM_ID");
            this.Property(x => x.Room_id, "Комната").Column(nameof(ExtractEgrn.RoomId).ToLower());
            this.Reference(x => x.ExtractId, "Выписка").Column(nameof(ExtractEgrn.ExtractId).ToLower()).Fetch();
        }
    }

    // ReSharper disable once UnusedMember.Global
    public class ExtractEgrnNhMapping : ClassMapping<ExtractEgrn>
    {
        public ExtractEgrnNhMapping()
        {
            this.Schema(ExtractEgrnMap.SchemaName);
        }
    }
}