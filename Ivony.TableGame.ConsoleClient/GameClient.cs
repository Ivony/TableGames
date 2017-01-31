using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.ConsoleClient
{
  public sealed class GameClient : IDisposable
  {

    private readonly string host;
    private readonly HttpClient client;

    public GameClient( string host )
    {
      this.host = host;
      client = new HttpClient( new HttpClientHandler { CookieContainer = new CookieContainer() } );
    }

    public void Dispose()
    {
      client.Dispose();
    }

    public async Task Run()
    {

      while ( true )
      {
        var lastRequestTime = DateTime.UtcNow;


        try
        {

          var status = await GetStatus( client );

          ShowMessages( status.Messages );

          if ( status.Gaming == false )
          {
            Console.Write( "您当前尚未加入游戏，请输入要加入的游戏房间名：" );
            var name = Console.ReadLine();

            var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );
            await client.GetAsync( host + "Game?name=" + name, source.Token );
            continue;
          }


          /*
          var compatibility = ((JArray) status.Compatibility).ToObject<string[]>();
          if ( compatibility.Contains( "Console", StringComparer.OrdinalIgnoreCase ) == false )
          {


            Console.Write( "游戏不兼容控制台客户端，正在退出" );
            await QuitGame();
            continue;
          }
          */

          if ( status.RespondingUrl != null )
          {
            await Responding( host + (string) status.RespondingUrl );
            continue;
          }

          var delay = lastRequestTime.AddSeconds( 1 ) - DateTime.UtcNow;

          if ( delay > TimeSpan.Zero )
            await Task.Delay( delay );


        }
        catch ( TaskCanceledException )
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine( "连接服务器超时，尝试重新连接" );
          Console.ResetColor();
        }
        catch ( NotSupportedException )
        {
          Console.ForegroundColor = ConsoleColor.Yellow;
          Console.WriteLine( "遇到不支持的特性，正在退出游戏" );
          await QuitGame();

          Console.ResetColor();
        }
        catch ( Exception e )
        {
          Console.ForegroundColor = ConsoleColor.Red;
          Console.WriteLine( "出现异常：" );
          Console.WriteLine( e );
          Console.WriteLine( "5秒后尝试重新连接服务器" );
          await Task.Delay( TimeSpan.FromSeconds( 5 ) );
          Console.WriteLine( "尝试重新连接" );
          Console.ResetColor();
        }
      }
    }


    private void ShowMessages( dynamic messages )
    {
      foreach ( var item in messages )
      {

        switch ( (string) item.Type )
        {
          case "System":
            Console.ForegroundColor = ConsoleColor.Cyan;
            break;

          case "Warning":
            Console.ForegroundColor = ConsoleColor.Yellow;
            break;

          case "Error":
            Console.ForegroundColor = ConsoleColor.Red;
            break;

          case "SystemError":
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.BackgroundColor = ConsoleColor.DarkCyan;
            break;


        }

        Console.WriteLine( "[{0}] {1}", item.Date, item.Message );

        Console.ForegroundColor = ConsoleColor.Gray;

      }
    }

    private async Task Responding( string url )
    {

      if ( url == null )
        throw new ArgumentNullException( "url" );


      var response = await client.GetAsync( url );
      var responding = (dynamic) await response.Content.ReadAsJsonAsync();




      var type = (string) responding.Type;
      if ( type == "Options" )
        await RespondingOption( url, responding );


      else
        throw new NotSupportedException();


    }


    private static readonly char[] indexKeys = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
    private async Task RespondingOption( string url, dynamic responding )
    {

      Console.Write( responding.PromptText );
      var options = ((JArray) responding.Options).Select( ( item, index ) => new { Index = index, Name = (string) item["Name"] } ).ToArray();



      foreach ( var item in options )
        Console.Write( "{0}.{1} ", indexKeys[item.Index], item.Name );


      Console.Write( ">" );

      int optionIndex;


      Console.Beep( 1000, 500 );

      while ( true )
      {

        while ( Console.KeyAvailable )
          Console.ReadKey( true );


        do
        {
          if ( await EnsureResponding( url ) == false )
            return;

          await Task.Delay( 100 );

        } while ( Console.KeyAvailable == false );

        var key = Console.ReadKey( true );

        optionIndex = ((IList<char>) indexKeys).IndexOf( Char.ToUpperInvariant( key.KeyChar ) );
        if ( optionIndex < 0 || optionIndex >= options.Length )
        {
          Console.Beep( 3000, 200 );
          Thread.Sleep( 50 );
          Console.Beep( 3000, 200 );

          continue;
        }
        break;
      }


      var response = await SendResponding( url, optionIndex.ToString() );
      if ( response.StatusCode != HttpStatusCode.OK )
      {
        RespondingTimeout();
        return;
      }

      Console.WriteLine();
    }

    private async Task<bool> EnsureResponding( string url )
    {
      var response = await client.GetAsync( url );
      if ( response.StatusCode == HttpStatusCode.OK )
        return true;

      RespondingTimeout();
      return false;
    }

    private static void RespondingTimeout()
    {
      Console.ForegroundColor = ConsoleColor.Red;
      Console.WriteLine( "响应超时" );
      Console.ResetColor();
      Console.Beep( 3700, 700 );
    }

    private async Task<HttpResponseMessage> SendResponding( string url, string message )
    {
      var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );

      var content = new StringContent( message, Encoding.UTF8, "text/responding" );
      return await client.PostAsync( url, content, source.Token );
    }



    private async Task QuitGame()
    {
      await client.GetAsync( host + "QuitGame" );
    }



    private async Task<dynamic> GetStatus( HttpClient client )
    {
      var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );

      var response = await client.GetAsync( host, source.Token );

      if ( response.StatusCode != HttpStatusCode.OK )
        throw new Exception( "访问服务器出现错误" );

      return await response.Content.ReadAsJsonAsync();
    }

  }

}
