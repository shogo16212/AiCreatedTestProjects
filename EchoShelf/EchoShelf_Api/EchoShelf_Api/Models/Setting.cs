using System;
using System.Collections.Generic;

namespace EchoShelf_Api.Models;

public partial class Setting
{
    public int SettingId { get; set; }

    public string SettingKey { get; set; } = null!;

    public string SettingValue { get; set; } = null!;

    public string? Description { get; set; }
}
