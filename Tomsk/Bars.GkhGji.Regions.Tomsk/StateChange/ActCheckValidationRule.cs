namespace Bars.GkhGji.Regions.Tomsk.StateChange
{
    using System.Linq;
    using Bars.B4;
    using Bars.B4.Modules.States;
    using Bars.Gkh.Enums;
    using Bars.GkhGji.Entities;

    public class ActCheckValidationRule : GkhGji.StateChange.ActCheckValidationRule
    {
        protected override ValidateResult ValidateActCheck(ActCheck act)
        {
            var result = new ValidateResult();

            // проверяем заполненность обязательных полей
            // получаем количество инспекторов
            var inspectorCount = Container.Resolve<IDomainService<DocumentGjiInspector>>().GetAll()
                .Count(x => x.DocumentGji.Id == act.Id);

            if (act.DocumentDate == null || inspectorCount == 0)
            {
                result.Message = "Необходимо заполнить обязательные поля на форме";
                result.Success = false;
                return result;
            }

            // Для Акта проверки также необходимо проверить чтобы у всех домов стоял признак выявлены или невыявлены проверки
            // То ест ьесли ест ьхотя бы один дом с признаком 'Незадано' То выдаем ошибку
            if (Container.Resolve<IDomainService<ActCheckRealityObject>>().GetAll()
                .Any(x => x.HaveViolation == YesNoNotSet.NotSet && x.ActCheck.Id == act.Id))
            {
                result.Message = "Необходимо указать результаты проверки по всем домам";
                result.Success = false;
                return result;
            }

            return ValidateResult.Yes();
        }
    }
}
