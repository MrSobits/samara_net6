namespace Bars.GkhGji.Rules
{
    using System.Linq;

    using B4;

    using Bars.B4.DataAccess;

    using Entities;

    using Enums;

    using Castle.Windsor;

    using Bars.GkhGji.Contracts;

    public class DispPrescrCode6Rule : IKindCheckRule
    {
        public IWindsorContainer Container { get; set; }

        public IRepository<Disposal> DisposalRepository { get; set; }

        public IDisposalText DisposalText { get; set; }

        public int Priority => 0;

        public virtual string Code => "DispPrescrCode6";

        public virtual string Name => $"{this.DisposalText.SubjectiveCase} на проверку предписания " +
            $"(код вида проверки первого {this.DisposalText.GenetiveCase.ToLower()} - 6)";

        public TypeCheck DefaultCode => TypeCheck.Monitoring;

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

            if (mainDisposal.KindCheck != null && mainDisposal.KindCheck.Code != TypeCheck.Monitoring)
                return false;

            return true;
        }
    }
}