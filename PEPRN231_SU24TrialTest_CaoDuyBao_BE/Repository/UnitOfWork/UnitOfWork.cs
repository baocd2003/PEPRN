using Repository.Implementation;
using Repository.Interface;
using Repository.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IAccountRepository _accountRepository {  get; set; }
        public IPaintingRepository _paintingRepository { get; set; } 
        public IStyleRepo _styleRepository { get; set; }

        private WatercolorsPainting2024DBContext _context;
        public UnitOfWork(WatercolorsPainting2024DBContext context)
        {
            _context = context;
        }
        public IAccountRepository AccountRepository {
            get {
                return _accountRepository = _accountRepository ?? new AccountRepo(_context);
            }
        }

        public IPaintingRepository PaintingRepository
        {
            get
            {
                return _paintingRepository = _paintingRepository ?? new PaintingRepo(_context);
            }
        }

        public IStyleRepo StyleRepository
        {
            get
            {
                return _styleRepository = _styleRepository ?? new StyleRepo(_context);
            }
        }
    }
}
