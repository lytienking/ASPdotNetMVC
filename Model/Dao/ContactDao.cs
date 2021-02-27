using Model.EF;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model.Dao
{
    public class ContactDao
    {
        OnlineShopDbContext db = null;
        private int status = 0;
        public ContactDao()
        {
            db = new OnlineShopDbContext();
        }
        public long Insert(Contact contact)
        {
            db.Contacts.Add(contact);
            db.SaveChanges();
            return contact.ID;
        }
        public bool Delete(int id)
        {
            try
            {
                var ct = db.Contacts.Find(id);
                db.Contacts.Remove(ct);
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }

        }
        public IEnumerable<Contact> ListAllPaging(string searchString, int page, int pageSize)
        {
            IQueryable<Contact> model = db.Contacts;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.CustomerID == int.Parse( searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
    }
}
