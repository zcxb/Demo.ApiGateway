using System.ComponentModel.DataAnnotations;

namespace Demo.ApiGateway.Core.Models
{
    public partial class RouteMapping
    {
        [Key]
        public int MappingId { get; set; }

        public int? GlobalId { get; set; }

        public int? RouteId { get; set; }
    }
}
