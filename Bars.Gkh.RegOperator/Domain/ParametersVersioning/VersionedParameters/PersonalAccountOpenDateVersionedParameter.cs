namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using B4.DataAccess;
    using Castle.Windsor;
    using Entities;

    public class PersonalAccountOpenDateVersionedParameter : AbstractVersionedParameter
    {
        private readonly BasePersonalAccount account;

        public PersonalAccountOpenDateVersionedParameter(IWindsorContainer container, BasePersonalAccount account) : base(container)
        {
            this.account = account;
        }

        /// <summary>
        /// Имя параметра
        /// </summary>
        public override string ParameterName
        {
            get { return VersionedParameters.PersonalAccountOpenDate; }
            set { }
        }

        protected internal override PersistentObject GetPersistentObject()
        {
            return this.account;
        }
    }
}