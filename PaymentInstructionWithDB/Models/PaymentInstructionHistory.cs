using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

#nullable disable

namespace PaymentInstructionWithDB.Models
{
    public partial class PaymentInstructionHistory
    {
        public int Id { get; set; }
	    public int PaymentInstructionId { get; set; }
        public int CurrencyId { get; set; }
        public int BillTypeId { get; set; }
        public string BeneficiaryName { get; set; }
        public string BeneficiaryBiccode { get; set; }
        [NotMapped]
        public virtual BillType BillType { get; set; }
        [NotMapped]
        public virtual Currency Currency { get; set; }
	    [NotMapped]
	    public virtual PaymentInstruction PaymentInstruction { get; set; }
    }
}
