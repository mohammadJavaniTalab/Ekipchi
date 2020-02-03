﻿using System;

namespace Hastnama.Ekipchi.Data.Event.Schedule
{
    public class UpdateEventScheduleDto
    {
        public Guid Id { get; set; }

        public Guid EventId { get; set; }

        public DateTime RegistrationDate { get; set; }

        public DateTime? EndRegistrationDate { get; set; }


        public DateTime EventDate { get; set; }

        public TimeSpan StartHour { get; set; }

        public TimeSpan EndHour { get; set; }

        public DateTime? RemoveEventInfoDate { get; set; }

    }
}