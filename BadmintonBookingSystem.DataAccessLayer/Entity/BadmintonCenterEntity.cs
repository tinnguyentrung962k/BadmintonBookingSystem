using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BadmintonBookingSystem.DataAccessLayer.Entity
{
    [Table("BadmintonCenter")]
    public class BadmintonCenterEntity : BaseEntity
    {
        public string Name { get; set; }
        public string Location { get; set; }
    }
}
