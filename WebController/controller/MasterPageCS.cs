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
            masterPageItems.Add(new MasterPageItem
            { TargetType = typeof(AddPathCS)
                
            });
			masterPageItems.Add (new MasterPageItem {
				Title = "Home Page",
				IconSource = "contacts.png",
                TargetType = typeof(HomePageCS),
                PathEntity = null,
			});
			masterPageItems.Add (new MasterPageItem {
				Title = "Personal Info Page",
				IconSource = "todo.png",
                TargetType = typeof(PersonalInfoPageCS)
			});
			//masterPageItems.Add (new MasterPageItem {
			//	Title = "Developer Page",
			//	IconSource = "reminders.png",
			//	TargetType = typeof(DeveloperPageCS)
			//});

            // add all the paths to navigation
            foreach(var path in App.PathList){
                masterPageItems.Add(new MasterPageItem
                {
                    Title = path.Path,
                    IconSource = "reminders.png",
                    TargetType = typeof(HomePageCS),
                    PathEntity = new HomePathEntity(path.Path, path.Parent)
				});
            }

			masterPageItems.Add(new MasterPageItem
			{
				Title = "Logout",
				IconSource = "reminders.png",
                TargetType = typeof(KeyPadCS)
			});

			listView = new ListView {
				ItemsSource = masterPageItems,
				ItemTemplate = new DataTemplate (() => {
                    //var textCell = new TextCell();
                    //textCell.SetBinding(TextCell.TextProperty, "Title");
                    //var image = new Image();
                    //image.SetBinding(Image.SourceProperty, "IconSource");
                    //return textCell;
                    var grid = new Grid { Padding = new Thickness(5, 10) };
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = new GridLength(30) });
                    grid.ColumnDefinitions.Add(new ColumnDefinition { Width = GridLength.Star });

                    var image = new Image();
                    image.SetBinding(Image.SourceProperty, "IconSource");
                    var label = new Label { VerticalOptions = LayoutOptions.FillAndExpand };
                    label.SetBinding(Label.TextProperty, "Title");
                    label.TextColor = Color.White;
                    grid.Children.Add(image);
                    grid.Children.Add(label, 1, 0);
                    return new ViewCell { View = grid };

				}),
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorVisibility = SeparatorVisibility.None
			};
            listView.BackgroundColor = Color.FromHex("2196F3");

			Padding = new Thickness (0, 0, 0, 0);

			Icon = "hamburger.png";
			Title = "Personal Organiser";
			Content = new StackLayout {
				VerticalOptions = LayoutOptions.FillAndExpand,
				Children = {
					listView
				},
			};

		}

        public void refresh(){
            
        }
	}
}
