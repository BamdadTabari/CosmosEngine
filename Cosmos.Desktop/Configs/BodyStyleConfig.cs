using Cosmos.Desktop.Dtos;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cosmos.Desktop.Configs
{
    public sealed class BodyStyleConfig
    {
        public Dictionary<string, BodyStyleDto>
            Styles
        { get; set; } = [];
    }
}
