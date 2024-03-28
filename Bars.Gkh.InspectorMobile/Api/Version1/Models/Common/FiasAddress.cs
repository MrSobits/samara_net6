namespace Bars.Gkh.InspectorMobile.Api.Version1.Models.Common
{
    using System;
    using System.ComponentModel.DataAnnotations;

    using Bars.Gkh.Extensions;

    using Castle.Windsor;

    /// <summary>
    /// Адрес по ФИАС
    /// </summary>
    public class FiasAddress
    {
        /// <summary>
        /// Адрес до населенного пункта (без улицы, дома, корпуса и тд)
        /// </summary>
        [Required]
        public string PlaceAddressName { get; set; }

        /// <summary>
        /// Код населенного пункта
        /// </summary>
        public string PlaceCode { get; set; }

        /// <summary>
        /// Гуид записи ФИАСа населенного пункта
        /// </summary>
        [Required]
        public string PlaceGuidId { get; set; }

        /// <summary>
        /// Наименование населенного пункта
        /// </summary>
        [Required]
        public string PlaceName { get; set; }

        /// <summary>
        /// Код улицы
        /// </summary>
        public string StreetCode { get; set; }

        /// <summary>
        /// Гуид записи ФИАС улицы
        /// </summary>
        public string StreetGuidId { get; set; }

        /// <summary>
        /// Наименование улицы
        /// </summary>
        public string StreetName { get; set; }

        /// <summary>
        /// Глобальный уникальный идентификатор дома
        /// </summary>
        public Guid? HouseGuid { get; set; }

        /// <summary>
        /// Номер дома
        /// </summary>
        [Required]
        public string House { get; set; }

        /// <summary>
        /// Корпус
        /// </summary>
        public string Housing { get; set; }

        /// <summary>
        /// Почтовый индекс
        /// </summary>
        public string PostCode { get; set; }

        /// <summary>
        /// Получить <see cref="Bars.B4.Modules.FIAS.FiasAddress"/>
        /// из текущего экземпляра модели адреса
        /// </summary>
        /// <param name="container">IoC-контейнер</param>
        /// <param name="currentFiasAddress">Текущий экземпляр типа <see cref="Bars.B4.Modules.FIAS.FiasAddress"/></param>
        /// <returns>Экземпляр типа <see cref="Bars.B4.Modules.FIAS.FiasAddress"/></returns>
        public Bars.B4.Modules.FIAS.FiasAddress GetFiasAddress(IWindsorContainer container, Bars.B4.Modules.FIAS.FiasAddress currentFiasAddress = null)
        {
            var entity = this.CopyIdenticalProperties<Bars.B4.Modules.FIAS.FiasAddress>()
                .BuildCompositeFields(container);

            if (currentFiasAddress?.Id > 0)
            {
                entity.Id = currentFiasAddress.Id;
            }

            return entity;
        }
    }
}