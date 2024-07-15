using BadmintonBookingSystem.DataAccessLayer.Entities.BaseEntities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Entities
{
    [Table("BadmintonCenter")]
    public class BadmintonCenterEntity : BaseAuditEntity<string>
    {
        public string Name { get; set; }
        public string Location { get; set; }
        public TimeOnly OperatingTime { get; set; }
        public TimeOnly ClosingTime { get; set; }
        public string ManagerId { get; set; }

        [ForeignKey(nameof(ManagerId))]
        public UserEntity Manager { get; set; }

        public IEnumerable<BadmintonCenterImage>? BadmintonCenterImages { get; set; }
        public IEnumerable<CourtEntity>? Courts { get; set; }
    }
}
