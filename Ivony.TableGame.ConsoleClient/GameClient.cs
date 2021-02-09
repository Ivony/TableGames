﻿using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Ivony.TableGame.ConsoleClient
{
  public sealed class GameClient : IDisposable
  {

    private readonly Uri host;
    private readonly GameHostConsole console;

    public GameClient( Uri host )
    {

      if ( host == null )
        throw new ArgumentNullException( nameof( host ) );

      if ( host.IsAbsoluteUri == false )
        throw new ArgumentException( "host 必须是一个绝对 URL", nameof( host ) );

      this.host = host;
      HttpClient = new HttpClient( new HttpClientHandler { CookieContainer = new CookieContainer() } )
      {
        BaseAddress = host
      };

      console = new GameHostConsole( this );
    }


    internal HttpClient HttpClient { get; }

    public void Dispose()
    {
      Release();
    }

    private bool _disposed = false;

    /// <summary>
    /// 释放玩家资源，退出游戏系统。
    /// </summary>
    public void Release()
    {
      if ( _disposed )
        return;

      _disposed = true;
      HttpClient.PostAsync( "Exit", new StringContent( "" ) ).Wait();
      HttpClient.Dispose();
    }


    public async Task Run()
    {


      while ( _disposed == false )
      {
        var lastRequestTime = DateTime.UtcNow;


        try
        {

          var status = await GetStatus( HttpClient );

          ShowMessages( status.Messages );


          if ( status.RespondingUrl != null )
          {
            await Responding( (string) status.RespondingUrl );
            continue;
          }


          if ( status.Gaming == false )
          {
            await console.Run();
            continue;
          }




          var delay = lastRequestTime.AddSeconds( 1 ) - DateTime.UtcNow;

          if ( delay > TimeSpan.Zero )
            await Task.Delay( delay );


        }
        catch ( ContinueRunningException )
        {
          continue;
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
          Console.WriteLine( "客户端不兼容正在进行的游戏，正在退出" );
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


      Console.WriteLine( "客户端已经退出..." );
    }




    private static void ShowMessages( dynamic messages )
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
        throw new ArgumentNullException( nameof( url ) );


      var response = await HttpClient.GetAsync( url );
      if ( response.IsSuccessStatusCode == false )
        return;

      var responding = (dynamic) await response.Content.ReadAsJsonObjectAsync();




      var type = (string) responding.Type;
      if ( type == "Options" )
        await RespondingOption( url, responding );


      else
      {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine( $"遇到错误，服务器端提供了不被识别的响应类型 \"{type}\"。" );
        throw new NotSupportedException();
      }


    }


    private static readonly char[] indexKeys = "1234567890ABCDEFGHIJKLMNOPQRSTUVWXYZ".ToArray();
    private async Task RespondingOption( string url, dynamic responding )
    {

      Console.Write( responding.PromptText );
      var options = ((JArray) responding.Options).Select( ( item, index ) => new { Index = index, Name = (string) item["Name"], Disabled = (bool) item["Disabled"] } ).ToArray();



      foreach ( var item in options )
      {
        if ( item.Disabled )
        {
          Console.ForegroundColor = ConsoleColor.DarkYellow;
          Console.Write( " .{0} ", item.Name );
        }
        else
        {
          Console.ForegroundColor = ConsoleColor.White;
          Console.Write( "{0}.{1} ", indexKeys[item.Index], item.Name );
        }
      }
      Console.ResetColor();


      Console.Write( ">" );

      int optionIndex;


      while ( Console.KeyAvailable )
        Console.ReadKey( true );
      Console.Beep( 1000, 500 );


      while ( true )
      {
        while ( Console.KeyAvailable == false )
        {
          if ( await EnsureResponding( url ) == false )
            return;

          await Task.Delay( 100 );
        };


        var key = Console.ReadKey( true );

        optionIndex = ((IList<char>) indexKeys).IndexOf( Char.ToUpperInvariant( key.KeyChar ) );
        if ( optionIndex < 0 || optionIndex >= options.Length || options[optionIndex].Disabled )
        {
          Console.Beep( 3000, 200 );
          Thread.Sleep( 50 );
          Console.Beep( 3000, 200 );

          continue;
        }
        break;
      }


      var indexKey = indexKeys[optionIndex];
      Console.Write( indexKey + "\b" );

      if ( await SendResponding( url, optionIndex.ToString() ) )
      {
        Console.WriteLine( indexKey + "." + options[optionIndex].Name );
      }
    }

    private async Task<bool> EnsureResponding( string url )
    {
      var response = await HttpClient.GetAsync( url );
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

    private async Task<bool> SendResponding( string url, string message )
    {
      var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );

      var content = new StringContent( message, Encoding.UTF8, "text/responding" );
      var response = await HttpClient.PostAsync( url, content, source.Token );

      if ( response.StatusCode != HttpStatusCode.OK )
      {
        RespondingTimeout();
        return false;
      }

      else
        return true;

    }



    public async Task QuitGame()
    {
      await HttpClient.GetAsync( "QuitGame" );
    }






    private static async Task<dynamic> GetStatus( HttpClient client )
    {
      var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );

      var response = await client.GetAsync( "", source.Token );

      if ( response.StatusCode != HttpStatusCode.OK )
        throw new Exception( "访问服务器出现错误" );

      return await response.Content.ReadAsJsonObjectAsync();
    }




    private class ContinueRunningException : Exception
    {
    }
  }
}
