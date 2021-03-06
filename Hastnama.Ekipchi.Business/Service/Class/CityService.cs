﻿using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Ekipchi.Business.Service.Interface;
using Hastnama.Ekipchi.Common.Helper;
using Hastnama.Ekipchi.Common.Message;
using Hastnama.Ekipchi.Common.Result;
using Hastnama.Ekipchi.Data.City;
using Hastnama.Ekipchi.DataAccess.Context;
using Hastnama.Ekipchi.DataAccess.Entities;
using Hastnama.Ekipchi.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Ekipchi.Business.Service.Class
{
    public class CityService : Repository<EkipchiDbContext, City>, ICityService
    {
        private readonly IMapper _mapper;

        public CityService(EkipchiDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<Result<PagedList<CityDto>>> List(FilterCityQueryDto filterQueryDto)
        {
            var cities = await WhereAsyncAsNoTracking(c =>
                    (string.IsNullOrEmpty(filterQueryDto.Keyword) ||
                     c.Name.ToLower().Contains(filterQueryDto.Keyword.ToLower())
                     && (string.IsNullOrEmpty(filterQueryDto.Keyword.ToLower()) ||
                         c.County.Name.ToLower().Contains(filterQueryDto.Keyword.ToLower()))), filterQueryDto,
                c => c.County, c => c.Regions);

            return Result<PagedList<CityDto>>.SuccessFull(cities.MapTo<CityDto>(_mapper));
        }

        public async Task<Result> Update(UpdateCityDto updateCityDto)
        {
            var city = await FirstOrDefaultAsync(c => c.Id == updateCityDto.Id);

            if (city.CountyId != updateCityDto.CountyId)
            {
                var county = await Context.Counties.FirstOrDefaultAsync(u => u.Id == updateCityDto.CountyId);
                if (county == null)
                    return Result.Failed(new BadRequestObjectResult(new ApiMessage
                        {Message = ResponseMessage.InvalidCountyId}));
                city.County = county;
            }

            _mapper.Map(updateCityDto, city);
            await Context.SaveChangesAsync();

            return Result.SuccessFull();
        }

        public async Task<Result<CityDto>> Create(CreateCityDto createCityDto)
        {
            var county = await Context.Counties.FirstOrDefaultAsync(u => u.Id == createCityDto.CountyId);
            if (county == null)
                return Result<CityDto>.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.InvalidCountyId}));

            var city = _mapper.Map<City>(createCityDto);
            city.County = county;

            await AddAsync(city);
            await Context.SaveChangesAsync();

            return Result<CityDto>.SuccessFull(_mapper.Map<CityDto>(city));
        }

        public async Task<Result<CityDto>> Get(int id)
        {
            var city = await FirstOrDefaultAsyncAsNoTracking(c => c.Id == id, c => c.County);
            if (city == null)
                return Result<CityDto>.Failed(new NotFoundObjectResult(
                    new ApiMessage
                        {Message = ResponseMessage.CityNotFound}));

            return Result<CityDto>.SuccessFull(_mapper.Map<CityDto>(city));
        }
    }
}