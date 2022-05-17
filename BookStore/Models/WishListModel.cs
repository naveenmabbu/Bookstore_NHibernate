using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace BookStore.Models
{
    public class WishListModel
    {
        public virtual int WishlistId { get; set; }
        public virtual int UserId { get; set; }
        [JsonIgnore]
        public virtual UserModel UserModels { get; set; }
        public virtual int BookId { get; set; }
        [JsonIgnore]
        public virtual BookModel BookModels { get; set; }
    }
}