namespace Model.EF
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Role")]
    public partial class Role
    {
        public long ID { get; set; }
        [StringLength(50)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Detail { get; set; }
    }
}
