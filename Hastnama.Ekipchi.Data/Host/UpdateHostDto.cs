﻿using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Hastnama.Ekipchi.Common.Enum;
using Hastnama.Ekipchi.Data.Host.AvailableDate;

namespace Hastnama.Ekipchi.Data.Host
{
    public class UpdateHostDto
    {
        [JsonIgnore] public Guid? Id { get; set; }

        public string Name { get; set; }

        public string Address { get; set; }

        public string Latitude { get; set; }

        public string Longitude { get; set; }

        public string ContactPerson { get; set; }

        public string CallNumber { get; set; }

        public string CoverPhoto { get; set; }

        public string Email { get; set; }

        public string WebSite { get; set; }

        public PlaceType PlaceType { get; set; }

        public int Capacity { get; set; }

        public double PricePerHour { get; set; }

        public string Logo { get; set; }

        public string TermsAndCondition { get; set; }

        public bool IsDeleted { get; set; }

        public virtual List<string> Galleries { get; set; }

        public virtual List<int> Categories { get; set; }

        public virtual List<HostAvailableDateDto> HostAvailableDates { get; set; }
    }
}