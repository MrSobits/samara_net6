namespace Bars.Gkh.RegOperator.Domain.ParametersVersioning
{
    using NHibernate;

    public class NhInterceptor : EmptyInterceptor
    {
        private readonly LogsHolder _holder;

        public NhInterceptor(LogsHolder holder)
        {
            _holder = holder;
        }

        #region Overrides of EmptyInterceptor

        /// <summary>
        /// Called after a transaction is committed or rolled back.
        /// </summary>
        public override void AfterTransactionCompletion(ITransaction tx)
        {
            if (tx != null)
            {
                if (tx.WasCommitted)
                {
                    _holder.Flush();
                }
                else if (tx.WasRolledBack)
                {
                    _holder.Clear();
                }
            }

            base.AfterTransactionCompletion(tx);
        }

        #endregion
    }
}