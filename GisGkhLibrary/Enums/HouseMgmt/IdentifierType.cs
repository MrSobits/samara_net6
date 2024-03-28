namespace GisGkhLibrary.Enums.HouseMgmt
{
    /// <summary>
    /// Тип гуида помещения
    /// </summary>
    public enum AccommodationIdentifierType
    {
        /// <summary>
        /// Глобальный уникальный идентификатор дома по ФИАС
        /// </summary>
        FIASHouseGuid = 1,

        /// <summary>
        /// Идентификатор помещения
        /// </summary>
        PremisesGUID = 2,

        /// <summary>
        /// Идентификатор комнаты
        /// </summary>
        LivingRoomGUID = 3
    }
}
