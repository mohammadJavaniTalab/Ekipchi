﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Ekipchi.Common.Helper
{
    public class PagedList<T>
    {
        public Meta Meta { get; set; }

        public List<T> Items { get; }

        public PagedList(IEnumerable<T> items, Meta meta)
        {
            Meta = new Meta
            {
                TotalCount = meta.TotalCount,
                PageSize = meta.PageSize,
                CurrentPage = meta.CurrentPage,
                TotalPages = (int) Math.Ceiling(meta.TotalCount / (double) meta.TotalPages),
                ObjectInPage = meta.ObjectInPage
            };

            Items = items.ToList();
        }

        public PagedList<TDest> MapTo<TDest>(IMapper mapper, string lang = "fa-IR")
        {
            var items = mapper.Map<List<TDest>>(Items, opt => opt.Items["lang"] = lang);

            return new PagedList<TDest>(items,
                new Meta
                {
                    TotalCount = Meta.TotalCount, CurrentPage = Meta.CurrentPage, PageSize = Meta.PageSize,
                    TotalPages = Meta.TotalPages, ObjectInPage = Meta.ObjectInPage
                });
        }

        public static async Task<PagedList<T>> CreateAsync(IQueryable<T> query, int pageNumber, int pageSize, int count)
        {
            var items = await query.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

            int totalPages = (int) Math.Ceiling((decimal) count / pageSize);

            int objectInPage = (pageNumber < totalPages) ? pageSize :
                pageNumber == 1 ? count : (count - (pageNumber * pageSize));

            return new PagedList<T>(items,
                new Meta
                {
                    TotalCount = count, CurrentPage = pageNumber, PageSize = pageSize, TotalPages = totalPages,
                    ObjectInPage = objectInPage
                });
        }

        public static PagedList<T> CreateAsync(IEnumerable<T> query, int pageNumber, int pageSize, int count)
        {
            if (pageSize <= 0)
                pageSize = 10;

            var rowsCount = query.ToList().Count;

            if (rowsCount <= pageSize || pageNumber <= 1)
                pageNumber = 1;

            return new PagedList<T>(query,
                new Meta
                {
                    TotalCount = count, CurrentPage = pageNumber, PageSize = pageSize,
                    TotalPages = (int) Math.Ceiling((decimal) count / pageSize)
                });
        }
    }
}