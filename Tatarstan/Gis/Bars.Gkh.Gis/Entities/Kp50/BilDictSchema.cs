using Bars.B4.DataAccess;

namespace Bars.Gkh.Gis.Entities.Kp50
{
    /// <summary>
    /// Справочник префиксов схем баз данных биллинга
    /// </summary>
    public class BilDictSchema : PersistentObject
    {
        /// <summary>
        /// Наименование префикса схемы, где находится дом
        /// </summary>
        public virtual string LocalSchemaPrefix { get; set; }


        /// <summary>
        /// Наименование префикса центральной схемы
        /// </summary>
        public virtual string CentralSchemaPrefix { get; set; }

        /// <summary>
        /// Строка соединения к базе данных
        /// </summary>
        public virtual string ConnectionString { get; set; }

        /// <summary>
        /// Описание схемы
        /// </summary>
        public virtual string Description { get; set; }


        /// <summary>
        /// Активность схемы (резервное поле)
        /// 1 - активно
        /// 0 - неактивно 
        /// </summary>
        public virtual int IsActive { get; set; }
        

        /// <summary>
        /// Код расчетного центра
        /// </summary>
        public virtual long ErcCode { get; set; }

        /// <summary>
        /// Наименование префикса схемы в системе отправителя данных 
        /// </summary>
        public virtual string SenderLocalSchemaPrefix { get; set; }


        /// <summary>
        /// Наименование префикса центральной схемы в системе отправителя данных 
        /// </summary>
        public virtual string SenderCentralSchemaPrefix { get; set; }

    }
}
