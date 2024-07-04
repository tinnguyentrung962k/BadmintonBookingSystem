using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using BadmintonBookingSystem.DataAccessLayer.Entities.BaseEntities;

namespace BadmintonBookingSystem.DataAccessLayer.Entities
{
    [Table("Court")]
    public class CourtEntity : BaseAuditEntity<string>
    {
        [Required]
        public string CourtName { get; set; }
        [Required]
        public string CenterId { get; set; }
        
        [ForeignKey(nameof(CenterId))]
        public virtual BadmintonCenterEntity BadmintonCenter { get; set; }
        public IEnumerable<BookingOrderEntity> BookingOrders { get; set; } 


    }
}
