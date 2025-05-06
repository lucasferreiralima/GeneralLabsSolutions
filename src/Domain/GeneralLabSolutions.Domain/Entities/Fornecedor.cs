using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.Domain.Entities
{
    public class Fornecedor : EntityBase
    {
        // EF
        public Fornecedor() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="documento"></param>
        /// <param name="tipoDePessoa"></param>
        public Fornecedor(
                            string nome, 
                            string documento, 
                            TipoDePessoa tipoDePessoa,
                            string email)
        {
            Pessoa = new Pessoa();
            PessoaId = Pessoa.Id;
            Email = email;
            Nome = nome;
            Documento = documento;
            TipoDePessoa = tipoDePessoa;

        }

        public Guid PessoaId { get; private set; }

        public Pessoa Pessoa { get; private set; }

        public string Nome { get; private set; }
        public string Documento { get; private set; }
        public TipoDePessoa TipoDePessoa { get; private set; }

        public string Email { get; set; }

        // Status do fornecedor (e.g., Ativo, Inativo)
        public StatusDoFornecedor StatusDoFornecedor { get; set; } = StatusDoFornecedor.Ativo;
        // Coleção de produtos fornecidos pelo fornecedor
        public virtual ICollection<Produto> Produtos { get; set; } = new List<Produto>();

        public void AddProduto(Produto produto) 
            => Produtos.Add(produto);

        public void SetNome(string nome)
            => Nome = nome;

        // Define o tipo de pessoa
        public void SetTipoDePessoa(TipoDePessoa tipoDePessoa)
            => TipoDePessoa = tipoDePessoa;

        // Define o documento da pessoa
        public void SetDocumento(string documento)
            => Documento = documento;

    }

}