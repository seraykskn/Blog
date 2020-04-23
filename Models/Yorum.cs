namespace mvcBlog.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Yorum")]
    public partial class Yorum
    {
        public int id { get; set; }

        [Required]
        public string YorumIcerik { get; set; }

        public int kullaniciId { get; set; }

        public int makaleId { get; set; }

        public DateTime tarih { get; set; }

        public virtual Kullanici Kullanici { get; set; }

        public virtual Makale Makale { get; set; }
    }
}
