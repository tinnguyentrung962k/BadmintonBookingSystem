using BadmintonBookingSystem.DataAccessLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Entities
{
    [Table("BadmintonCenterImage")]
    public class BadmintonCenterImage : BaseAuditEntity<string>
    {
        public string BadmintonCenterId { get; set; }
        public string ImageLink { get; set; }

        [ForeignKey(nameof(BadmintonCenterId))]
        public BadmintonCenterEntity BadmintonCenterEntity { get; set; }
    }
}
