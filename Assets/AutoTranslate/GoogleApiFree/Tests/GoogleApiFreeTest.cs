using GoodTime.Tools.InterfaceTranslate;
using NUnit.Framework;
using System.Collections.Generic;

public class GoogleApiFreeTest
{
    [Test]
    public void Translate_SimpleText_Hello()
    {
        GoogleApiFree googleApiFree = new GoogleApiFree();

        string result = googleApiFree.Translate("Hello", "en", "ru");

        Assert.IsNotNull(result, "Result is null");
        Assert.True(string.Equals(result, "Привет"), "No true translate");
    }

    [Test]
    public void Translate_BiggerText_Hello()
    {
        GoogleApiFree googleApiFree = new GoogleApiFree();

        string result = googleApiFree.Translate("The technology was developed for different platforms.", "en", "ru");

        Assert.IsNotNull(result, "Result is null");
        Assert.True(string.Equals(result, "Технология разрабатывалась для разных платформ."), "No true translate");
    }

    [Test]
    public void DictionaryTranslate_OneWord_Hello()
    {
        GoogleApiFree googleApiFree = new GoogleApiFree();

        Dictionary<string, string> words = new Dictionary<string, string>();
        words.Add("Hello", "Hello");

        Dictionary<string, string> result = googleApiFree.Translate(words, "en", "ru");

        Assert.IsNotNull(result, "Result is null");
        Assert.True(result.Count == words.Count, "The number of words does not match the introductory words");
        Assert.True(string.Equals(result["Hello"], "Привет"), "No true translate");
    }

    [Test]
    public void DictionaryTranslate_LotWords_HelloByeMotherFather()
    {
        GoogleApiFree googleApiFree = new GoogleApiFree();

        Dictionary<string, string> words = new Dictionary<string, string>();
        words.Add("Hello", "Hello");
        words.Add("Bye", "Bye");
        words.Add("Mother", "Mother");
        words.Add("Father", "Father");

        Dictionary<string, string> result = googleApiFree.Translate(words, "en", "ru");

        Assert.IsNotNull(result, "Result is null");
        Assert.True(result.Count == words.Count, "The number of words does not match the introductory words");
        Assert.True(string.Equals(result["Hello"], "Привет"), "No true translate (Hello)");
        Assert.True(string.Equals(result["Bye"], "Пока"), "No true translate (Bye)");
        Assert.True(string.Equals(result["Mother"], "Мать"), "No true translate (Mother)");
        Assert.True(string.Equals(result["Father"], "Отец"), "No true translate (Father)");
    }

    // A UnityTest behaves like a coroutine in Play Mode. In Edit Mode you can use
    // `yield return null;` to skip a frame.
    //[UnityTest]
    //public IEnumerator GoogleApiFreeTestWithEnumeratorPasses()
    //{
    //    // Use the Assert class to test conditions.
    //    // Use yield to skip a frame.
    //    yield return null;
    //}
}
