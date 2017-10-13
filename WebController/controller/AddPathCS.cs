using System;
using Xamarin.Forms;

namespace WebController
{
    public class AddPathCS : ContentPage
    {
		public YangDb _database;
		public const string NO_PARENT = "No Parent";
		Entry PathEntry;
        Picker TopLevelSelector;
        Label messageLabel;
        PathEntity pathEntity;

        public AddPathCS(PathEntity pathEntity)
        {
            this.pathEntity = pathEntity;
			_database = new YangDb();
			App.PathList = _database.GetPaths(App.UserEntity.ID);

			Title = "Custom Paths";
            PathEntry = new Entry
            {
                Margin = new Thickness(20, 5),
                Keyboard = Keyboard.Create(KeyboardFlags.None)
            };
            TopLevelSelector = new Picker
            {
                Margin = new Thickness(20, 5),
                Title = "Top Level",
            };
            // fill parents picker
            // firstly add a No Parent option
            TopLevelSelector.Items.Add(NO_PARENT);

			foreach (var item in App.PathList)
			{
                TopLevelSelector.Items.Add(item.Path);
				// itself can not be its parent
				if (pathEntity != null && pathEntity.Path.Equals(item.Path))
				{
                    TopLevelSelector.Items.Remove(item.Path);
				}
			}

            if(pathEntity!=null && pathEntity.Parent!=null)
                TopLevelSelector.SelectedItem = pathEntity.Parent;
            

            // Grid container for buttons
            Grid btnCtn = new Grid
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Margin = new Thickness(20, 5),
            };
            // warnging label
            messageLabel = new Label() { TextColor = Color.Red, Margin = new Thickness(20, 5) };

            Button btnSave = new Button() { Text = "Save" };
            btnSave.Clicked += OnSaveAsync;
            btnCtn.Children.Add(btnSave, 0, 0);

            // only when editing the path, there is delete option
            if(pathEntity!=null){
                PathEntry.Text = pathEntity.Path;

				Button btnDelete = new Button() { Text = "Delete" };
				btnDelete.Clicked += OnDelete;
				btnCtn.Children.Add(btnDelete, 0, 1);
			}

            Content = new StackLayout()
            {
                VerticalOptions = LayoutOptions.StartAndExpand,
                Children = {
                    new Label { Text = "Path", Margin = new Thickness(20, 15, 0, 5) },
                    PathEntry,
                    new Label { Text = "Choose a Top Level", Margin = new Thickness(20,5) },
                    TopLevelSelector,
                    btnCtn,
                }
            };
        }

        async void OnSaveAsync(object sender, EventArgs e)
        {
            string pathStr = PathEntry.Text;

            // edit exist one
            if (pathEntity != null)
            {
                if (string.IsNullOrEmpty(pathStr))
                    //empty warning
                    messageLabel.Text = "Please fill the name with path";
                else
                {
                    if(TopLevelSelector.SelectedItem!=null)
                        pathEntity.Parent = TopLevelSelector.SelectedItem.ToString();
                    pathEntity.Path = PathEntry.Text;
                    _database.UpdatePath(pathEntity);
                    App.PathList = _database.GetPaths(App.UserEntity.ID);
					await Navigation.PopAsync();
				}
            }
            // save new one
            else
            {
                PathEntity pe = new PathEntity();
                pe.UserID = App.UserEntity.ID;
                if (TopLevelSelector.SelectedItem != null)
                {
                    pe.Parent = TopLevelSelector.SelectedItem.ToString();

                }
                pe.Path = PathEntry.Text;
                _database.AddPath(pe);
                App.PathList = _database.GetPaths(App.UserEntity.ID);
				await Navigation.PopAsync();
			}
		}

        async void OnDelete(object sender, EventArgs e)
        {
			var answer = await DisplayAlert("Delete", "Do you want to delete?", "Yes", "No");

			if(answer == true && pathEntity!=null){
                _database.DeletePath(pathEntity);
            }
			App.PathList = _database.GetPaths(App.UserEntity.ID);
			await Navigation.PopAsync();
		}

    }
}

