namespace Bars.Gkh.Gis.DomainService.CalcVerification.Impl
{
    using System;
    using System.Data;
    using Dapper;

    public class BillingInstrumentary
    {

        protected IDbConnection Connection;
        public BillingInstrumentary(IDbConnection connection)
        {
            Connection = connection;
        }

        /// <summary>
        /// Обертка для Execute
        /// </summary>
        /// <param name="sql">Текст запроса</param>
        /// <param name="flag">!flag - глушим Exceptions</param>
        public void ExecSQL(string sql, bool flag = true)
        {
            try
            {
                Connection.Execute(sql);
            }
            catch (Exception)
            {
                if (!flag)
                {
                    return;
                }
                throw;
            }
        }
        /// <summary>
        /// Обертка для ExecuteScalar
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="sql"></param>
        /// <param name="flag"></param>
        /// <returns></returns>
        public T ExecScalar<T>(string sql, bool flag = true)
        {
            try
            {
                return Connection.ExecuteScalar<T>(sql);
            }
            catch (Exception)
            {
                if (!flag)
                {
                    return default(T);
                }
                throw;
            }
        }
        /// <summary>
        /// Конструктор даты
        /// </summary>
        /// <param name="month"></param>
        /// <param name="day"></param>
        /// <param name="year"></param>
        /// <returns></returns>
        public string MDY(int month, int day, int year)
        {
            return string.Format(" '{0}-{1}-{2}'::date ", year, month, day);
        }
    }
}
