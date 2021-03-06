﻿using System;
using System.Threading.Tasks;
using Hastnama.Ekipchi.Common.Helper;
using Hastnama.Ekipchi.Common.Result;
using Hastnama.Ekipchi.Data.Comment;
using Hastnama.Ekipchi.DataAccess.Entities;
using Hastnama.Ekipchi.DataAccess.Repository;


namespace Hastnama.Ekipchi.Business.Service.Interface
{
    public interface ICommentService : IRepository<Comment>
    {
        Task<Result<PagedList<CommentDto>>> List(FilterCommentQueryDto filterCommentQueryDto);
        Task<Result> Update(UpdateCommentDto updateCommentDto);
        Task<Result<CommentDto>> Create(CreateCommentDto dto, Guid userId);
        Task<Result<CommentDto>> Get(Guid id);
        Task<Result> Delete(Guid id);
    }
}