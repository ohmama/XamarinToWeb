using System;
using System.ComponentModel;
using System.Windows.Input;
using Xamarin.Forms;
using System.Diagnostics;
using System.Threading.Tasks;

namespace WebController
{
    public class KeypadViewModel:INotifyPropertyChanged
    {
		private YangDb _database;

		string inputString = "";
		string pin1 = "";
		string pin2 = "";
		string pin3 = "";
		string pin4 = "";
		string pin5 = "";
		char[] specialChars = { '*', '#' };

		public event PropertyChangedEventHandler PropertyChanged;

		// Constructor
		public KeypadViewModel()
		{
			_database = new YangDb();

			this.AddCharCommand = new Command<string>((key) =>
			{
				// Add the key to the input string.
				this.InputString += key;
			});

			this.DeleteCharCommand = new Command((nothing) =>
			{
                if (string.IsNullOrEmpty(InputString))
                    return;
                Debug.WriteLine("input is "+InputString);
				// Strip a character from the input string.
				this.InputString = this.InputString.Substring(0,
									this.InputString.Length - 1);
                int len = InputString.Length;
                switch(len){
					case 0:
						Pin1 = "";
						((Command)this.DeleteCharCommand).ChangeCanExecute();
						break;
					case 1:
                        Pin2 = "";
                        break;
					case 2:
						Pin3 = "";
						break;
                    case 3:
						Pin4 = "";
						break;
					case 4:
						Pin5 = "";
						break;
                }


			},
				(nothing) =>
				{
					// Return true if there's something to delete.
					return this.InputString.Length > 0;
				});
		}

		// Public properties
		public string InputString
		{
			protected set
			{
				if (inputString != value)
				{
					inputString = value;
					OnPropertyChanged("InputString");
                    Debug.WriteLine("in input string -> "+ inputString);
                    formatPin(inputString);

					// Perhaps the delete button must be enabled/disabled.
					((Command)this.DeleteCharCommand).ChangeCanExecute();
				}
			}

			get { return inputString; }
		}

        public void formatPin(string inputStr){
            if(!string.IsNullOrEmpty(inputStr)){
                int len = inputStr.Length;
                if(len <= 5){
					switch (len)
					{
						case 1:
							Pin1 = "●";
							break;
						case 2:
							Pin2 = "●";
							break;
						case 3:
							Pin3 = "●";
							break;
						case 4:
							Pin4 = "●";
							break;
						case 5:
							Pin5 = "●";
                            Login_CheckAsync(inputStr);
							break;
					}
                }
			}
        }

		public string Pin1
		{
			protected set
			{
				if (pin1 != value)
				{
					pin1 = value;
					OnPropertyChanged("Pin1");
				}
			}
			get { return pin1; }
		}

		public string Pin2
		{
			protected set
			{
				if (pin2 != value)
				{
					pin2 = value;
					OnPropertyChanged("Pin2");
				}
			}
			get { return pin2; }
		}

		public string Pin3
		{
			protected set
			{
				if (pin3 != value)
				{
					pin3 = value;
					OnPropertyChanged("Pin3");
				}
			}
			get { return pin3; }
		}

		public string Pin4
		{
			protected set
			{
				if (pin4 != value)
				{
					pin4 = value;
					OnPropertyChanged("Pin4");
				}
			}
			get { return pin4; }
		}

		public string Pin5
		{
			protected set
			{
				if (pin5 != value)
				{
					pin5 = value;
					OnPropertyChanged("Pin5");
				}
			}
			get { return pin5; }
		}

		// ICommand implementations
		public ICommand AddCharCommand { protected set; get; }

		public ICommand DeleteCharCommand { protected set; get; }

		protected void OnPropertyChanged(string propertyName)
		{
			if (PropertyChanged != null)
				PropertyChanged(this,
					new PropertyChangedEventArgs(propertyName));
		}

		// click LOGIN event
		async void Login_CheckAsync(string pin)
		{
			var users = _database.GetUsers();
			// todo how go check usr
			foreach (var user in users)
			{
				if (user.Pin.Equals(pin))
				{
					App.IsUserLoggedIn = true;
					// set username to application
					App.UserEntity = user;
					// grab the paths
					if (!App.UserEntity.Url.EndsWith("/", StringComparison.Ordinal))
					{
						App.UserEntity.Url += "/";
					}
					App.PathList = _database.GetPaths(user.ID);

					await Task.Run(async () =>
					{
                        
						await Task.Delay(0);
						Device.BeginInvokeOnMainThread(() =>
						{
							Application.Current.MainPage = new MainPageCS();
						});
					});
					return;
				}
				else
				{
   					Debug.WriteLine("Pin not match");
				}

			}
			if (!App.IsUserLoggedIn)
			{
				await Task.Run(async () =>
				{
					await Task.Delay(0);
					Device.BeginInvokeOnMainThread(() =>
					{
						Application.Current.MainPage.DisplayAlert("Failed", "Pin not match", "Ok");
					});
				});
			}
            inputString = Pin1 = Pin2 = Pin3 = Pin4 = Pin5 = "";
		}

		void Login(object sender, EventArgs e)
		{
			Application.Current.MainPage = new MainPageCS();
		}
    }
}
