using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using TungHoanhReader.Models;
using TungHoanhReader.Wrapper;

namespace TungHoanhReader.Services
{
     internal interface ISettingService
    {
         UserSetting UpdateVersion(string version, UserSetting setting);
         void Init();
         Task<List<TruyenSqlFavoriteData>> ListFavoriteSong();
         void SaveStorage();
         void AddStoryFavorite(Truyen story);
         Task<bool> IsStoryFavorite(Truyen truyen);
         void RemoveStoryFavorite(Truyen story);
         Task<TruyenSqlReadData> GetTruyenStorageData(Wrapper.SiteTruyen site, string truyenUrl);
         Task<bool> IsHaveFavorite();
         UserSetting USetting { get; set; }
         void SetTruyenLastReadChapter(Wrapper.SiteTruyen site, string url, int lastReadChapter, int lastPageRead);
    }
}
