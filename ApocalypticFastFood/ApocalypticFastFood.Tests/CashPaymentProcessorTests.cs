using Xunit;

namespace ApocalypticFastFood.Tests
{
    public class CashPaymentProcessorTests
    {
        [Fact]
        public void ProcessCash_DoesNotThrow()
        {
            // Arrange
            var processor = new ApocalypticFastFood.CashPaymentProcessor();

            // Act
            processor.ProcessCash(10.0);

            // Assert
            // If no exception was thrown, the processor behaved as expected.
            Assert.True(true);
        }
    }
}
