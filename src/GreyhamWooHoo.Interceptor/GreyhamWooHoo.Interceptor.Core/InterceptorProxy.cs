using GreyhamWooHoo.Interceptor.Core.Contracts;
using GreyhamWooHoo.Interceptor.Core.Results;
using GreyhamWooHoo.Interceptor.Core.Rules;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;

namespace GreyhamWooHoo.Interceptor.Core
{
    /// <summary>
    /// Capture the ReturnValue of any methods
    /// </summary>
    /// <remarks>
    /// While the following reference is for Aspect Oriented Programming (static/source-code-level attributes), I have used the general pattern for this interceptor solution:
    /// Reference: https://www.c-sharpcorner.com/article/aspect-oriented-programming-in-c-sharp-using-dispatchproxy/
    /// </remarks>
    /// <typeparam name="T"></typeparam>
    public class InterceptorProxy<T> : DispatchProxy
    {
        private T _originalImplementation;
        private IEnumerable<IBeforeExecutionRule> _beforeExecutionRules;
        private IEnumerable<IStubExecutionRule> _stubExecutionRules;
        private IEnumerable<IAfterExecutionRule> _afterExecutionRules;
        private Action<Task> _taskWaiter;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            var name = targetMethod.Name;

            var beforeRule = _beforeExecutionRules.FirstOrDefault(f => f.MethodName == name);
            if(beforeRule != null)
            {
                try
                {
                    var parameters = targetMethod.GetParameters();
                    var beforeExecutionResult = new BeforeExectionResult(beforeRule, args, parameters);
                    beforeRule.Callback(beforeExecutionResult);
                }
                catch
                {
                    // Design Decision: if something goes wrong in the callout, we sink it and continue execution. 
                }
            }

            // If stubbed: use that value for the result and continue; else invoke the original method. 
            var stubbedRule = _stubExecutionRules.FirstOrDefault(f => f.MethodName == name);
            var result = default(object);
            if(stubbedRule != null)
            {
                result = stubbedRule.Value;
            }
            else
            {
                result = targetMethod.Invoke(_originalImplementation, args);
            }

            var afterRule = _afterExecutionRules.FirstOrDefault(f => f.MethodName == name);
            if (afterRule == null)
            {
                return result;
            }

            // ASSERTION: We need to callout with the return value. 
            var callbackResult = default(IAfterExecutionResult);

            if (result is Task task)
            {
                // Callback to wait for the task to finish. For some long running tasks, there is the chance a task might not complete before the test finishes... and therefore, the results will not be attached...!
                _taskWaiter(task);

                // Credit to:
                // https://www.c-sharpcorner.com/article/aspect-oriented-programming-in-c-sharp-using-dispatchproxy/");
                object taskResult = null;
                if (task.GetType().GetTypeInfo().IsGenericType && task.GetType().GetGenericTypeDefinition() == typeof(Task<>))
                {
                    var property = task.GetType().GetTypeInfo().GetProperties().FirstOrDefault(p => p.Name == "Result");
                    if (property != null)
                    {
                        taskResult = property.GetValue(task);
                    }

                    callbackResult = new AfterExecutionResult(afterRule, true, taskResult);
                }
                else
                {
                    callbackResult = new AfterExecutionResult(afterRule);
                }
            }
            else
            {
                // We now callout with the original value: it is better the recipient serialize / deserialize this by casting to the expected type first. 
                if (targetMethod.ReturnType == typeof(void))
                {
                    callbackResult = new AfterExecutionResult(afterRule);
                }
                else
                {
                    callbackResult = new AfterExecutionResult(afterRule, true, result);
                }
            }

            try
            {
                afterRule.Callback(callbackResult);
            }
            catch(Exception)
            {
                // Design decision: if anything goes wrong in the callback, we do not want to change the result of invoking the method. Therefore, sink the exception. 
            }

            return result;
        }

        private void SetParameters(T originalImplementation, IEnumerable<IBeforeExecutionRule> beforeExecutionRules, IEnumerable<IStubExecutionRule> stubExecutionRules, IEnumerable<IAfterExecutionRule> afterExecutionRules, Action<Task> taskWaiter)
        {
            _originalImplementation = originalImplementation;
            _taskWaiter = taskWaiter;

            _beforeExecutionRules = beforeExecutionRules.Select(i => new BeforeExecutionRule(i.MethodName, i.Callback));
            _afterExecutionRules = afterExecutionRules.Select(i => new AfterExecutionRule(i.MethodName, i.Callback));
            _stubExecutionRules = stubExecutionRules.Select(i => new StubExecutionRule(i.MethodName, i.Value));
        }

        public static T Create(T originalImplementation, IEnumerable<IBeforeExecutionRule> beforeExecutionRules, IEnumerable<IStubExecutionRule> stubExecutionRules, IEnumerable<IAfterExecutionRule> afterExecutionRules, Action<Task> taskWaiter)
        {
            if (null == originalImplementation) throw new ArgumentNullException(nameof(originalImplementation));
            if (null == beforeExecutionRules) throw new ArgumentNullException(nameof(beforeExecutionRules));
            if (null == stubExecutionRules) throw new ArgumentNullException(nameof(stubExecutionRules));
            if (null == afterExecutionRules) throw new ArgumentNullException(nameof(afterExecutionRules));
            if (null == taskWaiter) throw new ArgumentNullException(nameof(taskWaiter));

            object proxy = Create<T, InterceptorProxy<T>>();

            ((InterceptorProxy<T>)proxy).SetParameters(originalImplementation, beforeExecutionRules, stubExecutionRules, afterExecutionRules, taskWaiter);

            return (T)proxy;
        }
    }
}
