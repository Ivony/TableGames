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

    private Regex commandRegex = new Regex( @"^(?<command>[a-zA-Z-]+)(\s+((?<argument>[^""\s]+)|(""(?<argument>.+?)"")))*$" );


    private GameClient client;

    public GameHostConsole( GameClient client )
    {
      this.client = client;
    }


    public async Task Run()
    {

      Console.Write( ">" );
      try
      {
        await InvokeCommand( Console.ReadLine() );
      }
      catch ( SyntaxErrorException )
      {
        Help( null );
      }

    }

    private async Task InvokeCommand( string command )
    {

      var match = commandRegex.Match( command );
      if ( match.Success == false )
        throw new SyntaxErrorException();


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

      else if ( command.Equals( "create", StringComparison.OrdinalIgnoreCase ) )
        await Create( args );

      else if ( command.Equals( "exit", StringComparison.OrdinalIgnoreCase ) )
        Exit();

      else
        throw new SyntaxErrorException();
    }

    private async Task Join( string[] args )
    {
      var name = args.FirstOrDefault();
      if ( name == null )
        throw new SyntaxErrorException();

      var source = new CancellationTokenSource( new TimeSpan( 0, 0, 10 ) );
      await client.HttpClient.GetAsync( "GameRooms/Join?name=" + name, source.Token );
      await EnsureCompatibility();
    }

    private static readonly HashSet<string> supportedFeatures = new HashSet<string>( new[] { "OptionsResponding" } );

    /// <summary>
    /// 确认客户端兼容性
    /// </summary>
    /// <returns></returns>
    private async Task EnsureCompatibility()
    {

      var response = await client.HttpClient.GetAsync( "RequiredFeatures" );

      var features = (await response.Content.ReadAsJsonAsync()).ToObject<string[]>();
      if ( features == null )
        return;

      if ( supportedFeatures.IsSupersetOf( features ) )
        return;


      throw new NotSupportedException();

    }



    private async Task Create( string[] args )
    {

      var name = args.FirstOrDefault();
      if ( string.IsNullOrWhiteSpace( name ) )
        throw new SyntaxErrorException();

      var response = await client.HttpClient.GetAsync( $"GameRooms/Create?name={name}" );

    }




    private async Task List( string[] args )
    {
      var response = await client.HttpClient.GetAsync( "GameRooms/List" );
      var rooms =
        from dynamic item in (JArray) await response.Content.ReadAsJsonAsync()
        where (string) item.State == "Initialized"
        select new
        {
          Name = (string) item.Name,
          Players = (int) item.Players.Count,
        };

      if ( rooms.Any() )
      {
        foreach ( var item in rooms )
        {
          Console.Write( "{0,20}", $"{item.Name}({item.Players})" );
        }
        Console.WriteLine();
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
        var response = await client.HttpClient.GetAsync( "Player/Name" );
        name = (string) await response.Content.ReadAsJsonAsync();
        Console.ResetColor();
        Console.Write( "您当前在游戏中的昵称是：" );
        Console.ForegroundColor = ConsoleColor.White;
        Console.WriteLine( name );
        Console.ResetColor();
      }
      else
      {
        var response = await client.HttpClient.GetAsync( "Player/Name?name=" + name );
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


    private void Exit()
    {
      client.Dispose();
    }


    private class SyntaxErrorException : Exception
    {

    }

  }
}
