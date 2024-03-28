using System.Reflection;
using Bars.B4;

namespace Bars.GkhGji.Regions.BaseChelyabinsk.Services.Impl.Intfs
{
    /// <summary>
    /// Интерфейс валидатора
    /// </summary>
    public interface IValidator<in T>
    {
        /// <summary>
        /// Валидация
        /// </summary>
        /// <returns></returns>
        IDataResult Validate(PropertyInfo name, T value);
    }
}