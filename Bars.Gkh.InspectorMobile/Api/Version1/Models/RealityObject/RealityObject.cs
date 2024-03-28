namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.RealityObject
{
    using System;
    using System.Collections.Generic;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.HeatingSeason;

    /// <summary>
    /// Полная модель объекта жилищного фонда
    /// </summary>
    public class RealityObject : BaseRealityObject
    {
        /// <summary>
        /// Тип дома
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// Состояние дома
        /// </summary>
        public string Condition { get; set; }
        
        /// <summary>
        /// Год постройки
        /// </summary>
        public int? BuildYear { get; set; }
        
        /// <summary>
        /// Общая площадь
        /// </summary>
        public decimal? Square { get; set; }
        
        /// <summary>
        /// Количество подъездов
        /// </summary>
        public int? Entrance { get; set; }
        
        /// <summary>
        /// Количество квартир
        /// </summary>
        public int? Room { get; set; }
        
        /// <summary>
        /// Количество нежилых помещений
        /// </summary>
        public int? Premises { get; set; }
        
        /// <summary>
        /// Количество проживающих
        /// </summary>
        public int? Reside { get; set; }
        
        /// <summary>
        /// Форма собственности
        /// </summary>
        public string TypeOwnerShip { get; set; }
        
        /// <summary>
        /// Площадь частной собственности
        /// </summary>
        public decimal? SquarePrivate { get; set; }
        
        /// <summary>
        /// Площадь муниципальной собственности
        /// </summary>
        public decimal? SquareMunicipal { get; set; }
        
        /// <summary>
        /// Площадь жилых помещений
        /// </summary>
        public decimal? SquareRoom { get; set; }
        
        /// <summary>
        /// Площадь нежилых помещений
        /// </summary>
        public decimal? SquarePremises { get; set; }
        
        /// <summary>
        /// Минимальная этажность
        /// </summary>
        public int? FloorMin { get; set; }
        
        /// <summary>
        /// Максимальная этажность
        /// </summary>
        public int? FloorMax { get; set; }
        
        /// <summary>
        /// Материал кровли
        /// </summary>
        public string RoofMaterial { get; set; }
        
        /// <summary>
        /// Материал стен
        /// </summary>
        public string WallMaterial { get; set; }

        /// <summary>
        /// Тип кровли
        /// </summary>
        public string RoofType { get; set; }

        /// <summary>
        /// Система отопления
        /// </summary>
        public string Heating { get; set; }
        
        /// <summary>
        /// Фото дома
        /// </summary>
        public long? Photo { get; set; }
        
        /// <summary>
        /// Год последнего капитального ремонта
        /// </summary>
        public int? RepairYear { get; set; }
        
        /// <summary>
        /// Способ формирования фонда капитального ремонта
        /// </summary>
        public string FormWay { get; set; }
        
        /// <summary>
        /// Дата принятия решения о способе формирования фонда КР
        /// </summary>
        public DateTime? ProtocolDate { get; set; }

        /// <summary>
        /// Отопительный сезон
        /// </summary>
        public IEnumerable<HeatingSeason> HeatingSeason { get; set; }

        /// <summary>
        /// Управление домом
        /// </summary>
        public IEnumerable<ControlHome> ControlHome { get; set; }
        
        /// <summary>
        /// Конструктивные элементы дома
        /// </summary>
        public IEnumerable<StructuralElements> StructuralElements { get; set; }

        /// <summary>
        /// Программы капитального ремонта
        /// </summary>
        public IEnumerable<OverhaulProgram> OverhaulPrograms { get; set; }
    }

    /// <summary>
    /// Базовая модель объекта жилищного фонда
    /// </summary>
    public class BaseRealityObject
    {
        /// <summary>
        /// Уникальный идентификатор дома
        /// </summary>
        public long Id { get; set; }
        
        /// <summary>
        /// Код дома ФИАС
        /// </summary>
        public string HouseGuid { get; set; }
        
        /// <summary>
        /// Адрес дома
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Наименование региона
        /// </summary>
        public string Region { get; set; }

        /// <summary>
        /// Наименование МО
        /// </summary>
        public string Municipality { get; set; }
    }
}