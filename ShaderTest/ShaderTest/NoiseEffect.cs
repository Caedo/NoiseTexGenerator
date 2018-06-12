using SharpDX.D3DCompiler;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Effects;

namespace ShaderTest
{
    class NoiseEffect : ShaderEffect
    {
        public static readonly DependencyProperty InputProperty = ShaderEffect.RegisterPixelShaderSamplerProperty("Input", typeof(NoiseEffect), 0);
        public static readonly DependencyProperty ScaleProperty = DependencyProperty.Register("Scale", typeof(double), typeof(NoiseEffect), new UIPropertyMetadata(5D, PixelShaderConstantCallback(0)));

        public Brush Input {
            get { return (Brush)GetValue(InputProperty); }
            set { SetValue(InputProperty, value); }
        }

        public double Scale {
            get {
                return ((double)(this.GetValue(ScaleProperty)));
            }
            set {
                this.SetValue(ScaleProperty, value);
            }
        }

        public NoiseEffect()
        {
            using (var shaderByteCode = ShaderBytecode.CompileFromFile("noise.hlsl", "main", "ps_3_0", ShaderFlags.Debug))
            using (var stream = new MemoryStream())
            {
                if (shaderByteCode.HasErrors)
                {
                    System.Diagnostics.Debug.WriteLine(shaderByteCode.Message);
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine(shaderByteCode.Message);

                    var data = shaderByteCode.Bytecode.Data;
                    stream.Write(data, 0, data.Length);
                    stream.Position = 0;

                    PixelShader shader = new PixelShader();
                    shader.SetStreamSource(stream);

                    this.PixelShader = shader;
                }
            }

            UpdateShaderValue(InputProperty);
            UpdateShaderValue(ScaleProperty);
        }

    }
}
