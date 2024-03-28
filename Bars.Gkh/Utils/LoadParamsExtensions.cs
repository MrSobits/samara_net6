namespace Bars.Gkh.Utils
{
    using B4;
    using B4.Utils;

    /// <summary>
    /// 
    /// </summary>
    public static class LoadParamsExtensions
    {
        /// <summary>
        /// обнулить фильтр по ключу
        /// </summary>
        public static void SetComplexFilterNull(this LoadParam loadParam, string key)
        {
            var complexFilter = loadParam.ComplexFilter;

            if (complexFilter.Return(x => x.Field) == key)
            {
                loadParam.ComplexFilter = null;
                return;
            }

            if (complexFilter == null || complexFilter.Left == null && complexFilter.Right == null)
            {
                return;
            }

            SetComplexFilterNull(key, complexFilter);
        }

        private static void SetComplexFilterNull(string key, ComplexFilter complexFilter)
        {
            if(complexFilter == null) return;

            SetComplexFilterNull(key, complexFilter.Left);
            SetComplexFilterNull(key, complexFilter.Right);

            if (complexFilter.Left.Return(x => x.Field) == key)
            {
                complexFilter.Left.Value = "";
            }

            if (complexFilter.Right.Return(x => x.Field) == key)
            {
                complexFilter.Right.Value = "";
            }
        }

        /// <summary>
        /// найти значение комлексного фильтра
        /// </summary>
        public static ComplexFilter FindInComplexFilter(this LoadParam loadParam, string key)
        {
            var complexFilter = loadParam.ComplexFilter;

            if (complexFilter.Return(x => x.Field) == key)
            {
                return complexFilter;
            }

            if (complexFilter == null || complexFilter.Left == null && complexFilter.Right == null)
            {
                return null;
            }

            return FindInComplexFilter(key, complexFilter.Left, complexFilter.Right);
        }

        private static ComplexFilter FindInComplexFilter(string key, ComplexFilter complexFilter)
        {
            if (complexFilter.Return(x => x.Field) == key)
            {
                return complexFilter;
            }

            if (complexFilter == null || complexFilter.Left == null && complexFilter.Right == null)
            {
                return null;
            }

            return FindInComplexFilter(key, complexFilter.Left, complexFilter.Right);
        }

        private static ComplexFilter FindInComplexFilter(
            string key,
            ComplexFilter complexFilterLeft,
            ComplexFilter complexFilterRight)
        {
            if (complexFilterLeft.Return(x => x.Field) == key)
            {
                return complexFilterLeft;
            }

            if (complexFilterRight.Return(x => x.Field) == key)
            {
                return complexFilterRight;
            }

            return FindInComplexFilter(key, complexFilterLeft) ?? FindInComplexFilter(key, complexFilterRight);
        }
    }
}