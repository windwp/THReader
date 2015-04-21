using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.UI;
using Windows.UI.Xaml.Media;
using SQLite;
using TungHoanhReader.Common;
using TungHoanhReader.Models;
using TungHoanhReader.Wrapper;

namespace TungHoanhReader.Services
{
    class SettingServices : ISettingService
    {
        public UserSetting USetting { get; set; }
        private SQLiteAsyncConnection _connection;

        public UserSetting UpdateVersion(string version, UserSetting setting)
        {
            return setting;
        }

        public async Task<bool> DoesDbExist(string DatabaseName)
        {
            bool dbexist = true;
            try
            {
                StorageFile storageFile = await ApplicationData.Current.LocalFolder.GetFileAsync(DatabaseName);

            }
            catch
            {
                dbexist = false;
            }

            return dbexist;
        }
        public async void Init()
        {
            if (await DoesDbExist("Reader.db"))
            {
                USetting=new UserSetting();
                string color;
                var data=StorageHelper.GetSetting(SettingKeyString.FontSize,"");
                double num = 16.0f;
                if (double.TryParse(data, out num))
                USetting.FontSize = num;
                color=StorageHelper.GetSetting(SettingKeyString.FontColor, "");
                USetting.FontColor = SettingKeyString.StringToColor(color);
                // doc. data

            }
            else
            {
                USetting = new UserSetting();
                USetting.FontColor = Colors.Black;
                USetting.FontSize = 18;
                await Connection.CreateTableAsync<TruyenSqlFavoriteData>();
                await Connection.CreateTableAsync<TruyenSqlReadData>();
                StorageHelper.SetSetting(SettingKeyString.FontColor,SettingKeyString.ColorToString(USetting.FontColor));
                StorageHelper.SetSetting(SettingKeyString.FontSize, USetting.FontSize);
            }

        }

        public async Task<List<TruyenSqlFavoriteData>> ListFavoriteSong()
        {
            var result = await Connection.Table<TruyenSqlFavoriteData>().ToListAsync();
            return result;
        }

        public void SaveStorage()
        {

            StorageHelper.SetSetting(SettingKeyString.FontColor, SettingKeyString.ColorToString(USetting.FontColor));
            var value= USetting.FontSize.ToString();
            StorageHelper.SetSetting(SettingKeyString.FontSize,value);
        }


        public async void AddStoryFavorite(Truyen story)
        {
            // kiem tra xem thu trong favorite database co' no' k0
            var value = await Connection.Table<TruyenSqlFavoriteData>().Where(o => o.Site == story.Site && o.TruyenUrl == story.TruyenUrl).FirstOrDefaultAsync();
            if (value == null)
            {
                var truyenfavorite = TruyenSqlFavoriteData.Convert(story);
                await Connection.InsertAsync(truyenfavorite);
            }
        }

        public async Task<bool> IsStoryFavorite(Truyen truyen)
        {
            var value = await Connection.Table<TruyenSqlFavoriteData>().Where(o => o.Site == truyen.Site && o.TruyenUrl == truyen.TruyenUrl).FirstOrDefaultAsync();
            if (value == null)
            {
                return false;
            }
            return true;
        }
        public async void RemoveStoryFavorite(Truyen story)
        {
            var value = await Connection.Table<TruyenSqlFavoriteData>().Where(o => o.Site == story.Site && o.TruyenUrl == story.TruyenUrl).FirstOrDefaultAsync();
            if (value != null)
            {
                await Connection.DeleteAsync(value);
            }
        }

        public async Task<TruyenSqlReadData> GetTruyenStorageData(Wrapper.SiteTruyen site, string truyenUrl)
        {
            
            return   await
                   _connection.Table<TruyenSqlReadData>()
                       .Where(o => o.Site == site && o.TruyenUrl == truyenUrl)
                       .FirstOrDefaultAsync();
        }

        public async Task<bool> IsHaveFavorite()
        {
            var value = await Connection.Table<TruyenSqlFavoriteData>().FirstOrDefaultAsync();
            if (value == null)
            {
                return false;
            }
            return true;
        }

        public async void SetTruyenLastReadChapter(Wrapper.SiteTruyen site, string url, int lastReadChapter, int lastPageRead)
        {
            var value =
                await
                    _connection.Table<TruyenSqlReadData>()
                        .Where(o => o.Site == site && o.TruyenUrl == url)
                        .FirstOrDefaultAsync();
            if (value != null)
            {
                value.LastChapterRead = lastReadChapter;
                value.LastPageRead = lastPageRead;
                await _connection.UpdateAsync(value);
            }
            else
            {
                await _connection.InsertAsync(new TruyenSqlReadData()
                {
                    LastChapterRead = lastReadChapter,
                    Site = site,
                    TruyenUrl = url,
                    LastPageRead = lastPageRead

                });
            }
        }

        private const string DBName = "Reader.db";
        private SQLiteAsyncConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SQLiteAsyncConnection(DBName);
                    return _connection;
                }
                return _connection;
            }
        }
    }
}
