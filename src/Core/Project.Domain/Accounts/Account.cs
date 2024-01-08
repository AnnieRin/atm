using Project.Domain.ATMTransactions;
using Project.Domain.Common;
namespace Project.Domain.Accounts;
public class Account : BaseEntity
{
    public string AccountNumber { get; set; }
    public decimal Balance { get; set; }
    public string PersonalN { get; set; }
    public Guid UserNumber { get; set; }
    public virtual ICollection<ATMTransaction>? Transactions { get; set; }
}
