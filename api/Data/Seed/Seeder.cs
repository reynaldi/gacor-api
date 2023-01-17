using GacorAPI.Data.Entities;

namespace GacorAPI.Data.Seed
{
    public static class Seeder
    {
        public static void SeedData(GacorContext context)
        {
            SeedUser(context);
            SeedBlog(context);
        }

        private static void SeedUser(GacorContext context)
        {
            if(context.Users.Any()) return;
            var user = new User
            {
                Email = "reynaldi.surya@gmail.com",
                FirstName = "Reynaldi",
                LastName = "Surya",
                Password = "07c106e1da04f1a8c376dc7dccd036140f8b76343f73a1c23dc5326663370d01",
                IsActive = true
            };
            context.Add(user);
            context.SaveChanges();
        }

        private static void SeedBlog(GacorContext context)
        {
            if(context.Blogs.Any()) return;
            var blog = new Blog
            {
                Title = "First tweet",
                Body = "First tweet body",
                CreatedDate = DateTime.UtcNow,
                CreatorId = 1                
            };
            context.Add(blog);
            context.SaveChanges();
        }
    }
}