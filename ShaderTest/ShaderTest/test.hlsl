sampler2D implicitInputSampler : register(S0);
float scale : register(C0);


float4 main(float2 uv : TEXCOORD) : COLOR
{
	return float4(uv,1,1);
}


