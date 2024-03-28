using Bars.GkhGji.Regions.Tatarstan.Entities.Dict;

namespace Bars.GkhGji.Regions.Tatarstan.Controller.Dict
{
    public class KnmTypesController : B4.Alt.DataController<KnmTypes>
    {
        /// <summary>
        /// Получение наименования сущности, в которой нужно проверить наличие в БД записей с видом проверки
        /// </summary>
        /// <returns></returns>
        public string GetEntityType() => typeof(KnmTypeKindCheck).FullName;
    }
}
