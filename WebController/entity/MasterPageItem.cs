using System;

namespace WebController
{
	public class MasterPageItem
	{
		public string Title { get; set; }

		public string IconSource { get; set; }

		public Type TargetType { get; set; }

        public HomePathEntity PathEntity { get; set; }
	}
}
