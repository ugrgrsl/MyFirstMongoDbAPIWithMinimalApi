using TodoApp.Services;

namespace TodoApp
{
    public class Routes
    {
        private readonly MongoDbService _mongoDbService;
        public Routes(MongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }
        //public WebApplication ApplicationRoutes(WebApplication webApplication)
        //{
            
        //    return webApplication;
        //}
    }
}
