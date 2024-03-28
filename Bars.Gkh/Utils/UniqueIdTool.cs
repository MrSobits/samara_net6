namespace Bars.Gkh.Utils
{
    using System;

    /// <summary>
    /// Утилита получения уникального идентификатора из составного
    /// </summary>
    public static class UniqueIdTool
    {
        /// <summary>
        /// Преобразовать строку в уникальный идентификатор используя побитовый сдвиг
        /// <para>Работает только для значений меньших <see cref="uint.MaxValue"/></para>
        /// <para>id1 &lt;&lt; 32 | id2</para>
        /// </summary>
        public static long GetId(uint id1, uint id2)
        {
            return ((long) id1 << 32) | id2;
        }

        /// <summary>
        /// Преобразовать строку в уникальный идентификатор используя побитовый сдвиг
        /// <para>Работает только для значений меньших <see cref="uint.MaxValue"/>, иначе генерирует исключение</para>
        /// <para>id1 &lt;&lt; 32 | id2</para>
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public static long GetId(long id1, long id2)
        {
            if (id1 > uint.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(id1));
            }
            if (id2 > uint.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(id2));
            }
            return (id1 << 32) | id2;
        }

        /// <summary>
        /// Получить старшую часть комбинированного Id
        /// <para>GetHiDwordId( GetId( id1, id2 ) ) == id1</para>
        /// </summary>
        public static long GetHiDwordId(long id)
        {
            return (id >> 32) & 0xFFFFFFFFL;
        }

        /// <summary>
        /// Получить младшую часть комбинированного Id
        /// <para>GetLowDwordId( GetId( id1, id2 ) ) == id2</para>
        /// </summary>
        public static long GetLowDwordId(long id)
        {
            return id & 0xFFFFFFFFL;
        }
    }
}