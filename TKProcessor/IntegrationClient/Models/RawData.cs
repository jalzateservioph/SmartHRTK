using System;
using System.Collections.Generic;
using System.Text;

namespace IntegrationClient.Models
{
    public class RawData
    {
        public Guid Id { get; set; }
        public string BiometricsId { get; set; }
        public DateTime Transaction { get; set; }
    }
}
