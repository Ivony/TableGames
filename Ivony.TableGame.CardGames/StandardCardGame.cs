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
  public abstract class StandardCardGame<TPlayer, TCard> : CardGame
    where TPlayer : StandardCardGamePlayer<TCard>
    where TCard : StandardCard
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
    /// 重写 PlayerPlay 方法，在每个玩家操作完毕后，检查所有玩家是否都还活着
    /// </summary>
    /// <param name="player">正在执行操作的玩家</param>
    /// <param name="token">取消标识</param>
    /// <returns>获取一个 Task 用于等待操作完成</returns>
    protected override async Task PlayerPlay( CardGamePlayer player, CancellationToken token )
    {
      await base.PlayerPlay( player, token );
      await EnsurePlayersAlive();
    }


    /// <summary>
    /// 确保所有玩家都还活着，对已经死亡的玩家调用 OnDead 方法
    /// </summary>
    /// <returns>获取一个 Task 用于等待操作完成</returns>
    public async Task EnsurePlayersAlive()
    {
      foreach ( var player in Players.ToArray() )
        await player.EnsureAlive();
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
