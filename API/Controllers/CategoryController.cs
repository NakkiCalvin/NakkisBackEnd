using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BLL.Managers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/categories")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IDepartmentService _departmentService;

        public CategoryController(
            IProductService productService,
            ICategoryService categoryService,
            IDepartmentService departmentService
            )
        {
            _productService = productService;
            _departmentService = departmentService;
            _categoryService = categoryService;
        }

        [HttpGet("filter")]
        public object GetAllFiltered([FromQuery] string query)
        {
            var title = _productService.GetAll(null, null, null, null).Select(x => x.Title);
            var category = _categoryService.GetAll().Select(x => x.CategoryName);
            var department = _departmentService.GetAll().Select(x => x.DepartmentName);

            if (query == null)
            {
                var filter = new { department, category, title };
                return new { filter };
            }
            else
            {
                title = title.Where(x => x.Contains(query));
                category = category.Where(x => x.Contains(query));
                department = department.Where(x => x.Contains(query));

                var filter = new { department, category, title };
                return new { filter };
            }
        }
    }
}