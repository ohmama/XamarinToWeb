using System;
using System.IO;
using SQLite;
using WebController.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLite_iOS))]
namespace WebController.iOS
{
    public class SQLite_iOS:IFileHelper
    {
        public SQLite_iOS()
        {
        }
		public SQLite.Net.SQLiteConnection GetConnection()
		{
			var fileName = "Yangdb.db3";
			var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
			var libraryPath = Path.Combine(documentsPath, "..", "Library");
			var path = Path.Combine(libraryPath, fileName);

			var platform = new SQLite.Net.Platform.XamarinIOS.SQLitePlatformIOS();
			var connection = new SQLite.Net.SQLiteConnection(platform, path);

			return connection;
		}
    }
}
