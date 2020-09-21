using SQLite;
using StudXam.Droid.SqlServices;
using StudXam.Interfaces;
using System.IO;
using Xamarin.Forms;

[assembly: Dependency(typeof(SQLiteDb))]

namespace StudXam.Droid.SqlServices
{
    internal class SQLiteDb : ISQLiteDb
    {
        public SQLiteAsyncConnection GetConnection()
        {
            var documentsPath = System.Environment.GetFolderPath(System.Environment.SpecialFolder.MyDocuments);
            var path = Path.Combine(documentsPath, "Student.db3");
            return new SQLiteAsyncConnection(path);
        }
    }
}