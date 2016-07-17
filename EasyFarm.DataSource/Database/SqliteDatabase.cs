using System.Data.SQLite;

namespace EasyFarm.DataSource.Database
{
    public class SqliteDatabase : IDatabase
    {
        private readonly string _fileName;

        public SqliteDatabase(string fileName)
        {
            _fileName = fileName;
        }

        public void Create()
        {
            SQLiteConnection.CreateFile(_fileName);
        }
    }
}