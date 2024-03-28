namespace Bars.Gkh1468.Domain
{
    using Bars.Gkh.Enums;
    using Bars.Gkh1468.Enums;

    /// <summary>
    /// Расширения для удобного перевода между типами домов 1468 - оригинальный ЖКХ
    /// </summary>
    public static class HouseTypeEnumExtensions
    {
        /// <summary>
        /// К типу объекта из 1468
        /// </summary>
        /// <param name="gkhHouseType">Тип объекта из ЖКХ</param>
        /// <returns>Тип объекта 1468</returns>
        public static TypeRealObj To1468RealObjType(this TypeHouse gkhHouseType)
        {
            switch (gkhHouseType)
            {
                case TypeHouse.ManyApartments:
                case TypeHouse.SocialBehavior:
                    return TypeRealObj.Mkd;
                case TypeHouse.BlockedBuilding:
                case TypeHouse.Individual:
                    return TypeRealObj.RealityObject;
                default:
                    return TypeRealObj.NoPassport;
            }
        }
    }
}