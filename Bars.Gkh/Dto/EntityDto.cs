namespace Bars.Gkh.Dto
{
    using System;

    using Bars.B4.DataModels;

    /// <summary>
    /// Dto сущности
    /// </summary>
    public class EntityDto<TDto> : IHaveId
    {
        /// <summary>
        /// Идентификатор
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Наименование
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Тип сущности
        /// </summary>
        public Type DtoType => typeof(TDto);
    }
}