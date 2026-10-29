using Conde.Waerden;
using Xunit;

namespace Conde.Waerden.Testes;

/// <summary>
/// Os números do README presos aqui. Só entra o que eu vi o medidor imprimir.
/// </summary>
public class MedidasTestes
{
    [Theory]
    [InlineData(3, 8, 1, 6, 0, 14)]
    [InlineData(4, 16, 2, 10, 0, 26)]
    [InlineData(5, 32, 4, 14, 0, 46)]
    [InlineData(6, 64, 6, 20, 0, 74)]
    [InlineData(7, 128, 9, 16, 0, 114)]
    [InlineData(8, 256, 12, 6, 0, 146)]
    [InlineData(9, 512, 16, 0, 1, 158)]
    [InlineData(10, 1024, 20, 0, 1, 158)]
    [InlineData(11, 2048, 25, 0, 2, 158)]
    [InlineData(12, 4096, 30, 0, 2, 158)]
    public void ATabelaDeDuasCores(int n, long coloracoes, long progressoes, long sem, int minimo, long nos)
    {
        var l = Medidas.DuasCores([n])[0];
        Assert.Equal(coloracoes, l.Coloracoes);
        Assert.Equal(progressoes, l.Progressoes);
        Assert.Equal(sem, l.SemMonocromatica);
        Assert.Equal(minimo, l.MenosMonocromaticas);
        Assert.Equal(nos, l.NosDaBusca);
    }

    [Fact]
    public void OPicoDeColoracoesBoasEstaNoSeisENaoNoComeco()
    {
        var linhas = Medidas.DuasCores([3, 4, 5, 6, 7, 8, 9]);
        var pico = linhas.MaxBy(l => l.SemMonocromatica);
        Assert.Equal(6, pico.N);
        Assert.Equal(20, pico.SemMonocromatica);

        // E a queda do pico ate o zero leva tres passos.
        Assert.Equal(16, linhas.Single(l => l.N == 7).SemMonocromatica);
        Assert.Equal(6, linhas.Single(l => l.N == 8).SemMonocromatica);
        Assert.Equal(0, linhas.Single(l => l.N == 9).SemMonocromatica);
    }

    [Theory]
    [InlineData(5, 32, 46, 0.7)]
    [InlineData(10, 1024, 158, 6.48)]
    [InlineData(15, 32768, 158, 207.39)]
    [InlineData(20, 1048576, 158, 6636.56)]
    [InlineData(25, 33554432, 158, 212369.82)]
    [InlineData(30, 1073741824, 158, 6795834.33)]
    public void ATabelaDoCusto(int n, long coloracoes, long nos, double razao)
    {
        var l = Medidas.Custo([n])[0];
        Assert.Equal(coloracoes, l.Coloracoes);
        Assert.Equal(nos, l.NosDaBusca);
        Assert.Equal(razao, l.Razao);
    }

    [Fact]
    public void NoCincoABuscaCustaMaisQueAForcaBruta()
    {
        // Antes do numero de Van der Waerden a poda nao poda nada, e contar
        // tentativa por tentativa fica mais caro que varrer: razao abaixo de 1.
        Assert.True(Medidas.Custo([5])[0].Razao < 1.0);
    }

    [Theory]
    [InlineData(10, 59049, 17, true)]
    [InlineData(15, 14348907, 24, true)]
    [InlineData(20, 3486784401, 37, true)]
    [InlineData(24, 282429536481, 1073, true)]
    [InlineData(25, 847288609443, 16061, true)]
    [InlineData(26, 2541865828329, 35709, true)]
    [InlineData(27, 7625597484987, 2025831, false)]
    public void ATabelaDeTresCores(int n, long coloracoes, long nos, bool existe)
    {
        var l = Medidas.TresCores([n])[0];
        Assert.Equal(coloracoes, l.Coloracoes);
        Assert.Equal(nos, l.NosDaBusca);
        Assert.Equal(existe, l.Existe);
    }

    [Theory]
    [InlineData(2, 3, 9)]
    [InlineData(2, 4, 35)]
    [InlineData(3, 3, 27)]
    public void OsTresNumerosDeVanDerWaerden(int cores, int comprimento, int esperado)
    {
        Assert.Equal(esperado, Medidas.Numero(cores, comprimento, esperado + 1));
    }

    [Theory]
    [InlineData(8, 0, "AABBAABB")]
    [InlineData(9, 1, "AAABBAABB")]
    [InlineData(10, 1, "AABBBAABAB")]
    [InlineData(11, 2, "AAABBBAABAB")]
    [InlineData(12, 2, "AABABBBAABAB")]
    [InlineData(13, 3, "AABABBAAABABB")]
    [InlineData(14, 4, "AABAABBABAABAB")]
    [InlineData(15, 5, "AABAABBBAAABABB")]
    [InlineData(16, 6, "AABAABBBAAABBABB")]
    public void ATabelaDaMelhorColoracao(int n, int menos, string coloracao)
    {
        var (medido, texto) = Medidas.Melhor(n);
        Assert.Equal(menos, medido);
        Assert.Equal(coloracao, texto);
    }

    [Fact]
    public void AMelhorColoracaoRealmenteDeixaEsseTanto()
    {
        for (var n = 8; n <= 14; n++)
        {
            var (menos, texto) = Medidas.Melhor(n);
            var cores = new int[n + 1];
            for (var i = 0; i < texto.Length; i++)
            {
                cores[i + 1] = texto[i] - 'A';
            }

            Assert.Equal(menos, Juiz.Quantas(cores, Progressoes.Todas(n, 3)));
        }
    }

    [Fact]
    public void OMinimoNuncaCaiQuandoOTamanhoCresce()
    {
        var anterior = -1;
        for (var n = 8; n <= 16; n++)
        {
            var (menos, _) = Medidas.Melhor(n);
            Assert.True(menos >= anterior);
            anterior = menos;
        }
    }
}
