using System.Security.Cryptography;
using ToDoListWeb;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAllOrigins",
        builder =>
        {
            builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader();
        });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAllOrigins");

app.UseHttpsRedirection();

app.MapGet("/register", (string username, string password) =>
    {
        using var db = new ToDoListContext();
        if (db.Users.Any(u => u.Username == username))
        {
            return Result.Failure($"用户 {username} 已存在");
        }

        db.Users.Add(new User(username, HashPassword(password)));

        db.SaveChanges();
        return Result.Success();
    })
    .WithName("注册账户")
    .WithOpenApi();

app.MapGet("/remove", (string username, string password) =>
    {
        using var db = new ToDoListContext();
        var user = db.Users.FirstOrDefault(u => u.Username == username);
        if (user is null || !VerifyPassword(password, user.PasswordHash))
            return Result.Failure("用户名或密码错误");

        db.Users.Remove(user);

        db.SaveChanges();
        return Result.Success();
    })
    .WithName("移除账户")
    .WithOpenApi();

app.MapGet("/login", (string username, string password) =>
    {
        using var db = new ToDoListContext();
        var user = db.Users.FirstOrDefault(u => u.Username == username);
        if (user is null || !VerifyPassword(password, user.PasswordHash))
            return Result.Failure("用户名或密码错误");

        db.SaveChanges();
        return Result.Success();
    })
    .WithName("登录账户")
    .WithOpenApi();

app.MapGet("/upload", (string username, string password, string toDoList) =>
    {
        using var db = new ToDoListContext();
        var user = db.Users.FirstOrDefault(u => u.Username == username);
        if (user is null || !VerifyPassword(password, user.PasswordHash))
            return Result.Failure("用户名或密码错误");

        if (!db.UserToDoLists.Any(u => u.Username == username))
            db.UserToDoLists.Add(new UserToDoList(username, toDoList));
        else
            db.UserToDoLists.Update(new UserToDoList(username, toDoList));

        db.SaveChanges();
        return Result.Success();
    })
    .WithName("上传待办事项")
    .WithOpenApi();

app.MapGet("/download", (string username, string password) =>
{
    using var db = new ToDoListContext();
    var user = db.Users.FirstOrDefault(u => u.Username == username);
    if (user is null || !VerifyPassword(password, user.PasswordHash))
        return Result.Failure("用户名或密码错误");

    var toDoList = db.UserToDoLists.FirstOrDefault(u => u.Username == username);
    if (toDoList is null)
        return Result.Failure("待办事项不存在");

    db.SaveChanges();
    return (IResult)ResultWithResource<string>.Success(toDoList.ToDoList);
});

app.Run();

static string HashPassword(string password)
{
    // 生成盐值
    byte[] salt;
    new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

    // 使用Rfc2898DeriveBytes进行哈希
    var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);
    byte[] hash = pbkdf2.GetBytes(20);

    // 将盐值和哈希值合并
    byte[] hashBytes = new byte[36];
    Array.Copy(salt, 0, hashBytes, 0, 16);
    Array.Copy(hash, 0, hashBytes, 16, 20);

    // 将合并的值转换为Base64字符串
    string base64Hash = Convert.ToBase64String(hashBytes);
    return base64Hash;
}

static bool VerifyPassword(string inputPassword, string storedPassword)
{
    // 将Base64字符串还原为字节数组
    byte[] hashBytes = Convert.FromBase64String(storedPassword);

    // 从字节数组中提取盐值
    byte[] salt = new byte[16];
    Array.Copy(hashBytes, 0, salt, 0, 16);

    // 使用提取的盐值进行哈希
    var pbkdf2 = new Rfc2898DeriveBytes(inputPassword, salt, 10000);
    byte[] inputHash = pbkdf2.GetBytes(20);

    // 比较计算出的哈希值与存储的哈希值
    for (int i = 0; i < 20; i++)
    {
        if (hashBytes[i + 16] != inputHash[i])
        {
            return false;
        }
    }

    return true;
}

internal interface IResult
{
    bool IsSuccess { get; }
    IEnumerable<string> Errors { get; }
}


internal class Result(params string[] errors) : IResult
{
    public static Result Success() => new();
    public static Result Failure(params string[] errors) => new(errors);
    public bool IsSuccess => !Errors.Any();
    public IEnumerable<string> Errors { get; } = errors;
}

internal class ResultWithResource<T>(T resource, params string[] errors) : IResult
{
    public static ResultWithResource<T> Success(T resource) => new(resource);
    public bool IsSuccess => true;
    public IEnumerable<string> Errors { get; } = Enumerable.Empty<string>();
    public T Resource { get; } = resource;
}