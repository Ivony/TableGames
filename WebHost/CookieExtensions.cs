using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Web;
using System.Net.Http;

namespace Ivony.TableGame.WebHost
{
  public static class CookieExtensions
  {

    public static string GetCookieValue( this HttpRequestHeaders requestHeaders, string cookieName )
    {

      var cookie = requestHeaders.GetCookies( cookieName ).FirstOrDefault();
      if ( cookie == null )
        return null;

      return cookie.Cookies.Select( item => item.Value ).FirstOrDefault();
    }


    public static void SetCookieValue( this HttpResponseHeaders responseHeaders, string cookieName, string cookieValue, string path = "/" )
    {

      var cookie = new CookieHeaderValue( cookieName, cookieValue )
      {
        Path = path,
      };

      responseHeaders.AddCookies( new[] { cookie } );
    }

  }
}