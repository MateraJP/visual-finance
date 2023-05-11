using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualFinanceiro.Negocios.Entities
{
    public  class SeachRequest
    {
        public int PageSize { get; set; } = 20;
        public int PageIndex { get; set; } = 1;
    }
}
