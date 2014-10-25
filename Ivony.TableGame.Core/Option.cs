using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ivony.TableGame
{


  /// <summary>
  /// 定义一个 IOption 的标准实现，仅包含所需要的内容
  /// </summary>
  [Serializable]
  public sealed class Option
  {


    /// <summary>
    /// 创建 Option 对象
    /// </summary>
    /// <param name="name"></param>
    /// <param name="description"></param>
    /// <param name="disabled"></param>
    public Option( string name, string description, bool disabled )
    {
      Name = name;
      Description = description;
      Disabled = disabled;
    }


    /// <summary>
    /// 选项名称
    /// </summary>
    public string Name { get; private set; }

    /// <summary>
    /// 选项描述
    /// </summary>
    public string Description { get; private set; }

    /// <summary>
    /// 选项是否不可选
    /// </summary>
    public bool Disabled { get; private set; }

  }



  [Serializable]
  /// <summary>
  /// 定义一个 IOption 的标准实现，除了包含所需要的内容，还包含一个指向的对象实例
  /// </summary>
  public sealed class Option<T> where T : class
  {

    /// <summary>
    /// 创建 Option 对象
    /// </summary>
    /// <param name="option"></param>
    public Option( T obj, string name, string description, bool disabled = false )
    {

      OptionItem = new Option( name, description, disabled );

      OptionObject = obj;
    }


    public Option OptionItem
    {
      get;
      private set;
    }

    /// <summary>
    /// 选项名称
    /// </summary>
    public string Name { get { return OptionItem.Name; } }

    /// <summary>
    /// 选项描述
    /// </summary>
    public string Description { get { return OptionItem.Description; } }

    /// <summary>
    /// 选项是否不可选
    /// </summary>
    public bool Disabled { get { return OptionItem.Disabled; } }



    /// <summary>
    /// 该选项所代表的对象
    /// </summary>
    public T OptionObject { get; private set; }
  }


}
