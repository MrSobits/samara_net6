﻿using System;
using System.Linq;
using Bars.B4.DataAccess;
using Bars.B4.Utils;
using Bars.Gkh.Domain.CollectionExtensions;
using Bars.GkhGji.Entities;
using Bars.GkhGji.Enums;

namespace Bars.GkhGji.Regions.Khakasia.StateChange
{
    public class ActSurveyValidationNumberRule : BaseDocValidationNumberRule
    {
        public override string Id { get { return "GJI_KHAKASIA_actsurvey_validation_number"; } }

        public override string Name { get { return "Хакасия - Проверка возможности формирования номера акта обследования"; } }

        public override string TypeId { get { return "gji_document_actsur"; } }

        public override string Description { get { return "Хакасия - Данное правило проверяет формирование номера акта обследования в соответствии с правилами"; } }

    }
}