using RamenGo.API.Models;

namespace RamenGo.API.Repositories
{
    public class MockProteinRepository : IRepository<Protein>
    {
        private readonly List<Protein> _proteins;

        public MockProteinRepository()
        {
            _proteins = new List<Protein>
            {
                new()
                {
                    Id = "1",
                    Name = "Chasu",
                    Description = "A sliced flavourful pork meat with a selection of season vegetables.",
                    Price = "US$ 10",
                    ImageActive = "https://tech.redventures.com.br/icons/pork/active.svg",
                    ImageInactive = "https://tech.redventures.com.br/icons/pork/inactive.svg"
                },

                new()
                {
                    Id = "2",
                    Name = "Yasai Vegetarian",
                    Description = "A delicious vegetarian lamen with a selection of season vegetables.",
                    Price = "US$ 10",
                    ImageActive = "https://tech.redventures.com.br/icons/yasai/active.svg",
                    ImageInactive = "https://tech.redventures.com.br/icons/yasai/inactive.svg"
                },

                new()
                {
                    Id = "3",
                    Name = "Karaague",
                    Description = "Three units of fried chicken, moyashi, ajitama egg and other vegetables.",
                    Price = "US$ 12",
                    ImageActive = "https://tech.redventures.com.br/icons/chicken/active.svg",
                    ImageInactive = "https://tech.redventures.com.br/icons/chicken/inactive.svg"
                }
            };
        }

        public async Task<List<Protein>> GetAllAsync()
        {
            return await Task.FromResult(_proteins);
        }

        public async Task<Protein?> GetAsync(string id)
        {
            return await Task.FromResult(_proteins.FirstOrDefault(protein => protein.Id == id));
        }

        public async Task AddAsync(Protein item)
        {
            await Task.FromException<NotImplementedException>(new NotImplementedException());
        }
    }
}