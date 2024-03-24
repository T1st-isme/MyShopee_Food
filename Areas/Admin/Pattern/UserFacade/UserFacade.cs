using Shopee_Food.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Shopee_Food.Areas.Admin.Pattern.UserFacade
{
    public class UserFacade
    {
        private DBShopeeFoodEntities db = new DBShopeeFoodEntities();
        private UserSubControllerAdd _UserSubAdd;
        private UserSubControllerDelete _UserSubDelete;
        private UserSubControllerUpdate _UserSubUpdate;
        private UserSubControllerGetById _UserSubGetById;
        private UserSubControllerGetList _UserSubControllerGetList;

        public UserFacade(DBShopeeFoodEntities db)
        {
            _UserSubAdd = new UserSubControllerAdd(db);
            _UserSubGetById = new UserSubControllerGetById(db);
            _UserSubDelete = new UserSubControllerDelete(db);
            _UserSubUpdate = new UserSubControllerUpdate(db);
            _UserSubControllerGetList = new UserSubControllerGetList(db);
        }

        public List<User> GetAllUser()
        {
            return _UserSubControllerGetList.GetAllUser();
        }

        public void AddUser(User user)
        {
            _UserSubAdd.AddUser(user);
        }

        public void DeleteUser(int id)
        {
            _UserSubDelete.DeleteUser(id);
        }

        public void UpdateUser(User user)
        {
            _UserSubUpdate.UpdateUser(user);
        }

        public void GetUserById(int id)
        {
            _UserSubGetById.GetUserById(id);
        }
    }
}