namespace Bars.GkhGji.Interceptors
{
    using Bars.GkhGji.Entities;

    public class BaseDefaultInterceptor : BaseDefaultInterceptor<BaseDefault>
    {
    }

    public class BaseDefaultInterceptor<T> : InspectionGjiInterceptor<T>
        where T: BaseDefault
    {
        // просто вынес весь код в базовую сущность и в этом классе стал окакто грустно
        // Возможно что в регионах от этого класса наследовались , поэтому не удалять
    }
}