
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace com.srb.DependencyInjection
{
	public class SimpleDI
	{
		private static SimpleDI _globalInstance;
		public static SimpleDI Default => _globalInstance ??= new SimpleDI();
	
		private Dictionary<Type, object> _singletons = new Dictionary<Type, object>();
		private Stack<Type> _resolutionStack = new Stack<Type>();

		public void Bind<T>(T instance)
		{
			Type type = typeof(T);

			if (!_singletons.ContainsKey(type))
			{
				_singletons[type] = instance;
			}
			else
			{
				throw new ArgumentException($"Singleton instance of type {type} already exists.");
			}
		}

		public T GetInstance<T>()
		{
			Type type = typeof(T);
			return (T)GetInstance(type);
		}

		public object GetInstance(Type type)
		{
			if (_resolutionStack.Contains(type))
			{
				throw new InvalidOperationException($"Circular dependency detected for type {type}");
			}

			_resolutionStack.Push(type);
			object instance;

			if (_singletons.TryGetValue(type, out instance))
			{
				_resolutionStack.Pop();
				return instance;
			}

			ConstructorInfo constructor = type.GetConstructors().Single();
			ParameterInfo[] parameters = constructor.GetParameters();
			object[] arguments = new object[parameters.Length];

			for (int i = 0; i < parameters.Length; i++)
			{
				Type parameterType = parameters[i].ParameterType;

				if (parameterType == type)
				{
					arguments[i] = CreateProxy(type);
				}
				else
				{
					arguments[i] = GetInstance(parameterType);
				}
			}

			instance = Activator.CreateInstance(type, arguments);
			_singletons[type] = instance;

			_resolutionStack.Pop();
			return instance;
		}

		private object CreateProxy(Type type)
		{
			return new Lazy<object>(() => GetInstance(type)).Value;
		}
	}
}