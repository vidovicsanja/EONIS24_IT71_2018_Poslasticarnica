using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Poslasticarnica.Core;

namespace Poslasticarnica.Model.dto
{
    public class PaginationDto<TEntity>
    {
        public object metada { get; set; }  
        public List<TEntity> data { get; set; } 

        public PaginationDto(object metada, List<TEntity> data) {
            this.metada = metada;
            this.data = data;
        }
    }
}
