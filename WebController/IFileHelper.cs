using SQLite.Net;
namespace WebController
{
    public interface IFileHelper
    {
		//string GetLocalFilePath(string filename);
        SQLiteConnection GetConnection();
	}
}
