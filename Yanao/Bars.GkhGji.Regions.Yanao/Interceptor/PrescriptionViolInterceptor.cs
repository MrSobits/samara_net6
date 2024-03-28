namespace Bars.GkhGji.Regions.Yanao.Interceptor
{
    using System;
    using B4;
    using Entities;

    /// <summary>
    /// Интерцептор сущности "Этап указания к устранению нарушения в предписании" (см. <see cref="PrescriptionViol"/>)
    /// Регион: ЯНАО
    /// </summary>
    public class PrescriptionViolInterceptor : Interceptors.PrescriptionViolInterceptor
    {
        /// <summary>
        /// Проверка плановой даты устранения
        /// </summary>
        /// <param name="entity">Сущность <see cref="PrescriptionViol"/></param>
        /// <param name="actCheckDate">Дата родительского документа</param>
        /// <returns>Результат выполнения проверки</returns>
        protected override IDataResult CheckDatePlanRemoval(PrescriptionViol entity, DateTime? actCheckDate)
        {

            return Success();
        }
    }
}