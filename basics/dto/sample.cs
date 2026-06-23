using System;

public class loginDto{
    string email {get; set;}
    string password {get; set;}
}

public class registerDto{
    string fullName {get; set;}
    string password {get; set;}
    string email {get; set;}
}

public class updateDto{
    string fullName {get; set;}
    string password {get; set;}
    string email {get; set;}
}

public class responseDto{
    string message {get; set;}
    int statusCode {get; set;}
}
