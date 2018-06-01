namespace NoiseGeneratorWPF
{
    public interface IBitmapRenderer
    {
        byte[] GenerateNoiseMap(NoiseData data, INoise noiseClass);
    }
}
