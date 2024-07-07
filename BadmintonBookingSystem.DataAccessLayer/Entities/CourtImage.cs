using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BadmintonBookingSystem.DataAccessLayer.Entities.BaseEntities;

namespace BadmintonBookingSystem.DataAccessLayer.Entities
{
    [Table("CourtImage")]
    public class CourtImage : BaseAuditEntity<string>
    {
        public string CourtId { get; set; }
        public string ImageLink { get; set; }

        [ForeignKey(nameof(CourtId))]
        public CourtEntity CourtEntity { get; set; }
    }
}
