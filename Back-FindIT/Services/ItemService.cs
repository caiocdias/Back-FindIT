﻿using Back_FindIT.Controllers;
using Back_FindIT.Data;
using Back_FindIT.Dtos.CategoryDtos;
using Back_FindIT.Dtos.ItemDtos;
using Back_FindIT.Dtos.UserDtos;
using Back_FindIT.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.ML;
using Microsoft.ML.Data;
using Microsoft.ML.Trainers;

namespace Back_FindIT.Services
{
    public class ItemService
    {
        private readonly AppDbContext _appDbContext;
        public ItemService(AppDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<ItemRegisterDto?> AddItemAsync(ItemRegisterDto itemRegisterDto)
        {
            Item item = new Item
            {
                Name = itemRegisterDto.Name,
                Description = itemRegisterDto.Description,
                IsActive = true,
                CategoryId = itemRegisterDto.Category.Id,
                RegisteredBy = itemRegisterDto.RegisteredUser.Id,
                ClaimedBy = itemRegisterDto.ClaimedUser.Id
            };

            item.SetUpdatedAt();

            _appDbContext.Items.Add(item);
            await _appDbContext.SaveChangesAsync();

            return itemRegisterDto;
        }
        public async Task<ItemDto?> GetItemByIdAsync(int id)
        {
            var item = await _appDbContext.Items
                .Include(i => i.Category)
                .Include(i => i.RegisteredUser)
                .Include(i => i.ClaimedUser)
                .AsNoTracking()
                .FirstOrDefaultAsync(i => i.Id == id);

            if (item == null)
                return null;

            return new ItemDto(item);
        }

        public async Task<List<ItemDto>?> GetItemByCategoryIdAsync(int categoryId)
        {
            var items = await _appDbContext.Items
                .AsNoTracking()
                .Where(i => i.CategoryId == categoryId)
                .ToListAsync();

            if (items == null)
                return null;

            return items.Select(item => new ItemDto(item)).ToList();
        }

        public async Task<List<ItemDto>?> GetItemByNameAsync(string name)
        {
            var items = await _appDbContext.Items
                .AsNoTracking()
                .Where(i => EF.Functions.Like(i.Name, $"%{name}%") && i.IsActive)
                .ToListAsync();

            if (items == null || items.Count == 0)
                return null;

            return items.Select(item => new ItemDto(item)).ToList();
        }
        public async Task<List<ItemDto>?> GetItemByClaimedByIdAsync(int claimedById)
        {
            var items = await _appDbContext.Items
                .AsNoTracking()
                .Where(i => i.ClaimedBy == claimedById)
                .ToListAsync();

            if (items == null)
                return null;

            return items.Select(item => new ItemDto(item)).ToList();
        }

        public async Task<List<ItemDto>?> GetAllItemsAsync()
        {
            var items = await _appDbContext.Items
                .AsNoTracking()
                .ToListAsync();

            if (items == null)
                return null;

            return items.Select(item => new ItemDto(item)).ToList();
        }

        public async Task<bool> SoftDeleteItemAsync(int itemId)
        {
            var item = await _appDbContext.Items.FirstOrDefaultAsync(u => u.Id == itemId);

            if (item == null)
                return false;

            if (!item.IsActive)
                return true;


            item.IsActive = false;
            item.SetUpdatedAt();

            _appDbContext.Items.Update(item);
            await _appDbContext.SaveChangesAsync();

            return true;
        }

        public async Task<ItemDto?> UpdateItemAsync(int id, ItemDto itemDto)
        {
            var item = await _appDbContext.Items.FirstOrDefaultAsync(u => u.Id == itemDto.Id);

            if (item == null)
                return null;

            if (!item.IsActive)
                throw new UnauthorizedAccessException("Item desativado.");

            item.Name = itemDto.Name;
            item.Description = itemDto.Description;
            item.IsActive = itemDto.IsActive;
            item.CategoryId = itemDto.Category.Id;
            item.RegisteredBy = itemDto.RegisteredUser.Id;
            item.ClaimedBy = itemDto.ClaimedUser.Id;
            item.SetUpdatedAt();

            _appDbContext.Items.Update(item);
            await _appDbContext.SaveChangesAsync();

            return new ItemDto(item);
        }

        public async Task<List<ItemDto>> SearchItemsAsync(string query)
        {
            var items = await _appDbContext.Items
                .Include(i => i.Category)   
                .Include(i => i.RegisteredUser) 
                .Include(i => i.ClaimedUser)    
                .AsNoTracking()
                .ToListAsync();



            if (!items.Any())
                return new List<ItemDto>();

            var documents = items.Select(i => i.Description ?? "").ToList();

            var mlContext = new MLContext();
            var data = mlContext.Data.LoadFromEnumerable(documents.Select(d => new InputText { Text = d }));

            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(InputText.Text));
            var model = pipeline.Fit(data);
            var transformedData = model.Transform(data);

            var featureColumn = transformedData.GetColumn<float[]>("Features").ToArray();

            var queryVector = model.Transform(mlContext.Data.LoadFromEnumerable(new[] { new InputText { Text = query } }))
                                   .GetColumn<float[]>("Features")
                                   .FirstOrDefault();

            if (queryVector == null) return new List<ItemDto>();

            var similarities = featureColumn.Select((vector, index) => new
            {
                Item = items[index],
                Similarity = CosineSimilarity(queryVector, vector)
            })
            .OrderByDescending(x => x.Similarity)
            .Take(9) 
            .Select(x => new ItemDto(x.Item))
            .ToList();

            return similarities;
        }

