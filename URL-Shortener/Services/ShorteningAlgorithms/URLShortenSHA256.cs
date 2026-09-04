
using SimpleBase;
using System.Runtime.Intrinsics.Arm;
using System.Security.Cryptography;
using System.Text;
using URL_Shortener.Data.Repositories;

namespace URL_Shortener.Services.ShorteningAlgorithms
{
    public class URLShortenSHA256 (IURLsRepository _urlR) : IURLShortenAlgorithm
    {
        public virtual async Task<string?> ShortenURLAsync(string url)
        {
            string hash = HashURL(url);

            for(int i = 0; i < 64; i += 4)
            {
                string hash4 = hash.Substring(i, 4);
                string shortURLId = EncodeHash(hash4);

                if (await _urlR.AnyURLAsync(u => u.ShortURLId == shortURLId))
                    continue;

                return shortURLId;
            }

            return null;
        }

        public string HashURL(string url)
        {
            var data = Encoding.ASCII.GetBytes(url);
            var sha256 = SHA256.HashData(data);
            
            return Convert.ToHexStringLower(sha256);
        }

        private string EncodeHash(string hash)
        {
            byte[] data = Encoding.ASCII.GetBytes(hash);
            return Base62.Default.Encode(data);
        }
    }
}
