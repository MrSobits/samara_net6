using System.Linq;
using Bars.B4;
using Bars.B4.DataAccess;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;
using Bars.GkhGji.InspectionRules;

namespace Bars.GkhGji.Regions.Zabaykalye.InspectionRules.DocumentRules
{
    /// <summary>
    /// Правило создание документа 'Акт проверки' из документа 'Распоряжение' (по выбранным домам)
    /// </summary>
    public class ZabaykalyeDisposalToActCheckByRoRule : DisposalToActCheckByRoRule
    {
        public override string ResultName
        {
            get { return "Акт проверки"; }
        }

        public override IDataResult ValidationRule(DocumentGji document)
        {
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

                if (ActCheckDomain.GetAll().Any(x => x.Inspection.Id == disposal.Inspection.Id && (x.TypeActCheck == TypeActCheckGji.ActCheckGeneral || x.TypeActCheck == TypeActCheckGji.ActView)))
                {
                    return new BaseDataResult(false, "Нельзя создавать акт осмотра, если в проверке уже создан акт проверки");
                }
            }
            
            return new BaseDataResult();
        }
    }
}
