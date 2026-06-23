using System;

DerivedClass derived = new DerivedClass();
derived.ShowProtectedField(); // This will print "I am protected" because ShowProtectedField is

// BaseClass base = new BaseClass();
// Console.WriteLine(base.protectedField); // This will cause a compilation error because protectedField is protected and cannot be accessed from outside the class hierarchy

public class BaseClass{
    protected string protectedField = "I am protected";
}

public class DerivedClass : BaseClass{
    public void ShowProtectedField()
    {
        Console.WriteLine(protectedField);
    }
}

