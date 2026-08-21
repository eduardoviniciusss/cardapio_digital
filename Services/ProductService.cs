using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using cardapio_digital.Dtos;
using cardapio_digital.Entities;

namespace cardapio_digital.Services
{
    public class ProductService
    {
        private readonly AppDbContext _db;

        public ProductService(AppDbContext db)
        {
            _db = db;
        }

        public async Task<IResult> GetAll(int userId)
        {
            var school = await _db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
            if (school == null)
            {
                return Results.NotFound("School not found.");
            }

            var products = await _db.Products
                .Include(p => p.Category)
                .Where(p => p.SchoolId == school.Id)
                .ToListAsync();

            var response = products.Select(p => new ProductResponseDto
            {
                Id = p.Id,
                Name = p.Name,
                Price = p.Price,
                Category = p.Category
            }).ToList();

            return Results.Ok(response);
        }

        public async Task<IResult> GetById(int id, int userId)
        {
            var school = await _db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
            if (school == null)
            {
                return Results.NotFound("School not found.");
            }

            var product = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == id && p.SchoolId == school.Id);

            if (product == null)
            {
                return Results.NotFound("Product not found.");
            }

            var response = new ProductResponseDto
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                Category = product.Category
            };

            return Results.Ok(response);
        }

        public async Task<IResult> Register(ProductDto dto, int userId)
        {
            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest("Name is required.");
            }

            if (dto.Price <= 0)
            {
                return Results.BadRequest("Price must be greater than zero.");
            }

            var school = await _db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
            if (school == null)
            {
                return Results.NotFound("School not found.");
            }

            var categoryExists = await _db.Categories.FindAsync(dto.CategoryId);
            if (categoryExists is null)
            {
                return Results.BadRequest("Category not found.");
            }

            var product = new Product
            {
                Name = dto.Name,
                Price = dto.Price,
                CategoryId = dto.CategoryId,
                SchoolId = school.Id
            };

            _db.Products.Add(product);
            await _db.SaveChangesAsync();

            var productWithCategory = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (productWithCategory is null)
            {
                return Results.BadRequest("Erro retrieving the created product.");
            }

            var response = new ProductResponseDto
            {
                Id = productWithCategory.Id,
                Name = productWithCategory.Name,
                Price = productWithCategory.Price,
                Category = productWithCategory.Category
            };

            return Results.Created($"/products/{product.Id}", response);
        }

        public async Task<IResult> Update(int id, ProductDto dto, int userId)
        {
            var school = await _db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
            if (school == null)
            {
                return Results.NotFound("School not found.");
            }

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.SchoolId == school.Id);
            if (product is null)
            {
                return Results.NotFound("Product not found.");
            }

            if (string.IsNullOrWhiteSpace(dto.Name))
            {
                return Results.BadRequest("Name is required.");
            }

            var categoryExists = await _db.Categories.FindAsync(dto.CategoryId);
            if (categoryExists is null)
            {
                return Results.BadRequest("Category not found.");
            }

            product.Name = dto.Name;
            product.Price = dto.Price;
            product.CategoryId = dto.CategoryId;

            await _db.SaveChangesAsync();

            var productWithCategory = await _db.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(p => p.Id == product.Id);

            if (productWithCategory is null)
            {
                return Results.BadRequest("Erro retrieving the updated product.");
            }

            var response = new ProductResponseDto
            {
                Id = productWithCategory.Id,
                Name = productWithCategory.Name,
                Price = productWithCategory.Price,
                Category = productWithCategory.Category
            };

            return Results.Ok(response);
        }

        public async Task<IResult> Delete(int id, int userId)
        {
            var school = await _db.Schools.FirstOrDefaultAsync(e => e.UserId == userId);
            if (school == null)
            {
                return Results.NotFound("School not found.");
            }

            var product = await _db.Products.FirstOrDefaultAsync(p => p.Id == id && p.SchoolId == school.Id);
            if (product is null)
            {
                return Results.NotFound("Product not found.");
            }

            _db.Products.Remove(product);
            await _db.SaveChangesAsync();

            return Results.NoContent();
        }
    }
}