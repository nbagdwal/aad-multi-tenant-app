using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations;

namespace NotesKeeper.Models
{
    public class Item
    {
        [JsonProperty(PropertyName = "upn")]
        public string UPN { get; set; }
        [JsonProperty(PropertyName = "id")]
        public string Id { get; set; }

        [JsonProperty(PropertyName = "tenantId")]
        public string TenantId { get; set; }


        [JsonProperty(PropertyName = "name")]
        [JsonRequired]
        [Required]
        public string Name { get; set; }

        [JsonProperty(PropertyName = "description")]
        [JsonRequired]
        [Required]
        public string Description { get; set; }

        [JsonProperty(PropertyName = "isComplete")]
        public bool Completed { get; set; }

        [JsonProperty(PropertyName = "createdBy")]
        public string CreatedBy { get; set; }
       
    }
}
