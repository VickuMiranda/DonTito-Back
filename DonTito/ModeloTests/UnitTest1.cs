namespace ModeloTests
{
    public class UnitTest1
    {
        [Fact]
        public async void Test1()
        {
            await using var application = new WebApplicationFactory<>();
        }
    }
}