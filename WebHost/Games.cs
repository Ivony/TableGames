using Ivony.Data;
using Ivony.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using Ivony.TableGame.SimpleGames;
using System.Text.RegularExpressions;

namespace Ivony.TableGame.WebHost
{
  public class Games
  {

    private static object _sync = new object();
    private static GameHostCollection _games = new GameHostCollection();


    private static readonly Regex nameRegex = new Regex( @"^([a-zA-Z]{1,10})$|^([\p{IsEnclosedCJKLettersandMonths}\p{IsCJKCompatibility}\p{IsCJKUnifiedIdeographsExtensionA}\p{IsCJKUnifiedIdeographs}\p{IsCJKCompatibilityIdeographs}-[\P{L}]]{1,5})$", RegexOptions.Compiled );

    /// <summary>
    /// 获取或创建一个游戏
    /// </summary>
    /// <param name="name">游戏名称</param>
    /// <returns></returns>
    public static GameHost GetOrCreateGame( string name )
    {

      if ( name == null )
        throw new ArgumentNullException( name );

      if ( nameRegex.IsMatch( name ) == false )
        throw new ArgumentException( "游戏名称必须由10个英文字母或者5个汉字组成", "name" );


      lock ( _sync )
      {
        if ( _games.Contains( name ) )
          return _games[name];

        else
        {
          var game = new GameHost( name );
          _games.Add( game );
          return game;
        }
      }
    }


    /// <summary>
    /// 释放 GameHost 资源
    /// </summary>
    /// <param name="gameHost">要释放的 GameHost</param>
    internal static void ReleaseGameHost( GameHost gameHost )
    {
      lock ( _sync )
      {
        _games.Remove( gameHost );
      }
    }


    internal static GameHost[] GetAllGames()
    {
      lock ( _sync )
      {
        return _games.ToArray();
      }
    }



  }
}