using System;
using Xunit;
using Fody;
#pragma warning disable 618

public class Weaver_Test
{
    static TestResult testResult;

    public Weaver_Test()
    {
        var weavingTask = new ModuleWeaver();
        testResult = weavingTask.ExecuteTestRun("AssemblyToProcess.dll");

    }

    [Fact]
    void First()
    {
        var type = testResult.Assembly.GetType("AssemblyToProcess.DemoPUTO");
        var instance = (dynamic)Activator.CreateInstance(type);

        //var name = instance.Name2;
    }
}
