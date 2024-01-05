using System.ComponentModel.DataAnnotations;

namespace ToDoListWeb;

using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;

public class ToDoListContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<UserToDoList> UserToDoLists { get; set; }

    public string DbPath { get; }

    public ToDoListContext()
    {
        var folder = Environment.SpecialFolder.LocalApplicationData;
        var path = Environment.GetFolderPath(folder);
        DbPath = System.IO.Path.Join(path, "data.db");
    }

    // The following configures EF to create a Sqlite database file in the
    // special "local" folder for your platform.
    protected override void OnConfiguring(DbContextOptionsBuilder options)
        => options.UseSqlite($"Data Source={DbPath}");
}

public class User(string Username, string PasswordHash)
{
    [Key]
    public string Username { get; init; } = Username;
    public string PasswordHash { get; init; } = PasswordHash;

    public void Deconstruct(out string Username, out string PasswordHash)
    {
        Username = this.Username;
        PasswordHash = this.PasswordHash;
    }
}

public class UserToDoList(string Username, string ToDoList)
{
    [Key]
    public string Username { get; init; } = Username;
    public string ToDoList { get; set; } = ToDoList;

    public void Deconstruct(out string Username, out string ToDoList)
    {
        Username = this.Username;
        ToDoList = this.ToDoList;
    }
}