namespace WorkingWithKafkaAndTests.Test.Unit.Extensions;

public static class TaskExtensions
{
    public static async Task<T> WithTimeout<T>(this Task<T> task, TimeSpan timeout)
    {
        var delayTask = Task.Delay(timeout);
        if (await Task.WhenAny(task, delayTask) == delayTask)
            throw new TimeoutException($"The task did not complete within the specified timeout of {timeout}.");
        return await task;
    }
    // Overload for Task
    public static async Task WithTimeout(this Task task, TimeSpan timeout)
    {
        var delayTask = Task.Delay(timeout);
        if (await Task.WhenAny(task, delayTask) == delayTask)
            throw new TimeoutException($"The task did not complete within the specified timeout of {timeout}.");
    }
}