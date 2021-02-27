using Model.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PagedList;
using Common;

namespace Model.Dao
{
    public class CredentialDao
    {
        OnlineShopDbContext db = null;
        public CredentialDao()
        {
            db = new OnlineShopDbContext();
        }
        public long InSert(Credential entity)
        {
            db.Credentials.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(Credential entity)
        {
            try
            {
                var user = db.Credentials.Find(entity.ID);
                user.UserGroupID = entity.UserGroupID;
                user.RoleID = entity.RoleID;
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Credentials.Find(id);
                db.Credentials.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public IEnumerable<Credential> ListMod(string searchString, int page, int pageSize)
        {
            IQueryable<Credential> model = db.Credentials;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.UserGroupID.Contains(searchString));
            }
            return model.OrderByDescending(x=>x.ID).ToPagedList(page, pageSize);
        }
        public List<Role> ListAll()
        {
            return db.Roles.ToList();
        }
    }
}
