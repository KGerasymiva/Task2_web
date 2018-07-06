using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Task2.Models;

namespace Task2.NewFolder
{
    public class ServiceData
    {
        public static List<User> Users { get; set; }
        public static List<Post> Posts { get; set; }
        public static List<Comment> Comments { get; set; }
        public static List<Todo> Todos { get; set; }

        static HttpClient client = new HttpClient();

        static async Task<List<T>> GetAllDataAsync<T>(string path)
        {
            List<T> list = new List<T>();

            HttpResponseMessage response = await client.GetAsync(path);
            if (response.IsSuccessStatusCode)
            {
                list = await response.Content.ReadAsAsync<List<T>>();
            }
            return list;
        }


        public static async Task<bool> GetAllDataAsync()
        {
            Users = await GetAllDataAsync<User>("users");
            Posts = await GetAllDataAsync<Post>("posts");
            Comments = await GetAllDataAsync<Comment>("comments");
            Todos = await GetAllDataAsync<Todo>("todos");

            var commentsList = Posts.GroupJoin(Comments,
                p => p.Id,
                c => c.PostId,
                (p, collection) => p.Comments = collection.ToList()).ToList();

            var postsList = Users.GroupJoin(Posts,
                u => u.Id,
                p => p.UserId,
                (u, collection) => u.Posts = collection.ToList()).ToList();

            var todosList = Users.GroupJoin(Todos,
                u => u.Id,
                t => t.UserId,
                (u, collection) => u.Todos = collection.ToList()).ToList();

            return true;
        }

        public static async Task GetCommentCountAsync(int id) //1
        {
            await GetAllDataAsync();
            var query = Users
                .Where(x => x.Id == id)
                .Select(y => y.Posts)
                .FirstOrDefault()
                ?.Select(z => (Post: z, CommentsCount: z.Comments.Count()));

            foreach (var item in query)
            {
                Console.WriteLine($"Post with title:\"{item.Post.Title}\" has {item.CommentsCount} comment(s)");
            }
        }

        public static async Task GetCommentList50Async(int id)  //2
        {
            await GetAllDataAsync();
            var query = Users
                .Where(x => x.Id == id)
                .Select(y => y.Posts)
                .FirstOrDefault();
            var enumerable = query as Post[] ?? query?.ToArray();
            enumerable?.Select(c => c.Comments)
            .FirstOrDefault()
            ?.Where(z => z.Body.Length < 50);

            if (enumerable != null && enumerable.Any())
            {
                Console.WriteLine($"User with id {id} has comments:");

                foreach (var item in enumerable)
                {
                    Console.WriteLine($"Comment id: {item.Id} Comment Body:\"{item.Body}\"");
                }
            }


        }

        public static async Task GetTodoListAsync(int id)  //3
        {
            await GetAllDataAsync();
            var query = Users
                .Where(x => x.Id == id)
                .Select(y => y.Todos)
                .FirstOrDefault()
                ?.Where(z => z.IsComplete)
                .Select(res => (Id: res.Id, Name: res.Name));

            foreach (var item in query)
            {
                Console.WriteLine($"id: {item.Id} - Name:{item.Name}");
            }

        }

        public static async Task GetUserTodoListAsync() //4
        {
            await GetAllDataAsync();
            var query = Users
                .Join
                (Users,
                    u => u.Id,
                    t => t.Id,
                    (u, t) =>
                        (Name: u.Name, List: t.Todos.OrderByDescending(x => x.Name.Length).Select(z => z.ToString())
                        ))
                .OrderBy(y => y.Name);

            foreach (var item in query)
            {
                Console.WriteLine($"{item.Name} ");
                foreach (var y in item.List)
                {
                    Console.WriteLine($"{y}");
                }

                Console.WriteLine();
            }

        }


        public static async Task GetUserInfo5Async(int id)
        {
            await GetAllDataAsync();

            var user = Users //5.1
                .FirstOrDefault(x => x.Id == id);

            var lastPost = Users  //5.2
                .Where(x => x.Id == id)
                .Select(y => y.Posts)
                .FirstOrDefault()
                ?.OrderBy(z => z.CreatedAt)
                .LastOrDefault();

            var countLastPost = Users  //5.3 
                .Where(x => x.Id == id)
                .Select(y => y.Posts)
                .FirstOrDefault()
                ?.OrderBy(z => z.CreatedAt)
                .LastOrDefault()
                ?.Comments
                .Count();

            var undoneTasks = Users  //5.4 
                .Where(x => x.Id == id)
                .Select(y => y.Todos)
                .FirstOrDefault()
                ?.Where(z => !z.IsComplete).Count();


            var popPost80 = Posts  //5.5
                .Where(p => p.UserId == id)
                .SelectMany(y => y.Comments)
                .Where(z => z.Body.Length > 80)
                .GroupBy(i => i.PostId)
                .OrderBy(x => x.Count())
                .LastOrDefault()
                ?.Select(o => (Posts
                    .Where(p => (p.Id == o.PostId))
                    .FirstOrDefault()))
                .FirstOrDefault();

            var popPostLikes = Users  //5.6
                .Where(x => x.Id == id)
                .Select(y => y.Posts)
                .FirstOrDefault()
                ?.OrderBy(z => z.Likes).LastOrDefault();

            Console.WriteLine($"User Id:{user?.Name} \n last post title: {lastPost?.Title}\n comments count:{countLastPost}\n undone tasks count:{undoneTasks}\n popular post by comments: {popPost80?.Id}\n popular post by likes: {popPostLikes?.Id}");
        }


        public static async Task GetPostInfo6Async(int postId)  //6
        {
            await GetAllDataAsync();

            var post = Posts
                .FirstOrDefault(x => x.Id == postId);

            var longComment = Posts
                .Where(x => x.Id == postId)
                .Select(y => y.Comments)
                .FirstOrDefault()
                ?.OrderBy(z => z.Body.Length)
                .LastOrDefault();

            var likeComment = Posts
                .Where(x => x.Id == postId)
                .Select(y => y.Comments)
                .FirstOrDefault()
                ?.OrderBy(z => z.Likes)
                .LastOrDefault();

            var countComment = Posts
                .Where(x => x.Id == postId)
                .SelectMany(y => y.Comments)
                .Where(z => (z.Likes == 0) || (z.Body.Length < 80))
                .Count();

            Console.WriteLine($"post.Id: {post?.Id}\n the longest comment id: {longComment?.Id}\n the likest comment id: {likeComment?.Id}\n Comments count: {countComment}");

        }
       
       //!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
       public static void MainStart()
        {
            RunAsync().GetAwaiter().GetResult();
        }

        static async Task RunAsync()
        {
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://5b128555d50a5c0014ef1204.mockapi.io/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

            try
            {
                await GetAllDataAsync();

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
    }


}
