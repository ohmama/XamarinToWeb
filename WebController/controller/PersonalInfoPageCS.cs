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

        Entry usernameEntry, passwordEntry, pinEntry, urlEntry;
        Label messageLabel;
        List<PathItemUI> pathUIlist = new List<PathItemUI>();
        StackLayout mContent;

        public PersonalInfoPageCS()
        {
            _database = new YangDb();
            App.PathList = _database.GetPaths(App.UserEntity.ID);
            messageLabel = new Label() { TextColor = Color.Red, Margin = new Thickness(20, 5) };
            usernameEntry = new Entry
            {
                Margin = new Thickness(20, 5)
            };
            passwordEntry = new Entry
            {
                IsPassword = true,
                Margin = new Thickness(20, 5)
            };
            pinEntry = new Entry()
            {
                Keyboard = Keyboard.Numeric,
                Margin = new Thickness(20, 5)
            };
            pinEntry.TextChanged += Pin_Control;
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
            Label pathTitle = new Label { Text = "Path" };
            pathTitleContainer.Children.Add(pathTitle,
                            Constraint.Constant(0),
                            Constraint.Constant(0)
                           );

            // add ADD button
            Button btnAdd = new Button() { Text = "Add More Path" };
            btnAdd.Clicked += OnAddMoreClicked;
            pathTitleContainer.Children.Add(btnAdd,
                            Constraint.RelativeToView(pathTitle, (parent, sibling) => { return sibling.X + sibling.Width + 20; }),
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
                    pinEntry,
                    new Label { Text = "URL", Margin= new Thickness(20,5) },
                    urlEntry,
                    pathTitleContainer,

                }
            };

            foreach (var item in App.PathList)
            {
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
            Entry name = new Entry { 
                Placeholder = "Input Name", 
                Keyboard = Keyboard.Create(KeyboardFlags.None) 
            };
            Entry url = new Entry { 
                Placeholder = "Input Path URL",
				Keyboard = Keyboard.Create(KeyboardFlags.None)
			};
			Picker parentPathPicker = new Picker
			{
				Title = "Parent",
				VerticalOptions = LayoutOptions.CenterAndExpand
			};

            parentPathPicker.Items.Add(PathItemUI.NO_PARENT);
            foreach (var pathItem in App.PathList)
            {
                if(pathItem != pathEntiry){
                    parentPathPicker.Items.Add(pathItem.Name);
                }
            }

            name.Unfocused += ItemFieldUnfocus;
			url.Unfocused += ItemFieldUnfocus;

            pathCtn.Children.Add(name, 0, 0);
			pathCtn.Children.Add(url, 1, 0);
            // add picker
            pathCtn.Children.Add(parentPathPicker, 2, 0);


            PathItemUI item = new PathItemUI();
            item.grid = pathCtn;
            item.name = name;
            item.path = url;
            item.picker = parentPathPicker;
            item.number = pathUIlist.Count();
            if (pathEntiry != null)
            {
                item.name.Text = pathEntiry.Name;
                item.path.Text = pathEntiry.Path;
                if(pathEntiry.Parent!=null)
                    item.picker.SelectedItem = pathEntiry.Parent;
            }

            pathUIlist.Add(item);
            return pathCtn;
        }

        // add more path click
        void OnAddMoreClicked(object sender, EventArgs e)
        {
            bool isEmpty = false;
            foreach (PathItemUI item in pathUIlist)
            {
                if (string.IsNullOrEmpty(item.name.Text) || string.IsNullOrEmpty(item.path.Text))
                {
                    isEmpty = true;
                    break;
                }
                else
                {
                    isEmpty = false;
                }
            }
            if (isEmpty)
                messageLabel.Text = "Please fill the name with path";
            else
            {
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
                pinEntry.Text = App.UserEntity.Pin;
                urlEntry.Text = App.UserEntity.Url;
            }
        }

        // register click event
        async void OnSaveButtonClicked(object sender, EventArgs e)
        {
            App.UserEntity.Name = usernameEntry.Text;
            App.UserEntity.Password = passwordEntry.Text;
            App.UserEntity.Pin = pinEntry.Text;
            App.UserEntity.Url = urlEntry.Text;

            // Sign up logic goes here
            var signUpSucceeded = AreDetailsValid(App.UserEntity);
            if (signUpSucceeded && IsPathsValid())
            {
                //var rootPage = Navigation.NavigationStack.FirstOrDefault();
                _database.UpdateUser(App.UserEntity);
                // delete all the path exist
                foreach (var pt in App.PathList)
                {
                    _database.DeletePath(pt);
                }
                // add all the paths 
                for (int i = 0; i < pathUIlist.Count; i++)
                {
                    PathEntity path = new PathEntity();
                    PathItemUI item = pathUIlist.ElementAt(i);

                    path.Number = i;
                    path.Name = item.name.Text;
					path.Path = item.path.Text;
                    if(item.picker.SelectedItem!=null && !item.picker.SelectedItem.Equals(PathItemUI.NO_PARENT)){
						path.Parent = item.picker.SelectedItem.ToString();
                    }else{
                        path.Parent = PathItemUI.NO_PARENT;
                    }
                    path.UserID = App.UserEntity.ID;

                    _database.AddPath(path);
                }
				App.PathList = _database.GetPaths(App.UserEntity.ID);

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
        bool IsPathsValid()
        {
            foreach (var item in pathUIlist)
            {
                if (string.IsNullOrEmpty(item.name.Text) || string.IsNullOrEmpty(item.path.Text))
                {
                    return false;
                }
            }
            return true;
        }

        // control the pin as 4 digit
        void Pin_Control(object sender, TextChangedEventArgs e)
        {
            Entry thisBlock = ((Entry)sender);
            String pinStr = e.NewTextValue;

            Debug.WriteLine(Utils.IsNumeric(pinStr));
            if (pinStr == null || pinStr.Trim().Length == 0)
            {

            }
            else
            {
                //if(!Utils.IsNumeric(pingStr.Substring((pingStr.Length - 1), 1)))
                if (!Utils.IsNumeric(pinStr))
                {
                    thisBlock.Text = e.OldTextValue;
                }
                else if (pinStr.Length > 4)
                {
                    thisBlock.Text = pinStr.Remove(4);
                }
            }
        }


        void fillDataToPicker(Entry currentPathUI){
            List<string> datas = new List<string>();

			//exclude not complished one
			for (int i = 0; i < pathUIlist.Count(); i++)
			{
				PathItemUI pathItem = pathUIlist[i];

				 
			    if (string.IsNullOrEmpty(pathItem.name.Text) || string.IsNullOrEmpty(pathItem.path.Text))
			    {
			        Debug.WriteLine("not complish");
                } else{
                    datas.Add(pathItem.name.Text);
                }

				// exclude the current same path. 
				//if (currentPathUI != pathItem.name && currentPathUI != pathItem.path)
				//{
				//	// exclude not complished one
				//	if (string.IsNullOrEmpty(pathItem.name.Text) || string.IsNullOrEmpty(pathItem.path.Text))
				//	{
				//		Debug.WriteLine("not complish");
				//	}
				//	else
				//	{
				//		// if everthing all right, set the item
				//		pathItem.picker.Items.Add(pathItem.name.Text);
				//	}
				//}
				//else
				//{
				//	Debug.WriteLine("they are same one");
				//}
			}
            for (int i = 0; i < pathUIlist.Count(); i++){
                PathItemUI pathItem = pathUIlist[i];
				// first clear then add 
				pathItem.picker.Items.Clear();
				pathItem.picker.Items.Add(PathItemUI.NO_PARENT);

                foreach (var item in datas)
                {
                    if(!pathItem.name.Text.Equals(item)){
                        pathItem.picker.Items.Add(item);
                    }
                }
            }




		}
		// after text change, dynamicly update the picker's value
        void ItemFieldUnfocus(object sender, EventArgs e)
		{
			Entry thisBlock = ((Entry)sender);
            fillDataToPicker(thisBlock);
		}

        class PathItemUI
        {
            public const string NO_PARENT = "No Parent";
            public Grid grid;
            public Entry name;
			public Entry path;
            public Picker picker;
            public int number;
        }
    }
}

