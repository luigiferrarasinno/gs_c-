
# EnergySimulator

> **Projeto desenvolvido por Luig**
> Simulador de geração, distribuição e armazenamento de energia elétrica para cidades, considerando crises energéticas e limites de armazenamento, com interface de login e menu interativo.

---

## Descrição do Projeto

O **EnergySimulator** é um sistema em C# que simula o fluxo de energia elétrica entre usinas geradoras e cidades consumidoras, incluindo um sistema de armazenamento de energia para garantir o fornecimento mesmo em momentos de crise, quando a geração pode ser insuficiente. O projeto visa analisar como diferentes situações de demanda, oferta e crise impactam o sistema de energia, com logs detalhados para monitorar cada passo. Agora conta com um sistema simples de login obrigatório e menu interativo para iniciar a simulação.

---

## Objetivos

* Simular a geração de energia elétrica por usinas.
* Definir a demanda (metas) de energia para cidades.
* Distribuir a energia gerada para atender a demanda das cidades.
* Utilizar energia armazenada quando a geração for insuficiente.
* Implementar crises energéticas que podem reduzir geração, aumentar demanda ou ambos.
* Limitar a capacidade máxima do armazenamento.
* Fornecer logs detalhados para análise das rodadas.
* Implementar sistema de login simples com usuário e senha fixos.
* Apresentar menu interativo após login com opções para iniciar simulação ou outras funcionalidades (em construção).

---

## Estrutura do Projeto

```
EnergySimulator/
│
├── Models/
│   ├── Cidade.cs          # Representa uma cidade, com nome e meta de energia
│   ├── Usina.cs           # Representa uma usina geradora de energia
│   ├── Armazenador.cs     # Representa o armazenamento de energia 
│ 
├── Services/
│   ├── GerenciadorEnergia.cs  # Lógica central do simulador: geração, distribuição e armazenamento
│
├── Utilities/
│   ├── Logger.cs          # Responsável por registrar e formatar logs do sistema
│
├── Program.cs             # Ponto de entrada do programa, implementa login, menu e inicia simulação
├── README.md              # Documentação do projeto
```

---

## Explicação dos Arquivos

### Models

* **Cidade.cs**
  Define uma cidade com:

  * `Nome`: Identificação da cidade.
  * `MetaEnergia`: Demanda de energia que a cidade precisa na rodada.

* **Usina.cs**
  Define uma usina com:

  * `Nome`: Identificação da usina.
  * `Geracao`: Quantidade de energia gerada na rodada.

### Services

* **GerenciadorEnergia.cs**
  Implementa a lógica principal do simulador:

  * Geração de energia nas usinas, com variações baseadas em crises.
  * Definição das metas de energia das cidades, também afetadas por crises.
  * Distribuição da energia gerada para as cidades.
  * Uso da energia armazenada para suprir déficit.
  * Atualização do armazenamento, respeitando a capacidade máxima.
  * Gestão das rodadas, aplicando crises a cada duas rodadas.
  * Registro dos eventos e status via Logger.

### Utilities

* **Logger.cs**
  Fornece métodos para:

  * Logar mensagens formatadas.
  * Exibir listas de metas e geração.
  * Exibir fluxo de energia para cada cidade, incluindo avisos de déficit e uso de armazenamento.
  * Exibir separadores e mensagens centralizadas para melhor visualização.

### Program.cs

* Responsável por:

  * Implementar o sistema de login obrigatório, com usuário e senha fixos ("admin"/"admin").
  * Exibir menu interativo com opções para iniciar simulação ou opções "em construção".
  * Controlar o fluxo do programa e iniciar as rodadas da simulação após login.

---

## Lógica do Sistema

### 1. Geração de Energia

* Cada usina gera uma quantidade de energia variável a cada rodada.
* Em rodadas de crise (a cada 2 rodadas), a geração pode ser reduzida aleatoriamente (simulando queda de produção).

### 2. Demanda de Energia

* Cada cidade tem uma meta de energia variável, que pode aumentar em rodadas de crise (aumentando a demanda).
* A demanda varia aleatoriamente dentro de um intervalo definido, para simular consumo realista.

### 3. Distribuição de Energia

* A energia gerada é distribuída para as cidades conforme suas metas.
* Se a geração não for suficiente para todas as cidades, a energia armazenada é usada para suprir o déficit.
* Caso a energia armazenada também não seja suficiente, a cidade fica com déficit.

### 4. Armazenamento de Energia

* A energia que sobra após atender as demandas é armazenada, até um limite máximo (500 unidades).
* Quando há déficit, a energia armazenada é consumida para suprir a demanda.
* O sistema garante que o armazenamento nunca exceda sua capacidade máxima e nunca fique negativo.

### 5. Crises Energéticas

