using Project.Domain.Common;

namespace Project.Domain.ATMTransactions;
public class ATMTransaction : BaseEntity
{
    public string UserIBAN { get; set; }
    public string RecipientIBAN { get; set; }
    public decimal Amount { get; set; }
    public DateTime Date { get; set; } 
    public string Type { get; set; }
    public int AccountId { get; set; }
}
