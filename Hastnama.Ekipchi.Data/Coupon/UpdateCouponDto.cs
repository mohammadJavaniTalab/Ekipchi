﻿using System;
using System.Text.Json.Serialization;

namespace Hastnama.Ekipchi.Data.Coupon
{
    public class UpdateCouponDto
    {
        [JsonIgnore] public Guid? Id { get; set; }

        public string Code { get; set; }

        public int Count { get; set; }

        public int TotalCount { get; set; }

        public long Amount { get; set; }

        public bool IsUnlimited { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }

        public bool IsActive { get; set; }

        public bool IsDeleted { get; set; }
    }
}