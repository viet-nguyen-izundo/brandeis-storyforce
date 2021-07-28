using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using StoryForce.Server.Services;
using StoryForce.Shared.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace StoryForce.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoriesService _categoriesService;
        private readonly IStoryFileService _storyFileService;        

        public CategoryController(ICategoriesService categoriesService, IStoryFileService storyFileService)
        {
            _categoriesService = categoriesService;
            this._storyFileService = storyFileService;           
        }

        // GET: api/<Category>
        [HttpGet]
        public async Task<List<Category>> Get()
        {
            return await _categoriesService.GetAsync();
        }

        // GET api/<Category>/5
        [HttpGet("{id}", Name = "GetCategory")]
        public async Task<ActionResult<Category>> Get(int id)
        {
            return await _categoriesService.GetAsync(id);
        }

        //POST api/<Category>
        [HttpPost]
        public async Task<ActionResult> Post(CreateCategoryDto categoriesDto)
        {
            var createdCategory = new Category();
            if (categoriesDto.Name == "")
            {
                return BadRequest($"Error Category name because null");
            }
            else
            {
                createdCategory = await _categoriesService.CreateAsync(categoriesDto.ToEntity());
            }

            var storyFile = await _storyFileService.GetAsync(categoriesDto.StoryFileId);
            if (storyFile == null)
                return BadRequest($"Story file with id '{categoriesDto.StoryFileId}' not found.");
            storyFile.Categories.Add(createdCategory);
            await _storyFileService.UpdateAsync(storyFile.Id, storyFile);


            return CreatedAtRoute("GetCategory", new { id = createdCategory.Id }, createdCategory);
        }

        // PUT api/<Category>/5
        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, EditCategoriesDto category)
        {
            var categoryInDb = await _categoriesService.GetAsync(id);
            if (categoryInDb == null || category.Id != id)
                return BadRequest($"Category with id '{id}' not found.");
            categoryInDb.Name = category.Name;
            await _categoriesService.UpdateAsync(id, categoryInDb);
            return NoContent();
        }

        // DELETE api/<Category>/5
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var categoryInDb = await _categoriesService.GetAsync(id);
            if (categoryInDb == null)
                return BadRequest($"Category with id '{id}' not found.");
            await _categoriesService.RemoveAsync(id);
            return NoContent();
        }
        public class CreateCategoryDto
        {
            public string Name { get; set; }

            public int StoryFileId { get; set; }
            public Category ToEntity()
            {
                return new Category()
                {
                    Name = this.Name,
                    CreatedAt = DateTime.Now
                };
            }
        }

        public class EditCategoriesDto
        {
            public int Id { get; set; }
            public string Name { get; set; }

        }
    }
}
