namespace Bars.Gkh.RegOperator.Distribution
{
    using System.Collections;
    using System.Collections.Generic;

    /// <summary>
    /// Интерфейс аргументов распределения
    /// </summary>
    public interface IDistributionArgs
    {
        /// <summary>
        /// Цели распределения
        /// </summary>
        IEnumerable DistributionRecords { get; }
    }

    /// <summary>
    /// Интерфейс аргументов распределения
    /// </summary>
    public interface IDistributionArgs<out TRecord> : IDistributionArgs
    {
        /// <summary>
        /// Цели распределения
        /// </summary>
        new IEnumerable<TRecord> DistributionRecords { get; }
    }

    public abstract class AbstractDistributionArgs<TRecord> : IDistributionArgs<TRecord>
    {
        /// <inheritdoc />
        IEnumerable IDistributionArgs.DistributionRecords => this.DistributionRecords;

        /// <inheritdoc />
        public abstract IEnumerable<TRecord> DistributionRecords { get; protected set; }
    }
}