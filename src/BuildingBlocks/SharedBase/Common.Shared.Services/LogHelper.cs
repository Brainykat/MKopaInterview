using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Reflection;

namespace Common.Shared.Services
{
  public static class LogHelper
  {
    private static string MethodBaseToLogString(this MethodBase b)
    {
      string result = b.DeclaringType.ToString() + " => " + b.Attributes.ToString();
      var parameters = b.GetParameters();

      result += " [";
      for (int i = 1; i <= parameters.Length; i++)
      {
        result += "p" + i + " => " + parameters[i - 1].ToString();
      }
      result += "]";

      return result;
    }

    public static string GetRealException(this Exception error)
    {
      string result = "No Innner Exception";

      if (error != null && error.InnerException != null)
      {
        Exception realerror = error.InnerException;

        while (realerror.InnerException != null)
        {
          realerror = realerror.InnerException;
        }

        if (realerror != null)
        {
          result = realerror.ToString(); ;
        }
      }

      return result;
    }

    public static void LogError(this ILogger logger, Exception exception, MethodBase b)
    {
      logger.LogError($"{b.MethodBaseToLogString()} InnerExcpetion: {GetRealException(exception)}  Full Stack: { exception}"); // exception.ToString() will give everything
    }
    public static void LogInformation(this ILogger logger, MethodBase b)
    {
      logger.LogInformation(b.MethodBaseToLogString());
    }

    public static void LogInformation(this ILogger logger, string message, object[] args, MethodBase b)
    {
      logger.LogInformation(b.MethodBaseToLogString() + " :: " + message, JsonConvert.SerializeObject(args));
    }

    public static void LogTrace(this ILogger logger, string message, object[] args, MethodBase b)
    {
      logger.LogTrace(b.MethodBaseToLogString() + " :: " + message, JsonConvert.SerializeObject(args));
    }

    public static void LogDebug(this ILogger logger, string message, object[] args, MethodBase b)
    {
      logger.LogDebug(b.MethodBaseToLogString() + " :: " + message, JsonConvert.SerializeObject(args));
    }

    public static void LogWarning(this ILogger logger, string message, object[] args, MethodBase b)
    {
      logger.LogWarning(b.MethodBaseToLogString() + " :: " + message, JsonConvert.SerializeObject(args));
    }

    public static void LogCritical(this ILogger logger, string message, object[] args, MethodBase b)
    {
      logger.LogCritical(b.MethodBaseToLogString() + " :: " + message, args);
    }
  }
}
