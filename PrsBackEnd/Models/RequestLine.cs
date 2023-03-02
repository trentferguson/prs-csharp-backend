using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PrsBackEnd.Models
{
    public class RequestLine
    {

        public Product? Product { get; set; }

        [JsonIgnore]
        public int ProductId { get; set; }

        public int Id { get; set; }

        [ForeignKey(nameof(RequestId))]
        public Request? Request { get; set; }

        [JsonIgnore]
        public int RequestId { get; set; }

        public int Quantity { get; set; }

    }
}
