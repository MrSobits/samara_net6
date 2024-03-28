namespace Bars.Gkh.Gis.DomainService.UicHouse
{
    using Entities.Register.HouseRegister;

    /// <summary>
    /// Сервис генерации УИК для лицевого счета
    /// </summary>
    public interface IUicHouseService
    {
        /// <summary>
        /// Генерация УИК
        /// </summary>
        void GenerateUic(HouseRegister houseRegister);
    }
}