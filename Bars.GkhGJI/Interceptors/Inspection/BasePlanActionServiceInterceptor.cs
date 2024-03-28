namespace Bars.GkhGji.Interceptors
{

    using Bars.GkhGji.Entities;

    public class BasePlanActionServiceInterceptor : BasePlanActionServiceInterceptor<BasePlanAction>
    {
        // Все методы добавлять и изменять в generic
    }

    public class BasePlanActionServiceInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BasePlanAction
    {
        // Вынес весь код в базовый класс и тут стало както грустно
        // поскольку в регионах могли от нео наследоватся то неудаляйте данный класс
    }
}
