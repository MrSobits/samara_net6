namespace Bars.Gkh.DomainService
{
    using System.Collections.Generic;
    
    /// <summary>
    /// Сервис для получение количество собственников
    /// </summary>
    public interface IOwnersService
    {
        /// <summary>
        /// Получение количество собственников
        /// </summary>
        /// <param name="ids">Id жилых домов</param>
        /// <returns></returns>
        Dictionary<long, int> GetOwnersCount(long[] ids);
    }
}