﻿using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace BLL.Entities
{
    public class Role : IdentityRole<Guid>
    {

    }

    public class UserClaims : IdentityUserClaim<Guid>
    {

    }

    public class UserRole : IdentityUserRole<Guid>
    {
        //public Guid UserId { get; set; }
        //public User User { get; set; }
        //public Guid RoleId { get; set; }
        //public Role Role { get; set; }
        //[Key]
        //public Guid Id => Guid.NewGuid();
        //public Guid Id { get; set; }
    }

    public class UserLogin : IdentityUserLogin<Guid>
    {
        [Key]
        public Guid Id { get; set; }
    }

    public class RoleClaim : IdentityRoleClaim<Guid>
    {

    }

    public class UserToken : IdentityUserToken<Guid>
    {
        [Key]
        public Guid Id { get; set; }
    }

    public class User : IdentityUser<Guid>
    {
        public Cart Cart { get; set; }
    }
}
