using System.Linq;

namespace Bars.Gkh.PassportProvider
{
    /// <summary>
    /// Интерфейс для отображение видов работ(если капремонт закончен) из модуля капремонта по объекту недвижимости (Применение: техпаспорт,)
    /// </summary>
    public interface IRealtyObjTypeWorkProvider
    {
        IQueryable<RealtyObjectTypeWorkCr> GetWorks(long realtyObjectId);

        IQueryable<RealtyObjectTypeWorkCr> GetWorks(long realtyObjectId, long periodId);
    }
}