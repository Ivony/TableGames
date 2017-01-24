using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using System.Web;

namespace Ivony.TableGame.WebHost
{
  public static class PlayerNameManager
  {


    static PlayerNameManager()
    {
      var type = typeof( PlayerNameManager );
      using ( var reader = new StreamReader( Assembly.GetAssembly( type ).GetManifestResourceStream( type, "FirstNames.txt" ) ) )
      {
        firstNames = Regex.Split( reader.ReadToEnd(), "\r?\n" );
      }
      using ( var reader = new StreamReader( Assembly.GetAssembly( type ).GetManifestResourceStream( type, "LastNames.txt" ) ) )
      {
        lastNames = Regex.Split( reader.ReadToEnd(), "\r?\n" );
      }
    }


    private static readonly string[] lastNames;
    private static readonly string[] firstNames;

    private static readonly object sync = new object();
    private static readonly Regex playerNameRegex = new Regex( @"^([a-zA-Z]{3,10})$|^([\p{IsEnclosedCJKLettersandMonths}\p{IsCJKCompatibility}\p{IsCJKUnifiedIdeographsExtensionA}\p{IsCJKUnifiedIdeographs}\p{IsCJKCompatibilityIdeographs}-[\P{L}]]{2,5})$", RegexOptions.Compiled );
    private static HashSet<string> playerNames = new HashSet<string>();

    public static string CreateName()
    {

      string name;
      int time = 0;

      do
      {
        name = lastNames.RandomItem() + firstNames.RandomItem();

        if ( time++ > 50 )
          throw new InvalidOperationException( "无法找到一个合适的昵称" );

      } while ( playerNames.Contains( name ) );

      return name;
    }


    public static bool RemoveName( string name )
    {
      return playerNames.Remove( name );
    }


    public static bool ValidName( string name )
    {
      return playerNameRegex.IsMatch( name );
    }

    public static bool CanReset( string oldName, string newName )
    {
      lock ( sync )
      {

        if ( playerNames.Contains( newName ) )
          return false;

        if ( !playerNameRegex.IsMatch( newName ) )
          return false;

        playerNames.Remove( oldName );
        playerNames.Add( newName );
        return true;
      }
    }
  }
}