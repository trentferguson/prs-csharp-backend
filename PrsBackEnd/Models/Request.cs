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

        [StringLength(80)]
        public string Description { get; set; }

        [StringLength(80)]
        public string Justification { get; set; }

        [StringLength(80)]
        public string RejectionReason { get; set; }

        [Required, StringLength(20)]
        public string DeliveryMode { get; set; }

        public DateTime SubmittedDate { get; set; } = DateTime.Now;

        public DateTime DateNeeded { get; set; }

        [Required, StringLength(10)]
        public string Status { get; set; } = "NEW";

        [Column(TypeName = "decimal(11,2)")]
        public decimal Total { get; set; }

        [ForeignKey(nameof(UserId))]
        public User? User { get; set; }

        [JsonIgnore]
        public int UserId { get; set; }

        [JsonIgnore]
        public List<RequestLine>? RequestLines { get; set; }


    }
}