        private static float CosineSimilarity(float[] vecA, float[] vecB)
        {
            float dotProduct = vecA.Zip(vecB, (a, b) => a * b).Sum();
            float magnitudeA = (float)Math.Sqrt(vecA.Sum(a => a * a));
            float magnitudeB = (float)Math.Sqrt(vecB.Sum(b => b * b));
            return magnitudeA * magnitudeB == 0 ? 0 : dotProduct / (magnitudeA * magnitudeB);
        }

        private class InputText
        {
            public string Text { get; set; }
        }

        public async Task<List<ItemDto>> FindSimilarItemsAsync(int selectedItemId, int numberOfClusters = 5)
        {
            var items = await _appDbContext.Items
                .Include(i => i.Category)
                .Include(i => i.RegisteredUser)
                .Include(i => i.ClaimedUser)
                .AsNoTracking()
                .ToListAsync();

            if (!items.Any())
                return new List<ItemDto>();

            var selectedItem = items.FirstOrDefault(i => i.Id == selectedItemId);
            if (selectedItem == null)
                return new List<ItemDto>();

            var documents = items.Select(i => i.Description ?? "").ToList();

            var mlContext = new MLContext();
            var data = mlContext.Data.LoadFromEnumerable(documents.Select(d => new InputText { Text = d }));

            var pipeline = mlContext.Transforms.Text.FeaturizeText("Features", nameof(InputText.Text));
            var model = pipeline.Fit(data);
            var transformedData = model.Transform(data);

            var options = new KMeansTrainer.Options
            {
                NumberOfClusters = numberOfClusters, 
                FeatureColumnName = "Features",
                MaximumNumberOfIterations = 100 
            };

            var trainer = mlContext.Clustering.Trainers.KMeans(options);
            var clusteringModel = trainer.Fit(transformedData);

            var predictions = clusteringModel.Transform(transformedData);
            var clusters = predictions.GetColumn<uint>("PredictedLabel").ToArray(); 

            var selectedItemIndex = items.IndexOf(selectedItem);
            var selectedCluster = clusters[selectedItemIndex];

            var similarItems = items.Select((item, index) => new
            {
                Item = item,
                Cluster = clusters[index],
                Similarity = CosineSimilarity(
                    transformedData.GetColumn<float[]>("Features").ElementAt(selectedItemIndex),
                    transformedData.GetColumn<float[]>("Features").ElementAt(index))
            })
            .Where(x => x.Cluster == selectedCluster && x.Item.Id != selectedItemId)
            .OrderByDescending(x => x.Similarity) 
            .Take(3)
            .Select(x => new ItemDto(x.Item))
            .ToList();

            return similarItems;
        }


    }
}
