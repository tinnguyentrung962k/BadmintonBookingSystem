using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace BadmintonBookingSystem.DataAccessLayer.Entity
{
    [Table("Slot")]
    public class SlotEntity : BaseEntity
    {
        [Required]
        public string SlotName { get; set; }
        [Required]
        public string CenterId { get; set; }
        [Required]
        public double Price { get; set; }
        
        [ForeignKey(nameof(CenterId))]
        public virtual BadmintonCenterEntity BadmintonCenter { get; set; }
        public IEnumerable<BookingOrderEntity> BookingOrderEntities { get; set; } 


    }
}
