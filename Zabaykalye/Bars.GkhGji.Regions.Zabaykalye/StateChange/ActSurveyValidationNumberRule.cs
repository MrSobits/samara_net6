using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Zabaykalye.StateChange
{
    public class ActSurveyValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "gji_zabaykalye_actsurvey_validation_number"; } }

        public override string Name { get { return "Забайкалье - Проверка возможности формирования номера акта обследования"; } }

        public override string TypeId { get { return "gji_document_actsur"; } }

        public override string Description { get { return "Забайкалье - Данное правило проверяет формирование номера акта обследования в соответствии с правилами"; } }

    }
}