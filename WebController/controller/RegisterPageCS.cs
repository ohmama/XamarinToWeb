using System;
using System.Linq;
using Xamarin.Forms;
using System.Diagnostics;
using System.Collections.Generic;

namespace WebController
{
	public class RegisterPageCS : ContentPage
	{
		public YangDb _database;

		Entry usernameEntry, passwordEntry, pinEntry, urlEntry;
		Label messageLabel;

        string Url = "https://cirro.nimbus.co.nz/zd1/";

        public RegisterPageCS(KeyPadCS parent)
		{
			_database = new YangDb();
            messageLabel = new Label{Margin = new Thickness(20, 5),TextColor = Color.Red };
            this.BackgroundColor = Color.FromHex("BBDEFB");
            usernameEntry = new Entry
            {
                Placeholder = "Input username",
                Margin = new Thickness(20, 5),
                Keyboard = Keyboard.Create(KeyboardFlags.None)
			};
			passwordEntry = new Entry
			{
				Placeholder = "Input password",
				IsPassword = true,
                Margin = new Thickness(20, 5)
			};
			pinEntry = new Entry()
			{
				Placeholder = "Input 5 digit pin",
				Keyboard = Keyboard.Numeric,
                Margin = new Thickness(20, 5)
			};
			pinEntry.TextChanged += Entry_TextChanged;
			urlEntry = new Entry()
			{
				Placeholder = "Input your prefered URL",
                Margin = new Thickness(20, 5),
				Keyboard = Keyboard.Create(KeyboardFlags.None),
                Text = Url
			};

			var signUpButton = new Button
			{
                Text = "Sign Up",
                BackgroundColor = Color.White
			};
			signUpButton.Clicked += OnSignUpButtonClicked;

			Title = "Sign Up";

			Content = new StackLayout
			{
                VerticalOptions = LayoutOptions.CenterAndExpand,
				Children = {
                    new Label { Text = "Username", Margin= new Thickness(20,5) },
					usernameEntry,
					new Label { Text = "Password", Margin= new Thickness(20,5) },
					passwordEntry,
					new Label { Text = "5 Digital Pin", Margin= new Thickness(20,5) },
                    pinEntry,
					new Label { Text = "URL", Margin= new Thickness(20,5) },
					urlEntry,
					signUpButton,
					messageLabel
				}
			};
		}

        // Register click event
		async void OnSignUpButtonClicked(object sender, EventArgs e)
		{
			var newUser = new UserEntity()
			{
				Name = usernameEntry.Text,
				Password = passwordEntry.Text,
				Pin = pinEntry.Text,
				Url = urlEntry.Text
			};

			// Sign up logic goes here
            var isInputCorrect = AreDetailsValid(newUser);
			if (isInputCorrect)
			{
                if(!isPinExist(newUser)){
					var rootPage = Navigation.NavigationStack.FirstOrDefault();

					_database.AddUser(newUser);

					if (rootPage != null)
					{
						App.IsUserLoggedIn = true;
                        Navigation.InsertPageBefore(new KeyPadCS(), Navigation.NavigationStack.First());
						await Navigation.PopToRootAsync();
					}
                }else{
                    messageLabel.Text = "This Pin is not safe to login,Please Change.";
                }
				
			}
			else
			{
				messageLabel.Text = "Sign up failed";
			}
		}

        bool isPinExist(UserEntity newUser){
            List<UserEntity> users = _database.GetUsers();
            foreach(UserEntity u in users){
                if (newUser.Pin.Equals(u.Pin))
                    return true;
            }
            return false;
        }

		bool AreDetailsValid(UserEntity user)
		{
			return (!string.IsNullOrWhiteSpace(user.Name) && !string.IsNullOrWhiteSpace(user.Password) 
                    && !string.IsNullOrWhiteSpace(user.Pin) && Utils.IsNumeric(user.Pin) );
		}

		void Entry_TextChanged(object sender, TextChangedEventArgs e)
		{
			Entry thisBlock = ((Entry)sender);
            String pinStr = e.NewTextValue;

			Debug.WriteLine(Utils.IsNumeric(pinStr));
			if (pinStr == null || pinStr.Trim().Length == 0)
			{

			}
			else
			{
				if (!Utils.IsNumeric(pinStr))
				{
					thisBlock.Text = e.OldTextValue;
				}
				else if (pinStr.Length > 5)
				{
					thisBlock.Text = pinStr.Remove(5);
				}
			}
		}


	}
}
