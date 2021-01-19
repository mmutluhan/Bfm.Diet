using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Bfm.Diet.Core.Cache.Redis;
using Bfm.Diet.Core.Dependency;
using Bfm.Diet.Core.Json;
using Castle.DynamicProxy;
using Newtonsoft.Json;
using Bfm.Diet.Core.Extensions;
using System.Linq.Expressions;
using System.Text;
using System.Reflection;

//using Bfm.Diet.Core.Cache.Memory;

namespace Bfm.Diet.Core.Interceptor.Attributes
{
    public class CacheResults : AttributeInterceptor
    {
        private RedisCache _cacheManager;
        //public MemoryCache CacheManager
        //{
        //    get
        //    {
        //        return _cacheManager ??= (MemoryCache) ServiceResolver.ServiceProvider.GetService(typeof(MemoryCache));
        //    }
        //}

        public RedisCache CacheManager
        {
            get
            {
                return _cacheManager ??= (RedisCache) ServiceResolver.ServiceProvider.GetService(typeof(RedisCache));
            }
        } 

        public int Lifetime { get; set; }

        protected override void OnBefore(IInvocation invocation)
        {
            var key = CalculateCacheKey(invocation);
            var result = CacheManager.GetOrDefault(key);
            if (result != null)
                invocation.ReturnValue = result;
            Debug.WriteLine("Cache OnBefore ");
        }

        protected override void OnSuccess(IInvocation invocation)
        {
            var key = CalculateCacheKey(invocation); 

            CacheManager.Set(key, invocation.ReturnValue, TimeSpan.FromHours(1), TimeSpan.FromSeconds(Lifetime));
        }
         
        private string CalculateCacheKey(IInvocation invocation)
        {
            foreach (var argument in invocation.Arguments)
            {
                var argumentTyped = ObjectExtension.Cast(argument, argument.GetType());
                var args = Extract(argumentTyped);

            }

            return string.Empty;
        }

        private object Extract<TEntity>(Expression<Func<TEntity, bool>> expression)
        {
            var T = default(TEntity);
            var stack = new Stack<string>();
            Expression expression1 = expression.Body;
            while (expression1 != null)
            {
                if (expression1.NodeType == ExpressionType.Call)
                {
                    var methodCallExpression = (MethodCallExpression)expression1;
                    if (IsSingleArgumentIndexer(methodCallExpression))
                    {
                        stack.Push(string.Empty);
                        expression1 = methodCallExpression.Object;
                    }
                    else
                        break;
                }
                else if (expression1.NodeType == ExpressionType.ArrayIndex)
                {
                    var binaryExpression = (BinaryExpression)expression1;
                    stack.Push(string.Empty);
                    expression1 = binaryExpression.Left;
                }
                else if (expression1.NodeType == ExpressionType.MemberAccess)
                {
                    var memberExpression = (MemberExpression)expression1;
                    stack.Push("." + memberExpression.Member.Name);
                    expression1 = memberExpression.Expression;
                }
                else if (expression1.NodeType == ExpressionType.Parameter)
                {
                    stack.Push(string.Empty);
                    expression1 = null;
                }
                else if (expression1.NodeType == ExpressionType.Convert)
                {
                    var memberExp = ((UnaryExpression)expression1).Operand as MemberExpression;
                    stack.Push("." + memberExp.Member.Name);
                    expression1 = memberExp.Expression;
                }
                else if (expression1.NodeType == ExpressionType.AndAlso)
                {
                    var exp = ((BinaryExpression)expression1).Right;
                    if (exp.NodeType == ExpressionType.MemberAccess)
                    { 
                        var memberExpression = (MemberExpression)exp;

                        PropertyInfo outerProp = (PropertyInfo)memberExpression.Member;
                        MemberExpression innerMember = (MemberExpression)memberExpression.Expression;
                        FieldInfo innerField = (FieldInfo)innerMember.Member;
                        ConstantExpression ce = (ConstantExpression)innerMember.Expression;
                        object innerObj = ce.Value;
                        object outerObj = innerField.GetValue(innerObj);
                        string value = (string)outerProp.GetValue(outerObj, null);

                        stack.Push("." + memberExpression.Member.Name);
                        stack.Push("." + value);
                    }
                    else
                    {
                        var member = ((BinaryExpression)exp).Left;
                        var memberExpression = (MemberExpression)member;
                        stack.Push("." + memberExpression.Member.Name);
                        var value = ((BinaryExpression)exp).Right;
                        stack.Push("." + value);
                        expression1 = ((BinaryExpression)expression1).Left;
                    }
                   
                } 
            }

            if (stack.Count > 0 && string.Equals(stack.Peek(), ".model", StringComparison.OrdinalIgnoreCase))
                stack.Pop();
            if (stack.Count <= 0)
                return string.Empty;
            return (stack).Aggregate(((left, right) => left + right)).TrimStart(new[] { '.' });
        }

        private object GetValue(MemberExpression exp)
        {
            // expression is ConstantExpression or FieldExpression
            if (exp.Expression is ConstantExpression)
            {
                return (((ConstantExpression)exp.Expression).Value)
                    .GetType()
                    .GetField(exp.Member.Name)
                    .GetValue(((ConstantExpression)exp.Expression).Value);
            }
            else if (exp.Expression is MemberExpression)
            {
                return GetValue((MemberExpression)exp.Expression);
            }
            else
            {
                throw new NotImplementedException();
            }
        }


        private static bool IsSingleArgumentIndexer(Expression expression)
        {
            var methodExpression = expression as MethodCallExpression;
            if (methodExpression == null || methodExpression.Arguments.Count != 1)
                return false;
            return (methodExpression.Method.DeclaringType.GetDefaultMembers()).OfType<PropertyInfo>().Any((p => p.GetGetMethod() == methodExpression.Method));
        }

    }
}