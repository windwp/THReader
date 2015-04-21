using SQLite;
using TungHoanhReader.Wrapper;

namespace TungHoanhReader.Models
{
    class TruyenSqlFavoriteData
    {

        [PrimaryKey,AutoIncrement]
        public int Id { get; set; }
        public Wrapper.SiteTruyen Site { get; set; }
        public string TruyenUrl { get; set; }
        public string Author { get; set; }
        public string Category { get; set; }
        public int MaxPageTruyen { get; set; }
        public string Title { get; set; }



        public static TruyenSqlFavoriteData Convert(Truyen dataTruyen)
        {
            var item = new TruyenSqlFavoriteData();
            item.Site = dataTruyen.Site;
            item.TruyenUrl = dataTruyen.TruyenUrl;
            item.Author = dataTruyen.Author;
            item.Category = dataTruyen.Category;
            item.MaxPageTruyen = dataTruyen.MaxPageTruyen;
            item.Title = dataTruyen.Title;
            return item;
        }

        public  static Truyen Convert(TruyenSqlFavoriteData dataTruyensql)
        {
            var item = new Truyen(dataTruyensql.Site);
            item.TruyenUrl = dataTruyensql.TruyenUrl;
            item.Author = dataTruyensql.Author;
            item.Category = dataTruyensql.Category;
            item.MaxPageTruyen = dataTruyensql.MaxPageTruyen;
            item.Title = dataTruyensql.Title;
            return item;
        }
    }

    class TruyenSqlReadData
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public int LastChapterRead { get; set; }
        public int LastPageRead { get; set; }
        public Wrapper.SiteTruyen Site { get; set; }
        public string TruyenUrl { get; set; }

    }


}