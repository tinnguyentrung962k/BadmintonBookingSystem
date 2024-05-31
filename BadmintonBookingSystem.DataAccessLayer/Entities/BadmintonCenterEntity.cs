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
    }
}
