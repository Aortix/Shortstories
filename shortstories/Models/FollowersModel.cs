﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace shortstories.Models
{
    public class FollowersModel
    {
        public FollowersModel()
        {
        }
        [Required(ErrorMessage = "No follower id present. This should be automatic.")]
        [Key]
        [Column(TypeName = "int")]
        public int FollowersModelId { get; set; }
        [Required(ErrorMessage = "A follower id is required.")]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100, ErrorMessage = "A follower id cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9-]*$", ErrorMessage = "Profile id regex issue. This should be automatic.")]
        public string FollowersId { get; set; }
        [Required(ErrorMessage = "No profile id present. This should be automatic.")]
        [Column(TypeName = "varchar(100)")]
        [StringLength(100, ErrorMessage = "The profile id cannot exceed 100 characters.")]
        [RegularExpression(@"^[a-zA-Z0-9-]*$", ErrorMessage = "Profile id regex issue. This should be automatic.")]
        public string ProfileId { get; set; }

        [ForeignKey("ProfileId")]
        public ProfileModel Profile { get; set; }
    }
}
