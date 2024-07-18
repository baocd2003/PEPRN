using Repository.Interface;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class StyleRepo : GenericRepo<Style>, IStyleRepo
    {
        public StyleRepo(WatercolorsPainting2024DBContext dbContext) : base(dbContext)
        {
        }
    }
}
