﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Hastnama.Ekipchi.Common.Enum;

namespace Hastnama.Ekipchi.Data.User
{
    public class AdminUpdateUserDto
    {
        [JsonIgnore] public Guid Id { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public string Name { get; set; }
        public string Family { get; set; }
        public string Avatar { get; set; }
        public List<int> Roles { get; set; }
    }
}