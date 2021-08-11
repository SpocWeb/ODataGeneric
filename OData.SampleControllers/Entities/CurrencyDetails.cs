using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace ODataGeneric.SampleControllers.Entities
{
    /// <remarks> Generated with all Fields from the DB Table [Statics].[CurrencyDetails] </remarks>
    [Table(nameof(CurrencyDetails), Schema = Currency.Schema)]
    public class CurrencyDetails : AEntity<int>
    {

        public string? AlternativeCutOff { get; set; }

        //Rather navigate this from Entity!
        //[ForeignKey(nameof(Currency))]
        public int CurrencyId { get; set; }
        //public Currency Currency { get; set; }

        public int CutOffDays { get; set; }

        public TimeSpan CutOffTime { get; set; }

        public string? Extensions { get; set; }

        public int KickOffDays { get; set; }

        public TimeSpan KickOffTime { get; set; }
    }
}