using System;
using System.Collections.Generic;

namespace Common.Base.Shared.ValueObjects
{
  public class Range
  {
    public static Range Create(DateTime from, DateTime to)
    {
      return new Range(from, to);
    }
    public bool Overlaps(Range dateTimeRange)
    {
      return this.From < dateTimeRange.To &&
          this.To > dateTimeRange.From;
    }
    
    public override string ToString() => $"{From:dd/MM/yy} To {To:dd/MM/yy}";

    

    public DateTime From { get; private set; }
    public DateTime To { get; private set; }
    private Range() { }
    private Range(DateTime from, DateTime to)
    {
      if (to <= from)
      {
        throw new ArgumentOutOfRangeException(nameof(Range), "Date Range is Invalid");
      }
      From = from;
      To = to;
    }

    public static bool operator ==(Range left, Range right)
    {
      return EqualityComparer<Range>.Default.Equals(left, right);
    }

    public static bool operator !=(Range left, Range right)
    {
      return !(left == right);
    }
    public override bool Equals(object obj)
    {
      return obj is Range range &&
             From == range.From &&
             To == range.To;
    }

    public override int GetHashCode()
    {
      return HashCode.Combine(From, To);
    }
  }
}
