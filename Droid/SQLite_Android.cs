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
			//var fileName = "Yangdb.db3";
   //         var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			//var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			//path = Path.Combine(path, fileName);
			//return new SQLite.Net.SQLiteConnection(platform,Path.Combine(path, fileName));




            var fileName = "Yangdb.db3";
            var platform = new SQLite.Net.Platform.XamarinAndroid.SQLitePlatformAndroid();
			var path = System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal);
			path = Path.Combine(path, fileName);
            return new SQLite.Net.SQLiteConnection(platform,path);
            //return new SQLite.Net.SQLiteConnection(platform, Path.Combine(path, fileName));
		}
    }
}
