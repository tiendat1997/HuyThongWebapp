using htcustomer.entity;
using htcustomer.repository;
using htcustomer.service.Interfaces;
using htcustomer.service.ViewModel.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace htcustomer.service.Implements
{
    public class CategoryService : ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly IRepository<Category> categoryRepo;

        public CategoryService(IUnitOfWork unitOfWork, IRepository<Category> _categoryRepo)
        {
            this.unitOfWork = unitOfWork;
            this.categoryRepo = _categoryRepo;
        }
        public void Add(CategoryViewModel category)
        {
            if (category == null) throw new ArgumentNullException("Null Agurment");
            categoryRepo.Insert(new Category { Name = category.Name, Disable = false });
            unitOfWork.SaveChanges();
        }
        public void Delete(int categoryId)
        {
            var category = categoryRepo.GetByID(categoryId);
            if (category == null) throw new ArgumentNullException("Null Agurment");
            category.Disable = true;
            categoryRepo.Edit(category);
            unitOfWork.SaveChanges();
        }
        public void Edit(CategoryViewModel category)
        {
            if (category == null) throw new ArgumentNullException("Null Agurment");
            categoryRepo.Edit(new Category { CategoryID = category.CategoryID, Name = category.Name });
            unitOfWork.SaveChanges();
        }
        public IEnumerable<CategoryViewModel> GetAllCategories()
        {
            return categoryRepo.Gets()
                .Where(c => c.Disable != true)
                .Select(c => new CategoryViewModel
                {
                    CategoryID = c.CategoryID,
                    Name = c.Name
                });
        }
    }
}
