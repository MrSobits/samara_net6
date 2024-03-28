using Bars.B4.DataAccess;
using Bars.B4.Modules.FileStorage;
using Bars.Gkh.Entities;
using Bars.Gkh.Enums;
using Sobits.GisGkh.Enums;
using System;

namespace Sobits.GisGkh.Entities
{
    /// <summary>
    /// Помещение ГИС ЖКХ
    /// </summary>
    public class GisGkhPremises : BaseEntity
    {
        /// <summary>
        /// Дом в системе
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Тип помещения
        /// </summary>
        public virtual RoomType RoomType { get; set; }

        /// <summary>
        /// Номер помещения
        /// </summary>
        public virtual string PremisesNum { get; set; }

        /// <summary>
        /// Этаж
        /// </summary>
        public virtual string Floor { get; set; }

        /// <summary>
        /// Общая площадь
        /// </summary>
        public virtual decimal? TotalArea { get; set; }

        /// <summary>
        /// Жилая площадь
        /// </summary>
        public virtual decimal? GrossArea { get; set; }

        /// <summary>
        /// Дата изменения
        /// </summary>
        public virtual DateTime ModificationDate { get; set; }

        /// <summary>
        /// Дата аннулирования
        /// </summary>
        public virtual DateTime? TerminationDate { get; set; }

        /// <summary>
        /// Является общим имуществом
        /// </summary>
        public virtual bool? IsCommonProperty { get; set; }

        /// <summary>
        /// Уникальный номер помещения
        /// </summary>
        public virtual string PremisesUniqueNumber { get; set; }

        /// <summary>
        /// GUID помещения
        /// </summary>
        public virtual string PremisesGUID { get; set; }

        /// <summary>
        /// Номер подъезда
        /// </summary>
        public virtual string EntranceNum { get; set; }

        /// <summary>
        /// Кадастровый номер
        /// </summary>
        public virtual string CadastralNumber { get; set; }

        /// <summary>
        /// Нет сведений о кадастровом номере
        /// </summary>
        public virtual bool? No_RSO_GKN_EGRP_Data { get; set; }

        /// <summary>
        /// Ключ связи с ГКН/ЕГРП отсутствует
        /// </summary>
        public virtual bool? No_RSO_GKN_EGRP_Registered { get; set; }

    }
}
