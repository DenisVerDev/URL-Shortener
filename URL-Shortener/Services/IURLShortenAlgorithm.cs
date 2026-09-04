namespace URL_Shortener.Services
{
    public interface IURLShortenAlgorithm
    {
        Task<string?> ShortenURLAsync(string url);
    }
}
