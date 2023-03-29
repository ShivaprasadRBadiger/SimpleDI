
# SimpleDI

SimpleDI is a simple dependency injection container for .NET applications. It provides a lightweight and easy-to-use solution for managing dependencies between objects in your application.

## Usage

### Creating a SimpleDI instance

csharpCopy code

`SimpleDI.SimpleDI di = new SimpleDI.SimpleDI();` 

### Registering a Singleton Instance


`MyClass myInstance = new MyClass();
di.Bind(myInstance);` 

### Resolving a Singleton Instance


`MyClass myInstance = di.GetInstance<MyClass>();` 

### Resolving an Instance with Dependencies


`MyClass myInstance = di.GetInstance<MyClass>();` 

### Handling Circular Dependencies

SimpleDI can detect and handle circular dependencies. If a circular dependency is detected, SimpleDI will throw an `InvalidOperationException` with a message indicating the type that caused the circular dependency.

### Example

Here's a simple example of how you might use SimpleDI in your application:


`using System;

namespace MyApplication
{
    public interface IService
    {
        void DoSomething();
    }

    public class Service : IService
    {
        private readonly ILogger _logger;

        public Service(ILogger logger)
        {
            _logger = logger;
        }

        public void DoSomething()
        {
            _logger.Log("Doing something...");
        }
    }

    public interface ILogger
    {
        void Log(string message);
    }

    public class ConsoleLogger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }

    public class Application
    {
        private readonly IService _service;

        public Application(IService service)
        {
            _service = service;
        }

        public void Run()
        {
            _service.DoSomething();
        }
    }

    public class Program
    {
        public static void Main(string[] args)
        {
            SimpleDI.SimpleDI di = new SimpleDI.SimpleDI();
            di.Bind<ILogger>(new ConsoleLogger());
            di.Bind<IService>(new Service(di.GetInstance<ILogger>()));
            Application app = new Application(di.GetInstance<IService>());
            app.Run();
        }
    }
}` 

In this example, we have two classes that depend on an interface: `Service` and `Application`. We also have an implementation of the `ILogger` interface called `ConsoleLogger`.

We create a `SimpleDI` instance and register an instance of `ConsoleLogger` with it. We then use SimpleDI to resolve an instance of `Service`, passing in the `ILogger` instance we registered. Finally, we use SimpleDI to resolve an instance of `Application`, passing in the `IService` instance we resolved earlier.

When we run the application, we should see the message "Doing something..." printed to the console.
