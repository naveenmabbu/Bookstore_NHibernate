﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class UserLogin
    {
        public virtual string Email
        {
            get;
            set;
        }
        public virtual string Password
        {
            get;
            set;
        }
    }
}