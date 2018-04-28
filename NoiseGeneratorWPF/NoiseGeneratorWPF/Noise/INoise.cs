using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace NoiseGeneratorWPF
{
    public interface INoise
    {
        float GetValue(Vector2 position);
    }
}
