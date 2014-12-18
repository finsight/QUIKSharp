using System;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Security.Cryptography;
using System.Text;

namespace QuikSharp.Starter
{
    internal static class BytesUtilities
    {


        #region Compression
        public static string CompressToBase64(this string input)
        {

            return Convert.ToBase64String(input.CompressStringToBytes());

        }


        public static string DeCompressFromBase64(this string input)
        {

            return Convert.FromBase64String(input).DeCompressFromBytesToString();

        }


        public static byte[] CompressStringToBytes(this string input)
        {
            var bytes = Encoding.UTF8.GetBytes(input);
            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var compress = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        StreamExtentions.CopyTo(inStream, compress);
                        //compress.Write(_bytes, 0, _bytes.Length);
                    }
                    return outStream.ToArray();

                }
            }
        }


        public static string DeCompressFromBytesToString(this byte[] input)
        {
            var bytes = input;
            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var deCompress = new GZipStream(inStream, CompressionMode.Decompress))
                    {
                        StreamExtentions.CopyTo(deCompress, outStream);
                    }
                    return Encoding.UTF8.GetString(outStream.ToArray());
                }
            }
        }

        public static byte[] CompressBytesToBytes(this byte[] input)
        {
            var bytes = input;
            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var compress = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        StreamExtentions.CopyTo(inStream, compress);
                        //compress.Write(_bytes, 0, _bytes.Length);
                    }
                    return outStream.ToArray();

                }
            }
        }

        public static byte[] CompressStreamToBytes(this Stream input)
        {
            using (var inStream = new MemoryStream())
            {
                StreamExtentions.CopyTo(input, inStream);
                using (var outStream = new MemoryStream())
                {
                    using (var compress = new GZipStream(outStream, CompressionMode.Compress))
                    {
                        StreamExtentions.CopyTo(inStream, compress);
                        //compress.Write(_bytes, 0, _bytes.Length);
                    }
                    return outStream.ToArray();
                }
            }
        }


        public static byte[] DeCompressFromBytesToBytes(this byte[] input)
        {
            var bytes = input;
            using (var inStream = new MemoryStream(bytes))
            {
                using (var outStream = new MemoryStream())
                {
                    using (var deCompress = new GZipStream(inStream, CompressionMode.Decompress))
                    {
                        StreamExtentions.CopyTo(deCompress, outStream);
                    }
                    return outStream.ToArray();
                }
            }
        }


        #endregion


        #region Encryption
        //TODO:I compress stream before encryption

        /// <summary>
        /// Compresses and encrypts compressed content with given keys
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] Encrypt(this byte[] data, byte[] key, byte[] iv)
        {
            if (key.Length == 16 && iv.Length == 16)
            {
                using (var algorithm = Aes.Create())
                {
                    using (var inStream = new MemoryStream(data))
                    using (var outStream = new MemoryStream())
                    {
                        using (var encryptor = algorithm.CreateEncryptor(key, iv))
                        using (Stream crypt = new CryptoStream(outStream, encryptor, CryptoStreamMode.Write))
                        using (var compress = new GZipStream(crypt, CompressionMode.Compress))
                            StreamExtentions.CopyTo(inStream, compress);
                        return outStream.ToArray();
                    }
                }
            }
            else
            {
                Debug.Fail("Invalid key length in Ecrypt");
                return null;
            }
        }


        /// <summary>
        /// Decrypts and decompresses bytes array
        /// </summary>
        /// <param name="data"></param>
        /// <param name="key"></param>
        /// <param name="iv"></param>
        /// <returns></returns>
        public static byte[] Decrypt(this byte[] data, byte[] key, byte[] iv)
        {
            if (key.Length == 16 && iv.Length == 16)
            {
                using (var algorithm = Aes.Create())
                {
                    using (var inStream = new MemoryStream(data))
                    using (var outStream = new MemoryStream())
                    {
                        using (var decryptor = algorithm.CreateDecryptor(key, iv))
                        using (Stream crypt = new CryptoStream(inStream, decryptor, CryptoStreamMode.Read))
                        using (var deCompress = new GZipStream(crypt, CompressionMode.Decompress))
                            StreamExtentions.CopyTo(deCompress, outStream);
                        return outStream.ToArray();
                    }
                }
            }
            else
            {
                Debug.Fail("Invalid key length in Decrypt");
                return null;
            }
        }


        public static string Encrypt(this string data, byte[] key, byte[] iv)
        {
            return Convert.ToBase64String(
            Encrypt(Encoding.UTF8.GetBytes(data), key, iv));
        }

        public static string Decrypt(this string data, byte[] key, byte[] iv)
        {
            return Encoding.UTF8.GetString(
            Decrypt(Convert.FromBase64String(data), key, iv));
        }

        public static string Encrypt(this string data, byte[] key)
        {
            var iv = key;
            return Convert.ToBase64String(
            Encrypt(Encoding.UTF8.GetBytes(data), key, iv));
        }

        public static string Decrypt(this string data, byte[] key)
        {
            var iv = key;
            return Encoding.UTF8.GetString(Decrypt(Convert.FromBase64String(data), key, iv));
        }

        public static string Encrypt(this string data, string asciiKeyText)
        {
            var key = Encoding.ASCII.GetBytes(asciiKeyText);
            var iv = key;
            return Convert.ToBase64String(
                Encrypt(Encoding.UTF8.GetBytes(data), key, iv)
                );
        }

        public static string Decrypt(this string data, string asciiKeyText)
        {
            var key = Encoding.ASCII.GetBytes(asciiKeyText);
            var iv = key;
            return Encoding.UTF8.GetString(
                Decrypt(Convert.FromBase64String(data), key, iv)
                );
        }

        #endregion



        #region Hash


        public static string GetStringHash(this string input)
        {
            var byteArray = Encoding.ASCII.GetBytes(input);
            SHA1 sha = new SHA1CryptoServiceProvider();
            var hashedPasswordBytes = sha.ComputeHash(byteArray);
            return BitConverter.ToString(hashedPasswordBytes).ToLower().Replace("-", "");
        }

        public static byte[] GetBytesHash(this string input)
        {
            var byteArray = Encoding.ASCII.GetBytes(input);
            SHA1 sha = new SHA1CryptoServiceProvider();
            var hashedPasswordBytes = sha.ComputeHash(byteArray);
            return hashedPasswordBytes;
        }

        #endregion



        #region Serialization
        public static byte[] Serialize<T>(T obj)
        {
            // TODO catch serialization ex
            IFormatter formatter = new BinaryFormatter();
            byte[] data;
            using (var stream = new MemoryStream())
            {
                formatter.Serialize(stream, obj);
                data = stream.ToArray();
            }
            return data;
        }


        public static T Deserialize<T>(this byte[] data)
        {
            // deserialize 

            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomainAssemblyResolve;

            IFormatter formatter = new BinaryFormatter();
            //formatter.Binder = new AllowAllAssemblyVersionsDeserializationBinder();
            //formatter.Binder.BindToType("", typeof(T).Name);

            var obj = (T)formatter.Deserialize(new MemoryStream(data));

            AppDomain.CurrentDomain.AssemblyResolve -= CurrentDomainAssemblyResolve;

            return obj;

        }


        static Assembly CurrentDomainAssemblyResolve(this object sender, ResolveEventArgs args)
        {
            try
            {
                var assembly = Assembly.Load(args.Name);
                if (assembly != null)
                    return assembly;
            }
            catch { ;}

            return Assembly.GetExecutingAssembly();
        }

        #endregion

    }

    internal static class StreamExtentions
    {
        // Only useful before .NET 4
        public static void CopyTo(this Stream input, Stream output)
        {
            var buffer = new byte[16 * 1024]; // Fairly arbitrary size
            int bytesRead;

            while ((bytesRead = input.Read(buffer, 0, buffer.Length)) > 0)
            {
                output.Write(buffer, 0, bytesRead);
            }
        }
    }

}
