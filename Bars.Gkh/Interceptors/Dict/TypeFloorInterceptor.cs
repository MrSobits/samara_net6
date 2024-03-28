namespace Bars.Gkh.Interceptors.Dict
{
    using Bars.Gkh.Entities.Dicts;

    /// <summary>
    /// Интерцептор для сущности <see cref="TypeFloor"/>
    /// </summary>
    public class TypeFloorInterceptor : BaseGkhDictInterceptor<TypeFloor>
    {
        protected override string EntityName => "Тип перекрытия";
    }
}