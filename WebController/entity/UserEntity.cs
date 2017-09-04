using System;
using SQLite.Net.Attributes;

namespace WebController
{
    public class UserEntity:BaseEntiry
    {

		public string Name { get; set; }
		public string Password { get; set; }
		public string Pin { get; set; }
		public string Url { get; set; }

		public UserEntity()
		{
		}
    }
}
