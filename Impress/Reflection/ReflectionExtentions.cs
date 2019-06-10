using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Impress.Reflection
{
    public static class ReflectionExtentions
    {

        public static void CopyTo<T>(this T source, T target)
        {
            foreach (PropertyInfo property in source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                object value = property.GetGetMethod().Invoke(source, null);

                property.GetSetMethod().Invoke(target, new object[] { value });

            }
        }

        public static void CopyTo<T>(this T source, T target, Func<PropertyInfo, bool> filter)
        {
            foreach (PropertyInfo property in source.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                if (filter(property))
                {
                    object value = property.GetGetMethod().Invoke(source, null);

                    property.GetSetMethod().Invoke(target, new object[] { value });
                }
            }
        }

        public static T Make<T>(this T obj, Action<T> action)
        {
            if (obj != null)
            {
                action(obj);
            }
            return obj;
        }

        public static Maybe<T> Make<T>(this Maybe<T> obj, Action<T> action)
        {
            if (obj.HasValue)
            {
                action(obj.Value);
            }
            return obj;
        }

        public static Maybe<object> With(this object obj, string propertyName)
        {
            if (obj == null || string.IsNullOrEmpty(propertyName))
            {
                return Maybe<object>.Nothing;
            }
            return obj.GetType().GetProperty(propertyName).GetGetMethod().Invoke(obj, null).ToMaybe();
        }

        public static Maybe<object> With(this Maybe<object> obj, string propertyName)
        {
            if (!obj.HasValue || string.IsNullOrEmpty(propertyName))
            {
                return Maybe<object>.Nothing;
            }
            return obj.Value.GetType().GetProperty(propertyName).GetGetMethod().Invoke(obj, null).ToMaybe();
        }

        public static bool IsCollection(this Type type)
        {
            return type.IsArray ||
                type.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(ICollection<>))) ||
                type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(ICollection<>));
        }

        public static bool IsReadOnlyCollection(this Type type)
        {
            return type.IsArray ||
                type.GetInterfaces().Any(x => x.IsGenericType && (x.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>))) ||
                type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(IReadOnlyCollection<>));
        }

        public static bool IsNullable(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Nullable<>);
        }

        public static bool IsEnumerable(this Type type)
        {
            return typeof(IEnumerable<object>).IsAssignableFrom(type);
        }

        public static bool IsMaybe(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition() == typeof(Maybe<>);
        }

    }
}
