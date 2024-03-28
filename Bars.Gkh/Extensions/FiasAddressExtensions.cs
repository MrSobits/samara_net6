namespace Bars.Gkh.Extensions
{
    using System;
    using System.Collections.Generic;

    using Bars.B4.DataAccess;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;

    using Castle.Windsor;

    /// <summary>
    /// Класс расширений для <see cref="FiasAddress"/>
    /// </summary>
    public static class FiasAddressExtensions
    {
        /// <summary>
        /// Сформировать составные свойства <see cref="FiasAddress"/>
        /// </summary>
        /// <param name="fiasAddress">Адрес ФИАС</param>
        /// <param name="container">IoC-контейнер</param>
        /// <returns>Адрес ФИАС с заполненными составными свойствами</returns>
        /// <remarks>
        /// Логика формирования составных свойств адреса ФИАС основана на js-ках
        /// "B4.form.FiasSelectAddress" и "B4.form.FiasSelectAddressWindow"
        /// При необходимости расширения списка поддерживаемых
        /// литералов и тп. опираться на указанные файлы
        /// </remarks>
        public static FiasAddress BuildCompositeFields(this FiasAddress fiasAddress, IWindsorContainer container)
        {
            var fiasRepo = container.ResolveRepository<Fias>();

            using (container.Using(fiasRepo))
            {
                var region = fiasRepo.FirstOrDefault(x => x.AOLevel == FiasLevelEnum.Region);

                if (region.IsNull())
                {
                    throw new Exception("Не удалось получить информацию о текущем регионе ФИАС");
                }

                if (fiasAddress.PlaceGuidId.IsEmpty())
                {
                    throw new Exception("Не удалось определить GUID населенного пункта");
                }

                var addressFormattedGuids = new List<string>
                {
                    $"{(int)FiasLevelEnum.Region}_{region.AOGuid}",
                    $"{(int)FiasLevelEnum.Raion}_{fiasAddress.PlaceGuidId}"
                };

                if (fiasAddress.StreetGuidId.IsNotEmpty())
                {
                    addressFormattedGuids.Add($"{(int)FiasLevelEnum.Street}_{fiasAddress.StreetGuidId}");
                }

                if (fiasAddress.HouseGuid.HasValue)
                {
                    addressFormattedGuids.Add(fiasAddress.HouseGuid.ToString());
                }

                fiasAddress.AddressGuid = string.Join("#", addressFormattedGuids);

                var addressFormattedNames = new List<string> { fiasAddress.PlaceAddressName };

                if (fiasAddress.StreetName.IsNotEmpty())
                {
                    addressFormattedNames.Add(fiasAddress.StreetName);
                }

                if (fiasAddress.House.IsNotEmpty())
                {
                    addressFormattedNames.Add($"д. {fiasAddress.House}");
                }

                if (fiasAddress.Housing.IsNotEmpty())
                {
                    addressFormattedNames.Add($"корп. {fiasAddress.Housing}");
                }

                fiasAddress.AddressName = string.Join(", ", addressFormattedNames);

                return fiasAddress;
            }
        }
    }
}