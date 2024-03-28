namespace Bars.GkhGji.StateChange
{
    using B4.Modules.States;
    using Entities;
    using Castle.Windsor;

    public abstract class BaseDocSmolenskNumberRule : IRuleChangeStatus
    {
        public IWindsorContainer Container { get; set; }

        public abstract string Id { get; }

        public abstract string Name { get; }

        public abstract string TypeId { get; }

        public abstract string Description { get; }

        public ValidateResult Validate(IStatefulEntity statefulEntity, State oldState, State newState)
        {
            var result = new ValidateResult();

            if (statefulEntity is DocumentGji)
            {
                var document = statefulEntity as DocumentGji;

                if (string.IsNullOrEmpty(document.DocumentNumber))
                {
                    InternalValidate(document, result);

                    if (result.Success)
                    {
                        Action(document);
                    }                  
                }
            }
           
            return result;
        }

        protected virtual void InternalValidate(DocumentGji document, ValidateResult result)
        {
            result.Success = true;
        }

        protected virtual void Action(DocumentGji document)
        {
        }
    }
}