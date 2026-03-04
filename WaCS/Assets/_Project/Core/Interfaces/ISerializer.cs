public interface ISerializer
{
    string Serialize<T>(T obj); // Serialize an object of type T to a JSON string
    T Deserialize<T>(string json); // Deserialize a JSON string back into an object of type T
}
