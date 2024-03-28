namespace Bars.Gkh.Entities
{
    using Bars.Gkh.Entities;

    /// <summary>
    /// Информация по лифтам дома
    /// </summary>
    public class RealityObjectLiftSum : BaseImportableEntity
    {
        /// <summary>
        /// Дом
        /// </summary>
        public virtual RealityObject RealityObject { get; set; }

        /// <summary>
        /// Общее количество (не хранимое)
        /// </summary>
        public virtual int? MainCount { get; set; }

        /// <summary>
        /// Количество пассажирских (не хранимое)
        /// </summary>
        public virtual int? MainPassenger { get; set; }

        /// <summary>
        /// Количество грузовых (не хранимое)
        /// </summary>
        public virtual int? MainCargo { get; set; }

        /// <summary>
        /// Количество грузопассажирских (не хранимое)
        /// </summary>
        public virtual int? MainMixed { get; set; }

        /// <summary>
        /// Лифты навесные МЖИ
        /// </summary>
        public virtual int? Hinged { get; set; }

        /// <summary>
        /// Автоматы опускания
        /// </summary>
        public virtual int? Lowerings { get; set; }

        /// <summary>
        /// Общее количество МЖИ
        /// </summary>
        public virtual int? MjiCount { get; set; }

        /// <summary>
        /// Количество пассажирских МЖИ
        /// </summary>
        public virtual int? MjiPassenger { get; set; }

        /// <summary>
        /// Количество грузовых МЖИ
        /// </summary>
        public virtual int? MjiCargo { get; set; }

        /// <summary>
        /// Количество грузопассажирских МЖИ
        /// </summary>
        public virtual int? MjiMixed { get; set; }

        /// <summary>
        /// Подъёмные устройства
        /// </summary>
        public virtual int? Risers { get; set; }

        /// <summary>
        /// Количество шахт лифта
        /// </summary>
        public virtual int? ShaftCount { get; set; }

        /// <summary>
        /// Общее количество БТИ
        /// </summary>
        public virtual int? BtiCount { get; set; }

        /// <summary>
        /// Количество пассажирских БТИ
        /// </summary>
        public virtual int? BtiPassenger { get; set; }

        /// <summary>
        /// Количество грузовых БТИ
        /// </summary>
        public virtual int? BtiCargo { get; set; }

        /// <summary>
        /// Количество грузопассажирских БТИ
        /// </summary>
        public virtual int? BtiMixed { get; set; }
    }
}