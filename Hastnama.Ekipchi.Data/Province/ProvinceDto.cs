﻿namespace Hastnama.Ekipchi.Data.Province
{
    public class ProvinceDto
    {
        public int Id { get; set; }

        public string Name { get; set; }
        
        public virtual ProvinceDto Province { get; set; }
        
    }
}