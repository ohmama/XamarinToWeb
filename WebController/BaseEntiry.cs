using System;
using SQLite.Net.Attributes;
namespace WebController
{
    public class BaseEntiry
    {
		[PrimaryKey, AutoIncrement]
		public int ID { get; set; }
		public DateTime CreatedOn { get; set; }

		public BaseEntiry()
        {
        }
    }
}
