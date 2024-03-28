namespace Bars.Gkh.DataAccess
{
    using System;
    using System.Data;
    using System.Data.Common;

    using Bars.B4.DataAccess.UserTypes;
    using Bars.B4.Utils;

    public class ImprovedJsonSerializedType<T> : JsonSerializedType<T> where T : class
    {
        protected override void WriteToDb(IDbCommand cmd, object value, int index)
        {
            IDataParameter dataParameter = cmd.Parameters[index].As<IDataParameter>();
            if (value == null)
            {
                dataParameter.Value = DBNull.Value;
            }
            else
            {
                string str = this.Serialize(value);
                dataParameter.Value = (object) str;
            }
        }
    }
}