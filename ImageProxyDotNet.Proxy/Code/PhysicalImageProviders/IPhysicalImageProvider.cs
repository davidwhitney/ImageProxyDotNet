namespace ImageProxy.Net.Code.PhysicalImageProviders
{
    public interface IPhysicalImageProvider
    {
        byte[] LoadImage(string category, string name);
    }
}