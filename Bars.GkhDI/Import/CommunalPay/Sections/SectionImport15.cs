namespace Bars.GkhDi.Import.Sections
{
    using System;
    using System.Linq;
    using System.Text;

    using Bars.B4;
    using Bars.B4.IoC;
    using Bars.B4.Modules.FIAS;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;

    using Castle.Windsor;

    public class SectionImport15 : ISectionImport
    {
        public IFiasRepository FiasRepository { get; set; }

        public string Name => "Импорт из комплат секция #15";

        public IWindsorContainer Container { get; set; }

        public void ImportSection(ImportParams importParams)
        {
            var inn = importParams.Inn;
            var sectionData = importParams.SectionData;

            if (sectionData.Section15.Count == 0)
            {
                return;
            }

            var logImport = importParams.LogImport;

            var manOrgService = this.Container.Resolve<IDomainService<ManagingOrganization>>();
            var contragentService = this.Container.Resolve<IDomainService<Contragent>>();
            using (this.Container.Using(manOrgService, contragentService))
            {
                var contragent = contragentService.GetAll().FirstOrDefault(x => x.Inn == inn);

                var managingOrganization = manOrgService.GetAll().FirstOrDefault(x => x.Contragent.Id == contragent.Id);
                if (managingOrganization == null)
                {
                    logImport.Warn(this.Name, $"Не удалось найти управляющую организацию с ИНН {inn}");
                    return;
                }

                var section15Record = sectionData.Section15.FirstOrDefault();

                var aoGuid = this.FiasRepository
                    .GetAll()
                    .Where(x => x.ActStatus == FiasActualStatusEnum.Actual)
                    .Where(x => x.AOLevel == FiasLevelEnum.Street || x.AOLevel == FiasLevelEnum.Extr || x.AOLevel == FiasLevelEnum.Sext)
                    .Where(x => x.ParentGuid != null && x.ParentGuid != "" && x.KladrCode == section15Record.KladrCode)
                    .Select(x => x.AOGuid).FirstOrDefault();

                if (aoGuid.IsNotEmpty())
                {
                    var fiasAddress = this.CreateAddressByStreetAoGuid(aoGuid, section15Record.NumPost, section15Record.KorpusPost);
                    contragent.FiasMailingAddress = fiasAddress;
                }
                else
                {
                    logImport.Warn(this.Name, $"Не удалось найти улицу по коду КЛАДР {section15Record.KladrCode}");
                }

                contragent.Fax = section15Record.Fax;
                managingOrganization.NumberEmployees = section15Record.Staff;

                contragentService.Update(contragent);
                manOrgService.Update(managingOrganization);
            }
        }

        /// <summary>
        /// Создание FiasAddress на основе aoGuid улицы, номера и корпуса дома
        /// Важно! Корректно работает только для уровня улиц
        /// </summary>
        private FiasAddress CreateAddressByStreetAoGuid(string aoGuid, string house, string housing)
        {
            var dynamicAddress = this.FiasRepository.GetDinamicAddress(aoGuid);

            var addressName = new StringBuilder(dynamicAddress.AddressName);

            if (!string.IsNullOrEmpty(house))
            {
                addressName.Append(", д. ");
                addressName.Append(house);

                if (!string.IsNullOrEmpty(housing))
                {
                    addressName.Append(", корп. ");
                    addressName.Append(housing);
                }
            }

            var fiasAddress = new FiasAddress
            {
                AddressGuid = dynamicAddress.AddressGuid,
                AddressName = addressName.ToString(),
                PostCode = dynamicAddress.PostCode,
                StreetGuidId = dynamicAddress.GuidId,
                StreetName = dynamicAddress.Name,
                StreetCode = dynamicAddress.Code,
                House = house,
                Housing = housing,
                ObjectCreateDate = DateTime.Now,
                ObjectEditDate = DateTime.Now,

                // Поля ниже коррекны только если входной параметр aoGuid улицы
                PlaceAddressName = dynamicAddress.AddressName.Replace(dynamicAddress.Name, string.Empty).Trim(' ').Trim(','),
                PlaceGuidId = dynamicAddress.ParentGuidId,
                PlaceName = dynamicAddress.ParentName
            };

            return fiasAddress;
        }
    }
}