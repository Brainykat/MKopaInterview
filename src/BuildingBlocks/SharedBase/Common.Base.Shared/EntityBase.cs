using StackifyLib.Utils;
using System;
using System.ComponentModel.DataAnnotations;

namespace Common.Base.Shared
{
  public class EntityBase
  {
    [Key]
    public virtual Guid Id { get; set; }
    public bool IsTransient() => Id == Guid.Empty;

    /// <summary>
    /// See https://github.com/stackify/stackify-api-dotnet/blob/master/Src/StackifyLib/Utils/SequentialGuid.cs
    /// </summary>
    /// <param name="type"></param>
    public void GenerateNewIdentity()
    {
      if (IsTransient())
      {
        Id = SequentialGuid.NewGuid();
      }
    }

    public void ChangeCurrentIdentity(Guid id)
    {
      if (id != Guid.Empty)
        Id = id;
    }

    public override bool Equals(object obj)
    {
      if (!(obj is EntityBase other))
        return false;
      if (ReferenceEquals(this, other))
        return true;
      if (GetType() != other.GetType())
        return false;
      if (other.IsTransient() || IsTransient())
        return false;
      return Id == other.Id;
    }

    public static bool operator ==(EntityBase left, EntityBase right)
    {
      if (left is null && right is null)
        return true;
      if (left is null || right is null)
        return false;
      return left.Equals(right);
    }

    public static bool operator !=(EntityBase left, EntityBase right) => !(left == right);

    public override int GetHashCode() => (GetType().ToString() + Id).GetHashCode();
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    
  }
}
