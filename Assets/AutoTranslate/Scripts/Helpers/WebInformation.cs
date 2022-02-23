using System.Net;

namespace GoodTime.Tools.Helpers
{
    public class WebInformation
    {
        public static bool IsConnectedToInternet()
        {
            using (WebClient Client = new WebClient())
            {
                try
                {
                    Client.OpenRead("http://google.com/generate_204");
                }
                catch (WebException webException)
                {
                    if (webException.Status == WebExceptionStatus.ConnectFailure || webException.Status == WebExceptionStatus.NameResolutionFailure)
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
