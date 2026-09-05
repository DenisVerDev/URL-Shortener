
using SimpleBase;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using URL_Shortener.Data.Repositories;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace URL_Shortener.Services.ShorteningAlgorithms
{
    public class URLShortenSHA256 (IURLsRepository _urlR, IConfiguration _c) : IURLShortenAlgorithm
    {
        public virtual async Task<string?> ShortenURLAsync(string url)
        {
            byte[] hash = HashURL(url);
            string hashBase62 = Base62.Default.Encode(hash);

            int shortUrlIdLength = _c.GetValue<int>("ShorteningAlgorithms:URLShortenSHA256:Length");

            for (int i = 0; i < hashBase62.Length - shortUrlIdLength; i += shortUrlIdLength)
            {
                string shortUrlId = hashBase62.Substring(i, shortUrlIdLength);

                if (await _urlR.AnyURLAsync(u => u.ShortURLId == shortUrlId))
                    continue;

                return shortUrlId;
            }

            return null;
        }

        public byte[] HashURL(string url)
        {
            var data = Encoding.UTF8.GetBytes(url);          
            return SHA256.HashData(data);
        }
    }
}
