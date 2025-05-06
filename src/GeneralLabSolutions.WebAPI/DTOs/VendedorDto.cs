﻿using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Entities.Base;
using GeneralLabSolutions.Domain.Enums;

namespace GeneralLabSolutions.WebAPI.DTOs
{
    public class VendedorDto
    {

        [Key]
        public Guid Id { get; set; }

        [Required(ErrorMessage = "O campo '{0}' obrigatório!")]
        [StringLength(200, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres.", MinimumLength = 2)]
        public string Nome { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo '{0}' obrigatório!")]
        [StringLength(14, ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres, dependendo se for Pessoa Física ou Jurídica.", MinimumLength = 11)]
        public string Documento { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo '{0}' obrigatório!")]
        [DataType(DataType.EmailAddress, ErrorMessage = "O campo {0} está inválçido!")]
        [StringLength(254, ErrorMessage = "O campo {0} dve ter entre {2} e {1} caracteres.", MinimumLength = 6)]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "O campo '{0}' obrigatório!")]
        [DisplayName("Tipo de Pessoa")]
        public TipoDePessoa TipoDePessoa { get; set; }


        [Required(ErrorMessage = "O campo '{0}' obrigatório!")]
        [DisplayName("Status do Vendedor")]
        public StatusDoVendedor StatusDoVendedor { get; set; }
            = StatusDoVendedor.Contratado;


        public virtual ICollection<Pedido> Pedidos { get; set; }
            = new List<Pedido>();

        public virtual ICollection<Telefone> Telefones { get; set; }
                = new List<Telefone>();
    }
}
