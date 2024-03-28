namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using B4.DataAccess;
    using Castle.Windsor;
    using Gkh.Entities;

    public class RoomAreaVersionedParameter : AbstractVersionedParameter
    {
        public RoomAreaVersionedParameter(IWindsorContainer container, Room room) : base(container)
        {
            this._object = room;
        }

        public override string ParameterName
        {
            get { return VersionedParameters.RoomArea; }
            set { }
        }

        protected internal override PersistentObject GetPersistentObject()
        {
            return this._object;
        }

        private readonly PersistentObject _object;
    }
}