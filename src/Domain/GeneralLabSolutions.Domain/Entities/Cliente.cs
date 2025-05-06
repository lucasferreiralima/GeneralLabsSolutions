using GeneralLabSolutions.Domain.DomainObjects;
using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.Domain.Entities
{
    public class Cliente : EntityBase, IAggregateRoot
    {
        // EF
        public Cliente() { }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="nome"></param>
        /// <param name="documento"></param>
        /// <param name="tipoDePessoa"></param>
        /// <param name="email"></param>
        public Cliente(string nome, string documento, TipoDePessoa tipoDePessoa, string email)
        {
            Pessoa = new Pessoa();
            PessoaId = Pessoa.Id;
            Email = email;
            Nome = nome;
            Documento = documento;
            TipoDePessoa = tipoDePessoa;
        }

        public Guid PessoaId { get; set; }

        public Pessoa Pessoa { get; set; }

        public string Nome { get; private set; }

        public string Documento { get; private set; }
        public TipoDePessoa TipoDePessoa { get; private set; }

        // Email do cliente
        public string Email { get; private set; }
        // Status do cliente (e.g., Ativo, Inativo)
        public StatusDoCliente StatusDoCliente { get; set; } = StatusDoCliente.Ativo;
        // Tipo de cliente (e.g., Comum, Especial)
        public TipoDeCliente TipoDeCliente { get; set; } 
            = TipoDeCliente.Comum;
        // Coleção de pedidos realizados pelo cliente
        public virtual ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();

        // Define o email do cliente
        public void SetEmail(string newEmail) => Email = newEmail;

        public void AddPedido(Pedido pedido)
            => Pedidos.Add(pedido);

        public void SetNome(string nome) => Nome = nome;

        // Define o tipo de pessoa
        public void SetTipoDePessoa(TipoDePessoa tipoDePessoa)
            => TipoDePessoa = tipoDePessoa;

        // Define o documento da pessoa
        public void SetDocumento(string documento)
            => Documento = documento;
    }
}
