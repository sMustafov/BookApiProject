﻿namespace BookApiProject.Controllers
{
    using BookApiProject.Dtos;
    using BookApiProject.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : Controller
    {
        private ICategoryRepository categoryRepository;
        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        // api/categories
        [HttpGet]
        [ProducesResponseType(400)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetCategories()
        {
            var categories = this.categoryRepository.GetCategories();

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return Ok(categoriesDto);
        }

        // api/categories/categoryId
        [HttpGet("{categoryId}", Name = "GetCategory")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(CategoryDto))]
        public IActionResult GetCategory(int categoryId)
        {
            if (!this.categoryRepository.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var category = this.categoryRepository.GetCategory(categoryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryDto = new CategoryDto()
            {
                Id = category.Id,
                Name = category.Name
            };

            return Ok(categoryDto);
        }

        // api/categories/books/bookId
        [HttpGet("books/{bookId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(404)]
        [ProducesResponseType(200, Type = typeof(IEnumerable<CategoryDto>))]
        public IActionResult GetAllCategoriesForABook(int bookId)
        {
            // TODO - Validate the book exists
            var categories = this.categoryRepository.GetAllCategoriesForBook(bookId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }    

            var categoriesDto = new List<CategoryDto>();
            foreach (var category in categories)
            {
                categoriesDto.Add(new CategoryDto()
                {
                    Id = category.Id,
                    Name = category.Name
                });
            }

            return Ok(categoriesDto);
        }
    }
}
