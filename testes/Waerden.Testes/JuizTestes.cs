using Conde.Waerden;
using Xunit;

namespace Conde.Waerden.Testes;

public class JuizTestes
{
    private static int[] Colorir(string letras)
    {
        var cores = new int[letras.Length + 1];
        for (var i = 0; i < letras.Length; i++)
        {
            cores[i + 1] = letras[i] - 'A';
        }

        return cores;
    }

    [Fact]
    public void AColoracaoDeOitoQueEscapaNaoTemProgressao()
    {
        var progressoes = Progressoes.Todas(8, 3);
        Assert.False(Juiz.TemMonocromatica(Colorir("AABBAABB"), progressoes));
        Assert.Equal(0, Juiz.Quantas(Colorir("AABBAABB"), progressoes));
    }

    [Fact]
    public void TudoDeUmaCorSoTemTodasAsProgressoes()
    {
        var progressoes = Progressoes.Todas(9, 3);
        var cores = Colorir("AAAAAAAAA");
        Assert.True(Juiz.TemMonocromatica(cores, progressoes));
        Assert.Equal(progressoes.Count, Juiz.Quantas(cores, progressoes));
    }

    [Fact]
    public void UmaProgressaoObviaEEncontrada()
    {
        // 1, 2, 3 da mesma cor.
        var progressoes = Progressoes.Todas(4, 3);
        Assert.True(Juiz.TemMonocromatica(Colorir("AAAB"), progressoes));
    }

    [Fact]
    public void EstenderAColoracaoDeOitoComQualquerCorFechaUmaProgressao()
    {
        // E o nucleo de W(2,3) = 9: a unica coloracao de 8 que escapa (a menos
        // de troca de cores e espelho) nao sobrevive a uma posicao a mais.
        var progressoes = Progressoes.Todas(9, 3);
        Assert.True(Juiz.TemMonocromatica(Colorir("AABBAABBA"), progressoes));
        Assert.True(Juiz.TemMonocromatica(Colorir("AABBAABBB"), progressoes));
    }

    [Fact]
    public void OTextoMostraUmaLetraPorPosicao()
    {
        Assert.Equal("AABBAABB", Juiz.Texto(Colorir("AABBAABB")));
        Assert.Equal("ABC", Juiz.Texto(Colorir("ABC")));
    }

    [Fact]
    public void QuantasNuncaPassaDoTotalDeProgressoes()
    {
        var progressoes = Progressoes.Todas(10, 3);
        foreach (var (coloracao, _) in Enumerable.Range(0, 1024).Select(m =>
        {
            var cores = new int[11];
            for (var i = 1; i <= 10; i++)
            {
                cores[i] = (m >> (i - 1)) & 1;
            }

            return (cores, m);
        }))
        {
            Assert.InRange(Juiz.Quantas(coloracao, progressoes), 0, progressoes.Count);
        }
    }

    [Fact]
    public void NuloNaoEAceito()
    {
        Assert.Throws<ArgumentNullException>(() => Juiz.TemMonocromatica(null!, []));
        Assert.Throws<ArgumentNullException>(() => Juiz.TemMonocromatica(new int[3], null!));
        Assert.Throws<ArgumentNullException>(() => Juiz.Quantas(null!, []));
        Assert.Throws<ArgumentNullException>(() => Juiz.Quantas(new int[3], null!));
        Assert.Throws<ArgumentNullException>(() => Juiz.Texto(null!));
    }
}
