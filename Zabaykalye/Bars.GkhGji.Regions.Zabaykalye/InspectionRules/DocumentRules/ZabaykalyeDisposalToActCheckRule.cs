using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.InspectionRules;

namespace Bars.GkhGji.Regions.Zabaykalye.InspectionRules.DocumentRules
{
    public class ZabaykalyeDisposalToActCheckRule : DisposalToActCheckRule
    {
        public override IDataResult ValidationRule(DocumentGji document)
        {
            return new BaseDataResult(false, "В Сахе нет Акт проверки (общий) есть тольк оакт проверки с выбором 1го или несколько домов");


            /*
             тут проверяем, Если Распоряжение не Основное то недаем выполнить формирование
            */

            if (document != null)
            {
                var disposal = DisposalDomain.FirstOrDefault(x => x.Id == document.Id);

                if (disposal == null)
                {
                    return new BaseDataResult(false, string.Format("Не удалось получить распоряжение {0}", document.Id));
                }

                if (disposal.TypeDisposal != TypeDisposalGji.Base)
                {
                    return new BaseDataResult(false, "Акт проверки можно сформирвоать только из основного распоряжения");
                }

                if (!this.InspectionRoDomain.GetAll().Any(x => x.Inspection.Id == document.Inspection.Id))
                {
                    return new BaseDataResult(false, "Данный акт можно сформирвоать только для проверок с домами");
                }

                if (ActCheckDomain.GetAll().Any(x => x.Inspection.Id == disposal.Inspection.Id))
                {
                    return new BaseDataResult(false, "Нельзя создать акт проверки(общий), т.к. уже созданы акты проверки/осмотра по некоторым домам");
                }
            }


            return new BaseDataResult();
        }
    }
}
