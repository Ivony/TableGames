using Ivony.TableGame;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 提供基本的卡牌游戏实现
  /// </summary>
  /// <typeparam name="TPlayer"></typeparam>
  public abstract class StandardCardGame<TPlayer> : CardGame, IBasicGame
    where TPlayer : StandardCardGamePlayer
  {

    /// <summary>
    /// 创建 StandardCardGame 对象
    /// </summary>
    /// <param name="gameHost"></param>
    public StandardCardGame( IGameHost gameHost ) : base( gameHost ) { }


    /// <summary>
    /// 获取所有玩家
    /// </summary>
    public new TPlayer[] Players
    {
      get { return PlayerCollection.Cast<TPlayer>().ToArray(); }
    }





    /// <summary>
    /// 重写 Play 方法，在每个玩家操作完毕后，检查所有玩家是否都还活着
    /// </summary>
    /// <param name="player">正在执行操作的玩家</param>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task 用于等待操作完成</returns>
    protected override async Task Play( CardGamePlayer player, CancellationToken token )
    {
      await base.Play( player, token );
      await EnsurePlayersAlive();
    }


    /// <summary>
    /// 确保所有玩家都还活着
    /// </summary>
    /// <returns>获取一个 Task 用于等待操作完成</returns>
    private async Task EnsurePlayersAlive()
    {
      foreach ( var dead in Players.Where( item => item.HealthPoint <= 0 ).ToArray() )
        await dead.Dead();
    }




    /// <summary>
    /// 广播一个游戏事件
    /// </summary>
    /// <param name="gameEvent"></param>
    /// <returns></returns>
    public virtual async Task OnGameEvent( IGameEvent gameEvent )
    {

      foreach ( var player in Players )
        await player.OnGameEvent( gameEvent );
    }






    private IEffectCollection _effects = new NotSupportEffectCollection();

    /// <summary>
    /// 获取全局效果列表
    /// </summary>
    protected virtual IEffectCollection Effects
    {
      get { return _effects; }
    }




  }
}
