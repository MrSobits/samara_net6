namespace Bars.Gkh.InspectorMobile.Api.Version1.Services
{
    using System.Threading.Tasks;

    using Bars.Gkh.InspectorMobile.Api.Version1.Models.ObjectCr;

    /// <summary>
    /// Сервис для работы с данными об объекте капитального ремонта
    /// </summary>
    public interface IObjectCrService : IBaseApiService<object, ObjectCrUpdate>
    {
        /// <summary>
        /// Получить данные об объекте капитального ремонта
        /// </summary>
        /// <param name="objectId">Идентификатор объекта капитального ремонта</param>
        Task<ObjectCrGet> GetByObjectCr(long objectId);

        /// <summary>
        /// Получить данные об объекте капитального ремонта
        /// </summary>
        /// <param name="programId">Идентификатор программы капитального ремонта</param>
        /// <param name="addressId">Идентификатор дома</param>
        Task<ObjectCrGet> GetByProgramAndAddress(long programId, long addressId);
    }
}