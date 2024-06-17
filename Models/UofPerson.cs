using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace EF_WebApi.Models;

[Keyless]
public partial class UofPerson
{
    public string? WRITE_TIME { get; set; }

    public string? UOF_PEOPLE { get; set; }
}
