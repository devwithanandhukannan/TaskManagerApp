using System;

User user = new User();
Console.WriteLine(user.Name);
user.Name = "John"; // This is allowed because Name is public
// Console.WriteLine(user.Name);
// Console.WriteLine(user.Age); // This will cause a compilation error because Age is private
// user.Age = 30; // This will also cause a compilation error because Age is private

user.setAge(30); // This is allowed because setAge is public

public class User
{
    public string Name = "Anandhu";
    private int Age = 25;

    public void setAge(int age)
    {
        Age = age;
        Console.WriteLine($"Age set to: {Age}");
    }
}
