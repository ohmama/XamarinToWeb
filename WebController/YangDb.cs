using System;
using SQLite.Net;
using Xamarin.Forms;
using System.Collections.Generic;
using System.Linq;

namespace WebController
{
    public class YangDb
    {
        private SQLiteConnection _connection;
        public YangDb()
        {
			_connection = DependencyService.Get<IFileHelper>().GetConnection();
			_connection.CreateTable<UserEntity>();
            _connection.CreateTable<PathEntity>();
        }
		public List<UserEntity> GetUsers()
		{
			return (from t in _connection.Table<UserEntity>()
					select t).ToList();
		}

		public UserEntity GetUser(int id)
		{
			return _connection.Table<UserEntity>().FirstOrDefault(t => t.ID == id);
		}
		public UserEntity GetUser(String name)
		{
            return _connection.Table<UserEntity>().FirstOrDefault(t => t.Name == name);
		}

		public void DeleteUser(int id)
		{
			_connection.Delete<UserEntity>(id);
		}

        public void AddUser(UserEntity user)
		{
            if (user != null)
                user.CreatedOn = DateTime.Now;
			_connection.Insert(user);
		}
		public void UpdateUser(UserEntity user)
		{
            _connection.Update(user);
		}

        // path
        public List<PathEntity> GetPaths(int userId)
		{
			var query = _connection.Query<PathEntity>("SELECT * FROM PathEntity WHERE UserId = ? ORDER BY Number ASC", userId);
			return query.ToList();
			//return (from t in _connection.Table<PathEntity>()
					//select t).ToList();
		}
		public void AddPath(PathEntity path)
		{
			if (path != null)
				path.CreatedOn = DateTime.Now;
            _connection.Insert(path);
		}
        public void DeletePath(PathEntity path){
            _connection.Delete(path);
        }
		
    }
}
