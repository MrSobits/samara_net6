namespace Bars.Gkh.Modules.ClaimWork.DomainService.States
{
    using Bars.Gkh.Modules.ClaimWork.Enums;

    /// <summary>
    /// Мета информация по документу
    /// </summary>
    public class DocumentMeta
    {
        public DocumentMeta(ClaimWorkDocumentType docType)
        {
            DocType = docType;
        }

        /// <summary>
        /// Тип документа
        /// </summary>
        public ClaimWorkDocumentType DocType { get; private set; }

        /// <summary>
        /// Статусы, ассоциированные с типом документа
        /// </summary>
        public StateConfig StateConfig { get; set; }
    }

    public class StateConfig
    {
        public StateEntry NeededState { get; set; }

        public StateEntry FormedState { get; set; }

        public int GetOrder(string stateName)
        {
            if (stateName == NeededState.Name)
                return NeededState.Order;

            if (stateName == FormedState.Name)
                return FormedState.Order;

            return int.MaxValue;
        }
    }

    public struct StateEntry
    {
        public StateEntry(string name, int order)
        {
            Name = name;
            Order = order;
        }

        public string Name;

        public int Order;
    }
}