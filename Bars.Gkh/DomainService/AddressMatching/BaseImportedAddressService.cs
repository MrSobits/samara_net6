namespace Bars.Gkh.DomainService.AddressMatching
{
    using System.Collections.Generic;
    using Bars.Gkh.Dto;

    /// <summary>
    /// Базовая имплементация сервиса. Альтернативные имплементации могут быть зарегистрированы в регионах
    /// </summary>
    public class BaseImportedAddressService : IImportedAddressService
    {
        public void Save(List<NotMatchedAddressInfo> addresses, string filename, string importType)
        {
            //ничего не сохраняем
        }
    }
}
