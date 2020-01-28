﻿using Hastnama.Ekipchi.Common.Enum;

namespace Hastnama.Ekipchi.Data.User
{
    public class UserFilterQueryDto
    {
        public string Keyword { get; set; }
        public UserStatus? Status { get; set; }
        public Role? Role { get; set; }
    }
}