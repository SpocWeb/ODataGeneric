using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ODataGeneric.SampleControllers.Entities
{
    /// <remarks> Generated with all Fields from the DB Table [Statics].[Currency] </remarks>
    [Table(nameof(Currency), Schema = Schema)]
    [Index(nameof(Ccy), IsUnique = true, Name = "UX_" + nameof(Currency) + "_" + nameof(Ccy))]
    public class Currency : AEntity<int>
    {
        public const string Schema = "Statics";

        public Currency(){}

        [MaxLength(3)]
        [MinLength(3)]
        public string? Ccy { get; set; }

        //Rather navigate this from Entity!
        //[InverseProperty(nameof(CurrencyDetails.Currency))]
        //public List<CurrencyDetails> Details { get; set; }

        public string? CalendarCodes { get; set; }

        public int DecimalPoints { get; set; }

        public string? Description { get; set; }

        public int EarlyDispoDays { get; set; }

        public string? Extensions { get; set; }

        public bool IsDeliverable { get; set; }

        public bool IsManual { get; set; }

        public double? MaxThreshold { get; set; }

    }
}