using Bars.GkhGji.Regions.Tatarstan.Entities.Dict.KnmCharacters;

namespace Bars.GkhGji.Regions.Tatarstan.Controller.Dict
{
    public class KnmCharacterController : B4.Alt.DataController<KnmCharacter>
    {
        /// <summary>
        /// Получение наименования сущности, в которой нужно проверить наличие в БД записей с видом проверки
        /// </summary>
        /// <returns></returns>
        public string GetEntityType() => typeof(KnmCharacterKindCheck).FullName;
    }
}
