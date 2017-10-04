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
        //bool isInit = true;
        Entry usernameEntry, passwordEntry, pinEntry, urlEntry;
        Label messageLabel;
        List<Grid> pathUIlist = new List<Grid>();
        StackLayout mContent;
        ScrollView mScrollView;
        //Grid gridCtn;
		//ListView lvPaths;

		//string url = "https://cirro.nimbus.co.nz/zd1";
		//string username = "zd1.user2@cirro.nimbus.co.nz";
		//string password = "123abcdef**";

        public PersonalInfoPageCS()
        {
            Debug.WriteLine("on construction");
            _database = new YangDb();
            App.PathList = _database.GetPaths(App.UserEntity.ID);
            messageLabel = new Label() { TextColor = Color.Red, Margin = new Thickness(20, 5) };
            usernameEntry = new Entry
            {
                Margin = new Thickness(20, 5),
                Keyboard = Keyboard.Create(KeyboardFlags.None)
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
                Margin = new Thickness(20, 5),
                Keyboard = Keyboard.Create(KeyboardFlags.None)
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
            btnAdd.Clicked += OnAddMoreClickedAsync;
            pathTitleContainer.Children.Add(btnAdd,
                            Constraint.RelativeToView(pathTitle, (parent, sibling) => { return sibling.X + sibling.Width + 20; }),
                                   Constraint.RelativeToParent((parent) => { return -10; })
                           );

            var saveButton = new Button
            {
                Text = "Save",
                MinimumHeightRequest = 40,
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


			//int index = 0;
			//foreach (var item in App.PathList)
			//{
			//	//if()
			//	// add path
   //             mContent.Children.Add(MakeNewPath(item));

			//	index++;
			//}

			//isInit = false;
            mContent.Children.Add(saveButton);
            mContent.Children.Add(messageLabel);


			Content = new ScrollView { Content = mContent, Margin = new Thickness(0, 20) };
        }

        // create more path
        private Grid MakeNewPath(PathEntity pathEntity)
        {
            Grid pathCtn = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(20, 0),
                MinimumHeightRequest = 40
            };

            Label lblItemPath = new Label
            {
                Text = pathEntity.Path
			};
            Label lblTop = new Label
            {
                Text = pathEntity.Parent != null ? pathEntity.Parent:"No Parent"
            };

            Button btnEdit = new Button
            {
                Text = "Edit",
                FontSize = 12,
                HeightRequest = 40,
            };
            btnEdit.Clicked += OnEditClicked;
            btnEdit.CommandParameter = pathEntity;

			//Button btnDelete = new Button() { Text = "Delete" };
            //btnDelete.Clicked += OnDeleteClicked;

			//entryItemPath.Unfocused += ItemFieldUnfocus;
            //entryItemPath.TextChanged += PathChanging;

            //pathCtn.Children.Add(name, 0, 0);
            pathCtn.Children.Add(lblItemPath, 0, 0);
			// add choose top button
			pathCtn.Children.Add(lblTop, 1, 0);
			pathCtn.Children.Add(btnEdit, 2,0);


            pathUIlist.Add(pathCtn);
            return pathCtn;
        }

        async void OnEditClicked(object sender, EventArgs e)
        {
			await Navigation.PushAsync(new AddPathCS((PathEntity)((Button)sender).CommandParameter));
		}

        // add more path click
        async void OnAddMoreClickedAsync(object sender, EventArgs e)
        {
            // goto add path page
            await Navigation.PushAsync(new AddPathCS(null));
        }

    //    void OnDeleteClicked(object sender, EventArgs e){
    //        Button btn = (Xamarin.Forms.Button)sender;

    //        int num = -1;
    //        for (int i = 0; i < pathUIlist.Count(); i++)
    //        {
				//PathItemUI item = pathUIlist[i];
     //           if (item.delete == btn)
     //           {
     //               num = i;
					//if (num != -1)
					//{
     //                   pathUIlist.Remove(item);
					//	mContent.Children.Remove(item.grid);
     //                   break;
					//}
        //        }
        //    }
        //}

   //     async void OnChooseClickedAsync(object sender, EventArgs e)
   //     {
   //         string[] pathArray = new string[10];

			//Button btn = (Button)sender;

			//int num = 0;
			//for (int i = 0; i < pathUIlist.Count(); i++)
			//{
			//	PathItemUI item = pathUIlist[i];
   //             if (item.choose != btn)
			//	{
   //                 pathArray[i] = item.path.Text;
   //                 num++;
			//	}
			//}

			//var retTop = await DisplayActionSheet("Choose on as Top page", "Cancel", null, pathArray);
        //    Debug.WriteLine("top is " + retTop);
        //}


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
            if(App.PathList!=null){
                //if(gridCtn!=null)
                //mContent.Children.Remove(gridCtn);
                // Grid container for paths
                //gridCtn = new Grid
                //{
                //	HorizontalOptions = LayoutOptions.FillAndExpand,
                //                VerticalOptions = LayoutOptions.Fill,
                //	Margin = new Thickness(20, 5)
                //};

                //gridCtn.Children.Clear();

                // clear UIs
                foreach (var item in pathUIlist)
                {
                    if(mContent.Children.Contains(item)){
                        mContent.Children.Remove(item);
                    }
                }

				foreach (var item in App.PathList)
				{
					//if()
					// add path
					mContent.Children.Insert(mContent.Children.Count() - 2, MakeNewPath(item));

					//mContent.Children.Add(MakeNewPath(item));
				}
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
            if (signUpSucceeded )
            {
                //var rootPage = Navigation.NavigationStack.FirstOrDefault();
                _database.UpdateUser(App.UserEntity);
                // delete all the path exist
     //           foreach (var pt in App.PathList)
     //           {
     //               _database.DeletePath(pt);
     //           }
     //           // add all the paths 
     //           for (int i = 0; i < pathUIlist.Count; i++)
     //           {
     //               PathEntity path = new PathEntity();
     //               PathItemUI item = pathUIlist.ElementAt(i);

     //               path.Number = i;
					//path.Path = item.path.Text;

                //    if(item.lblTop.Text!=null && !item.lblTop.Text.Equals(PathItemUI.NO_PARENT)){
                //        path.Parent = item.lblTop.Text;
                //    }else{
                //        path.Parent = PathItemUI.NO_PARENT;
                //    }
                //    path.UserID = App.UserEntity.ID;

                //    _database.AddPath(path);
                //}
				App.PathList = _database.GetPaths(App.UserEntity.ID);

				Application.Current.MainPage = new MainPageCS();
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
        //bool IsPathsValid()
        //{
        //    foreach (var item in pathUIlist)
        //    {
        //        if (string.IsNullOrEmpty(item.path.Text))
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}

        // control the pin as 4 digit
        void Pin_Control(object sender, TextChangedEventArgs e)
        {
            Entry thisBlock = ((Entry)sender);
            String pinStr = e.NewTextValue;

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

  


    }
	public class PathItemUI
	{
		public const string NO_PARENT = "No Parent";
		public Grid grid;
		//public Entry name;
        public Label lblPath;
		//public Picker picker;
		//public Button choose;
		//public int number;
		//public Button delete;
        public Label lblTop;
	}
}

