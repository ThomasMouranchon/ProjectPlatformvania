#ifndef MYHLSLINCLUDE_INCLUDED
#define MYHLSLINCLUDE_INCLUDED

void GetCrossSampleUVs_float(float4 UV, float2 TexelSize, float OffestMultiplier, out float2 UVOriginal, out float2 UVTopRight, out float2 UVBottomLeft, out float2 UVTopLeft, out float2 UVBottomRight)
{
    UVOriginal = UV;
    UVTopRight = UV.xy + float2(TexelSize.x, TexelSize.y) * OffestMultiplier;
    UVBottomLeft = UV.xy - float2(TexelSize.x, TexelSize.y) * OffestMultiplier;
    UVTopLeft = UV.xy + float2(-TexelSize.x * OffestMultiplier,
     TexelSize.y * OffestMultiplier);
    UVBottomRight = UV.xy + float2(TexelSize.x * OffestMultiplier,
     -TexelSize.y * OffestMultiplier);
}
#endif