namespace Bars.GkhGji.Rules
{
    using System.Linq;

    using Bars.B4.DataAccess;
    using Bars.GkhGji.Entities;
    using Bars.GkhGji.Enums;

    using Castle.Windsor;

    using Bars.GkhGji.Contracts;

    public class DispPrescrCode8Rule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public IRepository<Disposal> DisposalRepository { get; set; }

        public IDisposalText DisposalText { get; set; }

        public int Priority => 0;

        public virtual string Code => "DispPrescrCode8";

        public virtual string Name => $"{this.DisposalText.SubjectiveCase} на проверку предписания " +
            $"(код вида проверки первого {this.DisposalText.GenetiveCase.ToLower()} - 8)";

        public TypeCheck DefaultCode => TypeCheck.NotPlannedExit;

        protected virtual TypeDocumentGji AvailableTypeDocumentGji => TypeDocumentGji.Disposal;

        public bool Validate(Disposal entity)
        {
            if (entity.TypeDocumentGji != this.AvailableTypeDocumentGji ||
                entity.TypeDisposal != TypeDisposalGji.DocumentGji)
                return false;

            var mainDisposal = this.DisposalRepository
                .GetAll()
                .FirstOrDefault(x => x.Inspection.Id == entity.Inspection.Id &&
                    x.TypeDisposal == TypeDisposalGji.Base &&
                    x.TypeDocumentGji == this.AvailableTypeDocumentGji);

            if (mainDisposal == null)
                return false;

            if (mainDisposal.KindCheck != null && mainDisposal.KindCheck.Code != TypeCheck.VisualSurvey)
                return false;

            return true;
        }
    }
}