* A cada 2 rodadas, ocorre uma crise que pode afetar:

  * A geração (reduzindo a produção das usinas).
  * A demanda (aumentando a meta das cidades).
  * Ou ambos simultaneamente.
* Isso cria situações realistas para testar a resiliência do sistema.

### 6. Login e Menu

* O programa inicia obrigando o usuário a logar.
* O usuário deve digitar "admin" para usuário e senha.
* Após login válido, o sistema mostra um menu com opções:

  * 1 para iniciar a simulação.
  * 2, 3, 4, 5 opções "em construção".
  * 0 para sair do programa.
* Caso a opção escolhida seja inválida, pede para escolher uma válida.
* A simulação roda indefinidamente, com rodadas periódicas, até o usuário interromper manualmente.

---

## Requisitos Funcionais

| Código | Descrição                                                                           |
| ------ | ----------------------------------------------------------------------------------- |
| RF01   | O sistema deve obrigar o usuário a realizar login antes de acessar a simulação.     |
| RF02   | O sistema deve validar usuário e senha fixos: "admin" / "admin".                    |
| RF03   | Após login, o sistema deve apresentar um menu com 5 opções, sendo apenas a 1 ativa. |
| RF04   | O sistema deve iniciar a simulação quando o usuário escolher a opção 1 no menu.     |
| RF05   | A simulação deve executar rodadas periódicas, exibindo logs detalhados no console.  |
| RF06   | O sistema deve mostrar mensagem "Em construção" para opções 2 a 5 do menu.          |
| RF07   | O sistema deve permitir sair do programa a partir da opção 0 no menu.               |

---

## Requisitos Não Funcionais

| Código | Descrição                                                                             |
| ------ | ------------------------------------------------------------------------------------- |
| RNF01  | O sistema deve ser desenvolvido em C# utilizando o .NET SDK.                          |
| RNF02  | O sistema deve ter interface de console simples e clara.                              |
| RNF03  | O sistema deve possuir código modular, com classes separadas para entidades e lógica. |
| RNF04  | O sistema deve garantir que as senhas digitadas não sejam exibidas em texto claro.    |
| RNF05  | O sistema deve possuir logs formatados para facilitar o entendimento das simulações.  |
| RNF06  | O sistema deve ser executado em ambiente local via terminal/console.                  |

---

## Regras de Negócio

* O usuário deve obrigatoriamente logar para acessar o menu.
* Apenas o usuário "admin" com senha "admin" é válido.
* A simulação é iniciada somente após a seleção da opção 1 no menu.
* A simulação funciona em rodadas infinitas até interrupção manual.
* As crises energéticas ocorrem a cada 2 rodadas e impactam geração e demanda.
* A energia armazenada nunca pode exceder 500 unidades nem ser negativa.
* Opções do menu que não estejam implementadas exibem mensagem "Em construção".
* O programa pode ser encerrado pelo usuário via menu (opção 0) ou via Ctrl+C durante a simulação.

---

## Tecnologias Utilizadas

* Linguagem: C#
* Plataforma: .NET 6 (ou superior)
* Ambiente de desenvolvimento: Visual Studio, VS Code, ou outro IDE compatível
* Sistema operacional: Multiplataforma (Windows, Linux, Mac) via .NET CLI

---


### Fluxograma 1: Processo de Login

![image](https://github.com/user-attachments/assets/dcab9f6f-8c00-4b00-b9f0-baf8c7a05127)

---

### Fluxograma 2: Menu Principal

1. Exibir menu principal com opções 1 a 5 e 0 para sair
2. Usuário escolhe opção
3. Se opção = 1 → Iniciar simulação
4. Se opção = 0 → Encerrar programa
5. Se opção ∈ {2,3,4,5} → Mostrar "Em construção" e voltar ao menu
6. Se opção inválida → Mostrar erro e voltar ao menu

---

### Fluxograma 3: Simulação - Rodada de Energia

![image](https://github.com/user-attachments/assets/8f62a5b4-6f87-4d05-a893-3c992432f071)

---

---

## Como Rodar

1. Clone o repositório.

2. Certifique-se de ter o .NET SDK instalado.

3. Navegue até a pasta do projeto.

4. Execute o comando:

   ```bash
   dotnet run
   ```

5. Siga as instruções de login e navegação pelo menu.

6. Para sair do programa, escolha a opção 0 no menu ou interrompa com Ctrl+C na simulação.

---

## Considerações Finais

Este projeto foi desenvolvido por **Luigi** com o intuito de explorar conceitos de sistemas energéticos, simulação de processos reais e gerenciamento de crises energéticas em ambientes controlados. A arquitetura modular e os logs detalhados facilitam a expansão futura, como inclusão de mais cidades, usinas, diferentes tipos de energia, e análises mais complexas. A adição do sistema de login e menu permite um fluxo mais controlado e preparado para futuras funcionalidades.

