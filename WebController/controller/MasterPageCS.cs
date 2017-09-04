using System.Collections.Generic;
using Xamarin.Forms;
using System.Diagnostics;

namespace WebController
{
	public class MasterPageCS : ContentPage
	{
		public ListView ListView { get { return listView; } }

		ListView listView;

		public MasterPageCS ()
		{
			var masterPageItems = new List<MasterPageItem> ();
			masterPageItems.Add (new MasterPageItem {
				Title = "Home Page",
				IconSource = "contacts.png",
                TargetType = typeof(HomePageCS),
                Path = ""
			});
			masterPageItems.Add (new MasterPageItem {
				Title = "Personal Info Page",
				IconSource = "todo.png",
                TargetType = typeof(PersonalInfoPageCS)
			});
			masterPageItems.Add (new MasterPageItem {
				Title = "Developer Page",
				IconSource = "reminders.png",
				TargetType = typeof(DeveloperPageCS)
			});

            // add all the paths to navigation
            foreach(var path in App.PathList){
				masterPageItems.Add(new MasterPageItem
				{
                    Title = "Home " + path.Name,
					IconSource = "reminders.png",
					TargetType = typeof(HomePageCS),
                    Path = path.Path
				});
            }

			masterPageItems.Add(new MasterPageItem
			{
				Title = "Logout",
				IconSource = "reminders.png",
                TargetType = typeof(LoginPageCS)
			});

			listView = new ListView {
				ItemsSource = masterPageItems,
				ItemTemplate = new DataTemplate (() => {
					var imageCell = new ImageCell ();
					imageCell.SetBinding (TextCell.TextProperty, "Title");
					imageCell.SetBinding (ImageCell.ImageSourceProperty, "IconSource");
					return imageCell;
				}),
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorVisibility = SeparatorVisibility.None
			};

			Padding = new Thickness (0, 40, 0, 0);
			Icon = "hamburger.png";
			Title = "Personal Organiser";
			Content = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					listView
				}	
			};
		}

        public void refresh(){
            
        }
	}
}
