using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Impress
{
    /// <summary>
    /// Represent a value that might not be present.
    /// 
    /// Maybe is a monad and comes with several monad methods based on C# monad support.
    /// The methods follow LINQ name convention since Maybe can be understood as a list that can only contain 0 or 1 elements.  
    /// </summary>
    /// <typeparam name="T">The type of the value</typeparam>
    public struct Maybe<T> : IEnumerable<T>
    {

        public readonly static Maybe<T> Nothing = new Maybe<T>(false);

        /// <summary>
        /// Creates a Maybe from an object. 
        /// </summary>
        /// <typeparam name="X">the type of the object</typeparam>
        /// <param name="value">the object to encapsulate</param>
        /// <returns>Maybe.Nothing is value is null, else return an instance of Maybe encapsulating the value</returns>
        public static Maybe<X> ValueOf<X>(X value) where X : class
        {
            return ValueOfValue(value);
        }

        /// <summary>
        /// Creates a Maybe from an object. 
        /// </summary>
        /// <typeparam name="X">the type of the object</typeparam>
        /// <param name="value">the object to encapsulate</param>
        /// <returns>Maybe.Nothing is value is null, else return an instance of Maybe encapsulating the value</returns>
        private static Maybe<X> ValueOfValue<X>(X value)
        {
            if (value == null || (value is string text && string.IsNullOrEmpty(text)))
            {
                return Maybe<X>.Nothing;
            }
            return new Maybe<X>(value);
        }

        /// <summary>
        /// Creates a Maybe from an struct. 
        /// </summary>
        /// <typeparam name="X">the type of the struct</typeparam>
        /// <param name="value">the object to encapsulate</param>
        /// <returns>an instance of Maybe encapsulating the value</returns>
        public static Maybe<X> ValueOfStruct<X>(X value) where X : struct
        {
            return new Maybe<X>(value);
        }

        /// <summary>
        /// Return Maybe.Nothing;
        /// </summary>
        /// <typeparam name="X"></typeparam>
        /// <returns></returns>
        public static Maybe<X> ValueOfNothing<X>()
        {
            return Maybe<X>.Nothing;
        }


        private readonly T obj;

        private Maybe(bool hasValue)
        {
            this.HasValue = hasValue;
            this.obj = default(T);
        }

        internal Maybe(T obj)
        {
            this.obj = obj;
            HasValue = true;
        }

        /// <summary>
        /// Determines the value is present.
        /// </summary>
        public bool HasValue { get; }

        /// <summary>
        /// Returns the value inside the maybe object. If the value is not present
        /// and exception is thrown. You can check if the value is present using HasValue.
        /// 
        /// </summary>
        public T Value
        {
            get
            {
                return OrThrow();
            }
        }

        /// <summary>
        /// Returns the value inside the maybe object. If the value is not present, throws an exception.
        /// This method behaves as Value, but makes is semanticly obvious an exception will be thrown
        /// </summary>
        /// <returns></returns>
        public T OrThrow()
        {
            if (!HasValue)
            {
                throw new Exception("No value of type " + typeof(T).Name + " is present");
            }

            return obj;
        }

        /// <summary>
        /// Returns the value inside the maybe object. If the value is not present, return the given default value.
        /// </summary>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public T Or(T defaultValue)
        {
            return HasValue ? obj : defaultValue;
        }

        /// <summary>
        /// Returns the value inside the maybe object. If the value is not present, call the defaultValueSupplier function and return that value (it can be null).
        /// This method is preferable to Or() when the default value must be calculated or is not otherwise immediately available.
        /// </summary>
        /// <param name="defaultValueSupplier">A function that retrives the default value</param>
        /// <returns></returns>
        public T OrGet(Func<T> defaultValueSupplier)
        {
            return HasValue ? obj : defaultValueSupplier();
        }

        /// <summary>
        /// Returns the value inside the maybe object. If the value is not present, call the defaultValueSupplier function and return that value after encapsulating it in a Maybe
        /// This method is preferable to Or() when you want to continue to use mode monad methods instead of terminating and returning a value.
        /// </summary>
        /// <param name="defaultValueSupplier">A function that retrives the default value</param>
        /// <returns></returns>
        public Maybe<T> OrGetAlternative(Func<T> defaultValueSupplier)
        {
            return HasValue ? this : defaultValueSupplier().ToMaybe();
        }

        /// <summary>
        /// Returns the value inside the maybe object. If the value is not present, call the defaultValueSupplier function and return an already encapsulated value
        /// This method is preferable to Or() when you want to continue to use mode monad methods instead of terminating and returning a value.
        /// This method is preferable to OrGetAlternative(Func<T>) when the calculation it self may return a maybe.
        /// </summary>
        /// <param name="defaultValueSupplier">A function that retrives the default value</param>
        /// <returns></returns>
        public Maybe<T> OrGetAlternative(Func<Maybe<T>> defaultValueSupplier)
        {
            return HasValue ? this : defaultValueSupplier();
        }

        /// <summary>
        /// Returns the value inside the maybe object. If the value is not present, call the exceptionSupplier function and throw the exception returned by it.
        /// 
        /// There is not OrThrow(Exception) because that will create the exception and capture the stacktrace even if it is not needed. Using a lambda is more efficient because the exception will only be created if needed.
        /// </summary>
        /// <typeparam name="E">The type of the exception</typeparam>
        /// <param name="exceptionSupplier"></param>
        /// <returns></returns>
        public T OrThrow<E>(Func<E> exceptionSupplier) where E : Exception
        {
            if (HasValue)
            {
                return obj;
            }

            throw exceptionSupplier();
        }

        /// <summary>
        /// Does something with the object with the maybe object. If this maybe has no value, no action is taken.
        /// </summary>
        /// <param name="consumer"></param>
        public void Consume(Action<T> consumer)
        {
            if (HasValue)
            {
                consumer(obj);
            }
        }

        public bool Equals(Maybe<T> other)
        {
            if (this.HasValue)
            {
                return other.HasValue && this.Value.Equals(other.Value);
            }
            else
            {
                return !other.HasValue;
            }
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !obj.GetType().IsMaybeType())
            {
                return false;
            }

            var other = obj.AsMaybe();

            if (this.HasValue)
            {
                return other.HasValue && this.Value.Equals(other.Value);
            }
            else
            {
                return !other.HasValue;
            }
        }

        public override int GetHashCode()
        {
            return this.HasValue ? Value.GetHashCode() : 0;
        }

        public override string ToString()
        {
            return this.HasValue ? Value.ToString() : string.Empty;
        }

        /// <summary>
        /// Determines the given value is equal to the value inside the maybe.
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Is(T other)
        {
            return this.HasValue && this.Value.Equals(other);
        }

        /// <summary>
        /// Determines if the given predicate is true for the value inside the maybe.
        /// 
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns> true if the value is present and passes the given predicate, false otherwise.</returns>
        public bool Is(Func<T, bool> predicate)
        {
            return this.HasValue && predicate(this.Value);
        }

        /// <summary>
        /// Transforms to another Maybe object with the same value unless the present value equals the given value.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public Maybe<T> AlsoNothing(T value)
        {
            if (this.Is(value))
            {
                return Maybe<T>.Nothing;
            }
            return this;
        }

        /// <summary>
        /// Transforms to another Maybe object with the same value unless the given predicate evaluates to true.
        /// In that case return Maybe.Nothing.
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public Maybe<T> AlsoNothing(Func<T, bool> predicate)
        {
            if (this.Is(predicate))
            {
                return Maybe<T>.Nothing;
            }
            return this;
        }

        /// <summary>
        /// Transformes the maybe to it self. This method is marked as Obsolete to warn the programmer that it sould not be using it. However it will not be removed. It is really auxiliar method to make the mistake obvious.
        /// As ToMaybe is also an Extention method, is easy to invoque it over the maybe it self. So this method overloads that idiom
        /// and marks it with a compiler warning.
        /// </summary>
        /// <returns></returns>
        [Obsolete]
        public Maybe<T> ToMaybe()
        {
            return this;
        }

        public IEnumerable<T> AsEnumerable()
        {
            return this.HasValue ? Enumerable.Repeat(this.Value, 1) : Enumerable.Empty<T>();
        }

        public IEnumerator<T> GetEnumerator()
        {
            return AsEnumerable().GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return AsEnumerable().GetEnumerator();
        }
    }

    public static class MaybeMonadExtention
    {
        /// <summary>
        /// Transforms a maybe of a struct back into a Nullable objet for compatibility with existing APIs.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Nullable<T> ToNullable<T>(this Maybe<T> value) where T : struct
        {
            if (!value.HasValue)
            {
                return null;
            }
            else
            {
                return value.Value;
            }
        }

        /// <summary>
        /// If the value is present, applies a convertion to the given string.
        /// If the value is not present, return Maybe.Nothing.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Maybe<T> Convert<T>(this Maybe<string> value) where T : struct
        {
            if (!value.HasValue)
            {
                return Maybe<T>.Nothing;
            }
            else
            {
                try
                {
                    return GenericTypeConverter.GetConverter().Convert<T>(value.Value).ToMaybe();
                }
                catch (ConverterException)
                {
                    return Maybe<T>.Nothing;
                }
            }
        }

        /// <summary>
        /// Extends Nullable with the Or method similar to Maybe.Or. 
        /// The Or method substitutes the use of the ?? operator and allows for fluent calls.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static S Or<S>(this Nullable<S> value, S defaultValue) where S : struct
        {
            return value.HasValue ? value.Value : defaultValue;
        }

        /// <summary>
        /// Extends Nullable with the AlsoNothing method similar to Maybe.AlsoNothing. 
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Nullable<S> AlsoNothing<S>(this Nullable<S> nullable, S value) where S : struct
        {
            if (!nullable.HasValue || nullable.Value.Equals(value))
            {
                return (Nullable<S>)null;
            }
            return nullable;
        }

        /// <summary>
        /// Extends Nullable with the AlsoNothing method similar to Maybe.AlsoNothing. 
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Nullable<S> AlsoNothing<S>(this Nullable<S> nullable, Func<S, bool> predicate) where S : struct
        {
            if (!nullable.HasValue || predicate(nullable.Value))
            {
                return (Nullable<S>)null;
            }
            return nullable;
        }

        /// <summary>
        /// Extends Nullable with the Select method similar to Maybe.Select. 
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Nullable<V> Select<S, V>(this Nullable<S> m, Func<S, V> k)
            where S : struct
            where V : struct
        {
            return !m.HasValue ? (Nullable<V>)null : k(m.Value);
        }

        /// <summary>
        /// Extends Nullable with the Select method similar to Maybe.Select. 
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Nullable<V> Select<S, V>(this Nullable<S> m, Func<S, Nullable<V>> k)
            where S : struct
            where V : struct
        {
            return !m.HasValue ? (Nullable<V>)null : k(m.Value);
        }

        /// <summary>
        /// Extends Nullable with the SelectMany method similar to Maybe.SelectMany but converting the result to a Maybe. 
        /// 
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <param name="value"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Maybe<V> SelectMany<S, V>(this Nullable<S> m, Func<S, Maybe<V>> k)
            where S : struct
        {
            return !m.HasValue ? Maybe<V>.Nothing : k(m.Value);
        }

        /// <summary>
        /// Transforms a Nullable object to a Maybe object.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="m"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Maybe<S> ToMaybe<S>(this Nullable<S> value) where S : struct
        {
            return !value.HasValue ? Maybe<S>.Nothing : new Maybe<S>(value.Value);
        }

        /// <summary>
        /// Transforms a string object to a Maybe object.
        /// This operation will result in a  Maybe.Nothing is the string is null or empty.
        /// </summary>
        /// <typeparam name="S"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="m"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Maybe<string> ToMaybe(this string value)
        {
            return string.IsNullOrEmpty(value) ? Maybe<string>.Nothing : new Maybe<string>(value);
        }

        /// <summary>
        /// Utility method to negate a maybe of boolean.
        /// Equivalent to 
        /// 
        /// value.Select ( val => !val);
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Maybe<bool> Negate(this Maybe<bool> value)
        {
            return value.Select(it => !it);
        }

        /// <summary>
        /// Similar to the Or method, but returns string.Empty is no value is present in the maybe object.
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string OrEmpty(this Maybe<string> value)
        {
            return value.Or(string.Empty);
        }

        /// <summary>
        /// Preferable for to generate a maybe from a value.
        /// Returns  Maybe.Nothing if the value is null.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Maybe<T> ToMaybe<T>(this T value)
        {
            if (value == null)
            {
                return Maybe<T>.Nothing;
            }

            // treat the case where value is already a Maybe. 
            // not necessarily a Maybe of T. Assume is of type R where R is subtype of T
            // if its not return nothing
            var valueType = value.GetType();

            if (valueType.IsGenericType && valueType.GetGenericTypeDefinition() == typeof(Maybe<>))
            {
                bool hasValue = (bool)valueType.GetProperty("HasValue").GetGetMethod().Invoke(value, null);

                if (hasValue)
                {
                    var val = valueType.GetProperty("Value").GetGetMethod().Invoke(value, null);
                    if (val is T)
                    {
                        return new Maybe<T>((T)val);
                    }
                }
                return Maybe<T>.Nothing;
            }
            return new Maybe<T>((T)value);
        }

        /// <summary>
        /// Cast the present value. If the cast fails, return  Maybe.Nothing .
        /// </summary>
        /// <typeparam name="TOrigin"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Maybe<TTarget> MaybeCast<TOrigin, TTarget>(this TOrigin value)
        {
            try
            {
                object v = value;
                return value == null ? Maybe<TTarget>.Nothing : ((TTarget)v).ToMaybe();
            }
            catch (InvalidCastException)
            {
                return Maybe<TTarget>.Nothing;
            }
        }

        /// <summary>
        /// Cast the present value. If the cast fails, return  Maybe.Nothing .
        /// </summary>
        /// <typeparam name="TOrigin"></typeparam>
        /// <typeparam name="TTarget"></typeparam>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Maybe<TTarget> MaybeCast<TOrigin, TTarget>(this Maybe<TOrigin> value)
        {
            try
            {
                return !value.HasValue ? Maybe<TTarget>.Nothing : ((TTarget)(object)value.Value).ToMaybe();
            }
            catch (InvalidCastException)
            {
                return Maybe<TTarget>.Nothing;
            }
        }

        /// <summary>
        /// Transform the Maybe acording to the given function. Similar to IEnumerable.Select.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="m"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Maybe<V> Select<T, V>(this Maybe<T> m, Func<T, V> k)
        {
            return !m.HasValue ? Maybe<V>.Nothing : k(m.Value).ToMaybe();
        }

        /// <summary>
        /// Transform the Maybe acording to the given function. Similar to IEnumerable.Select.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="m"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Maybe<V> Select<T, V>(this Maybe<T> m, Func<T, Nullable<V>> k) where V : struct
        {
            return !m.HasValue ? Maybe<V>.Nothing : ToMaybe(k(m.Value));
        }

        /// <summary>
        /// Transform the Maybe acording to the given function. Similar to IEnumerable.Select.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="m"></param>
        /// <param name="k"></param>
        /// <returns></returns>
        public static Maybe<V> SelectMany<T, V>(this Maybe<T> m, Func<T, Maybe<V>> k)
        {
            return !m.HasValue ? Maybe<V>.Nothing : k(m.Value);
        }

        /// <summary>
        /// Monadic operation so computable expressions can be used with Maybe. Example
        /// 
        /// var a = 1.ToMaybe();
        /// var b = 3.ToMaybe();
        /// 
        /// var c = from x in a
        ///         from y in b
        ///         select x + y;
        /// 
        /// Assert.AreEqual(3.ToMaybe(), c);
        /// 
        /// this allows to safe operation even if any of the values is not present.
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="U"></typeparam>
        /// <typeparam name="V"></typeparam>
        /// <param name="m"></param>
        /// <param name="k"></param>
        /// <param name="s"></param>
        /// <returns></returns>
        public static Maybe<V> SelectMany<T, U, V>(this Maybe<T> m, Func<T, Maybe<U>> k, Func<T, U, V> s)
        {
            return m.SelectMany(x => k(x).SelectMany(y => s(x, y).ToMaybe()));
        }

        /// <summary>
        /// Operate directly on the contained value without unwraping it first. If the value is not present, no action will be taken.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public static Maybe<T> Make<T>(this Maybe<T> m, Action<T> action)
        {
            if (m.HasValue)
            {
                action(m.Value);
            }
            return m;
        }

        /// <summary>
        /// Compares two Maybe objects that augment a type that implements the IComparable interface.
        /// CompareTo considers that Maybe.Nothing is less than every other value of Maybe.
        /// </summary>
        /// <typeparam name="T">A type that implements IComparable</typeparam>
        /// <param name="x">The value to compare</param>
        /// <param name="y">The value to compare with</param>
        /// <returns>0 if the values are the same, or both are Nothing. 1 if x is larger than y, or y is Nothing, and -1 if y is larger than x or x is Nothing.</returns>
        public static int CompareTo<T>(this Maybe<T> x, Maybe<T> y) where T : IComparable<T>
        {
            if (x.HasValue && y.HasValue)
            {
                return x.Value.CompareTo(y.Value);
            }
            else if (!x.HasValue && !y.HasValue)
            {
                return 0;
            }
            else if (x.HasValue)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// Compares two Maybe objects using a given implementation of an IComparer
        /// CompareTo considers that Maybe.Nothing is less than every other value of Maybe.
        /// </summary>
        /// <typeparam name="T">A type that implements IComparable</typeparam>
        /// <param name="x">The value to compare</param>
        /// <param name="y">The value to compare with</param>
        /// <param name="comparer">The IComparer to use in the comparation</param>
        /// <returns>0 if the values are the same, or both are Nothing. 1 if x is larger than y, or y is Nothing, and -1 if y is larger than x or x is Nothing.</returns>
        public static int CompareTo<T>(this Maybe<T> x, Maybe<T> y, IComparer<T> comparer)
        {
            if (x.HasValue && y.HasValue)
            {
                return comparer.Compare(x.Value, y.Value);
            }
            else if (!x.HasValue && !y.HasValue)
            {
                return 0;
            }
            else if (x.HasValue)
            {
                return 1;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// Tansforms an IEnumarable of Maybe<T> values to a Enumerable of T values removing all non present values in the process
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="all"></param>
        /// <returns></returns>
        public static IEnumerable<T> Compact<T>(this IEnumerable<Maybe<T>> all)
        {
            return all.Where(m => m.HasValue).Select(m => m.Value);
        }

        /// <summary>
        /// Tansforms an IEnumarable of Nullable<T> values to a Enumerable of T values removing all non present values in the process
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="all"></param>
        /// <returns></returns>
        public static IEnumerable<S> Compact<S>(this IEnumerable<Nullable<S>> all) where S : struct
        {
            return all.Where(m => m.HasValue).Select(m => m.Value);
        }


        /// <summary>
        /// Transforms to another Maybe object with the same value unless the valus is not present.
        /// In that case return a Maybe object encapsulating the alternative given value.
        /// 
        /// Obsolete: use OrGetAlternative() instead
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="alternative"></param>
        /// <returns></returns>
        [Obsolete]
        public static Maybe<T> WithAlternative<T>(this Maybe<T> m, T alternative)
        {
            if (!m.HasValue)
            {
                return alternative.ToMaybe();
            }
            return m;
        }

        /// <summary>
        /// Transforms to another Maybe object with the same value unless the valus is not present.
        /// In that case return the given alternative Maybe object.
        /// 
        /// Obsolete: use OrGetAlternative() instead
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="m"></param>
        /// <param name="alternative"></param>
        /// <returns></returns>
        [Obsolete]
        public static Maybe<T> WithAlternative<T>(this Maybe<T> m, Maybe<T> alternative)
        {
            if (!m.HasValue)
            {
                return alternative;
            }
            return m;
        }

        /// <summary>
        /// Tries to convert the given int to a valid value of enum E.
        /// If the E is not an enum or it does not contain a value conrresponding to the given int, return Maybe.Nothing
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Maybe<E> MaybeEnum<E>(this Maybe<int> m) where E : struct
        {
            if (!typeof(E).IsEnum)
            {
                return Maybe<E>.Nothing;
            }

            try
            {
                return m.Select(v => v.ToEnum<E>());
            }
            catch (EnumConversionException)
            {
                return Maybe<E>.Nothing;
            }
        }

        /// <summary>
        /// Tries to convert the given name to a valid value of enum E.
        /// If the E is not an enum or it does not contain a value conrresponding to the given name, return Maybe.Nothing
        /// </summary>
        /// <typeparam name="E"></typeparam>
        /// <param name="m"></param>
        /// <returns></returns>
        public static Maybe<E> MaybeEnum<E>(this Maybe<string> m) where E : struct
        {
            if (typeof(E).IsEnum && m.HasValue)
            {
                E value;
                if (Enum.TryParse(m.Value, out value))
                {
                    return value.ToMaybe();
                }
            }

            return Maybe<E>.Nothing;
        }

        /// <summary>
        /// Applies a binary operation to the contents of two maybe values, similar to IEnumerable.Zip.
        /// 
        /// This method is also equivalent to the do-notation 
        /// 
        /// var result = from a in x
        ///              from b in y
        ///              select a + b; 
        /// 
        /// var result = x.zip(y, (a,b) => a + b);
        /// 
        /// </summary>
        /// <typeparam name="T">The type in the first Maybe</typeparam>
        /// <typeparam name="S">The type in the second Maybe</typeparam>
        /// <typeparam name="R">The result type</typeparam>
        /// <param name="m">The target maybe</param>
        /// <param name="other">The other maybe to zip with the target.</param>
        /// <param name="function">The function to apply if both maybe have values</param>
        /// <returns>The result of the function wraped in a maybe</returns>
        public static Maybe<R> Zip<T, S, R>(this Maybe<T> m, Maybe<S> other, Func<T, S, R> function)
        {
            if (m.HasValue && other.HasValue)
            {
                return function(m.Value, other.Value).ToMaybe();
            }
            return Maybe<R>.Nothing;
        }

        /// <summary>
        /// Applies a binary operation to the contents of two maybe values, similar to IEnumerable.Zip.
        /// 
        /// This method is also equivalent to the do-notation 
        /// 
        /// var result = from a in x
        ///              from b in y
        ///              select a + b; 
        /// 
        /// var result = x.zip(y, (a,b) => a + b);
        /// 
        /// </summary>
        /// <typeparam name="T">The type in the first Maybe</typeparam>
        /// <typeparam name="S">The type in the second Maybe</typeparam>
        /// <typeparam name="R">The result type</typeparam>
        /// <param name="m">The target maybe</param>
        /// <param name="other">The other maybe to zip with the target.</param>
        /// <param name="function">The function to apply if both maybe have values</param>
        /// <returns>The result of the function</returns>
        public static Maybe<R> Zip<T, S, R>(this Maybe<T> m, Maybe<S> other, Func<T, S, Maybe<R>> function)
        {
            if (m.HasValue && other.HasValue)
            {
                return function(m.Value, other.Value);
            }
            return Maybe<R>.Nothing;
        }
    }


    public static class MaybeReflection
    {
        public static bool IsMaybeType(this Type type)
        {
            return type.IsGenericType && type.GetGenericTypeDefinition().Equals(typeof(Maybe<>));
        }

        /// <summary>
        /// Converts the type of an object that is a Maybe of some T, to a maybe of object.
        /// </summary>
        /// <param name="someObject"></param>
        /// <returns></returns>
        public static Maybe<object> AsMaybe(this object someObject)
        {
            if (someObject != null && someObject.GetType().IsMaybeType())
            {
                if ((bool)someObject.GetType().GetProperty("HasValue").GetGetMethod().Invoke(someObject, new object[0]))
                {
                    var obj = someObject.GetType().GetProperty("Value").GetGetMethod().Invoke(someObject, new object[0]);
                    return obj.ToMaybe();
                }
            }
            return Maybe<object>.Nothing;
        }

        public static Type ReadInnerType(this Type type)
        {
            if (type.IsMaybeType())
            {
                return type.GetGenericArguments()[0];
            }
            return null;
        }


        public static object ReflectionMaybeNothing(Type returnValueType)
        {
            Type generic = typeof(Maybe<>);
            Type constructed = generic.MakeGenericType(new Type[] { returnValueType });

            return constructed.GetMethod("ValueOfNothing").MakeGenericMethod(new Type[] { returnValueType }).Invoke(null, null);
        }

        public static object ReflectionMaybeStruct(Type returnValueType, object val)
        {
            Type generic = typeof(Maybe<>);
            Type constructed = generic.MakeGenericType(new Type[] { returnValueType });

            return constructed.GetMethod("ValueOfStruct").MakeGenericMethod(new Type[] { returnValueType }).Invoke(null, new object[] { val });
        }


        public static object ReflectionMaybe(Type returnValueType, object val)
        {
            Type generic = typeof(Maybe<>);
            Type constructed = generic.MakeGenericType(new Type[] { returnValueType });

            return constructed.GetMethod("ValueOfValue", BindingFlags.NonPublic | BindingFlags.Static).MakeGenericMethod(new Type[] { returnValueType }).Invoke(null, new object[] { val });
        }

    }


}
