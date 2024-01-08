namespace Project.Application.Exceptions;
public class AlreadyExistsException : ApplicationException
{
    public AlreadyExistsException(string name, object key)
            : base($"Entity {name} - ({key}) მომხმარებელი ასეთი სახელით უკვე არსებობს.")
    {
    }
}
