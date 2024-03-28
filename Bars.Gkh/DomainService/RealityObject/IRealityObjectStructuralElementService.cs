namespace Bars.Gkh.DomainService
{
    using Bars.B4;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Cервис для конструктивных элементов дома
    /// </summary>
    public interface IRealityObjectStructuralElementService
    {
        /// <summary>
        /// Проверить наличие хотя бы одного конструктивного элемента 
        /// и заполненность обязательных полей
        /// </summary>
        /// <param name="ro">жилой дом</param>
        IDataResult CheckBlankFields(RealityObject ro);
    }
}