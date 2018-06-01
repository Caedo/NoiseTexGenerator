using System.Numerics;

namespace NoiseGeneratorWPF
{

    public interface INoise
    {
        float GetValue(Vector2 position);
    }
}
