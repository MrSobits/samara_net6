using System;
using System.Collections.Generic;
using System.Data;
using Bars.Gkh.Gis.DomainService.CalcVerification.Impl;

namespace Bars.Gkh.Gis.Entities.CalcVerification
{


    public class TempTablesLifeTime : IDisposable
    {
        private BillingInstrumentary BillingInstrumentary;
        //список активных темповых таблиц в текущей сессии таблиц 
        private List<string> tempTablesListInCache;
        private TempTablesLifeTime() { }
        public TempTablesLifeTime(BillingInstrumentary billingInstrumentary)
        {
            BillingInstrumentary = billingInstrumentary;
            tempTablesListInCache = new List<string>();
        }

        public List<string> GetActiveTempTables => new List<string>(tempTablesListInCache);

        public void Dispose()
        {
            var temp = new List<string>(tempTablesListInCache);
            foreach (var table in temp)
            {
                DropTempTable(table);
            }
            BillingInstrumentary = null;
        }

        public void DropTempTable(string tableName, bool log=true)
        {
            if (tempTablesListInCache.Contains(tableName.Trim()))
            {
                BillingInstrumentary.ExecSQL("DROP TABLE IF EXISTS " + tableName, log);
                tempTablesListInCache.Remove(tableName);
            }
        }

        /// <summary>
        /// Добавить таблицу в наблюдаемые
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="sql"></param>
        /// <param name="log"></param>
        public void AddTempTable(string tableName, string sql, bool log = true)
        {
            if (!sql.ToLower().Contains("temp"))
            {
                throw new ArgumentException("Невозможно добавить постоянную таблицу в число временных!");
            }
            BillingInstrumentary.ExecSQL(sql, log);
            tempTablesListInCache.Add(tableName.Trim());
        }
    }

}
