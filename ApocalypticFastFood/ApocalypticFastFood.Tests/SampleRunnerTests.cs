using Xunit;
using ApocalypticFastFood.Samples;

namespace ApocalypticFastFood.Tests;

public class SampleRunnerTests
{
    [Fact]
    public void RunDemos_DoesNotThrow()
    {
        // Ensure demo code runs without throwing exceptions
        SampleRunner.RunDemos();
    }
}
