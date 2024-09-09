using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace GreenMarketBackend.Models
{
    public class Report
    {
        [Key]
        public int ReportId { get; set; }

        [Required]
        public string ReporterUserId { get; set; }

        [ForeignKey("ReporterUserId")]
        public virtual ApplicationUser ReporterUser { get; set; }

        [Required]
        public string ReportedUserId { get; set; }

        [ForeignKey("ReportedUserId")]
        public virtual ApplicationUser ReportedUser { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime Date { get; set; }
    }
}
