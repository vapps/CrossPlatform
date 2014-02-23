using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace CrossPlatform.Infrastructure.StoreApp.Commons
{
    class StreamUtils
    {
        public async static Task<byte[]> getBytes(Stream stream)
        {
            IInputStream inputStream = stream.AsInputStream();
            DataReader dataReader = new DataReader(inputStream);

            await dataReader.LoadAsync((uint)stream.Length);
            byte[] buffer = new byte[(int)stream.Length];

            dataReader.ReadBytes(buffer);

            return buffer;
        }
    }
}
