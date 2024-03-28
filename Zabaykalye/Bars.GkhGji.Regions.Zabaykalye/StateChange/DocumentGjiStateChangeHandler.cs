namespace Bars.GkhGji.Regions.Zabaykalye.StateChange
{
    using System;

    using Bars.B4.Modules.States;
    using Bars.GkhGji.Entities;

    using Castle.Windsor;

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
                    throw new DocumentGjiStateChangeException("Не удалось привести к типу DocumentGji");
                }

                // этот хэндлер выглядит странно, да
                // но здесь был еще более странный кусок неиспользуемого кода, так что сейчас стало лучше
                // если что, взять его можно в сахинском гжи
            }
        }

        private class DocumentGjiStateChangeException : Exception
        {
            public DocumentGjiStateChangeException(string message)
                : base(message)
            {
                
            }
        }
    }
}