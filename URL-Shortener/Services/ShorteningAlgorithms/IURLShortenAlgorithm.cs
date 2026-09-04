namespace URL_Shortener.Services.ShorteningAlgorithms
{
    public interface IURLShortenAlgorithm
    {
        Task<string?> ShortenURLAsync(string url);
    }
}
