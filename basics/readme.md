| Modifier           | Where accessible?             | Simple meaning |
| ------------------ | ----------------------------- | -------------- |
| public             | Everywhere                    | Open house     |
| private            | Only inside class             | Locked room    |
| protected          | Class + child classes         | Family only    |
| internal           | Same project only             | Same building  |
| protected internal | Project OR child classes      | Mixed access   |
| private protected  | Child classes in same project | Strict family  |


# get and set
```
public class User
{
    public string Name { get; set; }
}
What it means:
set → write value into Name
get → read value from Name
```