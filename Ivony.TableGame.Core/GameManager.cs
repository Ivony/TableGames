using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{

  /// <summary>
  /// 管理系统中所有注册的游戏的管理器
  /// </summary>
  public static class GameManager
  {
    private static readonly GameFactoryCollection _collection = new GameFactoryCollection();
    private static readonly object _sync = new object();

    private static bool _initialized = false;

    /// <summary>
    /// 注册一个游戏
    /// </summary>
    /// <param name="factory">用于创建游戏实例的工厂</param>
    public static void RegisterGame( IGameFactory factory )
    {
      lock ( _sync )
      {
        if ( _collection.Contains( factory ) )
          return;

        _collection.Add( factory );
      }
    }


    /// <summary>
    /// 获取一个游戏实例
    /// </summary>
    /// <param name="name">游戏名称</param>
    /// <param name="args">游戏参数</param>
    /// <returns>游戏实例</returns>
    public static GameBase GetGame( string name, string[] args )
    {
      EnsureInitialized();

      var factory = _collection[name];
      if ( factory == null )
        return null;

      return factory.CreateGame( args );
    }

    private static void EnsureInitialized()
    {
      if ( _initialized == false )
      {
        lock ( _sync )
        {
          if ( _initialized == false )
            Initialize();
        }
      }
    }

    private static readonly Type _gameFactoryType = typeof( IGameFactory );

    /// <summary>
    /// 获取所有已经注册的游戏
    /// </summary>
    public static IGameFactory[] RegisteredGames
    {
      get
      {
        EnsureInitialized();
        return _collection.ToArray();
      }
    }

    /// <summary>
    /// 初始化游戏管理器
    /// </summary>
    /// <param name="assemblies">要寻找游戏的所有程序集</param>
    public static void Initialize( Assembly[] assemblies = null )
    {

      assemblies = assemblies ?? AppDomain.CurrentDomain.GetAssemblies();

      foreach ( var type in assemblies.SelectMany( item => item.GetCustomAttributes<GameAttribute>().Select( i => i.GameFactoryType ) ).Distinct() )
      {
        RegisterGameFactoryType( type );
      }
    }


    /// <summary>
    /// 注册一个游戏
    /// </summary>
    /// <param name="type">用于创建游戏实例的工厂类型</param>
    public static void RegisterGameFactoryType( Type type )
    {
      if ( _gameFactoryType.IsAssignableFrom( type ) == false )
        throw new InvalidOperationException( $"类型 {type.AssemblyQualifiedName} 没有实现 IGameFactory 接口，无法被注册" );

      RegisterGame( (IGameFactory) Activator.CreateInstance( type ) );
    }


    private class GameFactoryCollection : KeyedCollection<string, IGameFactory>
    {
      protected override string GetKeyForItem( IGameFactory item )
      {
        return item.GameName;
      }
    }

  }
}
