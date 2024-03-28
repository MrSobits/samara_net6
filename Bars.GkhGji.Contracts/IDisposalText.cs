namespace Bars.GkhGji.Contracts
{
    /// <summary>
    /// Интерфейс для описания тектовых значений Распоряжения
    /// </summary>
    public interface IDisposalText
    {
        /// <summary>
        /// Именительный падеж (кто? что?)
        /// </summary>
        string SubjectiveCase { get; }
        
        /// <summary>
        /// Родительный падеж - кого? чего?
        /// </summary>
        string GenetiveCase { get; }

        /// <summary>
        /// Дательный падеж - кому? чему?
        /// </summary>
        string DativeCase { get;}

        /// <summary>
        /// Винительный падеж - кого? что?
        /// </summary>
        string AccusativeCase { get; }

        /// <summary>
        /// Творительный падеж - кем? чем?
        /// </summary>
        string InstrumentalCase { get; }

        /// <summary>
        /// Предложный падеж - о ком? о чём?
        /// </summary>
        string PrepositionalCase { get; }

        /// <summary>
        /// Именительный Множественный падеж (кто? что?)
        /// </summary>
        string SubjectiveManyCase { get; }

        /// <summary>
        /// Родительный Множественный падеж (кто? что?)
        /// </summary>
        string GenetiveManyCase { get; }
    }
}
