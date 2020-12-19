using System;
using System.Collections;
using System.Collections.Generic;

namespace MinimalCover
{
  /// <summary>
  /// Contain a readonly set of attribute
  /// </summary>
  public class ReadOnlySet<T> : ISet<T>, IReadOnlyCollection<T>
  {
    protected const string ReadonlySetMessage = "Readonly set does not support this method";

    protected ISet<T> m_set;

    /// <summary>
    /// Passed in set can still be update if there is a external reference to it.
    /// This constructor only stores a reference to <see cref="set"/>
    /// </summary>
    public ReadOnlySet(ISet<T> set)
    {
      m_set = set;
    }

    protected ReadOnlySet() { }

    public int Count => m_set.Count;

    public bool IsReadOnly => true;

    public bool Contains(T item) => m_set.Contains(item);

    public void CopyTo(T[] array, int arrayIndex) => m_set.CopyTo(array, arrayIndex);

    public IEnumerator<T> GetEnumerator() => m_set.GetEnumerator();

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)m_set).GetEnumerator();
    
    public bool IsProperSubsetOf(IEnumerable<T> other) => m_set.IsProperSubsetOf(other);

    public bool IsProperSupersetOf(IEnumerable<T> other) => m_set.IsProperSupersetOf(other);

    public bool IsSubsetOf(IEnumerable<T> other) => m_set.IsSubsetOf(other);

    public bool IsSupersetOf(IEnumerable<T> other) => m_set.IsSupersetOf(other);

    public bool Overlaps(IEnumerable<T> other) => m_set.Overlaps(other);

    public bool SetEquals(IEnumerable<T> other) => m_set.SetEquals(other);

    /// <summary>
    /// Method not supported.
    /// </summary>
    /// <exception cref="NotSupportedException">Method not supported for readonly set</exception>
    bool ISet<T>.Add(T item) => throw new NotSupportedException(ReadonlySetMessage);

    /// <summary>
    /// Method not supported.
    /// </summary>
    /// <exception cref="NotSupportedException">Method not supported for readonly set</exception>
    void ICollection<T>.Add(T item) => throw new NotSupportedException(ReadonlySetMessage);

    /// <summary>
    /// Method not supported.
    /// </summary>
    /// <exception cref="NotSupportedException">Method not supported for readonly set</exception>
    public void Clear() => throw new NotSupportedException(ReadonlySetMessage);

    /// <summary>
    /// Method not supported.
    /// </summary>
    /// <exception cref="NotSupportedException">Method not supported for readonly set</exception>
    public void ExceptWith(IEnumerable<T> other) => throw new NotSupportedException(ReadonlySetMessage);


    /// <summary>
    /// Method not supported.
    /// </summary>
    /// <exception cref="NotSupportedException">Method not supported for readonly set</exception>
    public void IntersectWith(IEnumerable<T> other) => throw new NotSupportedException(ReadonlySetMessage);

    /// <summary>
    /// Method not supported.
    /// </summary>
    /// <exception cref="NotSupportedException">Method not supported for readonly set</exception>
    public bool Remove(T item) => throw new NotSupportedException(ReadonlySetMessage);

    /// <summary>
    /// Method not supported.
    /// </summary>
    /// <exception cref="NotSupportedException">Method not supported for readonly set</exception>
    public void SymmetricExceptWith(IEnumerable<T> other) => throw new NotSupportedException(ReadonlySetMessage);

    /// <summary>
    /// Method not supported.
    /// </summary>
    /// <exception cref="NotSupportedException">Method not supported for readonly set</exception>
    public void UnionWith(IEnumerable<T> other) => throw new NotSupportedException(ReadonlySetMessage);

    public override string ToString() => $"{{{string.Join(',', m_set)}}}";

    public static bool operator ==(ReadOnlySet<T> a, ReadOnlySet<T> b)
    {
      if ((object) a == null || (object) b == null )
      {
        return false;
      }
      return a.Equals(b);
    }

    public static bool operator !=(ReadOnlySet<T> a, ReadOnlySet<T> b) => !(a == b);

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(this, obj))
      {
        return true;
      }
      else if (obj is IEnumerable<T>)
      {
        return SetEquals(obj as IEnumerable<T>);
      }
      return false;
    }

    public override int GetHashCode()
    {
      unchecked
      {
        int hashcode = 1430287;
        foreach (var item in m_set)
        {
          hashcode *= item.GetHashCode();
        }
        return hashcode * 17;
      }
    }

  }
}
