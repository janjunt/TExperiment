using System;
using NPOI.SS.UserModel;
using System.Collections.Concurrent;
using System.Reflection;
using System.Linq.Expressions;

namespace TExperiment.Excel.Builders
{
    public class RenderContext
    {
        private ConcurrentDictionary<string, Func<object,object>> _cachedAccessor = new ConcurrentDictionary<string, Func<object, object>>();

        public ISheet Sheet { get; set; }
        public RenderContext ParentContext { get; set; }

        public object Data { get; set; }


        public object GetValue(string name)
        {
            return _cachedAccessor.GetOrAdd(name, n =>
            {
                var dataType = Data.GetType();
                PropertyInfo p = dataType.GetProperty(name);
                if (p == null)
                {
                    if (ParentContext != null)
                    {
                        return ParentContext.GetValue(name);
                    }

                    throw new ArgumentException($"没有找到指定名称({name})的属性");
                }

                var paramExp = Expression.Parameter(dataType);
                var convertExp = Expression.Convert(paramExp, dataType);
                var propertyExp = Expression.Property(convertExp, p);

                var getValue = Expression.Lambda<Func<object, object>>(propertyExp, paramExp).Compile();

                return getValue(Data);
            });
        }

        public RenderContext CreateChildContext(Object data)
        {
            return new RenderContext()
            {
                Sheet = Sheet,
                Data = data,
                ParentContext = this,
            };
        }
    }
}
