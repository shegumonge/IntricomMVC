using JW;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace IntricomMVC.Models
{
    public class PaginationDoc<T>
    {

        public IEnumerable<string> Items { get; set; }
        public IEnumerable<T> Documents { get; set; }
        //public DocumentFilter Filter { get; set; }
        public Pager Pager { get; set; }
        public SelectList TotalItemsList { get; set; }
        public int TotalItems { get; set; }
        public SelectList PageSizeList { get; set; }
        public int PageSize { get; set; }
        public SelectList MaxPagesList { get; set; }
        public int MaxPages { get; set; }

    }
}