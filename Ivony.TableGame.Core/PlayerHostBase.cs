﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 提供 IPlayerHost 的基本实现
  /// </summary>
  public abstract class PlayerHostBase : IPlayerHost
  {


    /// <summary>
    /// 构造 PlayerHostBase 对象
    /// </summary>
    /// <param name="playerName">玩家名称</param>
    protected PlayerHostBase( string playerName )
    {
      Name = playerName;
      ID = Guid.NewGuid();
    }



    /// <summary>
    /// 获取玩家唯一ID
    /// </summary>
    public Guid ID { get; private set; }


    /// <summary>
    /// 获取玩家名称
    /// </summary>
    public string Name { get; protected set; }



    /// <summary>
    /// 获取用于同步的对象
    /// </summary>
    public object SyncRoot { get; } = new object();



    private PlayerConsoleBase _console;

    /// <summary>
    /// 获取玩家控制台，用于与玩家客户端通信
    /// </summary>
    public virtual PlayerConsoleBase Console
    {
      get
      {
        lock ( SyncRoot )
        {
          if ( _console == null )
            _console = CreatePlayerConsole();
        }

        return _console;
      }

      protected set
      {
        lock ( SyncRoot )
        {
          _console = value;
        }
      }
    }


    /// <summary>
    /// 派生类实现此方法创建玩家控制台
    /// </summary>
    /// <returns>玩家控制台对象</returns>
    protected virtual PlayerConsoleBase CreatePlayerConsole()
    {
      throw new InvalidOperationException();
    }

    /// <summary>
    /// 当玩家加入一个游戏时，调用此方法通知
    /// </summary>
    /// <param name="player">游戏中的玩家对象</param>
    public virtual void OnJoinedGame( GamePlayerBase player )
    {
      lock ( SyncRoot )
      {
        if ( Player != null )
          throw new InvalidOperationException( "玩家当前已经在另一个游戏，无法加入游戏" );

        _player = player;
      }
    }



    /// <summary>
    /// 退出当前游戏
    /// </summary>
    public virtual void QuitGame()
    {
      lock ( SyncRoot )
      {

        if ( Player == null )
          return;

        Player.QuitGame();
        _player = null;
      }
    }




    private GamePlayerBase _player;
    /// <summary>
    /// 获取当前在游戏的玩家对象（如果有的话）
    /// </summary>
    /// <returns>玩家对象</returns>
    public virtual GamePlayerBase Player
    {
      get
      {
        if ( _player == null )
          return null;

        else if ( _player.PlayerHost != this )
          return _player = null;

        else
          return _player;
      }
    }

  }
}
