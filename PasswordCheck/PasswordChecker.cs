using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;

namespace PasswordCheck
{

  // Saves a password and the number of occurrences in the database.
  class PasswordCounter
  {
    public string Password {get; set;}
    public string Count {get; set;}
  }


  // Provides method for checking the database for given passwords.
  class PasswordChecker
  {
    private const string baseUrl = "https://api.pwnedpasswords.com/pwnedpassword/";


    public static async Task <string> PasswordCount (string password)
    {
      HttpWebRequest request = WebRequest.CreateHttp (baseUrl + password);
      HttpWebResponse response;

      try {
        response = await request.GetResponseAsync () as HttpWebResponse;
      }
      catch (WebException ex)
      {
        if (ex.Status == WebExceptionStatus.ProtocolError)
        {
          response = ex.Response as HttpWebResponse;
        }
        else return "Failed";
      }

      switch (response.StatusCode)
      {
      case HttpStatusCode.OK:
        Stream s = response.GetResponseStream ();
        StreamReader reader = new StreamReader (s);
        String value = reader.ReadLine ();
        Int32.TryParse (value, out int result);
        return result.ToString () + "\u00d7";
      case HttpStatusCode.NotFound:
        return "0\u00d7";
      case (HttpStatusCode) 429: // Too many requests
        return "Too Many Requests";
      default:
        return "Error " + (int) response.StatusCode;
      }
    }

  } // class PasswordChecker

}
