namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using Bars.B4.DataAccess;
    using Bars.B4.Utils;
    using Bars.Gkh.SystemDataTransfer.Meta;

    /// <summary>
    /// Методы-расширения расширения подзапросов для авто-проецирования в DTO по одноименным свойствам
    /// </summary>
    internal static class QueryableExtensions
    {
        /// <summary>
        /// Метод расширения подзапросов для авто-проецирования в DTO по одноименным свойствам
        /// </summary>
        public static ProjectionExpression<TSource> Project<TSource>(this IQueryable<TSource> source)
            where TSource : IEntity
        {
            return new ProjectionExpression<TSource>(source);
        }

        /// <summary>
        /// Метод расширения подзапросов для авто-проецирования в DTO по одноименным свойствам
        /// </summary>
        public static ProjectionExpression<TSource> Project<TSource>(this IQueryable<TSource> source, ITransferEntityMeta meta)
            where TSource : IEntity
        {
            return new ProjectionExpression<TSource>(source, meta);
        }
    }

    public class MapperExpressionHelper<TSource>
        where TSource : IEntity
    {
        private static readonly Dictionary<string, Expression> expressionCache = new Dictionary<string, Expression>();

        protected Expression<Func<TSource, TDest>> GetCachedExpression<TDest>()
        {
            var key = this.GetCacheKey<TDest>();

            return MapperExpressionHelper<TSource>.expressionCache.ContainsKey(key) ? MapperExpressionHelper<TSource>.expressionCache[key] as Expression<Func<TSource, TDest>> : null;
        }

        protected Expression<Func<TSource, TDest>> BuildExpression<TDest>(ITransferEntityMeta meta, bool addToCache)
        {
            var sourceProperties = typeof(TSource).GetProperties();
            var destinationProperties = typeof(TDest).GetProperties().Where(dest => dest.CanWrite);
            var parameterExpression = Expression.Parameter(typeof(TSource), "src");

            var bindings = destinationProperties
                .Select(destinationProperty => this.BuildBinding(parameterExpression, destinationProperty, sourceProperties, meta))
                .Where(binding => binding != null);

            var expression = Expression.Lambda<Func<TSource, TDest>>(Expression.MemberInit(Expression.New(typeof(TDest)), bindings), parameterExpression);

            var key = this.GetCacheKey<TDest>();

            if (addToCache)
            {
                MapperExpressionHelper<TSource>.expressionCache.Add(key, expression);
            }

            return expression;
        }

        protected virtual MemberAssignment BuildBinding(
            ParameterExpression parameterExpression, 
            PropertyInfo destinationProperty, 
            IEnumerable<PropertyInfo> sourceProperties,
            ITransferEntityMeta meta)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);

            Expression valueExpression = null;
            if (sourceProperty != null)
            {
                valueExpression = Expression.Property(parameterExpression, sourceProperty);
            }

            var serializer = meta?.Serializer as AbstractDataSerializer<TSource>;
            if (serializer.IsNotNull() && destinationProperty.Name == "ComplexKey")
            {
                valueExpression = serializer.GetEntitySelectExpression(parameterExpression);
            }

            if (valueExpression.IsNotNull())
            {
                if (typeof(IEntity).IsAssignableFrom(valueExpression.Type) 
                    && (destinationProperty.PropertyType == typeof(long) || destinationProperty.PropertyType == typeof(long?) || destinationProperty.Name == "ComplexKey"))
                {
                    valueExpression = Expression.Property(valueExpression, "Id");
                }

                if (destinationProperty.PropertyType != valueExpression.Type)
                {
                    valueExpression = Expression.Convert(valueExpression, destinationProperty.PropertyType);
                }

                return Expression.Bind(destinationProperty, valueExpression);
            }

            return null;
        }

        protected string GetCacheKey<TDest>()
        {
            return string.Concat(typeof(TSource).FullName, typeof(TDest).FullName);
        }
    }

    public class ProjectionExpression<TSource> : MapperExpressionHelper<TSource>
        where TSource : IEntity
    {
        private readonly IQueryable<TSource> source;
        private readonly ITransferEntityMeta meta;

        public ProjectionExpression(IQueryable<TSource> source, ITransferEntityMeta meta = null)
        {
            this.source = source;
            this.meta = meta;
        }

        public IQueryable<TDest> To<TDest>(bool useCache = true)
        {
            Expression<Func<TSource, TDest>> queryExpression;

            if (useCache)
            {
                queryExpression = this.GetCachedExpression<TDest>() ?? this.BuildExpression<TDest>(this.meta, true);
            }
            else
            {
                queryExpression = this.BuildExpression<TDest>(this.meta, false);
            }

            return this.source.Select(queryExpression);
        }

        public IQueryable To(Type type, bool useCache = true)
        {
            var method = this.GetType().GetMethod("To", new[] { typeof(bool) });
            var genericMethod = method.MakeGenericMethod(type);

            return (IQueryable)genericMethod.Invoke(this, new object []{ useCache });
        }
    }

    public class ClassMappingExpression<TSource> : MapperExpressionHelper<TSource>
        where TSource : IEntity
    {
        private static readonly Dictionary<string, Delegate> funcCache = new Dictionary<string, Delegate>();
        private readonly TSource source;

        public ClassMappingExpression(TSource source)
        {
            this.source = source;
        }

        public TDest To<TDest>()
        {
            var queryExpression = this.GetCachedFunc<TDest>() ?? this.BuildFunc<TDest>();
            return queryExpression(this.source);
        }

        public object To(Type type)
        {
            var method = this.GetType().GetMethod("To", new Type[0]);
            var genericMethod = method.MakeGenericMethod(type);

            return genericMethod.Invoke(this, null);
        }

        private Func<TSource, TDest> BuildFunc<TDest>()
        {
            var expression = this.BuildExpression<TDest>(null, false);

            var func = expression.Compile();
            var key = this.GetCacheKey<TDest>();

            ClassMappingExpression<TSource>.funcCache.Add(key, func);

            return func;
        }

        private Func<TSource, TDest> GetCachedFunc<TDest>()
        {
            var key = this.GetCacheKey<TDest>();

            return ClassMappingExpression<TSource>.funcCache.ContainsKey(key) ? ClassMappingExpression<TSource>.funcCache[key] as Func<TSource, TDest> : null;
        }

        /// <inheritdoc />
        protected override MemberAssignment BuildBinding(
            ParameterExpression parameterExpression,
            PropertyInfo destinationProperty,
            IEnumerable<PropertyInfo> sourceProperties,
            ITransferEntityMeta meta)
        {
            var sourceProperty = sourceProperties.FirstOrDefault(src => src.Name == destinationProperty.Name);

            Expression valueExpression = null;
            if (sourceProperty != null)
            {
                valueExpression = Expression.Property(parameterExpression, sourceProperty);
            }

            var serializer = meta?.Serializer as AbstractDataSerializer<TSource>;
            if (serializer.IsNotNull() && destinationProperty.Name == "ComplexKey")
            {
                return null;
            }

            if (valueExpression.IsNotNull())
            {
                if (typeof(IEntity).IsAssignableFrom(valueExpression.Type)
                    && (destinationProperty.PropertyType == typeof(long) || destinationProperty.PropertyType == typeof(long?)))
                {
                    if (destinationProperty.PropertyType == typeof(long?))
                    {
                        var nullConstant = Expression.Constant(null, valueExpression.Type);
                        var idSelector = Expression.Property(valueExpression, "Id");
                        valueExpression = Expression.Condition(
                            Expression.NotEqual(valueExpression, nullConstant),
                            Expression.Convert(idSelector, typeof(long?)), 
                            Expression.Constant((long?)null, typeof(long?)));
                    }
                    else
                    {
                        valueExpression = Expression.Property(valueExpression, "Id");
                    }

                    
                }

                if (destinationProperty.PropertyType != valueExpression.Type)
                {
                    valueExpression = Expression.Convert(valueExpression, destinationProperty.PropertyType);
                }

                return Expression.Bind(destinationProperty, valueExpression);
            }

            return null;
        }
    }
}