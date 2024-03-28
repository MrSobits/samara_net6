namespace Bars.Gkh.DomainService
{
    using B4;
    using Entities;

    /// <summary>
    /// 
    /// </summary>
    public interface IRoomService
    {
        /// <summary>
        /// Проставить подъезд
        /// </summary>
        IDataResult SetEntrance(BaseParams baseParams);

        /// <summary>
        /// Проставить подъезд
        /// </summary>
        void SetEntrance(Entrance entrance, Room[] rooms);

        /// <summary>
        /// Получить список адресов помещений
        /// </summary>
        /// <param name="baseParams"></param>
        /// <returns></returns>
        IDataResult ListRoomAddress(BaseParams baseParams);
    }
}
