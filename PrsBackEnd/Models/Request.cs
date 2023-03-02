using Microsoft.Build.Evaluation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace PrsBackEnd.Models
{
    public class Request
    {

        [Key]
        public int Id { get; set; }

        public string Description { get; set; }

        public string Justification { get; set; }

        public string RejectionReason { get; set; }

        public string DeliveryMode { get; set; }

        public string Status { get; set; }

        public decimal Total { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        public int UserId { get; set; }

        [JsonIgnore]
        public List<RequestLine>? RequestLines { get; set; }


    }
}
