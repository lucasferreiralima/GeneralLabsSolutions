using AutoMapper;
using GeneralLabSolutions.Domain.Entities;
using GeneralLabSolutions.Domain.Entities.Base;
using VelzonModerna.ViewModels;

namespace VelzonModerna.Configuration.Mappings
{
    public class AutoMapperConfig : Profile
    {
        public AutoMapperConfig()
        {
            CreateMap<Cliente, ClienteViewModel>().ReverseMap();
            CreateMap<Contato, ContatoViewModel>().ReverseMap();
            CreateMap<Fornecedor, FornecedorViewModel>().ReverseMap();
            CreateMap<CategoriaProduto, CategoriaProdutoViewModel>().ReverseMap();
            CreateMap<Vendedor, VendedorViewModel>().ReverseMap();

            CreateMap<Produto, ProdutoViewModel>().ReverseMap();
            CreateMap<Pedido, PedidoViewModel>().ReverseMap();
            CreateMap<Telefone, TelefoneViewModel>().ReverseMap();


            // Kanban Mapper
            CreateMap<KanbanTask, KanbanTaskViewModel>()
                .ForMember(dest => dest.ParticipantIds,
                           opt => opt.MapFrom(src => src.Participantes.Select(p => p.Id)))
                .ReverseMap()
                // Ao mapear de VM → Entity, ignorar ou mapear manualmente
                .ForMember(dest => dest.Participantes, opt => opt.Ignore());

            CreateMap<Participante, ParticipanteViewModel>().ReverseMap();

        }
    }
}
