using System;
using System.Collections.Generic;

#nullable disable

namespace PaymentInstructionWithDB.Models
{
    public partial class Currency
    {
        public Currency()
        {
            PaymentInstructions = new HashSet<PaymentInstruction>();
            PaymentInstructionHistories = new HashSet<PaymentInstructionHistory>();
        }

        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }

        public virtual ICollection<PaymentInstruction> PaymentInstructions { get; set; }
        public virtual ICollection<PaymentInstructionHistory> PaymentInstructionHistories { get; set; }
    }
}
