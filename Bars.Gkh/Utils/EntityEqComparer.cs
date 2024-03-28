namespace Bars.Gkh.Utils
{
    using System.Collections.Generic;

    using Bars.B4.DataModels;
    using Bars.Gkh.FormatDataExport.ExportableEntities;

    /// <summary>
    /// Компаратор сущностей по идентификатору
    /// </summary>
    public static class EntityEqComparer
    {
        /// <summary>
        /// Вернуть компаратор по идентификатору сущности <see cref="IHaveId"/>
        /// </summary>
        public static IEqualityComparer<T> ById<T>()
            where T: IHaveId
        {
            return new IdEqualityComparer<T>();
        }

        /// <summary>
        /// Вернуть компаратор по идентификатору сущности <see cref="IHaveExportId"/>
        /// </summary>
        public static IEqualityComparer<T> ByExportId<T>()
            where T : IHaveExportId
        {
            return new ExportIdEqualityComparer<T>();
        }

        private class IdEqualityComparer<T> : IEqualityComparer<T>
            where T : IHaveId
        {
            /// <inheritdoc />
            public bool Equals(T x, T y)
            {
                return long.Equals(x?.Id, y?.Id);
            }

            /// <inheritdoc />
            public int GetHashCode(T obj)
            {
                return obj?.Id.GetHashCode() ?? base.GetHashCode();
            }
        }

        private class ExportIdEqualityComparer<T> : IEqualityComparer<T>
            where T : IHaveExportId
        {
            /// <inheritdoc />
            public bool Equals(T x, T y)
            {
                return long.Equals(x?.ExportId, y?.ExportId);
            }

            /// <inheritdoc />
            public int GetHashCode(T obj)
            {
                return obj?.ExportId.GetHashCode() ?? base.GetHashCode();
            }
        }
    }
}