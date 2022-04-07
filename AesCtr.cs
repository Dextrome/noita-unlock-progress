using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Noita_UnlockAllProgress
{
    public class AesCounterMode : SymmetricAlgorithm
    {
        private readonly ulong _nonce;
        private readonly ulong _counter;
        private readonly AesManaged _aes;

        public AesCounterMode(byte[] nonce, ulong counter)
          : this(ConvertNonce(nonce), counter)
        {
        }

        public AesCounterMode(ulong nonce, ulong counter)
        {
            _aes = new AesManaged
            {
                Mode = CipherMode.ECB,
                Padding = PaddingMode.None
            };

            _nonce = nonce;
            _counter = counter;
        }

        private static ulong ConvertNonce(byte[] nonce)
        {
            if (nonce == null) throw new ArgumentNullException(nameof(nonce));
            if (nonce.Length < sizeof(ulong)) throw new ArgumentException($"{nameof(nonce)} must have at least {sizeof(ulong)} bytes");

            return BitConverter.ToUInt64(nonce);
        }

        public override ICryptoTransform CreateEncryptor(byte[] rgbKey, byte[] ignoredParameter)
        {
            return new CounterModeCryptoTransform(_aes, rgbKey, _nonce, _counter);
        }

        public override ICryptoTransform CreateDecryptor(byte[] rgbKey, byte[] ignoredParameter)
        {
            return new CounterModeCryptoTransform(_aes, rgbKey, _nonce, _counter);
        }

        public override void GenerateKey()
        {
            _aes.GenerateKey();
        }

        public override void GenerateIV()
        {
            // IV not needed in Counter Mode
        }
    }

    public class CounterModeCryptoTransform : ICryptoTransform
    {
        private readonly byte[] _nonceAndCounter;
        private readonly ICryptoTransform _counterEncryptor;
        private readonly Queue<byte> _xorMask = new Queue<byte>();
        private readonly SymmetricAlgorithm _symmetricAlgorithm;

        private ulong _counter;

        public CounterModeCryptoTransform(SymmetricAlgorithm symmetricAlgorithm, byte[] key, ulong nonce, ulong counter)
        {
            if (key == null) throw new ArgumentNullException(nameof(key));

            _symmetricAlgorithm = symmetricAlgorithm ?? throw new ArgumentNullException(nameof(symmetricAlgorithm));
            _counter = counter;
            _nonceAndCounter = new byte[16];
            BitConverter.TryWriteBytes(_nonceAndCounter, nonce);
            BitConverter.TryWriteBytes(new Span<byte>(_nonceAndCounter, sizeof(ulong), sizeof(ulong)), counter);

            var zeroIv = new byte[_symmetricAlgorithm.BlockSize / 8];
            _counterEncryptor = symmetricAlgorithm.CreateEncryptor(key, zeroIv);
        }

        public byte[] TransformFinalBlock(byte[] inputBuffer, int inputOffset, int inputCount)
        {
            var output = new byte[inputCount];
            TransformBlock(inputBuffer, inputOffset, inputCount, output, 0);
            return output;
        }

        public int TransformBlock(byte[] inputBuffer, int inputOffset, int inputCount, byte[] outputBuffer,
            int outputOffset)
        {
            for (var i = 0; i < inputCount; i++)
            {
                if (NeedMoreXorMaskBytes())
                {
                    EncryptCounterThenIncrement();
                }

                var mask = _xorMask.Dequeue();
                outputBuffer[outputOffset + i] = (byte)(inputBuffer[inputOffset + i] ^ mask);
            }

            return inputCount;
        }

        private bool NeedMoreXorMaskBytes()
        {
            return _xorMask.Count == 0;
        }

        private byte[] _counterModeBlock;
        private void EncryptCounterThenIncrement()
        {
            _counterModeBlock ??= new byte[_symmetricAlgorithm.BlockSize / 8];

            _counterEncryptor.TransformBlock(_nonceAndCounter, 0, _nonceAndCounter.Length, _counterModeBlock, 0);
            IncrementCounter();

            foreach (var b in _counterModeBlock)
            {
                _xorMask.Enqueue(b);
            }
        }

        private void IncrementCounter()
        {
            _counter++;
            var span = new Span<byte>(_nonceAndCounter, sizeof(ulong), sizeof(ulong));
            BitConverter.TryWriteBytes(span, _counter);
        }

        public int InputBlockSize => _symmetricAlgorithm.BlockSize / 8;
        public int OutputBlockSize => _symmetricAlgorithm.BlockSize / 8;
        public bool CanTransformMultipleBlocks => true;
        public bool CanReuseTransform => false;

        public void Dispose()
        {
            _counterEncryptor.Dispose();
        }
    }

    //public void ExampleUsage()
    //{
    //    var key = new byte[16];
    //    RandomNumberGenerator.Create().GetBytes(key);
    //    var nonce = new byte[8];
    //    RandomNumberGenerator.Create().GetBytes(nonce);
    //
    //    var dataToEncrypt = new byte[12345];
    //
    //    var counter = 0;
    //    using var counterMode = new AesCounterMode(nonce, counter);
    //    using var encryptor = counterMode.CreateEncryptor(key, null);
    //    using var decryptor = counterMode.CreateDecryptor(key, null);
    //
    //    var encryptedData = new byte[dataToEncrypt.Length];
    //    var bytesWritten = encryptor.TransformBlock(dataToEncrypt, 0, dataToEncrypt.Length, encryptedData, 0);
    //
    //    var decrypted = new byte[dataToEncrypt.Length];
    //    decryptor.TransformBlock(encryptedData, 0, bytesWritten, decrypted, 0);
    //
    //    //decrypted.Should().BeEquivalentTo(dataToEncrypt);
    //}
}
