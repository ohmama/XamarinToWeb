using System;
using System.IO;
using SQLite;
using Xamarin.Forms;
using WebController.Droid;

[assembly: Dependency(typeof(SQLite_Android))]
namespace WebController.Droid
{
    public class SQLite_Android :IFileHelper
    {
        public SQLite_Android()
        {
        }
		public SQLite.Net.SQLiteConnection GetConnection()
		{
			var fileName = "Yangdb.db3";
			//var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			//var libraryPath = Path.Combine(documentsPath, "..", "Library");
			//var path = Path.Combine(libraryPath, fileName);

			         var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			//var connection = new SQLite.Net.SQLiteConnection(platform, path);

			//return connection;

			var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			path = Path.Combine(path, fileName);
			var conn = new SQLite.Net.SQLiteConnection(platform,path);
            return conn;

		}
    }
}
