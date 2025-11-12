using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Librium.Domain.Common.Repositories;

public interface IBaseRepository
{
    Task<bool> SaveChanges();
}
