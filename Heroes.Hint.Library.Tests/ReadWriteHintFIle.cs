using System;
using System.IO;
using System.Text;
using Xunit;

namespace Heroes.Hint.Library.Tests
{
    public class ReadWriteHintFile
    {
        private const string HintFileNameF = "hint001f.bin";
        private const string HintFileNameE = "hint001e.bin";

        [Fact]
        public void FromArrayFrench()
        {
            var hintFile = HintFile.FromArray(File.ReadAllBytes(HintFileNameF));
        }

        [Fact]
        public void FromArrayEnglish()
        {
            var hintFile = HintFile.FromArray(File.ReadAllBytes(HintFileNameE));
        }

        [Fact]
        public void ToArrayFrench()
        {
            byte[] originalData = File.ReadAllBytes(HintFileNameF);
            var hintFile        = HintFile.FromArray(originalData);
            byte[] newData      = hintFile.ToArray();
            Assert.Equal(originalData, newData);
        }

        [Fact]
        public void ToArrayEnglish()
        {
            byte[] originalData = File.ReadAllBytes(HintFileNameE);
            var hintFile = HintFile.FromArray(originalData);
            byte[] newData = hintFile.ToArray();
            Assert.Equal(originalData, newData);
        }
    }
}
