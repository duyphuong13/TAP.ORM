using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;

namespace TAP.Core.DatastoreORM
{
    public class TypesMetadataLoader
    {
        public const string InheritedTypePropertyName = "__InheritedType";

        private readonly Dictionary<Type, TypeMetadata> _typesMetadata;


        public TypesMetadataLoader(Type type)
        {
            try
            {
                _typesMetadata = ConstructTypesMetadata(type);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error loading DAO definition, type: {type}");
            }
        }

        public TypeMetadata GetTypeMetadata(Type type)
        {
            return _typesMetadata.ContainsKey(type) ? _typesMetadata[type] : null;
        }
        public Type GetTypeMetadata(string type)
        {
            return (from t in _typesMetadata where t.Value.InheritedClassType == type select t.Key).FirstOrDefault();
        }

        private static Dictionary<Type, TypeMetadata> ConstructTypesMetadata(Type type, string baseEntityName = null)
        {
            var typesMetadata = new Dictionary<Type, TypeMetadata>();
            Func<object> constructor;

            var inheritedEntityTypeAttribute = type.GetCustomAttribute<InheritedEntityTypeAttribute>();
            if (inheritedEntityTypeAttribute != null)
            {
                var inheritedTypes = type.Assembly.GetTypes().Where(a => a.IsSubclassOf(type));
                foreach (var t in inheritedTypes)
                {
                    var innerMetadata = ConstructTypesMetadata(t, inheritedEntityTypeAttribute.Type);
                    typesMetadata.AddRange(innerMetadata);
                }
            }

            if ((constructor = BuildEmptyConstructorAccessor(type)) == null)
            {
                return typesMetadata;
            }

            TypeMetadata typeMetadata;
            if (typeof(ICollection).IsAssignableFrom(type))
            {
                if (!type.GetGenericArguments().Any())
                {
                    //TODO: Add Warn log
                    return typesMetadata;
                }

                typeMetadata = new ListMetadata
                {
                    Constructor = constructor,
                    ItemsType = type.GetGenericArguments()[0],
                    AddItem = BuildMethodAccessorWithOneParameter(type.GetMethod("Add")),
                    GetItem = BuildMethodAccessorWithOneParameterAndReturnValue(type.GetMethod("get_Item"))
                };
            }
            else
            {
                typeMetadata = new TypeMetadata
                {
                    Constructor = constructor,
                    PropertiesInfo = GetPropertiesAccessors(type),
                    InheritedClassType = GetInheritedType(inheritedEntityTypeAttribute, baseEntityName)
                };
            }
            typesMetadata.Add(type, typeMetadata);

            //Traverse all the entity properties
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var innerMetadata = ConstructTypesMetadata(property.PropertyType);
                typesMetadata.AddRange(innerMetadata);
            }

            return typesMetadata;
        }
        private static Dictionary<string, Accessors> GetPropertiesAccessors(Type type)
        {
            var propertiesAccessors = new Dictionary<string, Accessors>();
            var properties = type.GetProperties();
            foreach (var property in properties)
            {
                var accessors = new Accessors
                {
                    Type = property.PropertyType,
                    Set = BuildMethodAccessorWithOneParameter(property.GetSetMethod()),
                    Get = BuildMethodAccessorWithReturnValue(property.GetGetMethod()),
                    CustomAttributes = property.CustomAttributes
                };
                propertiesAccessors.Add(property.Name, accessors);
            }

            return propertiesAccessors;
        }

        public static string GetInheritedType(InheritedEntityTypeAttribute inheritedEntityTypeAttribute, string baseEntityName = null)
        {
            if (inheritedEntityTypeAttribute != null)
            {
                return !baseEntityName.IsNullOrEmpty() ? $"{baseEntityName}.{inheritedEntityTypeAttribute.Type}" : inheritedEntityTypeAttribute.Type;
            }
            return null;
        }

        #region Expressions Builders

        private static Func<object> BuildEmptyConstructorAccessor(Type type)
        {
            try
            {
                var constructor = type.GetConstructor(BindingFlags.Instance | BindingFlags.Public, null, CallingConventions.HasThis, new Type[0], new ParameterModifier[0]);
                var constructorExpression = Expression.New(constructor);
                var lambda = Expression.Lambda<Func<object>>(constructorExpression);
                return lambda.Compile();
            }
            catch (Exception)
            {
                // ignored
            }
            return null;
        }
        private static Action<object, object> BuildMethodAccessorWithOneParameter(MethodInfo method)
        {
            var obj = Expression.Parameter(typeof(object), "o");
            var methodParameter = Expression.Parameter(typeof(object));

            Expression<Action<object, object>> expr =
                Expression.Lambda<Action<object, object>>(
                    Expression.Call(
                        Expression.Convert(obj, method.DeclaringType),
                        method,
                        Expression.Convert(methodParameter, method.GetParameters()[0].ParameterType)),
                    obj,
                    methodParameter);

            return expr.Compile();
        }
        private static Func<object, object> BuildMethodAccessorWithReturnValue(MethodInfo method)
        {
            var obj = Expression.Parameter(typeof(object), "o");
            Expression<Func<object, object>> expr =
                        Expression.Lambda<Func<object, object>>(
                            Expression.Convert(Expression.Call(Expression.Convert(obj, method.DeclaringType), method),
                                               typeof(object)), obj);
            return expr.Compile();
        }
        private static Func<object, object, object> BuildMethodAccessorWithOneParameterAndReturnValue(MethodInfo method)
        {
            var obj = Expression.Parameter(typeof(object), "o");
            var index = Expression.Parameter(typeof(object));

            Expression<Func<object, object, object>> expr =
                Expression.Lambda<Func<object, object, object>>(
                Expression.Convert(Expression.Call(
                          Expression.Convert(obj, method.DeclaringType),
                          method,
                          Expression.Convert(index, method.GetParameters()[0].ParameterType)), typeof(object))
                    ,
                    obj,
                    index);
            return expr.Compile();
        }

        #endregion

    }

    public class TypeMetadata
    {
        public Func<object> Constructor { get; set; }
        public Dictionary<string, Accessors> PropertiesInfo { get; set; }
        public string InheritedClassType { get; set; }
    }

    public class ListMetadata : TypeMetadata
    {
        public Type ItemsType { get; set; }
        public Action<object, object> AddItem { get; set; }
        public Func<object, object, object> GetItem { get; set; }
    }

    public class Accessors
    {
        public Type Type { get; set; }
        public IEnumerable<CustomAttributeData> CustomAttributes { get; set; }
        public Action<object, object> Set { get; set; }
        public Func<object, object> Get { get; set; }
    }
}
