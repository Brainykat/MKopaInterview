using System;
using System.Collections.Generic;

namespace Common.Base.Shared.ViewModels
{
  public class APIResponse
  {
    public APIResponse() { }
    public APIResponse(Guid id)
    {
      Status = true;
      Id = id;
    }
    public APIResponse(string message)
    {
      Status = false;
      Messages = new List<string> { message };
    }
    public APIResponse(string message, bool hasInternalErr = true)
    {
      Status = false;
      Messages = new List<string> { message };
      HasInternalErr = hasInternalErr;
    }
    public APIResponse(List<string> messages)
    {
      Status = false;
      Messages = messages;
    }
    public APIResponse(bool status, List<string> messages)
    {
      Status = status;
      Messages = messages;
    }
    public APIResponse(bool status, string message)
    {
      Status = status;
      Messages = new List<string> { message };
    }
    public bool Status { get; set; } = true;
    public List<string> Messages { get; set; }
    public Guid Id { get; set; } = default;
    public bool HasInternalErr { get; set; } = false;
  }
}
