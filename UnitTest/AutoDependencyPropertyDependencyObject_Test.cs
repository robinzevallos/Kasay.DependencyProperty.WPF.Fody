using System;
using System.Windows;
using Fody;
using Xunit;

public class AutoDependencyPropertyDependencyObject_Test
{
    readonly Type type;
    readonly dynamic instance;

    public AutoDependencyPropertyDependencyObject_Test()
    {
        var weavingTask = new ModuleWeaver();
        var testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll");

        type = testResult.Assembly.GetType("AssemblyToProcess.pueblo.DemoDependencyObject");
        instance = (dynamic)Activator.CreateInstance(type);
    }

    [Theory]
    [InlineData("SomeName", "Lalo")]
    [InlineData("SomeNumber", 14)]
    [InlineData("SomeCondition", true)]
    public void AutoProperty_Test(String propertyName, Object value)
    {
        var dependencyProperty = (DependencyProperty)type.GetField($"{propertyName}Property").GetValue(null);
        instance.GetType().GetProperty(propertyName).SetValue(instance, value);

        Assert.Equal(value, instance.GetType().GetProperty(propertyName).GetValue(instance));
        Assert.Equal(value, instance.GetValue(dependencyProperty));
    }
}
