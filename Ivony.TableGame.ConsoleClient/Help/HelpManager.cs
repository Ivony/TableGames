using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame.ConsoleClient.Help
{
  public static class HelpManager
  {

    private static readonly Dictionary<string, string> documents = new Dictionary<string, string>( StringComparer.OrdinalIgnoreCase );

    private static readonly object _sync = new object();

    public static string GetHelp( string command = null )
    {
      if ( command == null )
        return GetHelp( "help" );

      string result;
      lock ( _sync )
      {

        if ( documents.TryGetValue( command, out result ) )
          return result;

        var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream( typeof( HelpManager ), command + ".txt" );
        if ( stream == null )
          return GetHelp( "help" );

        using ( var reader = new StreamReader( stream, Encoding.UTF8 ) )
        {
          result = reader.ReadToEnd();
        }

        return documents[command] = result;

      }
    }
  }
}
