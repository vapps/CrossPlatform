using CrossPlatform.Infrastructure.Commons;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using Windows.Security.Cryptography;
using Windows.Security.Cryptography.Core;
using Windows.Storage.Streams;

namespace CrossPlatform.Infrastructure.StoreApp.Commons
{
    class Cipher
    {
        //PKCS#7 block padding modes:
        private static string ALGORITHM_NAME = SymmetricAlgorithmNames.AesCbcPkcs7;
        private const string KEY = "!@#$%^&*()<>sgfdlkadrewq?:";
        private const int SIZE_IV = 16;
        private const string EUC_KR = "euc-kr";

        private static CryptographicKey getKey(SymmetricKeyAlgorithmProvider Algorithm)
        {
            IBuffer keymaterial = CryptographicBuffer.CreateFromByteArray(ByteUtils.getBytes(KEY.Substring(0, SIZE_IV)));

            CryptographicKey _key = null;
            try
            {
                _key = Algorithm.CreateSymmetricKey(keymaterial);
            }
            catch (ArgumentException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return _key;
        }

        //for XML Data
        public async static Task<Stream> encrypt(Stream stream)
        {
            byte[] _buf = await StreamUtils.getBytes(stream);

            return new MemoryStream(encrypt(_buf)).AsInputStream().AsStreamForRead();
        }

        //for XML Data
        public async static Task<Stream> decrypt(Stream stream)
        {
            byte[] _buf = await StreamUtils.getBytes(stream);

            //if (XmlDataManager.isEncData(_buf))
            //{
            //    return new MemoryStream(decrypt(_buf)).AsInputStream().AsStreamForRead();
            //}
            //else
            //{
            //    stream.Position = 0;
            //    return stream;
            //}
            stream.Position = 0;
            return stream;
        }

        public static string encrypt(string input)
        {
            return Convert.ToBase64String(encrypt(ByteUtils.getBytes(input)));
        }

        public static string decrypt(string input)
        {
            return ByteUtils.getString(decrypt(Convert.FromBase64String(input)));
        }

        public static byte[] encrypt(byte[] input)
        {
            return crypto(input, CryptoCommand.ENCRYPT);
        }

        public static byte[] decrypt(byte[] input)
        {
            return crypto(input, CryptoCommand.DECRYPT);
        }

        private static byte[] crypto(byte[] input, CryptoCommand cmd)
        {
            if (input == null || input.Length == 0) return input;

            IBuffer _iv = null;
            IBuffer _src = null;
            IBuffer _dest = null;
            IBuffer _fullData = null;

            SymmetricKeyAlgorithmProvider _algorithm = SymmetricKeyAlgorithmProvider.OpenAlgorithm(ALGORITHM_NAME);

            CryptographicKey _key = getKey(_algorithm);

            try
            {
                switch (cmd)
                {
                    case CryptoCommand.ENCRYPT:
                        if (ALGORITHM_NAME.Contains("CBC"))
                            _iv = CryptographicBuffer.GenerateRandom(_algorithm.BlockLength);

                        _src = CryptographicBuffer.CreateFromByteArray(input);
                        _dest = Windows.Security.Cryptography.Core.CryptographicEngine.Encrypt(_key, _src, _iv);

                        byte[] _encFull = new byte[_iv.Length + _dest.Length];

                        System.Array.Copy(WindowsRuntimeBufferExtensions.ToArray(_iv), 0, _encFull, 0, (int)_iv.Length);
                        System.Array.Copy(WindowsRuntimeBufferExtensions.ToArray(_dest), 0, _encFull, (int)_iv.Length, (int)_dest.Length);

                        _fullData = CryptographicBuffer.CreateFromByteArray(_encFull);

                        break;

                    case CryptoCommand.DECRYPT:
                        byte[] _sepIv = new byte[_algorithm.BlockLength];
                        byte[] _sepEnc = new byte[input.Length - _sepIv.Length];

                        System.Array.Copy(input, 0, _sepIv, 0, _sepIv.Length);
                        System.Array.Copy(input, _sepIv.Length, _sepEnc, 0, _sepEnc.Length);

                        if (ALGORITHM_NAME.Contains("CBC"))
                            _iv = CryptographicBuffer.CreateFromByteArray(_sepIv);

                        _src = CryptographicBuffer.CreateFromByteArray(_sepEnc);

                        _dest = Windows.Security.Cryptography.Core.CryptographicEngine.Decrypt(_key, _src, _iv);

                        _fullData = _dest;
                        break;
                }
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e.Message);
                _fullData = CryptographicBuffer.CreateFromByteArray(input);
            }

            return WindowsRuntimeBufferExtensions.ToArray(_fullData);
        }

        public static string getMD5(string input)
        {
            String algName = "MD5";

            // Create a HashAlgorithmProvider object.
            HashAlgorithmProvider Algorithm = HashAlgorithmProvider.OpenAlgorithm(algName);
            //IBuffer vector = CryptographicBuffer.DecodeFromBase64String(input);
            IBuffer vector = CryptographicBuffer.CreateFromByteArray(System.Text.Encoding.GetEncoding(EUC_KR).GetBytes(input));

            // Compute the hash in one call.
            IBuffer digest = Algorithm.HashData(vector);

            return CryptographicBuffer.EncodeToHexString(digest);
        }

        public static byte[] encodeBase64(byte[] input)
        {
            return ByteUtils.getBytes(System.Convert.ToBase64String(input));
        }

        public static byte[] decodeBase64(byte[] input)
        {
            return System.Convert.FromBase64String(ByteUtils.getString(input));
        }
    }
}
