﻿namespace Bars.GkhGji.Regions.Tomsk.Rules
{
    using B4;

    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;
    using Bars.GkhGji.Rules;

    using Castle.Windsor;
    using System.Linq;
    using Bars.GkhGji.Contracts;

    public class TomskDispNotPlanDocumentationRule : IKindCheckRule
    {

        public IWindsorContainer Container { get; set; }

        public int Priority { get { return 0; } }

        public string Code { get { return @"TomskDispNotPlanDocumentationRule"; } }

        public string Name { 
            get 
            {
                var dispText = Container.Resolve<IDisposalText>();
                return string.Format("{0} на проверку на основе документарной проверки (код вида проверки - 4)",
                    dispText.SubjectiveCase);
            } 
        }

        public TypeCheck DefaultCode { get { return TypeCheck.NotPlannedDocumentation; } }

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDisposal != TypeDisposalGji.Base)
                return false;

            var mainDisposal = Container.Resolve<IDomainService<Disposal>>()
                                 .GetAll()
                                 .FirstOrDefault(x => x.Inspection.Id == entity.Inspection.Id && x.TypeDisposal == TypeDisposalGji.Base);

            if (mainDisposal == null)
                return false;

            if (mainDisposal.KindCheck != null && mainDisposal.KindCheck.Code != TypeCheck.NotPlannedDocumentation)
                return false;

            return true;
        }
    }
}