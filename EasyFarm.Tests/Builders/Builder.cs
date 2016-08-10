using System;
using System.Linq.Expressions;
using System.Reflection;

namespace EasyFarm.Tests.Builders
{  
    public class Builder<T>
    {
        private readonly T _instance;

        public Builder(T instance)
        {
            _instance = instance;
        }

        public T Build()
        {
            return _instance;
        }

        public Builder<T> With<TProperty>(
            Expression<Func<T, TProperty>> memberPicker,
            TProperty value)
        {
            var me = memberPicker.Body as MemberExpression;
            if (me == null) throw new ArgumentException("Argument is most likely not a property or field.");

            var pi = me.Member as PropertyInfo;
            if (pi != null)
            {
                if (pi.GetSetMethod() == null) throw new ArgumentException("Property is not writable.");
                pi.SetValue(_instance, value);
            }

            var fi = me.Member as FieldInfo;
            fi?.SetValue(_instance, value);

            return this;
        }
    }
}
