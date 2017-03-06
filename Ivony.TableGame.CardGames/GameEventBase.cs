using Ivony.TableGame;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.Reflection;

namespace Ivony.TableGame.CardGames
{

  /// <summary>
  /// 提供 IGameEvent 类型的基础实现
  /// </summary>
  public abstract class GameEventBase : IGameEvent
  {
    private readonly DynamicBag _data = new DynamicBag();

    /// <summary>
    /// 提供一个容器，用于储存事件数据
    /// </summary>
    public dynamic DataBag { get { return _data; } }

    /// <summary>
    /// 提供一个容器，用于储存事件数据
    /// </summary>
    public IDictionary<string, object> Data { get { return _data; } }


  }


  public sealed class DynamicBag : DynamicObject, IDictionary<string, object>
  {

    public override bool TryGetMember( GetMemberBinder binder, out object result )
    {
      result = this[binder.Name];
      return true;
    }

    public override bool TrySetMember( SetMemberBinder binder, object value )
    {
      this[binder.Name] = value;
      return true;
    }

    private readonly Dictionary<string, object> _dic = new Dictionary<string, object>();

    public object this[string key]
    {
      get
      {
        object value;
        if ( _dic.TryGetValue( key, out value ) )
          return value;

        else
          return null;
      }

      set
      {
        _dic[key] = value;
      }
    }

    public int Count
    {
      get { return _dic.Count; }
    }

    public bool IsReadOnly
    {
      get { return false; }
    }

    public ICollection<string> Keys
    {
      get { return _dic.Keys; }
    }

    public ICollection<object> Values
    {
      get { return _dic.Values; }
    }

    public void Add( KeyValuePair<string, object> item )
    {

      Add( item.Key, item.Value );
    }

    public void Add( string key, object value )
    {
      _dic.Add( key, value );
    }

    public void Clear()
    {
      _dic.Clear();
    }

    public bool Contains( KeyValuePair<string, object> item )
    {
      return _dic.Contains( item );
    }

    public bool ContainsKey( string key )
    {
      return _dic.ContainsKey( key );
    }

    public void CopyTo( KeyValuePair<string, object>[] array, int arrayIndex )
    {
      ((ICollection<KeyValuePair<string, object>>) _dic).CopyTo( array, arrayIndex );
    }

    public IEnumerator<KeyValuePair<string, object>> GetEnumerator()
    {
      return _dic.GetEnumerator();
    }

    public bool Remove( KeyValuePair<string, object> item )
    {
      return ((ICollection<KeyValuePair<string, object>>) _dic).Remove( item );
    }

    public bool Remove( string key )
    {
      return _dic.Remove( key );
    }

    public bool TryGetValue( string key, out object value )
    {
      return _dic.TryGetValue( key, out value );
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
