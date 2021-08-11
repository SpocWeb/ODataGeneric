using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ODataGeneric.SampleControllers.Entities
{
    /// <remarks> Generated with all Fields from the DB Table [Statics].[Counterparty] </remarks>
    [Table(nameof(Counterparty), Schema = Schema)]
    [Index(nameof(LegalName), IsUnique = true, Name = "UX_" + nameof(Counterparty) + "_" + nameof(LegalName))]
    public class Counterparty : AEntity<int>
    {
        public const string Schema = "Statics";

        /// <inheritdoc />
        public Counterparty(int id, string changeUser)
            : base(id, changeUser) { }

        public Counterparty() { }

        [ForeignKey(nameof(Agent))]
        public int? AgentId { get; set; }
        public Counterparty? Agent { get; set; }

        public string? Comment { get; set; }

        public string? CountryId { get; set; }

        public string? EligibleSwiftMts { get; set; }

        public string? Extensions { get; set; }

        public string? FinancialType { get; set; }

        public string? Info1 { get; set; }

        public string? Info2 { get; set; }

        public string? IssuerName { get; set; }

        [MaxLength(50)]
        public string? LegalName { get; set; }

        public string? Location { get; set; }

        public bool ManualPayment { get; set; }

        public bool PayDifferenceForChanges { get; set; }

        public bool TradeNetting { get; set; }

        public string? Type { get; set; }

    }
}