# Estrutura (informal) do Projeto GeneralLabSolutions:

## Visão Geral

O **GeneralLabSolutions** é uma aplicação desenvolvida para informatizar e automatizar os principais processos de uma empresa de importação e exportação de maquinários e itens de laboratório, operando através de dropshipping. O sistema é projetado para permitir que o dono da empresa gerencie todas as operações de maneira eficiente, automatizando tarefas manuais e gerando relatórios detalhados para a tomada de decisões.

## Objetivos do Projeto

1. **Automatizar processos comerciais** como vendas, compras, orçamentos e pedidos.
2. **Fornecer ferramentas analíticas** para ajudar na gestão da empresa.
3. **Facilitar a comunicação e o relacionamento** com clientes e fornecedores.
4. **Gerenciar tarefas e atividades** dos vendedores e outros funcionários.
5. **Implementar um controle financeiro** eficiente com geração automatizada de notas fiscais e relatórios financeiros.

## Estrutura do Sistema

### 1. Processo Principal

O fluxo principal do sistema envolve a gestão de propostas de venda, pedidos de venda e pedidos de compra. Cada etapa do processo gera dados que alimentam relatórios e análises.

1. **Proposta de Venda:**
   - O vendedor inicia o contato com o cliente, podendo registrar um novo cliente ou selecionar um já existente.
   - Cria uma "Proposta de Venda" (em formato PDF) que inclui uma lista de produtos, quantidades, preços, e informações adicionais.
   - A proposta tem um prazo de expiração e pode ser revisada, gerando novas versões que devem ser rastreadas.
   - As propostas servem como base para negociações e decisões futuras.

2. **Pedido de Venda:**
   - Quando a proposta é aceita, transforma-se em um "Pedido de Venda", contendo as mesmas informações, com datas de efetivação e prazos de entrega.
   - Representa a necessidade do cliente e aguarda o processo de compra para a entrega do produto.

3. **Pedido de Compra:**
   - Após a compra do produto, o pedido de venda é convertido em um "Pedido de Compra".
   - Inclui detalhes sobre o fornecedor, logística, formas de pagamento, e prazos.
   - Serve para monitorar o status dos pedidos que estão sendo atendidos.

### 2. Relatórios e Análises

O sistema deve gerar uma variedade de relatórios e análises para apoiar a gestão da empresa, como:

- Desempenho de vendedores.
- Estatísticas de clientes, fornecedores, e produtos.
- Relatórios regionais de vendas (quais locais vendem mais/menos).
- Análise de vendas por períodos.
- Monitoramento de propostas, pedidos de venda e compra.

### 3. Controle de Funcionários e Tarefas

Implementar um sistema de **Kanban** para organizar as tarefas dos vendedores, incluindo:

- Registro de atividades diárias.
- Notificações para lembrar os vendedores de interações pendentes com clientes.
- Integração com o sistema de propostas e pedidos para lembretes automáticos.

### 4. Sistema de Mensageria

O sistema deve facilitar a comunicação com clientes através de:

- Notificações para recompra de produtos.
- Promoções e ofertas especiais baseadas no histórico de compras.
- Alertas sobre a validade de produtos e necessidade de reposição.

### 5. Sistema de Agenda

Uma agenda compartilhada deve permitir:

- Marcação de eventos e compromissos.
- Geração de notificações automáticas.
- Integração com o sistema de mensageria e controle de tarefas.

### 6. Cálculo de Preços

Implementar um sistema de automação para cálculos de preços, configurado pelo dono da empresa e utilizado pelos vendedores. Isso substituirá o processo manual onde os vendedores consultam o dono.

### 7. Geração de Documentos

- **Orçamentos em PDF:** Geração de orçamentos padronizados para envio aos clientes.
- **Notas Fiscais Automáticas:** Inclusão de cálculos necessários e controle de faturamento.

### 8. Controle de Estoque Interno

Embora a empresa opere por dropshipping, será necessário um sistema para:

- Monitorar produtos e insumos que sobram ou estão temporariamente disponíveis na empresa.
- Gerar relatórios sobre esses itens para otimização do uso.

### 9. Gestão de Dados e Snapshots

O sistema deve ser capaz de:

