using System.Text.Json.Serialization;

namespace PrsBackEnd.Models
{
    public class RequestLine
    {

        public int Id { get; set; }

        [JsonIgnore]
        public virtual Request Request { get; set; }

        public int RequestId { get; set; }

        public virtual Product Product { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

    }
}
