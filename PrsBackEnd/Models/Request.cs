﻿using Microsoft.Build.Evaluation;

namespace PrsBackEnd.Models
{
    public class Request
    {

        public int Id { get; set; }

        public string Description { get; set; }

        public string Justification { get; set; }

        public string RejectionReason { get; set; }

        public string DeliveryMode { get; set; }

        public string Status { get; set; }

        public decimal Total { get; set; }

        public virtual User User { get; set; }

        public int UserId { get; set; }

        public virtual List<RequestLine> RequestLines { get; set; }

        public Request()
        {
        }
    }
}
