using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Task2.Models;

namespace Task2.NewFolder
{
    public static class ServiceData
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
            // Update port # in the following line.
            client.BaseAddress = new Uri("https://5b128555d50a5c0014ef1204.mockapi.io/");
            client.DefaultRequestHeaders.Accept.Clear();
            client.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json"));

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

            var commentsUserList = Users.GroupJoin(Comments,
                u => u.Id,
                c => c.UserId,
                (u, collection) => u.Comments = collection.ToList()).ToList();
            return true;
        }

        public static Query1Model GetCommentCount(int id) //1
        {

            var query = Users
                .Where(x => x.Id == id)
                .Select(y => y.Posts)
                .FirstOrDefault()
                ?.Select(z => (Tuple.Create(z, z.Comments.Count()))).ToList(); //Post, CommentsCount


            //if  
            return new Query1Model()
            {
                Query1 = query
            };


        }

        public static List<Comment> GetCommentList50(int id)  //2
        {
           
            var query = Users
                .Where(x => x.Id == id)
                .Select(y => y.Posts)
                .FirstOrDefault();
            var enumerable = query as Post[] ?? query?.ToArray();
            var query1=enumerable?.Select(c => c.Comments)
            .FirstOrDefault()
            ?.Where(z => z.Body.Length < 50).ToList();



            if (enumerable != null && enumerable.Any())
            {
                return query1;
                // return new Query2Model() {Query2 = query1};
                //Console.WriteLine($"UserList with id {id} has comments:");

                //foreach (var item in enumerable)
                //{
                //    Console.WriteLine($"Comment id: {item.Id} Comment Body:\"{item.Body}\"");
                //}
            }

            return null;
        }

        public static List<Todo> GetTodoList(int id)  //3
        {
          
            var query = Users
                .Where(x => x.Id == id)
                .Select(y => y.Todos)
                .FirstOrDefault()
                ?.Where(z => z.IsComplete)
                .ToList();

            return query;

        }

        public static Query4Model GetUserTodoList() //4
        {
            //await GetAllDataAsync();
            var query = Users
                .Join
                (Users,
                    u => u.Id,
                    t => t.Id,
                    (u, t) =>
                        Tuple.Create(u, (t.Todos.OrderByDescending(x => x.Name.Length).ToList())
                        ))
                .OrderBy(y => y.Item1.Name).ToList();

            return new Query4Model() {TodoList = query};

            //foreach (var item in query)
            //{
            //    Console.WriteLine($"{item.Name} ");
            //    foreach (var y in item.List)
            //    {
            //        Console.WriteLine($"{y}");
            //    }

            //    Console.WriteLine();
            //}

        }


        public static Query5Model GetUserInfo5(int id)
        {
            

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

            return new Query5Model()
            {
                User =user,
                LastPost = lastPost,
                LastPostCommentCount = countLastPost,
                UndoneTasksCount = undoneTasks,
                PopPostByComments = popPost80,
                PopPostByLikes = popPostLikes

            };
  
        }


        public static Query6Model GetPostInfo6(int postId)  //6
        {
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

            return new Query6Model()
            {
                Post = post,
                LongestComment = longComment,
                LikestComment = likeComment,
                LikesCount = countComment
            };
        }


        public static List<User> GetAllData()
        {
           // return new UsersListModelcs() {UserList = Users};
            return Users;
        }

        public static Todo GetTodoById(int id)
        {
            return Todos.FirstOrDefault(x => x.Id.Equals(id));
        }

        public static User GetUserById(int id)
        {
            return Users.FirstOrDefault(x => x.Id.Equals(id));
        }

        public static Post GetPostById(int id)
        {
            return Posts.FirstOrDefault(x => x.Id.Equals(id));
        }

        public static Comment GetCommentById(int id)
        {
            return Comments.FirstOrDefault(x => x.Id.Equals(id));
        }


    }


}
