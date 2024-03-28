using System.Collections;
using Bars.B4;
using Bars.GkhCr.Entities;

namespace Bars.GkhCr.DomainService
{
    /// <summary>
    ///  Сервис для работы с Подрядчиками нарушителями прядка
    /// </summary>
    public interface IBuilderViolatorService
    {
        /// <summary>
        /// Добавление нарушений
        /// </summary>
        IDataResult AddViolations(BaseParams baseParams);

        /// <summary>
        /// Метод получат список в реестре Подрядчиков нарушивших условие договора
        /// </summary>
        IList GetList(BaseParams baseParams, bool isPaging, ref int totalCount);

        /// <summary>
        /// Метод очистки реестра подрядчиков нарушивших условие договора
        /// </summary>
        IDataResult Clear(BaseParams baseParams);

        /// <summary>
        /// Метод формирования реестра подрядчиков нарушивших условие договора
        /// </summary>
        IDataResult MakeNew(BaseParams baseParams);

        /// <summary>
        /// Метод вычисления количества дней просрочки 
        /// </summary>
        void CalculationDays(BuilderViolator violator);

        /// <summary>
        /// Метод проверки создания претензеонной работы 
        /// </summary>
        IDataResult ValidateToCreateClaimWorks(BaseParams baseParams);

        /// <summary>
        /// Метод создания претензеонной работы 
        /// </summary>
        IDataResult CreateClaimWorks(BaseParams baseParams);
    }

}
