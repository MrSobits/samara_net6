namespace Bars.Gkh.ExecutionAction.Impl
{
    using System;
    using System.Collections.Generic;
    using System.Linq;

    using Bars.B4;
    using Bars.B4.DataAccess;
    using Bars.Gkh.Domain;
    using Bars.Gkh.Entities;

    /// <summary>
    /// Действие, чтобы проставить параметр "Дом не участвует в программе КР"
    /// в паспорте дома если у статуса этого дома признак "Конечный"
    /// </summary>
    public class SetParamInvolvedCrToHouseFinalStateAction : BaseExecutionAction
    {
        /// <summary>
        /// The list of reality objects for save.
        /// </summary>
        private readonly List<RealityObject> realityObjectsForSave = new List<RealityObject>();

        /// <summary>
        /// Gets the code.
        /// </summary>
        /// <summary>
        /// Gets the interface code.
        /// </summary>
        /// <summary>
        /// Gets the description.
        /// </summary>
        public override string Description
            => "Проставить параметр \"Дом не участвует в программе КР\" в паспорте дома, если у статуса этого дома признак \"Конечный\".";

        /// <summary>
        /// Gets the name.
        /// </summary>
        public override string Name => "Проставить параметр (Дом не участвует в программе КР) в паспорте дома";

        /// <summary>
        /// Gets the action.
        /// </summary>
        public override Func<IDataResult> Action => this.Execute;

        /// <summary>
        /// Gets or sets the container.
        /// </summary>
        /// <summary>
        /// The execute method.
        /// </summary>
        /// <returns>
        /// The <see cref="BaseDataResult" />.
        /// </returns>
        public BaseDataResult Execute()
        {
            var roDomain = this.Container.ResolveDomain<RealityObject>();

            var roQuery = roDomain.GetAll().Where(x => x.State.FinalState);

            foreach (var realityObject in roQuery.ToList())
            {
                realityObject.IsNotInvolvedCr = true;
                this.realityObjectsForSave.Add(realityObject);
            }

            TransactionHelper.InsertInManyTransactions(this.Container, this.realityObjectsForSave);

            return new BaseDataResult();
        }
    }
}