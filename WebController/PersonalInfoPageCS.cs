using System;
using System.Linq;
using Xamarin.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace WebController
{
	public class PersonalInfoPageCS : ContentPage
	{
		public YangDb _database;

		Entry usernameEntry, passwordEntry, pingEntry, urlEntry;
		Label messageLabel;
        List<PathItemUI> pathUIlist = new List<PathItemUI>();
        StackLayout mContent;
		public PersonalInfoPageCS()
		{
			_database = new YangDb();
            App.PathList = _database.GetPaths(App.UserEntity.ID);
            messageLabel = new Label(){TextColor=Color.Red,Margin = new Thickness(20, 5) };
			usernameEntry = new Entry
			{
				Margin = new Thickness(20, 5)
			};
			passwordEntry = new Entry
			{
				IsPassword = true,
				Margin = new Thickness(20, 5)
			};
			pingEntry = new Entry()
			{
				Keyboard = Keyboard.Numeric,
				Margin = new Thickness(20, 5)
			};
			pingEntry.TextChanged += Pin_Control;
			urlEntry = new Entry()
			{
				Margin = new Thickness(20, 5)
			};
			// Path container
            RelativeLayout pathTitleContainer = new RelativeLayout()
			{
				Margin = new Thickness(20, 5)
			};
			// path title
			Label pathLabel = new Label { Text = "Path" };
			pathTitleContainer.Children.Add(pathLabel,
							Constraint.Constant(0),
							Constraint.Constant(0)
						   );
            
			// add path button
			Button btnAdd = new Button() { Text = "Add More Path" };
            btnAdd.Clicked += OnAddMoreClicked;
			pathTitleContainer.Children.Add(btnAdd,
							Constraint.RelativeToView(pathLabel, (parent, sibling) => { return sibling.X + sibling.Width+20; }),
                                   Constraint.RelativeToParent((parent) => { return -10; })
						   );

            var saveButton = new Button
			{
				Text = "Save"
			};
			saveButton.Clicked += OnSaveButtonClicked;

			Title = "Personal Info";
			mContent = new StackLayout()
			{
				VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
					new Label { Text = "Username", Margin= new Thickness(20,5) },
					usernameEntry,
					new Label { Text = "Password", Margin= new Thickness(20,5) },
					passwordEntry,
					new Label { Text = "4 Digital Ping", Margin= new Thickness(20,5) },
					pingEntry,
					new Label { Text = "URL", Margin= new Thickness(20,5) },
					urlEntry,
					pathTitleContainer,
					
				}
			};

            foreach(var item in App.PathList){
				// add path
				Grid pathCtn = MakeNewPath(item);
                mContent.Children.Add(pathCtn);
            }

			mContent.Children.Add(saveButton);
			mContent.Children.Add(messageLabel);
			Content = mContent;

		}

        // create more path
        private Grid MakeNewPath(PathEntity pathEntiry)
		{
			Grid pathCtn = new Grid
			{
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Margin = new Thickness(20, 5)
			};
			Entry name = new Entry { Placeholder = "Input Name" };
			Entry url = new Entry { Placeholder = "Input Path URL" };
			pathCtn.Children.Add(name, 0, 0);
			pathCtn.Children.Add(url, 1, 0);

            PathItemUI item = new PathItemUI();
			item.grid = pathCtn;
            item.name = name;
            item.path = url;
            item.number = pathUIlist.Count();
            if(pathEntiry != null){
				item.name.Text = pathEntiry.Name;
                item.path.Text = pathEntiry.Path;
            }

			pathUIlist.Add(item);
			return pathCtn;
		}

        // add more path click
        void OnAddMoreClicked(object sender, EventArgs e)
        {
            bool isEmpty = false;
            foreach(PathItemUI item in pathUIlist){
                if(string.IsNullOrEmpty(item.name.Text) || string.IsNullOrEmpty(item.path.Text)){
                    isEmpty = true;
                    break;
                }else{
                    isEmpty = false;
                }
            }
            if(isEmpty)
                messageLabel.Text = "Please fill the name with path";
            else{
				mContent.Children.Insert(mContent.Children.Count - 2, MakeNewPath(null));
                messageLabel.Text = "";
			}
        }


        protected override void OnAppearing()
		{
			base.OnAppearing();
			if (App.UserEntity != null)
			{
				usernameEntry.Text = App.UserEntity.Name;
				passwordEntry.Text = App.UserEntity.Password;
				pingEntry.Text = App.UserEntity.Pin;
				urlEntry.Text = App.UserEntity.Url;
			}
		}

		// register click event
        async void OnSaveButtonClicked(object sender, EventArgs e)
		{
			App.UserEntity.Name = usernameEntry.Text;
			App.UserEntity.Password = passwordEntry.Text;
			App.UserEntity.Pin = pingEntry.Text;
			App.UserEntity.Url = urlEntry.Text;

			// Sign up logic goes here
			var signUpSucceeded = AreDetailsValid(App.UserEntity);
			if (signUpSucceeded && IsPathsValid())
			{
				//var rootPage = Navigation.NavigationStack.FirstOrDefault();
				_database.UpdateUser(App.UserEntity);
                for (int i = 0; i < pathUIlist.Count; i++)
                {
                    PathEntity path = new PathEntity();
                    PathItemUI item = pathUIlist.ElementAt(i);

                    path.Number = i;
                    path.Name = item.name.Text;
                    path.Path = item.path.Text;
                    path.UserID = App.UserEntity.ID;

                    foreach(var pt in App.PathList){
                        _database.DeletePath(pt);
                    }
                    _database.AddPath(path);
                    App.PathList = _database.GetPaths(App.UserEntity.ID);
                }
				
                Application.Current.MainPage = new MainPageCS();
                //var answer = await DisplayAlert("Logout", "Do you want to logout?", "Yes", "No");
                await DisplayAlert("Success", "Update success", "Ok");
				
			}
			else
			{
				messageLabel.Text = "Update User failed";
			}
		}

        // check user fields
		bool AreDetailsValid(UserEntity user)
		{
            return (!string.IsNullOrWhiteSpace(user.Name) && !string.IsNullOrWhiteSpace(user.Password) && !string.IsNullOrWhiteSpace(user.Pin) && Utils.IsNumeric(user.Pin));
		}
        // check path valid
        bool IsPathsValid(){
            foreach (var item in pathUIlist)
            {
                if(string.IsNullOrEmpty(item.name.Text) || string.IsNullOrEmpty(item.path.Text)){
                    return false;
                }
            }
            return true;
        }

		// control the pin as 4 digit
        void Pin_Control(object sender, TextChangedEventArgs e)
		{
			Entry thisBlock = ((Entry)sender);
			String pingStr = e.NewTextValue;

			Debug.WriteLine(Utils.IsNumeric(pingStr));
			if (pingStr == null || pingStr.Trim().Length == 0)
			{

			}
			else
			{
				//if(!Utils.IsNumeric(pingStr.Substring((pingStr.Length - 1), 1)))
				if (!Utils.IsNumeric(pingStr))
				{
					thisBlock.Text = e.OldTextValue;
				}
				else if (pingStr.Length > 4)
				{
					thisBlock.Text = pingStr.Remove(4);
				}
			}
		}

		class PathItemUI
		{
			public Grid grid;
			public Entry name;
			public Entry path;
			public int number;
		}
	}
}

