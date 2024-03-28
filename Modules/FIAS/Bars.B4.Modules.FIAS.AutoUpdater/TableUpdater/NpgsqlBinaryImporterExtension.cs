namespace Bars.B4.Modules.FIAS.AutoUpdater.TableUpdater
{
    using System;

    using Bars.B4.Utils;

    using Npgsql;

    using NpgsqlTypes;

    /// <summary>
    /// Расширение импортера <see cref="NpgsqlBinaryImporter"/>
    /// </summary>
    internal static class NpgsqlBinaryImporterExtension
    {
        /// <summary>
        /// Записать параметра с проверкой его типа
        /// </summary>
        /// <typeparam name="T">Тип параметра</typeparam>
        /// <param name="writer">Объект-импортер<see cref="NpgsqlBinaryImporter"/></param>
        /// <param name="value">Записываемое значение</param>
        /// <param name="type">Тип данных в БД<see cref="NpgsqlDbType"/></param>
        public static void WriteWithTypeCheck<T>(this NpgsqlBinaryImporter writer, T value, bool isEnumSmallInt = false)
        {
            if (value == null)
            {
                writer.WriteNull();
                return;
            }

            if (value is Guid)
            {
                writer.Write((Guid)(object)value, NpgsqlDbType.Uuid);
                return;
            }

            if (value is DateTime)
            {
                writer.Write((DateTime)(object)value);
                return;
            }

            if (value is Enum)
            {
                if (isEnumSmallInt)
                {
                    writer.Write(Convert.ToInt16(value));
                }
                else
                {
                    writer.Write(Convert.ToInt32(value));
                }

                return;
            }

            writer.Write(value);
        }
    }
}