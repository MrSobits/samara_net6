namespace Bars.Gkh.BaseApiIntegration.Startup
{
    using System.Reflection;

    /// <summary>
    /// Resolver для получения контроллеров ТОЛЬКО в указанной сборке
    /// </summary>
    public class AssemblyHttpControllerTypeResolver : DefaultHttpControllerTypeResolver
    {
        /// <inheritdoc />
        public AssemblyHttpControllerTypeResolver(Assembly moduleAssembly)
            : base(type => DefaultHttpControllerTypeResolver.IsControllerType(type) && moduleAssembly.Equals(type.Assembly))
        {
        }
    }
}