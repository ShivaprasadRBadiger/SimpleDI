using System;
using System.Collections.Generic;
using com.srb.DependencyInjection;

using NUnit.Framework;

public class TestClassA
{
  public string Message { get; set; }
}

public class TestClassB
{
  public TestClassA Dependency { get; }

  public TestClassB(TestClassA dependency)
  {
    Dependency = dependency;
  }
}



public class DependencyProviderTests
{
  private SimpleDI _dependencyProvider;

  [SetUp]
  public void Setup()
  {
    _dependencyProvider = new SimpleDI();
  }

  [Test]
  public void Bind_SingletonInstanceExists_ThrowsArgumentException()
  {
    // Arrange
    var testClassA1 = new TestClassA { Message = "Hello world!" };
    _dependencyProvider.Bind(testClassA1);

    // Act and assert
    Assert.Throws<ArgumentException>(() => _dependencyProvider.Bind(testClassA1));
  }

  [Test]
  public void GetInstance_SingletonInstanceDoesNotExist_ReturnsNewInstance()
  {
    // Act
    var testClassA = _dependencyProvider.GetInstance<TestClassA>();

    // Assert
    Assert.IsNotNull(testClassA);
  }

  [Test]
  public void GetInstance_SingletonInstanceExists_ReturnsSingletonInstance()
  {
    // Arrange
    var testClassA1 = new TestClassA { Message = "Hello world!" };
    _dependencyProvider.Bind(testClassA1);

    // Act
    var testClassA2 = _dependencyProvider.GetInstance<TestClassA>();

    // Assert
    Assert.AreEqual(testClassA1, testClassA2);
  }

  [Test]
  public void GetInstance_WithDependency_ReturnsInstanceWithDependency()
  {
    // Arrange
    var testClassA = new TestClassA { Message = "Hello world!" };
    _dependencyProvider.Bind(testClassA);

    // Act
    var testClassB = _dependencyProvider.GetInstance<TestClassB>();

    // Assert
    Assert.IsNotNull(testClassB);
    Assert.AreEqual(testClassA, testClassB.Dependency);
  }


}
