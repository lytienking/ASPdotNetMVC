using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ParentIDCategoryDao
    {
        OnlineShopDbContext db = null;
        public ParentIDCategoryDao()
        {
            db = new OnlineShopDbContext();
        }
        public List<ParentIDCategory> ListAll()
        {
            return db.ParentIDCategories.OrderBy(x => x.ID).ToList();
        }
        public ParentIDCategory ViewDetail(long id)
        {
            return db.ParentIDCategories.Find(id);
        }
        public IEnumerable<ParentIDCategory> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<ParentIDCategory> model = db.ParentIDCategories;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Name.Contains(searchString));
            }
            return model.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }
    }
}
