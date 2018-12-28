float4x4 World;
float4x4 View;
float4x4 Projection;

// TODO: add effect parameters here.
Texture TextureImage;

///////////////////////////////////////////////////////////////////
// Texture Sampler
///////////////////////////////////////////////////////////////////
sampler ColoredTextureSampler = sampler_state
{
	texture = <TextureImage>;
	magfilter = LINEAR;
	minfilter = LINEAR;
	mipfilter = LINEAR;
	AddressU = clamp;
	AddressV = clamp;
};

///////////////////////////////////////////////////////////////////
// Shader Structs
///////////////////////////////////////////////////////////////////
struct VertexIn
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;
};

struct VertexOut
{
	float4 Position : POSITION;
	float2 TexCoords : TEXCOORD0;
};

///////////////////////////////////////////////////////////////////
// Texture Shader
///////////////////////////////////////////////////////////////////
VertexOut VertexShaderFunction(VertexIn input)
{
	VertexOut output = (VertexOut)0;
	float4 worldPosition = mul(input.Position, World);
	float4 viewPosition = mul(worldPosition, View);
	output.Position = mul(viewPosition, Projection);
	output.TexCoords = input.TexCoords;

	return output;
}

float4 PixelShaderFunction(VertexOut input) : COLOR0
{
	float4 output = tex2D(ColoredTextureSampler, input.TexCoords);
	return output;
}

technique Technique1
{
	pass Pass0
	{
		VertexShader = compile vs_2_0 VertexShaderFunction();
		PixelShader = compile ps_2_0 PixelShaderFunction();
	}
}
