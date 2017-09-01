using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using WebController.Droid;

namespace WebController.Droid
{
    public class SQLite_Android
    {
        public SQLite_Android()
        {
        }
		public SQLite.Net.SQLiteConnection GetConnection()
		{
			var fileName = "Yangdb.db3";
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var libraryPath = Path.Combine(documentsPath, "..", "Library");
			var path = Path.Combine(libraryPath, fileName);

            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var connection = new SQLite.Net.SQLiteConnection(platform, path);

			return connection;
		}
    }
}
