﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;
using ThanalSoft.SmartComplex.Entities.Security;

namespace ThanalSoft.SmartComplex.Entities.Complex
{
    [DataContract]
    [Table("tblMemberFlat")]
    public class MemberFlat : BaseModel
    {
        [DataMember]
        [Required]
        public int ApartmentId { get; set; }

        [DataMember]
        [Required]
        public int FlatId { get; set; }

        [DataMember]
        [Required]
        public Int64 UserId { get; set; }

        [DataMember]
        [Required]
        public bool IsOwner { get; set; }

        [ForeignKey("UserId")]
        public virtual LoginUser User { get; set; }

        [ForeignKey("FlatId")]
        public virtual Flat Flat { get; set; }

        [ForeignKey("ApartmentId")]
        public virtual Apartment Apartment { get; set; }

    }
}