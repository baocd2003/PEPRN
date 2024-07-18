using Repository.Interface;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.Implementation
{
    public class PaintingRepo : GenericRepo<WatercolorsPainting>, IPaintingRepository
    {
        public PaintingRepo(WatercolorsPainting2024DBContext dbContext) : base(dbContext)
        {
        }
    }
}
