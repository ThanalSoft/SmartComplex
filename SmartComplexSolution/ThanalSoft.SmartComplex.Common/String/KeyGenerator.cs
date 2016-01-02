using System.Security.Cryptography;
using System.Text;

namespace ThanalSoft.SmartComplex.Common.String
{
    public class KeyGenerator
    {
        public static string GetUniqueKey(int pMaxSize)
        {
            char[] chars = new char[62];
            chars =
            "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];
            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[pMaxSize];
                crypto.GetNonZeroBytes(data);
            }
            StringBuilder result = new StringBuilder(pMaxSize);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }
            return result.ToString();
        }
    }
}
