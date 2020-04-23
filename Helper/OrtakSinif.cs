using mvcBlog.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvcBlog.Helper
{
    public class OrtakSinif
    {
        Blog db = new Blog();

        public static bool EditIsimYetkiVarMi(int id,Kullanici user)
        {
            
            if (user.id == id)
            {
                return true;

            }
            else if (user.yetkiId > 2)
            {
                return true;
            }
            return false;

        }

        public static bool DeleteIsimYetkiVarMi(int id, Kullanici user)
        {

            if (user.id == id)
            {
                return true;

            }
            else if (user.yetkiId > 3)
            {
                return true;
            }
            return false;

        }
    }
}