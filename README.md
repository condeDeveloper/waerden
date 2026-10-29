# Números de Van der Waerden

Pinte os números de 1 a `n` com duas cores, do jeito que quiser. A partir de certo `n`,
**não existe jeito**: alguma progressão aritmética de três termos vai acabar com as três
casas da mesma cor. Van der Waerden provou isso em 1927 para qualquer número de cores e
qualquer comprimento, e o menor `n` que força a situação leva o nome dele.

Para duas cores e três termos esse número é **9**. Aqui ele não é citado: é provado, com
as 512 colorações de 1 a 9 montadas uma a uma.

```
dotnet test testes/Waerden.Testes/Waerden.Testes.csproj -c Release
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- duas
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- custo
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- tres
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- numero
dotnet run  --project ferramentas/Medidor/Medidor.csproj -c Release -- melhor
```

## Os dois caminhos

- A **força bruta** gera as `cores^n` colorações e pergunta ao juiz, que confere a lista
  inteira de progressões sem poda nenhuma. É lenta de propósito.
- A **busca com retrocesso** pinta de 1 para `n` e abandona o ramo assim que a última cor
  fecha uma progressão. A poda é a única esperteza do repositório, e ela é mínima: ao
  pintar a posição `p`, só as progressões que *terminam* em `p` podem ter acabado de ficar
  monocromáticas.

Os dois contam a mesma coisa e são comparados em todo tamanho em que os dois cabem. O
medidor **quebra sozinho** se eles discordarem.

E a enumeração das progressões tem o seu próprio juiz: a conta fechada
`Σ (n − (k−1)d)` diz quantas deveriam existir, sem olhar nenhuma.

```
W(2,3) = 9
W(2,4) = 35
W(3,3) = 27
```

Os três saem da mesma busca, em dois segundos.

## Os achados

### 1. O número de colorações que escapam sobe antes de desabar

```
n   coloracoes  progressoes  sem monocromatica  minimo  nos da busca
3   8           1            6                  0       14
4   16          2            10                 0       26
5   32          4            14                 0       46
6   64          6            20                 0       74
7   128         9            16                 0       114
8   256         12           6                  0       146
9   512         16           0                  1       158
```

Eu esperava uma descida: quanto maior o tamanho, menos saídas. O que acontece é que o
número de colorações boas **cresce** até `n = 6`, onde são 20, e só então despenca —
16, 6, zero. Do pico até a impossibilidade são três passos.

E a queda final é brutal: com 8 restam **seis** colorações em 256, e uma posição a mais
mata todas as seis.

### 2. Depois do número, a busca para de crescer. Exatamente

```
n   coloracoes  nos da busca  razao
5   32          46            0.7
10  1024        158           6.48
15  32768       158           207.39
20  1048576     158           6636.56
25  33554432    158           212369.82
30  1073741824  158           6795834.33
```

De `n = 9` até `n = 30`, a busca visita **os mesmos 158 nós**. Não é "mais ou menos o
mesmo": é o mesmo número, porque a partir do nono lugar nenhum ramo sobrevive, e o que
existe depois do nono lugar nunca é visitado. A força bruta, enquanto isso, vai de 512 a
mais de um bilhão de colorações.

No fim da tabela a busca é **6,8 milhões de vezes** mais barata, e a razão só cresce: o
problema parou de crescer para quem desiste cedo.

### 3. Antes do número, a poda custa mais do que economiza

Na linha do `n = 5` a razão é **0,7** — a busca abre 46 nós para varrer 32 colorações.
Enquanto quase tudo é resposta, conferir a cada passo é trabalho jogado fora, e o caminho
burro ganha. A poda não é boa nem ruim em si: ela é boa onde há o que podar.

### 4. Com três cores a força bruta deixa de ser uma opção, e a busca nem sente

```
n   coloracoes      nos da busca  existe coloracao sem progressao
20  3486784401      37            sim
24  282429536481    1073          sim
25  847288609443    16061         sim
26  2541865828329   35709         sim
27  7625597484987   2025831       NAO
```

Com 26 posições e três cores são **2,5 trilhões** de colorações, e a busca acha uma que
escapa em 35.709 nós. No 27 ela prova que não existe nenhuma em 2.025.831 nós: sete
trilhões e meio de colorações descartadas visitando dois milhões de estados, uma razão de
**3,7 milhões para um**.

O salto também diz algo: até o 20 a resposta sai em dezenas de nós, e do 24 ao 27 o custo
multiplica por dois mil. A dificuldade está toda colada no limite.

## Quando zero deixa de ser possível

Passado o nono lugar, a pergunta muda: já que alguma progressão vai sobrar, quantas no
mínimo?

```
n    minimo  coloracao
8    0       AABBAABB
9    1       AAABBAABB
10   1       AABBBAABAB
12   2       AABABBBAABAB
14   4       AABAABBABAABAB
16   6       AABAABBBAAABBABB
```

Dobrar o tamanho, de 8 para 16, custa **seis** progressões monocromáticas. O teorema diz
que é impossível evitar; a varredura diz que é impossível por pouco.

## O que as medidas me corrigiram

- **Um laço que nunca termina, escondido no caso mais bobo.** Com comprimento 1, a
  condição `a + (k−1)·d ≤ n` vira `a ≤ n`, que nunca deixa de valer, e o gerador de
  progressões roda para sempre. O mesmo acontece na conta fechada. Eu só descobri porque
  escrevi um teste do caso degenerado — a bateria inteira travou por dez minutos sem uma
  linha de saída, e o travamento não apontava para lugar nenhum. **O caso que parece bobo
  demais para testar foi o único que quebrou.**

- **Eu esperava uma descida monótona.** Escrevi "o número de colorações boas cai conforme
  o tamanho cresce" antes de medir. Ele sobe até 20 e só depois cai. A intuição de que
  mais espaço é mais aperto está certa no fim e errada no começo.

- **Eu ia escrever que a busca fica mais cara com `n`.** Ela satura em 158 nós e fica lá,
  de 9 a 30. "Mais ou menos constante" seria uma descrição ruim de uma igualdade exata.

- **Eu tratava a poda como sempre boa.** No `n = 5` ela perde para a força bruta. Antes do
  número de Van der Waerden quase tudo é resposta, e aí desistir cedo não tem de quê
  desistir.

## Estrutura

```
src/Waerden/
  Progressoes.cs  as progressoes de 1..n, com a conta fechada como juiz
  Juiz.cs         confere a lista inteira, sem poda: a resposta de referencia
  Varredura.cs    a forca bruta sobre as cores^n coloracoes
  Busca.cs        o retrocesso que desiste assim que a cor fecha uma progressao
  Medidas.cs      as tabelas deste README
testes/Waerden.Testes/
ferramentas/Medidor/
```

79 testes.
