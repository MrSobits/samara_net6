using Bars.GkhGji.Contracts;
namespace Bars.GkhGji.StateChange
{
    public class DisposalValidationRule : BaseDocValidationRule
    {
        public override string Id
        {
            get { return "gji_document_disp_validation_rule"; }
        }

        public override string Name
        {
            get 
            {
                var dispText = Container.Resolve<IDisposalText>();

                return string.Format("Проверка заполненности карточки {0}", dispText.GenetiveCase.ToLower()); 
            }
        }

        public override string TypeId
        {
            get { return "gji_document_disp"; }
        }

        public override string Description
        {
            get 
            {
                var dispText = Container.Resolve<IDisposalText>();
                return string.Format("Данное правило проверяет заполненность необходимых полей {0}", dispText.GenetiveCase.ToLower());
            }
        }
    }
}