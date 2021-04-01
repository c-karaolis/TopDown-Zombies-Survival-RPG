#ifndef EROSION_LIB_INCLUDED
#define EROSION_LIB_INCLUDED

int To1DIndex(int x, int z, int width)
{
	return z * width + x;
}

float4 SampleTextureBilinear(Texture2D<float4> buffer, float width, float height, float2 uv)
{
	float4 value = 0;
	float2 pixelCoord = float2(lerp(0, width - 1, uv.x), lerp(0, height - 1, uv.y));
	//apply a bilinear filter
	int xFloor = floor(pixelCoord.x);
	int xCeil = ceil(pixelCoord.x);
	int yFloor = floor(pixelCoord.y);
	int yCeil = ceil(pixelCoord.y);

	float4 f00 = buffer[uint2(xFloor, yFloor)];
	float4 f01 = buffer[uint2(xFloor, yCeil)];


	float4 f10 = buffer[uint2(xCeil, yFloor)];
	float4 f11 = buffer[uint2(xCeil, yCeil)];

	float2 unitCoord = float2(pixelCoord.x - xFloor, pixelCoord.y - yFloor);

	value = f00 * (1 - unitCoord.x) * (1 - unitCoord.y) + f01 * (1 - unitCoord.x) * unitCoord.y + f10 * unitCoord.x * (1 - unitCoord.y) + f11 * unitCoord.x * unitCoord.y;

	return value;
}

float DecodeFloatRG(float2 enc)
{
	float2 kDecodeDot = float2(1.0, 1 / 255.0);
	return dot(enc, kDecodeDot);
}

float RandomValue (float2 uv)
{
    return frac(sin(dot(uv, float2(12.9898, 78.233)))*43758.5453);
}

#endif //GRIFFIN_GRASS_COMMON_INCLUDED