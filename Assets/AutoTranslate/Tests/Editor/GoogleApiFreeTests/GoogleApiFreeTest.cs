using GoodTime.Tools.InterfaceTranslate;
using NUnit.Framework;

namespace GoodTime.HernetsMaksym.AutoTranslate.Editor.Tests
{
	public class GoogleApiFreeTest
    {
        [Test]
        public void JustDoIt_Translate()
		{
            string text = "apple";
            GoogleApiFree googleApiFree = new GoogleApiFree();
            string result = "apple";
            Assert.AreNotEqual(text, result, "bad");
        }
    }
}