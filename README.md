# Arquitetura de Microsserviços com Gateway API

Este projeto demonstra uma arquitetura de microsserviços com três serviços principais: Inventário, Preço e Vendas. Um Gateway API (Ocelot) é usado para rotear as requisições para os microsserviços apropriados.

## Microsserviços

### 1. Microserviço de Inventário

**Funcionalidades:**

* **Adicionar Produto:** Permite adicionar novos produtos com informações como nome, descrição e quantidade em estoque.
* **Consultar Produtos:** Lista todos os produtos ou retorna detalhes de um produto específico, incluindo a quantidade em estoque.
* **Verificar Disponibilidade:** Verifica se um produto possui estoque suficiente para a quantidade solicitada.
* **Atualizar Estoque:** Após uma venda, atualiza o estoque de um produto subtraindo a quantidade vendida.

**Fluxo:**

Quando uma venda é criada, o microserviço de Vendas consulta o microserviço de Inventário para verificar a disponibilidade do produto. Se houver estoque suficiente, o estoque é atualizado e a venda é processada.

### 2. Microserviço de Preço

**Funcionalidades:**

* **Consultar Preço:** Retorna o preço atual de um produto.
* **Cadastrar/Atualizar Preço:** Adiciona ou atualiza o preço de um produto específico.

**Fluxo:**

Quando uma venda é criada, o microserviço de Vendas consulta o microserviço de Preço para obter o preço do produto, que é usado para calcular o valor total da venda.

### 3. Microserviço de Vendas

**Funcionalidades:**

* **Criar Venda:** Cria uma nova venda, validando o estoque e obtendo o preço do produto.
* **Consultar Venda:** Retorna as informações de uma venda específica.

**Fluxo:**

1. **Verificar Estoque:** Consulta o microserviço de Inventário para garantir que haja estoque suficiente.
2. **Consultar Preço:** Consulta o microserviço de Preço para obter o preço do produto.
3. **Criar Venda:** Se o estoque estiver disponível, cria a venda no banco de dados.

## Gateway API (Ocelot)

O Gateway API (Ocelot) atua como um ponto de entrada único para todos os microsserviços, simplificando o acesso do cliente e fornecendo recursos adicionais, como:

* **Roteamento:** Encaminha as requisições para o microsserviço correto com base no caminho da URL.
* **Agregação:** (Opcional) Pode agregar respostas de múltiplos microsserviços em uma única resposta.
* **Autenticação e Autorização:** (Opcional) Pode ser configurado para autenticar e autorizar requisições antes de encaminhá-las aos microsserviços.


## Executando o projeto

1. Inicie cada microsserviço individualmente.
2. Inicie o Gateway API.


## Diagrama da Arquitetura


| Cliente | --> | Gateway API | --> | Microsserviços |
+-----------------+
| | Inventario |
| | Preço |
| | Vendas |


## Tecnologias Utilizadas

* .NET / C#
* Ocelot
