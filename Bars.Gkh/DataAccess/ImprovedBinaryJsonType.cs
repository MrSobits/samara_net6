namespace Bars.Gkh.DataAccess
{
    using System;
    using System.Data;
    using System.Data.Common;

    using Bars.B4.DataAccess.UserTypes;
    using Bars.B4.Utils;

    /// <summary>
    /// Пользовательский тип "Json - последовательность байт" для маппинга
    /// </summary>
    public class ImprovedBinaryJsonType<T> : BinaryJsonType<T> where T : class 
    {
        /// <inheritdoc />
        protected override void WriteToDb(IDbCommand cmd, object value, int index)
        {
            IDataParameter dataParameter = cmd.Parameters[index].As<IDataParameter>();
            if (value == null)
            {
                dataParameter.Value = DBNull.Value;
            }
            else
            {
                byte[] utf8Bytes = this.Serialize(value).ToUtf8Bytes();
                dataParameter.Value = (object) utf8Bytes;
            }
        }
    }
}