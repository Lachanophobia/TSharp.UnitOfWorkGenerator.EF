﻿using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using TSharp.UnitOfWorkGenerator.DataAccess.Entities;
using TSharp.UnitOfWorkGenerator.DataAccess.Models;
using TSharp.UnitOfWorkGenerator.DataAccess.Repositories.IRepository;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace TSharp.UnitOfWorkGenerator.API.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public TestController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        [HttpGet]
        [Route("GetPosts")]
        public async Task<IActionResult> GetPosts(CancellationToken cancellationToken)
        {
            return Ok(await _unitOfWork.Post.GetFirstOrDefaultAsync(x => x.BlogId!= 1, cancellationToken));
        }

        [HttpGet]
        [Route("GetPostsFromPartialClass")]
        public async Task<IActionResult> GetPostsFromPartialClass(CancellationToken cancellationToken)
        {
            return Ok(await _unitOfWork.Post.GetPostsFromPartialClass(cancellationToken));
        }

        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
        {
            var blogs = await _unitOfWork.Blog.GetAllAsync(cancellationToken: cancellationToken,includeProperties: x => x.Posts);
            return Ok(blogs);
        }

        [HttpPost]
        [Route("AddNewEmployee")]
        public async Task<IActionResult> AddNewEmployee(EmployeeRequest employeeRequest, CancellationToken cancellationToken)
        {
            var employee = new Employee()
            {
                Age = employeeRequest.Age,
                Address = employeeRequest.Address,
                FirstName = employeeRequest.FirstName,
                LastName = employeeRequest.LastName,
            };

            await _unitOfWork.Employee.AddAsync(employee, cancellationToken);
            await _unitOfWork.SaveAsync();

            return Ok(employee);
        }

        [HttpPost]
        [Route("AddNewPost")]
        public async Task<IActionResult> AddNewPost(PostRequest postRequest, CancellationToken cancellationToken)
        {
            var post = new Post()
            {
                BlogId = postRequest.BlogId,
                Content = postRequest.Content,
                Title = postRequest.Title
            };

            await _unitOfWork.Post.AddAsync(post, cancellationToken);
            await _unitOfWork.SaveAsync();

            return Ok(post);
        }
    }
}
