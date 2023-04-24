A entrada de dados será dada a partir de um vetor de strings, onde cada posição do vetor conterá informações para geração da cada fase.

Exemplo:
```json
{
    "input": ["33352", "45353", "66354"]
}
```

A estrutura de entrada se dá em 5 dígitos: "XXXXX".
- O primeiro dígito é a quantidade de coletáveis na fase.
- O segundo dígito é a quantidade de inimigos na fase.
- O terceiro dígito é o número de vizinhos que fazem com que uma célula viva se torne morta.
- O quarto dígito é o número de vizinhos que fazem com que uma célula morta se torne viva.
- O quinto dígito é o número de vezes que realizamos a etapa de simulação.
