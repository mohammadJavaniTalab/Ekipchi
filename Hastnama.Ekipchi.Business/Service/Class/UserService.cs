﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Hastnama.Ekipchi.Business.Service.Interface;
using Hastnama.Ekipchi.Common.Enum;
using Hastnama.Ekipchi.Common.Helper;
using Hastnama.Ekipchi.Common.Message;
using Hastnama.Ekipchi.Common.Result;
using Hastnama.Ekipchi.Common.Util;
using Hastnama.Ekipchi.Data.Auth;
using Hastnama.Ekipchi.Data.Event;
using Hastnama.Ekipchi.Data.Group;
using Hastnama.Ekipchi.Data.Role;
using Hastnama.Ekipchi.Data.User;
using Hastnama.Ekipchi.DataAccess.Context;
using Hastnama.Ekipchi.DataAccess.Entities;
using Hastnama.Ekipchi.DataAccess.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hastnama.Ekipchi.Business.Service.Class
{
    public class UserService : Repository<EkipchiDbContext, User>, IUserService
    {
        private readonly IMapper _mapper;

        public UserService(EkipchiDbContext context, IMapper mapper) : base(context)
        {
            _mapper = mapper;
        }

        public async Task<Result<User>> Login(LoginDto loginDto)
        {
            var user = await FirstOrDefaultAsyncAsNoTracking(u =>
                    u.Username == loginDto.Username
                    || u.Email == loginDto.Username
                    || u.Mobile == loginDto.Username,
                u => u.UserInRoles.Select(ur => ur.Role.RolePermissions.Select(rp => rp.Permission)));
            if (user == null)
                return Result<User>.Failed(new NotFoundObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserNotFound}));

            if (!StringUtil.CheckPassword(loginDto.Password, user.Password))

                return Result<User>.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.InvalidUserCredential}));

            if (user.Status != UserStatus.Active)
                return Result<User>.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserIsDeActive}));

            return Result<User>.SuccessFull(user);
        }


        public async Task<Result<User>> Register(RegisterDto registerDto)
        {
            var user = await FirstOrDefaultAsyncAsNoTracking(u =>
                (string.IsNullOrEmpty(u.Username) || u.Username == registerDto.Username)
                && (string.IsNullOrEmpty(u.Email) || u.Email == registerDto.Email)
                && (string.IsNullOrEmpty(u.Mobile) || u.Mobile == registerDto.Mobile));

            if (user != null)
                return Result<User>.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserAlreadyExist}));

            user = _mapper.Map<User>(registerDto);
            var role = await Context.Roles.FirstOrDefaultAsync(r => r.Name == "User");
            user.UserInRoles = new List<UserInRole>() {new UserInRole {Role = role, User = user}};

            await AddAsync(user);
            await Context.SaveChangesAsync();

            return Result<User>.SuccessFull(user);
        }

        public async Task<Result<PagedList<UserDto>>> List(FilterUserQueryDto filterQueryDto)
        {
            var users = await WhereAsyncAsNoTracking(u =>
                    u.Status != UserStatus.Delete
                    && (
                        string.IsNullOrEmpty(filterQueryDto.Keyword)
                        || u.Name.ToLower().Contains(filterQueryDto.Keyword.ToLower())
                        || u.Family.ToLower().Contains(filterQueryDto.Keyword.ToLower())
                        || u.Username.ToLower().Contains(filterQueryDto.Keyword.ToLower())
                        || u.Mobile.ToLower().Contains(filterQueryDto.Keyword.ToLower())
                        || u.Email.ToLower().Contains(filterQueryDto.Keyword.ToLower()))
                    && (filterQueryDto.Status == null || u.Status == filterQueryDto.Status)
                    && (filterQueryDto.RoleId == null || u.UserInRoles.Any(ur => ur.RoleId == filterQueryDto.RoleId))
                , filterQueryDto,
                u => u.UserInRoles.Select(ur => ur.Role));
            if (users == null)
                return Result<PagedList<UserDto>>.Failed(new NotFoundObjectResult(new ApiMessage
                    {Message = ResponseMessage.BadRequestQuery}));

            return Result<PagedList<UserDto>>.SuccessFull(users.MapTo<UserDto>(_mapper));
        }

        public async Task<Result<UserDto>> UpdateProfile(UpdateUserDto updateUserDto)
        {
            var user = await FirstOrDefaultAsync(u => u.Id == updateUserDto.Id,
                u => u.UserInRoles.Select(ur => ur.Role));

            if (user == null)
                return Result<UserDto>.Failed(new NotFoundObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserNotFound}));

            _mapper.Map(updateUserDto, user);


            await Context.SaveChangesAsync();

            var result = await Get(user.Id);
            return Result<UserDto>.SuccessFull(result.Data);
        }

        public async Task<Result> UpdateProfile(AdminUpdateUserDto adminUpdateUserDto)
        {
            var user = await FirstOrDefaultAsync(u => u.Id == adminUpdateUserDto.Id,u=>u.UserWallet,
                u => u.UserInRoles.Select(ur => ur.Role));

            if (user == null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserNotFound}));

            _mapper.Map(adminUpdateUserDto, user);

            if (!user.UserInRoles.Select(g => g.RoleId).SequenceEqual(adminUpdateUserDto.Roles))
            {
                // get all roles that are removed 
                var removeRoles = user.UserInRoles
                    .Where(ur => !adminUpdateUserDto.Roles.Contains(ur.RoleId)).ToList();
                if (removeRoles.Any())
                    Context.UserInRoles.RemoveRange(removeRoles);

                // get all roles id that are added
                var addedRolesId = adminUpdateUserDto.Roles.Where(roleId =>
                    !user.UserInRoles.Select(u => u.RoleId).Contains(roleId)).ToList();
                var addedRoles = await Context.Roles.Where(u => addedRolesId.Contains(u.Id)).ToListAsync();

                // if invalid role id sent 
                if (addedRoles.Count != addedRolesId.Count)
                    return Result.Failed(new BadRequestObjectResult(new ApiMessage
                        {Message = ResponseMessage.RoleNotFound}));

                var addedUserRoles = addedRoles.Select(role => new UserInRole
                    {Id = Guid.NewGuid(), Role = role, User = user}).ToList();

                if (addedUserRoles.Any())
                    await Context.UserInRoles.AddRangeAsync(addedUserRoles);

                user.UserInRoles = addedUserRoles.Union(user.UserInRoles.Where(ur =>
                        !addedRolesId.Contains(ur.RoleId) && !removeRoles.Select(rr => rr.RoleId).Contains(ur.RoleId)))
                    .ToList();
            }

            // if (!string.IsNullOrEmpty(adminUpdateUserDto.Password))
                // user.Password = StringUtil.HashPass(adminUpdateUserDto.Password);

            await Context.SaveChangesAsync();

            return Result.SuccessFull();
        }

        public async Task<Result<UserDto>> Create(CreateUserDto dto)
        {
            if (string.IsNullOrEmpty(dto.Username))
                dto.Username = $"{dto.Name} {dto.Family}";

            var user = _mapper.Map<User>(dto);

            if (user.Email != null)
            {
                var duplicateUser = (await FirstOrDefaultAsyncAsNoTracking(u => u.Email == user.Email));

                if (duplicateUser != null)
                    return Result<UserDto>.Failed(new BadRequestObjectResult(new ApiMessage
                        {Message = ResponseMessage.EmailAddressAlreadyExist}));
            }

            if (user.Mobile != null)
            {
                var duplicateUser = (await FirstOrDefaultAsyncAsNoTracking(u => u.Mobile == user.Mobile));

                if (duplicateUser != null)
                    return Result<UserDto>.Failed(new BadRequestObjectResult(new ApiMessage
                        {Message = ResponseMessage.MobileAlreadyExist}));
            }

            if (user.Username != null)
            {
                var duplicateUser = (await FirstOrDefaultAsyncAsNoTracking(u => u.Username == user.Username));

                if (duplicateUser != null)
                    return Result<UserDto>.Failed(new BadRequestObjectResult(new ApiMessage
                        {Message = ResponseMessage.UsernameAlreadyExist}));
            }

            user.UserInRoles = dto.Roles.Select(r => new UserInRole {RoleId = r}).ToList();
            await AddAsync(user);
            await Context.SaveChangesAsync();
            user.UserInRoles = new List<UserInRole>();
            return Result<UserDto>.SuccessFull(_mapper.Map<UserDto>(user));
        }

        public async Task<Result<UserDto>> Get(Guid id)
        {
            if (id == Guid.Empty)
                return Result<UserDto>.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.InvalidUserId}));

            var user = await FirstOrDefaultAsyncAsNoTracking(u => u.Id == id,u=>u.UserWallet,
                u => u.UserInRoles.Select(ur => ur.Role.RolePermissions.Select(rp => rp.Permission.Parent)));

            if (user == null)
                return Result<UserDto>.Failed(new NotFoundObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserNotFound}));

            var dto = _mapper.Map<UserDto>(user);

            return Result<UserDto>.SuccessFull(dto);
        }

        public async Task<User> GetUserWithActivationCode(string activationCode)
        {
            return await GetAll().FirstOrDefaultAsync(x => x.ConfirmCode == activationCode);
        }

        public async Task<Result<User>> GetByEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
                return Result<User>.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.InvalidEmailAddress}));

            var user = await FirstOrDefaultAsyncAsNoTracking(u => u.Email == email);

            if (user == null)
                return Result<User>.Failed(new NotFoundObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserNotFound}));

            return Result<User>.SuccessFull(user);
        }

        public async Task<Result> UpdateStatus(Guid id, UserStatus userStatus)
        {
            if (id == Guid.Empty)
                return Result.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.InvalidUserId}));

            var user = await FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return Result.Failed(new NotFoundObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserNotFound}));

            user.Status = userStatus;
            await Context.SaveChangesAsync();

            return Result.SuccessFull();
        }

        public async Task<Result<IList<GroupDto>>> UserGroups(Guid id)
        {
            if (id == Guid.Empty)
                return Result<IList<GroupDto>>.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.InvalidUserId}));
            var user = await FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return Result<IList<GroupDto>>.Failed(new NotFoundObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserNotFound}));

            // groups that created by user
            var groups = await Context.Groups.Where(ug => ug.OwnerId == id).ToListAsync();
            // groups that invited or added user
            var userInGroups = await Context.UserInGroups.Where(ug => ug.UserId == id).Include(ug => ug.Groups)
                .ToListAsync();
            if (userInGroups.Any())
                groups.AddRange(userInGroups.Select(ug => ug.Groups));

            return Result<IList<GroupDto>>.SuccessFull(_mapper.Map<List<GroupDto>>(groups));
        }

        public async Task<Result<IList<EventDto>>> UserEvents(Guid id)
        {
            if (id == Guid.Empty)
                return Result<IList<EventDto>>.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.InvalidUserId}));
            var user = await FirstOrDefaultAsync(u => u.Id == id);

            if (user == null)
                return Result<IList<EventDto>>.Failed(new NotFoundObjectResult(new ApiMessage
                    {Message = ResponseMessage.UserNotFound}));

            var events = await Context.UserInEvents.Where(ug => ug.UserId == id)
                .Include(ug => ug.Event.Category)
                .Include(ug => ug.Event.EventSchedule)
                .Include(ug => ug.Event.EventGallery)
                .ToListAsync();

            return Result<IList<EventDto>>.SuccessFull(_mapper.Map<List<EventDto>>(events));
        }

        public async Task<Result<IList<RoleDto>>> UserRoles(Guid userId)
        {
            var roles = await Context.UserInRoles.Where(ur => ur.UserId == userId)
                .Include(ur => ur.Role.RolePermissions)
                .ThenInclude(rp => rp.Permission).ThenInclude(p => p.Parent).ToListAsync();

            return Result<IList<RoleDto>>.SuccessFull(_mapper.Map<List<RoleDto>>(roles.Select(r => r.Role)));
        }

        public async Task<Result> ChangePassword(ChangePasswordDto changePasswordDto, Guid userId)
        {
            var user = await FirstOrDefaultAsync(u => u.Id == userId);

            if (!StringUtil.CheckPassword(changePasswordDto.OldPassword, user.Password))
                return Result.Failed(new BadRequestObjectResult(new ApiMessage
                    {Message = ResponseMessage.WrongPassword}));


            user.Password = StringUtil.HashPass(changePasswordDto.NewPassword);


            await Context.SaveChangesAsync();
            return Result.SuccessFull();
        }

    }
}