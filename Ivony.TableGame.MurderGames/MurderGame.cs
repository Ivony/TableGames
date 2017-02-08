using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ivony.TableGame.MurderGames
{
  public class MurderGame : GameBase
  {




    public MurderGame()
      : base()
    {

    }



    /// <summary>
    /// 重写此方法以实现杀人游戏过程
    /// </summary>
    /// <param name="token">取消标识</param>
    /// <returns></returns>
    protected override async Task RunGame( CancellationToken token )
    {

      AssignRoles();


      while ( true )
      {

        await MurderRound( token );

        await CivilianRound( token );

      }

    }

    protected virtual void AssignRoles()
    {
    }


    protected virtual Task MurderRound( CancellationToken token )
    {
      throw new NotImplementedException();
    }

    protected virtual Task CivilianRound( CancellationToken token )
    {
      throw new NotImplementedException();
    }


  }
}
