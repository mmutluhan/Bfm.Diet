using System;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace Bfm.Diet.Core.Interceptor
{
    public abstract class AttributeInterceptor : AttributeInterceptorBase
    {
        protected virtual void OnBefore(IInvocation invocation)
        {
        }

        protected virtual void OnAfter(IInvocation invocation)
        {
        }

        protected virtual void OnException(IInvocation invocation, Exception e)
        {
        }

        protected virtual void OnSuccess(IInvocation invocation)
        {
        }

        public override void Intercept(IInvocation invocation)
        {
            var isSuccess = true;
            OnBefore(invocation);
            try
            {
                if (invocation.ReturnValue == null)
                    switch (invocation.GeMethodType())
                    {
                        case MethodType.AsyncAction:
                            invocation.Proceed();
                            invocation.ReturnValue = (Task) invocation.ReturnValue;
                            break;

                        case MethodType.AsyncFunction:
                            invocation.Proceed();
                            invocation.ReturnValue = (Task) invocation.ReturnValue;
                            break;

                        default:
                            invocation.Proceed();
                            break;
                    }
            }
            catch (Exception e)
            {
                isSuccess = false;
                OnException(invocation, e);
                throw;
            }
            finally
            {
                if (isSuccess) OnSuccess(invocation);
            }

            OnAfter(invocation);
        }
    }
}