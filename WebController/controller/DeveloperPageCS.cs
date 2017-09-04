using Xamarin.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace WebController
{
	public class DeveloperPageCS : ContentPage
	{
		private YangDb _database;
		private ListView _userList;

		public DeveloperPageCS()
		{
			_database = new YangDb();
			var users = _database.GetUsers();

			_userList = new ListView();
			_userList.ItemsSource = users;
			_userList.ItemTemplate = new DataTemplate(typeof(TextCell));
			_userList.ItemTemplate.SetBinding(TextCell.TextProperty, "Name");
			_userList.ItemTemplate.SetBinding(TextCell.DetailProperty, "Pin");
            _userList.ItemSelected += (sender, e) => {
				//((ListView)sender).SelectedItem = null;

			};

			var toolbarItem = new ToolbarItem
			{
				Text = "Add",
			};

			ToolbarItems.Add(toolbarItem);

			Content = _userList;

            List<PathEntity> mList = _database.GetPaths(App.UserEntity.ID);
            for (int i = 0; i < mList.Count; i++){
                Debug.WriteLine("path "+i+" " + mList[i].Name);
            }


		}
		public void Refresh()
		{
			_userList.ItemsSource = _database.GetUsers();
		}
	}
}
