﻿using System.Threading.Tasks;
using Hastnama.Ekipchi.Common.General;
using Hastnama.Ekipchi.Common.Result;
using Hastnama.Ekipchi.Data.Faq;
using Hastnama.Ekipchi.DataAccess.Entities;
using Hastnama.Ekipchi.DataAccess.Repository;
using Hastnama.GuitarIranShop.DataAccess.Helper;

namespace Hastnama.Ekipchi.Business.Service.Interface
{
    public interface IFaqService : IRepository<Faq>
    {
        Task<Result<PagedList<FaqDto>>> List(PagingOptions pagingOptions, FilterFaqQueryDto filterQueryDto);
        Task<Result> Update(UpdateFaqDto updateFaqDto);
        Task<Result<FaqDto>> Create(CreateFaqDto dto);
        Task<Result<FaqDto>> Get(int id);
    }
}