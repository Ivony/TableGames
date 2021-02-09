using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Ivony.TableGame.WebHost
{

  /// <summary>
  /// 提供一系列静态方法，管理所有的游戏宿主
  /// </summary>
  public class GameRoomsManager
  {

    private static readonly object _sync = new object();
    private static readonly GameHostCollection _games = new GameHostCollection();


    private static readonly Regex nameRegex = new Regex( @"^([a-zA-Z]{1,10})$|^([\p{IsEnclosedCJKLettersandMonths}\p{IsCJKCompatibility}\p{IsCJKUnifiedIdeographsExtensionA}\p{IsCJKUnifiedIdeographs}\p{IsCJKCompatibilityIdeographs}-[\P{L}]]{1,5})$", RegexOptions.Compiled );

    /// <summary>
    /// 获取或创建一个游戏
    /// </summary>
    /// <param name="name">游戏名称</param>
    /// <returns></returns>
    public static GameRoom GetGame( string name )
    {

      if ( name == null )
        throw new ArgumentNullException( name );


      lock ( _sync )
      {
        if ( _games.Contains( name ) )
          return _games[name];

        else
          return null;
      }
    }


    /// <summary>
    /// 创建游戏房间
    /// </summary>
    /// <param name="name">房间名称</param>
    /// <param name="privateRoom">是否为私有房间</param>
    /// <param name="owner">房间所有人</param>
    /// <returns></returns>
    public static async Task<GameRoom> CreateGame( PlayerHost owner, string name, bool privateRoom )
    {


      if ( nameRegex.IsMatch( name ) == false )
        throw new ArgumentException( "游戏名称必须由10个英文字母或者5个汉字组成", nameof( name ) );


      GameRoom room;

      lock ( _sync )
      {
        if ( _games.Contains( name ) )
          throw new InvalidOperationException( "游戏房间已经存在" );


        room = GameRoom.Create( name, privateRoom );
        _games.Add( room );
      }

      try
      {
        await room.Initialize( owner );
      }
      catch
      {
        _games.Remove( room );
        throw;
      }

      return room;

    }


    /// <summary>
    /// 释放游戏房间资源
    /// </summary>
    /// <param name="gameHost">要释放的游戏房间</param>
    internal static void ReleaseRoom( GameRoom gameHost )
    {
      lock ( _sync )
      {
        _games.Remove( gameHost );
      }
    }


    /// <summary>
    /// 获取所有的游戏房间
    /// </summary>
    /// <returns></returns>
    internal static GameRoom[] PublicGames()
    {
      lock ( _sync )
      {
        return _games.Where( item => item.GameHostState != GameHostState.Initializing ).Where( item => item.PrivateRoom == false ).ToArray();
      }
    }
  }
}