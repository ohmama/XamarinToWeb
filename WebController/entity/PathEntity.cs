﻿using System;
using SQLite.Net.Attributes;
namespace WebController
{
    public class PathEntity:BaseEntity
    {
        public int UserID { get; set; }
		//public string Name { get; set; }
		public string Path { get; set; }
		public string Parent { get; set; }
        public int Number { get; set; }
        public PathEntity()
        {
        }


    }
}
