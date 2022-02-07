using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RestaurantAPI.Services
{
    public interface IDishService
    {
        int Create(int restaurantId, CreateDishDto dto);
        DishDto GetById(int restaurantId, int dishId);
        List<DishDto> GetAll(int restaurantId);
        void Update(int restaurantId, int dishId, UpdateDishDto dto);
        void Delete(int restaurantId, int dishId);
        void DeleteAll(int restaurantId);
    }

    public class DishService : IDishService
    {

        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<DishService> _logger;

        public DishService(RestaurantDbContext dbContext, IMapper mapper, ILogger<DishService> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _logger = logger;
        }
        public int Create(int restaurantId, CreateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dishEntity = _mapper.Map<Dish>(dto);

            dishEntity.RestaurantId = restaurantId;

            _dbContext.Dishes.Add(dishEntity);
            _dbContext.SaveChanges();

            return dishEntity.Id;
            
        }
        public DishDto GetById(int restaurantId,int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _dbContext
                .Dishes
                .FirstOrDefault(d => d.Id == dishId && d.RestaurantId == restaurantId);

            if (dish is null) throw new NotFoundException("Dish not found");

            var result = _mapper.Map<DishDto>(dish);
            return result;
        }

        public List<DishDto> GetAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var result = _mapper.Map<List<DishDto>>(restaurant.Dishes);
            return result;
        }
        public void Update(int restaurantId, int dishId, UpdateDishDto dto)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _dbContext
                .Dishes
                .FirstOrDefault(d => d.Id == dishId && d.RestaurantId == restaurantId);

            if (dish is null) throw new NotFoundException("Dish not found");

            dish.Name = dto.Name;
            dish.Description = dto.Description;
            dish.Price = dto.Price;
            _dbContext.SaveChanges();

        }

        public void DeleteAll(int restaurantId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            _dbContext.RemoveRange(restaurant.Dishes);
            _dbContext.SaveChanges();
        }

        public void Delete(int restaurantId, int dishId)
        {
            var restaurant = GetRestaurantById(restaurantId);

            var dish = _dbContext
                .Dishes
                .FirstOrDefault(d => d.Id == dishId && d.RestaurantId == restaurantId);

            if (dish is null) throw new NotFoundException("Dish not found");

            _dbContext.Dishes.Remove(dish);
            _dbContext.SaveChanges();

        }    
        
        private Restaurant GetRestaurantById(int restaurantId)
        {
            var restaurant = _dbContext
               .Restaurants
               .Include(r => r.Dishes)
               .FirstOrDefault(r => r.Id == restaurantId);

            if (restaurant is null) throw new NotFoundException("Restaurant not found");

            return restaurant;
        }
    }
}

