using System.Resources;

namespace Common.Resources;
public static class Resources
{
    private static readonly Lazy<ResourceManager> ResourceManager = new Lazy<ResourceManager>(() =>
    {
        var resourceType = typeof(Resources);
        return new ResourceManager(resourceType.FullName, resourceType.Assembly);
    });

    public static string Get(string key)
    {
        var resourceValue = ResourceManager.Value.GetString(key);
        return resourceValue ?? key;
    }
}
