namespace Bars.Gkh.SystemDataTransfer.Utils
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using System.Reflection.Emit;

    using Bars.B4.Utils;

    /// <summary>
    /// Абстрактный класс для динамического типаобразования (for Ilnaz)
    /// </summary>
    /// <remarks>
    /// 1. Наследуемся от типа
    /// 2. Реализуем метод IDictionary{string, Type} GetProperties() - создаваемые свойства
    /// 3. Реализуем свойство TypeSignature - имя создаваемого типа, при желании можно изменить ModuleName
    /// 4. Пользуемся
    /// </remarks>
    public abstract class TypeBuilderBase
    {
        protected static AssemblyBuilder AssemblyBuilder;

        /// <summary>
        /// Имя сборки
        /// </summary>
        public virtual string ModuleName => "DynamicEntityModule";

        /// <summary>
        /// Наименование типа
        /// </summary>
        public abstract string TypeSignature { get; }

        /// <summary>
        /// Создать экземпляр
        /// </summary>
        /// <returns>Объект</returns>
        public object CreateInstance()
        {
            return Activator.CreateInstance(this.GetOrCreateType());
        }

        /// <summary>
        /// Вернуть сгенерированный тип
        /// </summary>
        public Type GetDefinedType()
        {
            return this.GetOrCreateType();
        }

        /// <summary>
        /// Метод получения типа
        /// </summary>
        protected virtual Type GetOrCreateType()
        {
            var definedType = AppDomain.CurrentDomain.GetAssemblies()
                .FirstOrDefault(x => x.GetName().Name == this.ModuleName)
                ?.GetType(this.TypeSignature);

            return definedType ?? this.CompileResultType();
        }

        /// <summary>
        /// Метод возвращает генератор тип (переопределяем, если хотим добавить атрибуты, интерфейсы и т.п.)
        /// </summary>
        protected virtual TypeBuilder GetTypeBuilder()
        {
            var assemblyBuilder = this.GetAssemblyBuilder();

            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule(this.TypeSignature);
            TypeBuilder tb = moduleBuilder.DefineType(this.TypeSignature,
                TypeAttributes.Public |
                TypeAttributes.Class |
                TypeAttributes.AutoClass |
                TypeAttributes.AnsiClass |
                TypeAttributes.BeforeFieldInit |
                TypeAttributes.AutoLayout,
                null);

            return tb;
        }

        /// <summary>
        /// Метод возвращает необходимые для создания свойства
        /// </summary>
        protected abstract IDictionary<string, Type> GetProperties();

        /// <summary>
        /// Метод создания свойства
        /// </summary>
        /// <param name="tb">Генератор типа</param>
        /// <param name="propertyName">Имя свойства</param>
        /// <param name="propertyType">Тип</param>
        protected virtual void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName, propertyType, FieldAttributes.Private);

            PropertyBuilder propertyBuilder = tb.DefineProperty(propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr = tb.DefineMethod("get_" + propertyName,
                MethodAttributes.Public |
                MethodAttributes.SpecialName |
                MethodAttributes.HideBySig |
                MethodAttributes.Virtual,
                propertyType,
                Type.EmptyTypes);
            ILGenerator getIl = getPropMthdBldr.GetILGenerator();

            getIl.Emit(OpCodes.Ldarg_0);
            getIl.Emit(OpCodes.Ldfld, fieldBuilder);
            getIl.Emit(OpCodes.Ret);

            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig |
                    MethodAttributes.Virtual,
                    null,
                    new[] { propertyType });

            ILGenerator setIl = setPropMthdBldr.GetILGenerator();
            Label modifyProperty = setIl.DefineLabel();
            Label exitSet = setIl.DefineLabel();

            setIl.MarkLabel(modifyProperty);
            setIl.Emit(OpCodes.Ldarg_0);
            setIl.Emit(OpCodes.Ldarg_1);
            setIl.Emit(OpCodes.Stfld, fieldBuilder);

            setIl.Emit(OpCodes.Nop);
            setIl.MarkLabel(exitSet);
            setIl.Emit(OpCodes.Ret);

            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }

        private AssemblyBuilder GetAssemblyBuilder()
        {
            if (TypeBuilderBase.AssemblyBuilder.IsNotNull())
            {
                return TypeBuilderBase.AssemblyBuilder;
            }

            var an = new AssemblyName(this.ModuleName);
            TypeBuilderBase.AssemblyBuilder = AssemblyBuilder.DefineDynamicAssembly(an, AssemblyBuilderAccess.Run);

            return TypeBuilderBase.AssemblyBuilder;
        }

        private Type CompileResultType()
        {
            TypeBuilder tb = this.GetTypeBuilder();
            tb.DefineDefaultConstructor(MethodAttributes.Public | MethodAttributes.SpecialName | MethodAttributes.RTSpecialName);

            foreach (var field in this.GetProperties())
            {
                this.CreateProperty(tb, field.Key, field.Value);
            }

            return tb.CreateType();
        }
    }
}