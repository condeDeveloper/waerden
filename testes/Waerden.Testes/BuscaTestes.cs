using Conde.Waerden;
using Xunit;

namespace Conde.Waerden.Testes;

public class BuscaTestes
{
    [Fact]
    public void ABuscaEAForcaBrutaContamOMesmo()
    {
        for (var n = 1; n <= 14; n++)
        {
            var bruta = Varredura.Contar(n, 2, 3);
            var busca = new Busca(n, 2, 3);
            Assert.Equal(bruta.SemMonocromatica, busca.Contar());
        }
    }

    [Fact]
    public void ABuscaEAForcaBrutaConcordamComTresCores()
    {
        for (var n = 1; n <= 9; n++)
        {
            var bruta = Varredura.Contar(n, 3, 3);
            var busca = new Busca(n, 3, 3);
            Assert.Equal(bruta.SemMonocromatica, busca.Contar());
        }
    }

    [Fact]
    public void AColoracaoAchadaRealmenteNaoTemProgressao()
    {
        for (var n = 1; n <= 8; n++)
        {
            var achada = new Busca(n, 2, 3).Primeira();
            Assert.NotNull(achada);
            Assert.False(Juiz.TemMonocromatica(achada!, Progressoes.Todas(n, 3)));
        }
    }

    [Fact]
    public void ApartirDeNoveNaoExisteColoracaoComDuasCores()
    {
        for (var n = 9; n <= 14; n++)
        {
            Assert.Null(new Busca(n, 2, 3).Primeira());
        }
    }

    [Theory]
    [InlineData(2, 3, 9)]
    [InlineData(2, 4, 35)]
    [InlineData(3, 3, 27)]
    public void OsNumerosDeVanDerWaerden(int cores, int comprimento, int esperado)
    {
        Assert.Equal(esperado, Busca.Numero(cores, comprimento, esperado + 2));
    }

    [Fact]
    public void AntesDoNumeroSempreExisteColoracaoDepoisNunca()
    {
        var w = Busca.Numero(2, 3, 20);
        for (var n = 1; n < w; n++)
        {
            Assert.NotNull(new Busca(n, 2, 3).Primeira());
        }

        Assert.Null(new Busca(w, 2, 3).Primeira());
    }

    [Fact]
    public void LimiteCurtoDevolveZero()
    {
        Assert.Equal(0, Busca.Numero(2, 3, 5));
    }

    [Fact]
    public void OCustoDaBuscaParaDeCrescerDepoisDoNumero()
    {
        var nos = new List<long>();
        foreach (var n in new[] { 9, 12, 20, 30 })
        {
            var busca = new Busca(n, 2, 3);
            busca.Contar();
            nos.Add(busca.Nos);
        }

        Assert.All(nos, x => Assert.Equal(nos[0], x));
    }

    [Fact]
    public void ArgumentosInvalidosNaoSaoAceitos()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new Busca(-1, 2, 3));
        Assert.Throws<ArgumentOutOfRangeException>(() => new Busca(5, 0, 3));
        Assert.Throws<ArgumentOutOfRangeException>(() => Varredura.Quantidade(-1, 2));
        Assert.Throws<ArgumentOutOfRangeException>(() => Varredura.Quantidade(5, 0));
    }

    [Theory]
    [InlineData(3, 2, 8)]
    [InlineData(10, 2, 1024)]
    [InlineData(5, 3, 243)]
    [InlineData(0, 2, 1)]
    public void AQuantidadeDeColoracoesECoresElevadoAN(int n, int cores, long esperado)
    {
        Assert.Equal(esperado, Varredura.Quantidade(n, cores));
        Assert.Equal(esperado, Varredura.Contar(n, cores, 3).Coloracoes);
    }
}