- Armazenar snapshots (instantâneos) das transações, permitindo o rastreamento histórico mesmo após alterações.
- Manter dados antigos acessíveis para relatórios e análises históricas.

## Perguntas para Discussão com o Contratante

1. **Necessidade de Estoque Interno:** Qual o volume e a frequência de itens que precisam ser gerenciados internamente? Vale a pena investir em um módulo de estoque interno?
   
2. **Relatórios e Dashboards:** Quais são as métricas mais críticas que o contratante deseja monitorar? Algum relatório específico que ainda não foi mencionado?
   
3. **Cálculos de Preço Automatizados:** Como os cálculos de preços são atualmente definidos? Existem fórmulas específicas ou regras que precisam ser configuradas no sistema?

4. **Integração com Sistemas Existentes:** Existe algum sistema atual que precisa ser integrado, como ERPs ou sistemas financeiros?

5. **Segurança e Backup:** Quais são as expectativas de segurança de dados e como os backups serão gerenciados? Qual é a política de retenção de dados?

6. **Escalabilidade:** Existe alguma previsão de crescimento da empresa que possa impactar a necessidade de escalabilidade do sistema?

## Próximos Passos

1. **Revisão e Aprovação:** Apresentar este documento ao contratante para revisão e aprovação.
2. **Desenvolvimento do MVP:** Iniciar o desenvolvimento do Produto Mínimo Viável (MVP) com foco no fluxo principal de propostas, pedidos de venda e compra.
3. **Integração e Testes:** Implementar os módulos de integração e realizar testes exaustivos.
4. **Feedback e Ajustes:** Recolher feedback do contratante e fazer os ajustes necessários antes do lançamento.

## Links Importantes

- [Consultar e Estudar a Estratégia de Snapshot](https://backupgarantido.com.br/blog/o-que-e-snapshot/)

### CodeBase

- Nossa classe de Produto:

```csharp
using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.Domain.Entities
{
    public class Produto : EntityBase
	{
		public Produto(string? codigo, 
					   string? descricao, 
					   string ncm, 
					   decimal valorUnitario, 
					   Guid categoriaId, 
					   Guid fornecedorId)
		{
			Codigo = codigo;
			Descricao = descricao;
			Ncm = ncm;
			ValorUnitario = valorUnitario;
			CategoriaId = categoriaId;
			FornecedorId = fornecedorId;
		}

		// EF
		public Produto() { }

		public string? Codigo { get; init; }
		public string? Descricao { get; private set; }
		public string? Ncm { get; private set; }
		public decimal ValorUnitario { get; private set; }
		public StatusDoProtudo Status { get; set; } 
			= StatusDoProtudo.Ativo;

		public DateTime DataDeValidade { get; set; } = DateTime.UtcNow.AddMonths(10);
		public string Imagem { get; set; } = "/cooperchip/imagens/img-padrao.png";

        // Todo: Alterar Status para StatusDoProduto



        // Relacionamentos
        public Guid CategoriaId { get; private set; }
		public virtual CategoriaProduto? CategoriaProduto { get; set; }

		public Guid FornecedorId { get; private set; }
		public virtual Fornecedor? Fornecedor { get; set; }

		// Métodos de atualização
		public void AlterarValorUnitario(decimal novoValor) => this.ValorUnitario = novoValor;
		public void AlterarDescricao(string descricao) => this.Descricao = descricao;

		public void AlterarStatus(StatusDoProtudo status) => this.Status = status;
	}
}


```
---

### Informação importante para Regras de Negócios:

> A sigla "NCM" refere-se à Nomenclatura Comum do Mercosul, que é um código utilizado para classificar mercadorias de acordo com a sua natureza, com o objetivo de padronizar e facilitar as operações de comércio exterior dentro dos países que compõem o Mercosul. Este código é essencial para determinar a tributação, tanto de importação quanto de exportação, e para o controle aduaneiro.

> No seu exemplo específico, o código "9027.90.99" é um código NCM que se refere a Instrumentos e aparelhos para análise física ou química, que não estão especificados em outros lugares. Este tipo de código é usado para classificar mercadorias de alta precisão, geralmente utilizadas em laboratórios ou processos industriais específicos.
