using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using Bfm.Diet.Core.Base;

namespace Bfm.Diet.Core.Extensions
{
    public static class LinqExtensions
    {
        public static KeyValuePair<Type, object>[] ResolveArgs<TEntity>(this Expression<Func<TEntity, bool>> expression)
        {
            var body = (MethodCallExpression) expression.Body;
            var values = new List<KeyValuePair<Type, object>>();

            foreach (var argument in body.Arguments)
            {
                var exp = ResolveMemberExpression(argument);
                var type = argument.Type;

                var value = GetValue(exp);

                values.Add(new KeyValuePair<Type, object>(type, value));
            }

            return values.ToArray();
        }


        public static MemberExpression ResolveMemberExpression(Expression expression)
        {
            if (expression is MemberExpression)
                return (MemberExpression) expression;
            if (expression is UnaryExpression)
                // if casting is involved, Expression is not x => x.FieldName but x => Convert(x.Fieldname)
                return (MemberExpression) ((UnaryExpression) expression).Operand;
            throw new NotSupportedException(expression.ToString());
        }

        private static object GetValue(MemberExpression exp)
        {
            // expression is ConstantExpression or FieldExpression
            if (exp.Expression is ConstantExpression)
                return ((ConstantExpression) exp.Expression).Value
                    .GetType()
                    .GetField(exp.Member.Name)
                    .GetValue(((ConstantExpression) exp.Expression).Value);
            if (exp.Expression is MemberExpression)
                return GetValue((MemberExpression) exp.Expression);
            throw new NotImplementedException();
        }

        public static Expression<Func<TEntity, bool>> UnboxExpression<TEntity>(this object source) 
        {
            var parameter = Expression.Parameter(typeof(object), "input");

            var cast = Expression.TypeAs(Expression.Convert(parameter, source.GetType()), typeof(object));

            var lambda = Expression.Lambda<Func<TEntity, bool>>(cast, parameter);

            var func = lambda.Compile();

            return  func(source);
        }
    }
}