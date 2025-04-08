using System;

namespace Incident.Common.Types;

public sealed class MessageInfo
{
    public Action? Action { get; set; }
    public string? Header { get; set; }
    public string? Message { get; set; }
    public bool IsModal { get; set; } = true;
}