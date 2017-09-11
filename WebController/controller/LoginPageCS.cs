using System;
using Xamarin.Forms;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace WebController
{
	public class LoginPageCS : ContentPage
	{

        //IDictionary<Entry, Entry> pinNext = new Dictionary<Entry, Entry>();
        List<Entry> mlist = new List<Entry>();

		private YangDb _database;
		bool alertLock;
		//bool isSuccess;


		public Entry pin1 = new Entry
        {
            HorizontalTextAlignment = TextAlignment.Center,
            Keyboard = Keyboard.Numeric,
            //TextColor = Color.Transparent
			//IsPassword = true
		};
		Entry pin2 = new Entry
		{
			HorizontalTextAlignment = TextAlignment.Center,
			Keyboard = Keyboard.Numeric,
			//IsPassword = true
		};

		Entry pin3 = new Entry
		{
            
			HorizontalTextAlignment = TextAlignment.Center,
			Keyboard = Keyboard.Numeric,
			//IsPassword = true
		};
		Entry pin4 = new Entry
		{
			HorizontalTextAlignment = TextAlignment.Center,
			Keyboard = Keyboard.Numeric,
			//IsPassword = true
		};
        Label warning = new Label();
		public LoginPageCS()
		{

			//InitializeComponent();
			_database = new YangDb();
			
            Title = "Login";			
            Grid grid = new Grid
			{
				VerticalOptions = LayoutOptions.Center,
				WidthRequest = 200,
				HorizontalOptions = LayoutOptions.Center,
			};
			
			grid.RowDefinitions.Add(new RowDefinition { Height = 50 });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });
			grid.ColumnDefinitions.Add(new ColumnDefinition { Width = 50 });


            pin1.TextChanged += Entry_TextChanged;
            pin2.TextChanged += Entry_TextChanged;
            pin3.TextChanged += Entry_TextChanged;
            pin4.TextChanged += Last_LastEntry;

		
			mlist.Add(pin1);
			mlist.Add(pin2);
			mlist.Add(pin3);
			mlist.Add(pin4);


			grid.Children.Add(pin1, 0, 0);
			grid.Children.Add(pin2, 1, 0);
			grid.Children.Add(pin3, 2, 0);
			grid.Children.Add(pin4, 3, 0);

			// login button
			var LoginButton = new Button
			{
				Text = "Login"
			};
			var RegisterButton = new Button
			{
				Text = "Register"
			};
			LoginButton.Clicked += Login_CheckAsync;
			RegisterButton.Clicked += Register_ClickAsync;

			RelativeLayout mainStackLayOut = new RelativeLayout
			{
				BackgroundColor = Color.Yellow,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				//Orientation = StackOrientation.Vertical
			};

			Func<View, View, double> getSwitchWidth = (parent, self) => self.Measure(parent.Width, parent.Height).Request.Width;
			Func<View, View, double> getSwitchHeight = (parent, self) => self.Measure(parent.Width, parent.Height).Request.Height;
			mainStackLayOut.Children.Add(grid,
				Constraint.RelativeToParent((parent) => parent.Width / 2 - getSwitchWidth(parent, grid) / 2),
				Constraint.RelativeToParent((parent) => parent.Height / 2 - getSwitchHeight(parent, grid) * 2)
			);

            warning.Text = "";
            warning.TextColor = Color.Red;
            mainStackLayOut.Children.Add(warning,
                                         Constraint.RelativeToParent((parent) => parent.Width / 2 - getSwitchWidth(parent, warning) / 2),
				Constraint.RelativeToParent((parent) => parent.Height / 2 - getSwitchHeight(parent, grid) -  100)
			);

			mainStackLayOut.Children.Add(LoginButton,
										 Constraint.RelativeToParent((parent) => parent.Width / 2 - getSwitchWidth(parent, LoginButton) / 2),
										 Constraint.RelativeToParent(parent => parent.Height / 2));
			mainStackLayOut.Children.Add(RegisterButton,
										 Constraint.RelativeToParent((parent) => parent.Width / 2 - getSwitchWidth(parent, RegisterButton) / 2),
				Constraint.RelativeToParent((parent) => parent.Height - 100)
			);

			this.Content = mainStackLayOut;
		}

        protected override void OnAppearing()
        {
            base.OnAppearing();
            Entry_Disanable(pin1);
        }

        void Entry_Disanable(Entry en){
			pin1.IsEnabled = false;
			pin2.IsEnabled = false;
			pin3.IsEnabled = false;
			pin4.IsEnabled = false;
            en.IsEnabled = true;
            en.Focus();
		}

		void Entry_TextChanged(object sender, TextChangedEventArgs e)
        {
            //isSuccess = false;
            alertLock = false;
            Debug.WriteLine(e.OldTextValue+","+e.NewTextValue);
            if (e.NewTextValue.Equals(e.OldTextValue) || (string.IsNullOrWhiteSpace(e.NewTextValue) && (string.IsNullOrWhiteSpace(e.OldTextValue))))
                return;
			Entry thisBlock = ((Entry)sender);
            // delete event
    //        if(string.IsNullOrWhiteSpace(e.NewTextValue) && !string.IsNullOrEmpty(e.OldTextValue)){
				//int index = mlist.IndexOf(thisBlock) - 1;
				//if (index > 3)
				//{
				//	index = 0;
				//}
				//Entry next = mlist.ElementAt(index);
				//Entry_Disanable(next);
            //}
			//Entry next = new Entry();
			//pinNext.TryGetValue(thisBlock, out next);
            // focus next
			//if (next != null)
				//next.Focus();

			if (string.IsNullOrEmpty(thisBlock.Text) || !Utils.IsNumeric(thisBlock.Text))
			{
				thisBlock.Text = "";
			}
			else
			{
                // input more than 1 charactor
				if (e.NewTextValue.Length > 1)
				{
					// when cursor is start from begining
					if (e.NewTextValue.IndexOf(e.OldTextValue, StringComparison.Ordinal) == 0)
					{
						thisBlock.Text = e.NewTextValue.Substring(1, 1);
					}
					else
					{
                        // when cursor is start from end
						thisBlock.Text = thisBlock.Text.Remove(1);
					}
                } else{
                    // next field will focus
                    int index = mlist.IndexOf(thisBlock) + 1;
                    if(index > 3){
                        index = 0;
					}
					Entry next = mlist.ElementAt(index);
                    Entry_Disanable(next);
					
				}
			}
		}

		// last block entry event
		void Last_LastEntry(object sender, TextChangedEventArgs e)
		{
            if(!alertLock){
				Entry_Disanable(pin1);
				//Entry_TextChanged(sender, e);
				Login_CheckAsync(sender, e);
            }
		
		}

		// click LOGIN event
		async void Login_CheckAsync(object sender, EventArgs e)
		{

			if (!(string.IsNullOrEmpty(pin1.Text) || string.IsNullOrEmpty(pin2.Text) || string.IsNullOrEmpty(pin3.Text) || string.IsNullOrEmpty(pin4.Text)))
			{
				if (Utils.IsNumeric(pin1.Text) && Utils.IsNumeric(pin2.Text) && Utils.IsNumeric(pin3.Text) && Utils.IsNumeric(pin4.Text))
				{
					String pin = pin1.Text + pin2.Text + pin3.Text + pin4.Text;
					var users = _database.GetUsers();
					// todo how go check usr
					foreach (var user in users)
					{
						if (user.Pin.Equals(pin))
						{
							App.IsUserLoggedIn = true;
							//isSuccess = true;
                            // set username to application
                            App.UserEntity = user;
							// grab the paths
							if (!App.UserEntity.Url.EndsWith("/", StringComparison.Ordinal))
							{
								App.UserEntity.Url += "/";
							}
							App.PathList = _database.GetPaths(user.ID);

                            // test log
							foreach (var item in App.PathList)
							{
                                string ret = "User ID:" + item.UserID + ",Path: " + item.Path + ", Parent: " + item.Parent;
								Debug.WriteLine(ret);
							}

							//App.Current.MainPage = new NavigationPage(new MainPageCS());
							Application.Current.MainPage = new MainPageCS();
                            break;
						}
						else
						{
                            //warning.Text = "Pin not match";
							Debug.WriteLine("Pin not match");
						}
					}
				}
				else
				{
					Debug.WriteLine("Pin only support number");
				}
				Debug.WriteLine("contain empty");
			}

			if (!alertLock)
			{
				alertLock = true;
                pin1.TextChanged -= Entry_TextChanged;
				pin2.TextChanged -= Entry_TextChanged;
				pin3.TextChanged -= Entry_TextChanged;
                pin4.TextChanged -= Last_LastEntry;
				pin1.Text = "";
				pin2.Text = "";
				pin3.Text = "";
				pin4.Text = "";
				pin1.TextChanged += Entry_TextChanged;
				pin2.TextChanged += Entry_TextChanged;
				pin3.TextChanged += Entry_TextChanged;
				pin4.TextChanged += Last_LastEntry;

				Debug.WriteLine("fail to login");
                //Entry_Disanable(pin1);
				await DisplayAlert("Failed", "Please try again", "Ok");
			}
			pin1.Focus();
		}



		// click REGISTER
		async void Register_ClickAsync(object sender, EventArgs e)
		{
			await Navigation.PushAsync(new RegisterPageCS(this));
		}
	}
}
