namespace ReadGood.Infrastructure.Exceptions
{
    [Serializable]
    public class NotFoundException : Exception
    {
        public string ResourceName { get; }
        public string ResourceId { get; }

        public NotFoundException(string resource, string id) : base($"{resource} with ID '{id}' not found")
        {
            ResourceName = resource;
            ResourceId = id;
        }
        public NotFoundException(string resource, string id, System.Exception inner) : base($"{resource} with ID '{id}' not found", inner)
        {
            ResourceName = resource;
            ResourceId = id;
        }
    }
}