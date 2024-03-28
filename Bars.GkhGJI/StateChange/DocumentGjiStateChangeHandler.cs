namespace Bars.GkhGji.StateChange
{
    using System;
    using System.Collections.Generic;
    using System.Globalization;
    using System.Linq;

    using B4.Modules.States;

    using Bars.B4;
    using Bars.B4.Utils;
    using Bars.Gkh.Entities;
    using Bars.GkhGji.Enums;

    using Entities;

    using Castle.Windsor;

    using NHibernate.Mapping;

    /// <summary>
    /// Обработчик события смены статуса документа ГЖИ
    /// </summary>
    public class DocumentGjiStateChangeHandler : IStateChangeHandler
    {
        public IWindsorContainer Container { get; set; }

        public void OnStateChange(IStatefulEntity entity, State oldState, State newState)
        {
            if (entity.State.TypeId.Contains("gji_document"))
            {
                var document = entity as DocumentGji;

                if (document == null)
                {
                    throw new InvalidCastException("Не удалось привести к типу DocumentGji");
                }
            }
        }
    }
}