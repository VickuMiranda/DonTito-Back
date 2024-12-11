using Models.Models;

namespace ModeloTest
{
    public class MarcaShould
    {
        [Fact]
        public void ValidarValoresMarca()
        {
            // Arrange
            string expected = "Mak";
            var sut = new Marca()
            {
                Nombre = expected
            };

            // Act
            var actual = sut.Nombre;

            // Assert
            Assert.Equal(expected, actual);
        }

    }
}
