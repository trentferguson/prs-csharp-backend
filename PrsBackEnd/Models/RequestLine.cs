using Microsoft.Build.Evaluation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;


namespace PrsBackEnd.Models
{
    public class RequestLine
    {
        [Key]
        public int Id { get; set; }

        public int RequestId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(RequestId))]
        public Request? Request { get; set; }

        [JsonIgnore]
        [ForeignKey(nameof(ProductId))]
        public Product? Product { get; set; }

        [JsonIgnore]
        public List<RequestLine>? GetAllRequestLines { get; set; }


    }
}
