using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class CommentDao
    {
        OnlineShopDbContext db = null;
        private int status = 0;
        public CommentDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(FeedBack feedback)
        {
            db.FeedBacks.Add(feedback);
            db.SaveChanges();
            return feedback.ID;
        }
        public List<FeedBack> ListFeedBack()
        {
            return db.FeedBacks.OrderByDescending(x => x.ID).ToList();
        }
        public IEnumerable<FeedBack> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<FeedBack> model = db.FeedBacks;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Customer.Contains(searchString));
            }
            return model.OrderBy(x => x.ID).ToPagedList(page, pageSize);
        }
        public bool Delete(int id)
        {
            try
            {
                var feedback = db.FeedBacks.Find(id);
                db.FeedBacks.Remove(feedback);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
    }
}
