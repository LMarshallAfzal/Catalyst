public class RouteDefinition
{
    public string Path { get; set; }
    public string Method { get; set; }
    public string Response { get; set; }
    public string Controller { get; set; }
    public string Action { get; set; }

    public RouteDefinition()
    {
        Path = "";
        Method = "";
        Response = "";
        Controller = "";
        Action = "";

    }
}