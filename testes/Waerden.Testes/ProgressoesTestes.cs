using Conde.Waerden;
using Xunit;

namespace Conde.Waerden.Testes;

public class ProgressoesTestes
{
    [Theory]
    [InlineData(1, 3, 0)]
    [InlineData(2, 3, 0)]
    [InlineData(3, 3, 1)]
    [InlineData(4, 3, 2)]
    [InlineData(5, 3, 4)]
    [InlineData(6, 3, 6)]
    [InlineData(7, 3, 9)]
    [InlineData(8, 3, 12)]
    [InlineData(9, 3, 16)]
    [InlineData(10, 3, 20)]
    public void OGeradorConcordaComAContaFechada(int n, int k, long esperado)
    {
        Assert.Equal(esperado, Progressoes.Todas(n, k).Count);
        Assert.Equal(esperado, Progressoes.Quantidade(n, k));
    }

    [Fact]
    public void OsDoisCaminhosConcordamEmTodoTamanhoEComprimento()
    {
        for (var n = 0; n <= 40; n++)
        {
            for (var k = 1; k <= 6; k++)
            {
                Assert.Equal(Progressoes.Quantidade(n, k), Progressoes.Todas(n, k).Count);
            }
        }
    }

    [Fact]
    public void TodaProgressaoTemPassoConstanteEPositivo()
    {
        foreach (var p in Progressoes.Todas(20, 4))
        {
            var passo = p[1] - p[0];
            Assert.True(passo > 0);
            for (var i = 2; i < p.Length; i++)
            {
                Assert.Equal(passo, p[i] - p[i - 1]);
            }
        }
    }

    [Fact]
    public void TodaProgressaoCabeNaFaixa()
    {
        foreach (var p in Progressoes.Todas(15, 3))
        {
            Assert.All(p, x => Assert.InRange(x, 1, 15));
        }
    }

    [Fact]
    public void NenhumaProgressaoSeRepete()
    {
        var todas = Progressoes.Todas(25, 3).Select(p => string.Join(",", p)).ToList();
        Assert.Equal(todas.Count, todas.Distinct().Count());
    }

    [Fact]
    public void ComComprimentoUmTodoNumeroEUmaProgressao()
    {
        Assert.Equal(10, Progressoes.Todas(10, 1).Count);
        Assert.Equal(10, Progressoes.Quantidade(10, 1));
    }

    [Fact]
    public void AsProgressoesPorFimSomamOTotal()
    {
        var porFim = Progressoes.PorFim(14, 3);
        Assert.Equal(Progressoes.Quantidade(14, 3), porFim.Sum(l => (long)l.Count));
    }

    [Fact]
    public void CadaProgressaoEstaNaCaixaDoProprioUltimoTermo()
    {
        var porFim = Progressoes.PorFim(12, 3);
        for (var fim = 0; fim < porFim.Length; fim++)
        {
            Assert.All(porFim[fim], p => Assert.Equal(fim, p[^1]));
        }
    }

    [Fact]
    public void ArgumentosInvalidosNaoSaoAceitos()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => Progressoes.Todas(-1, 3));
        Assert.Throws<ArgumentOutOfRangeException>(() => Progressoes.Todas(10, 0));
        Assert.Throws<ArgumentOutOfRangeException>(() => Progressoes.Quantidade(-1, 3));
        Assert.Throws<ArgumentOutOfRangeException>(() => Progressoes.Quantidade(10, 0));
    }
}
