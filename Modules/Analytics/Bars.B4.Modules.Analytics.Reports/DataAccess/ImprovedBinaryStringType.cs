namespace Bars.B4.Modules.Analytics.Reports.DataAccess
{
  using Bars.B4.Utils;
  using NHibernate.SqlTypes;
  using NHibernate.UserTypes;
  using System;
  using System.Data;
  using System.Data.Common;
  using System.IO;

  using NHibernate.Engine;

  /// <summary>
  /// Пользовательский тип "Строка - последовательность байт" для маппинга
  /// </summary>
  public class ImprovedBinaryStringType : IUserType
  {
    protected string GetFromDb(IDataReader rs, string[] names, object owner)
    {
      byte[] array = (byte[]) null;
      using (MemoryStream source = new MemoryStream())
      {
        byte[] buffer = new byte[1024];
        int ordinal = rs.GetOrdinal(names[0]);
        int num = 0;
        if (!rs.IsDBNull(ordinal))
        {
          int bytes;
          for (; (bytes = (int) rs.GetBytes(ordinal, (long) num, buffer, 0, 1024)) > 0; num += bytes)
            source.Write(buffer, 0, bytes);
        }
        source.Seek(0L, SeekOrigin.Begin);
        array = source.ReadAllBytes();
      }
      return array.AsString();
    }

    protected void WriteToDb(IDbCommand cmd, object value, int index)
    {
      IDataParameter dataParameter = cmd.Parameters[index].As<IDataParameter>();
      if (value == null)
      {
        dataParameter.Value = DBNull.Value;
      }
      else
      {
        byte[] utf8Bytes = value.ToString().ToUtf8Bytes();
        dataParameter.Value = (object) utf8Bytes;
      }
    }

    object IUserType.Assemble(object cached, object owner) => cached;

    object IUserType.DeepCopy(object value) => value == null ? (object) "" : (object) string.Concat(value);

    object IUserType.Disassemble(object value) => value == null ? (object) "" : (object) value.ToString();

    bool IUserType.Equals(object x, object y)
    {
      if (x == y)
        return true;
      return x != null && y != null && x == y;
    }

    int IUserType.GetHashCode(object x) => (x.IsNull<object>() ? (object) "" : (object) x.ToString()).GetHashCode();

    bool IUserType.IsMutable => true;

    object IUserType.NullSafeGet(DbDataReader rs, string[] names, ISessionImplementor implementor, object owner) => (object) this.GetFromDb(rs, names, owner);

    void IUserType.NullSafeSet(DbCommand cmd, object value, int index, ISessionImplementor implementor) => this.WriteToDb(cmd, value, index);

    object IUserType.Replace(object original, object target, object owner) => this.As<IUserType>().DeepCopy(original);

    Type IUserType.ReturnedType => typeof (string);

    SqlType[] IUserType.SqlTypes => new SqlType[1]
    {
      new SqlType(DbType.Binary)
    };
  }
}