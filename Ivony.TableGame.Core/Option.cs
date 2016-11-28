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
  public sealed class OptionItem
  {


    /// <summary>
    /// 创建 OptionItem 对象
    /// </summary>
    /// <param name="name">选项名称</param>
    /// <param name="description">选项描述</param>
    /// <param name="disabled">是否禁用</param>
    public OptionItem( string name, string description, bool disabled )
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



  /// <summary>
  /// 定义一个 IOption 的标准实现，除了包含所需要的内容，还包含一个指定值对象的实例
  /// </summary>
  /// <typeparam name="T">值对象类型</typeparam>
  [Serializable]
  public sealed class Option<T> where T : class
  {

    /// <summary>
    /// 创建 Option 对象
    /// </summary>
    /// <param name="value">选项值</param>
    /// <param name="name">选项名称</param>
    /// <param name="description">选项描述</param>
    /// <param name="disabled">是否禁用</param>
    public Option( T value, string name, string description, bool disabled = false )
    {

      OptionItem = new OptionItem( name, description, disabled );

      OptionValue = value;
    }


    /// <summary>
    /// 选项实例
    /// </summary>
    public OptionItem OptionItem { get; }


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
    /// 该选项所代表的值对象
    /// </summary>
    public T OptionValue { get; private set; }
  }


}
