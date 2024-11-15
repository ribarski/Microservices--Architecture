Microserviço de Inventário
- Permite adicionar novos produtos com informações sobre nome, descrição e quantidade em estoque.
- Consultar Produtos: Pode listar todos os produtos ou retornar detalhes de um produto específico, incluindo a quantidade em estoque.
- Verificar Disponibilidade: Pode verificar se um produto possui estoque suficiente para a quantidade solicitada.
- Atualizar Estoque: Após uma venda, o estoque de um produto é atualizado subtraindo a quantidade vendida.
- Fluxo: Quando uma venda é criada, o microserviço de Vendas faz uma requisição ao microserviço de Inventário para verificar se a quantidade de um produto está disponível. Se estiver, o estoque é atualizado (quantidade subtraída) e a venda é processada.

# Microserviço de Preço 
- Consultar Preço: Retorna o preço atual de um produto.
- Cadastrar Preço: Adiciona ou atualiza o preço de um produto específico.
- Fluxo: Quando uma venda é criada, o microserviço de Vendas consulta o microserviço de Preço para obter o preço do produto. Esse preço é utilizado para calcular o valor total da venda.

# Microserviço de Vendas
- Criar Venda: Cria uma nova venda, realizando a validação de estoque e obtendo o preço do produto.
- Consultar Venda: Retorna as informações de uma venda específica.
- Fluxo: Quando uma venda é realizada, o microserviço de Vendas realiza as seguintes etapas:
- Verifica o Estoque: Faz uma requisição para o microserviço de Inventário para garantir que há estoque suficiente.
- Consulta o Preço: Faz uma requisição ao microserviço de Preço para obter o preço do produto.
- Criação da Venda: Se o estoque estiver disponível, cria a venda no banco de dados.

