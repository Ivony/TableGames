using Ivony.TableGame.ConsoleClient.Help;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.ConsoleClient
{
  public class GameHostConsole
  {

    private Regex commandRegex = new Regex( @"^(?<command>[a-z]+)(\s+((?<argument>[^""\s]+)|(""(?<argument>.+?)"")))*$" );

    private HttpClient client;

    public GameHostConsole( HttpClient client )
    {

      this.client = client;
    }


    public async Task Run()
    {

      Console.Write( ">" );
      await InvokeCommand( Console.ReadLine() );

    }

    private async Task InvokeCommand( string command )
    {

      var match = commandRegex.Match( command );
      if ( match.Success == false )
      {
        Help( null );
        return;
      }

      await InvokeCommand( match.Groups["command"].Value, match.Groups["argument"].Captures.Cast<Capture>().Select( item => item.Value ).ToArray() );

    }

    private async Task InvokeCommand( string command, string[] args )
    {
      if ( command.Equals( "help", StringComparison.OrdinalIgnoreCase ) )
        Help( args.FirstOrDefault() );

      else if ( command.Equals( "name", StringComparison.OrdinalIgnoreCase ) )
        await Name( args.FirstOrDefault() );

      else if ( command.Equals( "list", StringComparison.OrdinalIgnoreCase ) )
        await List( args );

      else if ( command.Equals( "join", StringComparison.OrdinalIgnoreCase ) )
        await Join( args );

      else
        Help( null );
    }

    private async Task Join( string[] args )
    {
      var name = args.FirstOrDefault();
      if ( name == null )
      {
        Help( null );
        return;
      }

      var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );
      await client.GetAsync( "Game?name=" + name, source.Token );
      await EnsureCompatibility();
    }





    private static readonly HashSet<string> supportedFeatures = new HashSet<string>( new[] { "OptionsResponding" } );

    /// <summary>
    /// 确认客户端兼容性
    /// </summary>
    /// <returns></returns>
    private async Task EnsureCompatibility()
    {

      var response = await client.GetAsync( "RequiredFeatures" );

      var features = (await response.Content.ReadAsJsonAsync()).ToObject<string[]>();
      if ( features == null )
        return;

      if ( supportedFeatures.IsSupersetOf( features ) )
        return;


      throw new NotSupportedException();

    }



    private async Task List( string[] args )
    {
      var response = await client.GetAsync( "GameRooms" );
      var rooms = from dynamic item in (JArray) await response.Content.ReadAsJsonAsync()
                  where (string) item.State == "Initialized"
                  select new
                  {
                    Name = (string) item.Name,
                    Players = (int) item.Players.Count,
                  };

      if ( rooms.Any() )
      {
        foreach ( var item in rooms )
          Console.Write( $"{item.Name}({item.Players})\t" );
      }
      else
      {
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine( "当前没有可用的游戏房间" );
        Console.ResetColor();
      }

    }


    private async Task Name( string name )
    {
      if ( name == null )
      {
        var response = await client.GetAsync( "Player/Name" );
        name = (string) await response.Content.ReadAsJsonAsync();
        Console.ResetColor();
        Console.Write( "您当前在游戏中的昵称是：" );
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine( name );
        Console.ResetColor();
      }
      else
      {
        var response = await client.GetAsync( "Player/Name?name=" + name );
        if ( response.IsSuccessStatusCode )
          Console.WriteLine( "昵称修改成功" );
        else
          Console.WriteLine( "修改昵称失败" );
      }
    }

    private void Help( string command )
    {
      Console.ResetColor();

      Console.Write( HelpManager.GetHelp( command ) );

    }
  }
}
