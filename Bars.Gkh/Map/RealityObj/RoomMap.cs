namespace Bars.Gkh.Map
{
    using Bars.Gkh.Entities;
    using System;

    
    /// <summary>Маппинг для "Помещение"</summary>
    public class RoomMap : BaseImportableEntityMap<Room>
    {
        
        public RoomMap(): 
                base("Помещение", "GKH_ROOM")
        {
        }

        protected override void Map()
        {
            this.Property(x => x.Description, "Описание").Column("CDESCRIPTION").Length(1000);
            this.Property(x => x.RoomNum, "Номер помещения").Column("CROOM_NUM").Length(100);
            this.Property(x => x.Area, "Площадь").Column("CAREA").NotNull();
            this.Property(x => x.LivingArea, "Площадь").Column("LAREA");
            this.Reference(x => x.RealityObject, "Дом").Column("RO_ID").NotNull().Fetch();
            this.Property(x => x.Type, "Тип помещения").Column("TYPE");
            this.Property(x => x.OwnershipType, "Тип собственности").Column("OWNERSHIP_TYPE");
            this.Property(x => x.RoomsCount, "Количество комнат").Column("ROOMS_COUNT");
            this.Property(x => x.EntranceNum, "Номер подъезда").Column("ENTRANCE_NUM");
            this.Property(x => x.Floor, "Этаж").Column("FLOOR");
            this.Property(x => x.CreatedFromPreviouisVersion, "Флаг: создано из предыдущих таблиц").Column("FROM_PREV").DefaultValue(false).NotNull();
            this.Property(x => x.IsRoomHasNoNumber, "Флаг: У помещения отсутствует номер").Column("HAS_NO_NUM").DefaultValue(false).NotNull();
            this.Property(x => x.Notation, "Примечание").Column("NOTATION").Length(300);
            this.Property(x => x.CadastralNumber, "Кадастровый номер").Column("CADASTRAL").Length(50);
            this.Property(x => x.IsRoomCommonPropertyInMcd, "Помещение составляет общее имущество в МКД").Column("COMMON_PROPERTY_IN_MCD").DefaultValue(false).NotNull();
            this.Reference(x => x.Entrance, "Подъезд").Column("ENTRANCE_ID").Fetch();
            this.Property(x => x.ChamberNum, "Номер комнаты").Column("CHAMBER_NUM").Length(100).DefaultValue(String.Empty);
            this.Property(x => x.HasSeparateEntrance, "Помещение имеет отдельный вход").Column("HAS_SEPARATE_ENTRANCE").NotNull();

            this.Property(x => x.IsCommunal, "Признак - является ли коммунальной квартирой").Column("IS_COMMUNAL");
            this.Property(x => x.CommunalArea, "Площадь общего имущества в коммунальной квартире").Column("COMMUNAL_AREA");
            this.Property(x => x.PrevAssignedRegNumber, "Ранее присвоенный гос. учетный номер").Column("PREV_ASS_REG_NUM");
            this.Property(x => x.RecognizedUnfit, "Наличие признания квартиры непригодной для проживания").Column("REC_UNFIT");
            this.Property(x => x.RecognizedUnfitReason, "Основание признания квартиры непригодной для проживания").Column("REC_UNFIT_REASON");
            this.Property(x => x.RecognizedUnfitDocNumber, "Номер документа").Column("REC_UNFIT_DOC_NUMBER");
            this.Property(x => x.RecognizedUnfitDocDate, "Дата документа").Column("REC_UNFIT_DOC_DATE");
            this.Property(x => x.GisGkhPremisesGUID, "ГИС ЖКХ PremisesGUID").Column("GIS_GKH_PREMISES_GUID");
            this.Reference(x => x.RecognizedUnfitDocFile, "Документ о признании квартиры непригодной для проживания").Column("REC_UNFIT_DOC_FILE_ID");
        }
    }
}