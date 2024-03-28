namespace Bars.Gkh.Entities
{
    using System;

    using Bars.B4.Modules.FileStorage;
    using Bars.Gkh.Enums;

    /// <summary>
    /// Помещение
    /// </summary>
    public class Room : BaseImportableEntity
    {
        /// <inheritdoc />
        public Room()
        {
        }

        /// <inheritdoc />
        public Room(RealityObject realty)
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            this.RealityObject = realty;
        }

        /// <summary>
        /// Описание
        /// </summary>
        public virtual string Description { get; set; }

        /// <summary>
        /// Номер помещения
        /// </summary>
        public virtual string RoomNum { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal Area { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public virtual decimal? LivingArea { get; set; }

        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Тип помещения
        /// </summary>
        public virtual RoomType Type { get; set; }

        /// <summary>
        /// Тип собственности
        /// </summary>
        public virtual RoomOwnershipType OwnershipType { get; set; }

        /// <summary>
        /// Количество комнат
        /// </summary>
        public virtual int? RoomsCount { get; set; }

        /// <summary>
        /// Номер подъезда
        /// </summary>
        public virtual int? EntranceNum { get; set; }

        /// <summary>
        /// Подъезд
        /// </summary>
        public virtual Entrance Entrance { get; set; }

        /// <summary>
        /// Этаж
        /// </summary>
        public virtual int? Floor { get; set; }

        /// <summary>
        /// Флаг: создано из предыдущих таблиц
        /// </summary>
        public virtual bool CreatedFromPreviouisVersion { get; set; }

        /// <summary>
        /// Флаг: У помещения отсутствует номер
        /// </summary>
        public virtual bool IsRoomHasNoNumber { get; set; }

        /// <summary>
        /// Примечание
        /// </summary>
        public virtual string Notation { get; set; }

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastralNumber { get; set; }

        /// <summary>
        /// Флаг: Помещение составляет общее имущество в МКД
        /// </summary>
        public virtual bool IsRoomCommonPropertyInMcd { get; set; }

        /// <summary>
        /// Номер комнаты
        /// </summary>
        public virtual string ChamberNum { get; set; }

        /// <summary>
        /// Признак - является ли коммунальной квартирой
        /// </summary>
        public virtual bool IsCommunal { get; set; }

        /// <summary>
        /// Площадь общего имущества в коммунальной квартире
        /// </summary>
        public virtual decimal? CommunalArea { get; set; }

        /// <summary>
        /// Ранее присвоенный гос. учетный номер
        /// </summary>
        public virtual decimal? PrevAssignedRegNumber { get; set; }

        /// <summary>
        /// Наличие признания квартиры непригодной для проживания
        /// </summary>
        public virtual YesNo RecognizedUnfit { get; set; }

        /// <summary>
        /// Основание признания квартиры непригодной для проживания
        /// </summary>
        public virtual string RecognizedUnfitReason { get; set; }

        /// <summary>
        /// Номер документа
        /// </summary>
        public virtual int? RecognizedUnfitDocNumber { get; set; }

        /// <summary>
        /// Дата документа
        /// </summary>
        public virtual DateTime? RecognizedUnfitDocDate { get; set; }

        /// <summary>
        /// Документ о признании квартиры непригодной для проживания
        /// </summary>
        public virtual FileInfo RecognizedUnfitDocFile { get; set; }

        /// <summary>
        /// ГИС ЖКХ PremisesGUID
        /// </summary>
        public virtual string GisGkhPremisesGUID { get; set; }
        
        /// Помещение имеет отдельный вход
        /// </summary>
        public virtual bool HasSeparateEntrance { get; set; }
    }
}