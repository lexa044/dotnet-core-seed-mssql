using System;

using AutoMapper;
using DNSeed.Domain;
using DNSeed.Models.Command;
using DNSeed.Models.Query;

namespace DNSeed
{
    public sealed class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductResponseModel>()
                .ForMember(d => d.CreatedDate,
                    opt => opt.MapFrom(src => new DateTimeOffset(src.CreatedDate.ToUniversalTime()).ToUnixTimeMilliseconds())
                ).ForMember(d => d.UpdatedDate,
                    opt => opt.MapFrom(src => src.UpdatedDate.HasValue ? new DateTimeOffset(src.UpdatedDate.Value.ToUniversalTime()).ToUnixTimeMilliseconds() : 0)
                    );
            CreateMap<ProductRequestModel, Product>();
            CreateMap<PagedRequestModel, PagedRequest>()
                .ForMember(d => d.FromDate,
                    opt => opt.MapFrom(src => src.From > 0 ? DateTimeOffset.FromUnixTimeMilliseconds(src.From).DateTime.ToLocalTime() : DateTime.MinValue)
                ).ForMember(d => d.ToDate,
                    opt => opt.MapFrom(src => src.To > 0 ? DateTimeOffset.FromUnixTimeMilliseconds(src.To).DateTime.ToLocalTime() : DateTime.MinValue)
                );
        }
    }
}
