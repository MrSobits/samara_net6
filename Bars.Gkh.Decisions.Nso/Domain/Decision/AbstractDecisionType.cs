namespace Bars.Gkh.Decisions.Nso.Domain.Decisions
{
    using B4;
    using B4.Application;
    using B4.Utils;
    using Bars.Gkh.Decisions.Nso.Entities;
    using Castle.Windsor;

    public abstract class AbstractDecisionType : IDecisionType
    {
        protected AbstractDecisionType(string name, string js, bool isDefault = false)
        {
            Name = name;
            Js = js;
            IsDefault = isDefault;
        }

        public virtual string Name { get; set; }

        public abstract string Code { get; }

        public virtual string Js { get; set; }

        public virtual bool IsDefault { get; set; }

        public abstract IDataResult AcceptDecicion(GenericDecision baseDecision, BaseParams baseParams);

        public abstract IDataResult GetValue(GenericDecision baseDecision);

        public override int GetHashCode()
        {
            return Code.IsNotEmpty() ? Code.GetHashCode() : base.GetHashCode();
        }

        public IWindsorContainer Container { get { return ApplicationContext.Current.Container; } }
    }
}