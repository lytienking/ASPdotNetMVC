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
    public class UserDao
    {
        OnlineShopDbContext db = null;
        public UserDao()
        {
            db = new OnlineShopDbContext();
        }
        public long InSert(User entity)
        {
            db.Users.Add(entity);
            db.SaveChanges();
            return entity.ID;
        }
        public bool Update(User entity)
        {
            try
            {
                var user = db.Users.Find(entity.ID);
                if(!string.IsNullOrEmpty(entity.Password))
                {
                    user.Password = entity.Password;
                }
                user.Name = entity.Name;
                user.Phone = entity.Phone;
                user.Address = entity.Address;
                user.Email = entity.Email;
                user.GroupID = entity.GroupID;
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            
        }
        public bool Delete(int id)
        {
            try
            {
                var user = db.Users.Find(id);
                db.Users.Remove(user);
                db.SaveChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            
        }
        public User ViewDetail(int id)
        {
            return db.Users.Find(id);
        }
        public IEnumerable<User> ListAllPaging(string searchString, int page,int pageSize)
        {
            IQueryable<User> model = db.Users;
            if(!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.Username.Contains(searchString) || x.Name.Contains(searchString));
            }
            return model.OrderByDescending(x=>x.ID).ToPagedList(page,pageSize);
        }
        public IEnumerable<Contact> ListAll(string searchString, int page, int pageSize)
        {
            IQueryable<Contact> model = db.Contacts;
            if (!string.IsNullOrEmpty(searchString))
            {
                model = model.Where(x => x.CustomerID==long.Parse(searchString));
            }
            return model.OrderByDescending(x => x.ID).ToPagedList(page, pageSize);
        }
        public User GetUserName(string UserName)
        {
            return db.Users.SingleOrDefault(x => x.Username == UserName);
        }
        public int Login(string username, string password,bool isLoginAdmin=false)
        {
            var result = db.Users.SingleOrDefault(x => x.Username == username);
            if (result == null)
            {
                return 0;
            }
            else
            {
                if(isLoginAdmin==true)
                {
                    if(result.GroupID==CommonConstant.ADMIN_GROUP|| result.GroupID==CommonConstant.MOD_GROUP||result.GroupID=="MOD1")
                    {
                        if(result.Status==false)
                        {
                            return -2;
                        }
                        else
                        {
                            if (result.Password == password)
                            {
                                return 1;
                            }
                            else
                                return -1;
                        }
                    }
                    else
                    {
                        return -3;
                    }
                }
                else
                {
                    if (result.Status == false)
                    {
                        return -2;
                    }
                    else
                    {
                        if (result.Password == password)
                        {
                            return 1;
                        }
                        else
                            return -1;
                    }
                }                
            }        
        } 
        public List<String> GetlistCredential(string username)
        {
            var user = db.Users.Single(x => x.Username == username);
            var data = (from a in db.Credentials
                       join b in db.UserGroups on a.UserGroupID equals b.ID
                       join c in db.Roles on a.RoleID equals c.Name
                       where b.ID == user.GroupID
                       select new 
                       {
                           RoleID = a.RoleID,
                           UserGroupID = a.UserGroupID
                       }).AsEnumerable().Select(x=> new Credential() {
                           RoleID = x.RoleID,
                           UserGroupID = x.UserGroupID
                       });
            return data.Select(x => x.RoleID).ToList();
        }
        public bool ChangeStatus(long id)
        {
            var user = db.Users.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
        public bool ChangeStatusContact(long id)
        {
            var user = db.Contacts.Find(id);
            user.Status = !user.Status;
            db.SaveChanges();
            return user.Status;
        }
        public bool CheckUsername(string username)
        {
            return db.Users.Count(x => x.Username == username) > 0;
        }
        public bool CheckEmail(string email)
        {
            return db.Users.Count(x => x.Email == email) > 0;
        }
    }
}
