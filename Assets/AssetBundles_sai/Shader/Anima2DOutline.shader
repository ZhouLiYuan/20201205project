// Pseudo Outline Shader for SpriteMesh(Anima2D)
// based on 'Sprites-Default'

Shader "SAI/Anima2DOutline"
{
    Properties
    {
  [Enum(UnityEngine.Rendering.BlendMode)]_BlendSrc("Blend Src", int) = 5
      [Enum(UnityEngine.Rendering.BlendMode)]_BlendDst("Blend Dst", int) = 10
        
        _ColorScale("ColorScale",Range(0,1))=1
        _AlphaScale("AlphaScale",Range(0,1))=1         _MainTex("Sprite Texture", 2D) = "white" {}
	     _HeightMap("HeightMap", 2D) = "gray" {}
         _Weight("Weight",Range(0,1)) = 0

         _HeightFactor ("Height Factor", Range(0.0, 0.1)) = 0.02
         _AnimationOffset("AnimationOffset", Range(0,1)) = 0
         _AnimationSpeedScale("AnimationSpeedScale", Range(0,10)) = 2
         _AnimationDegree("AnimationDegree", Range(0,100)) = 70
      
        _Color("Tint", Color) = (1,1,1,1)
        _FadeColor("Fade Color", Color) = (0,0,0,0)
        _FadeAmount("Fade Amount", Range(0,1)) = 0
        [MaterialToggle] Outline("Outline", Float) = 0
        [HDR] _OutlineColor("Outline Color", Color) = (0,1,0,1)
        [HideInInspector] _RendererColor("RendererColor", Color) = (1,1,1,1)
        [HideInInspector] _Flip("Flip", Vector) = (1,1,1,1)
    }

    SubShader
    {
        Tags
        {
            "Queue" = "Transparent"
            "IgnoreProjector" = "True"
            "RenderType" = "Transparent"
            "PreviewType" = "Plane"
            "CanUseSpriteAtlas" = "True"
	        "LightMode"="ForwardBase"
        }

        Cull Off
        Lighting Off
        ZWrite Off
          
        Pass
        {
            Stencil{
                Ref 128
                Comp always
                Pass replace
            }
            Blend SrcAlpha OneMinusSrcAlpha

        CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment CustomFrag
            #pragma target 2.0
            #include "UnityCG.cginc"
            fixed4 _RendererColor;
            fixed2 _Flip;
            fixed4 _Color;
            float _Weight;
            sampler2D _HeightMap;
            float _HeightFactor;
            float _AnimationOffset;
            float  _AnimationSpeedScale;
            float _AnimationDegree;
            float _HitX;
            float _HitY;
            float _BrushSize;
            float _Transparent;
            fixed4 _BrushColor;
            float _AlphaScale,_ColorScale;
            #define DEG_TO_RAD( value ) 0.01744444444 * value

            struct appdata_t0
            {
              float4 vertex   : POSITION;
              float4 color    : COLOR;
              float2 texcoord : TEXCOORD0;
              float2 center : TEXCOORD1;
              float2 extents : TEXCOORD2;
              };
          
            struct v2f
            {
              float4 vertex   : SV_POSITION;
              fixed4 color    : COLOR;
              float2 texcoord : TEXCOORD0;
              float4 viewDir : TEXCOORD2;
              };

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            fixed4 SampleSpriteTexture (float2 uv)
            {
                fixed4 color = tex2D (_MainTex, uv);
                return color;
            }

          
            inline float4 UnityFlipSprite(in float3 pos, in fixed2 flip)
            {
              return float4(pos.xy * flip, pos.z, 1.0);
            }
          
            v2f SpriteVert(appdata_t0 IN)
            {
              v2f OUT;
              OUT.viewDir = 0;
              OUT.color = 0;
              OUT.vertex = UnityFlipSprite(IN.vertex, _Flip);
              OUT.vertex = UnityObjectToClipPos(OUT.vertex);
              OUT.texcoord = IN.texcoord;

              OUT.color = fixed4(1,1,1,_Color.a);
              float y = sin( _Time.z * _AnimationSpeedScale + _AnimationOffset ) * DEG_TO_RAD( _AnimationDegree );
              float x = cos( _Time.z * _AnimationSpeedScale + _AnimationOffset ) * DEG_TO_RAD( _AnimationDegree );
              //float y = lerp( 0, sin( _Time.z * _AnimationSpeedScale + _AnimationOffset ) * DEG_TO_RAD( _AnimationDegree ), 1 );
              /*float4x4 rot = float4x4( float4( cos(y), 0, sin(y), 0 ),
                                        float4( 0, 1, 0, 0 ),
                                        float4( -sin(y), 0, cos(y), 0 ),
                                        float4( 0, 0, 0, 1 ) );
              OUT.viewDir.rgb = mul(rot, ObjSpaceViewDir(IN.vertex));*/
			  OUT.viewDir.y = y * _MainTex_TexelSize.y * 2000 * _HeightFactor ;
              OUT.viewDir.x = x * _MainTex_TexelSize.x * 5000 * _HeightFactor ;

              return OUT;
            }

          
            fixed4 _FadeColor;
            float  _FadeAmount;

            fixed4 CustomFrag(v2f IN) : SV_Target
            {
              float2 hit = float2(_HitX,_HitY);
              float len = length( hit - IN.texcoord );
              float overray = 0;
              if( len < _BrushSize && hit.x >= 0 ){
                overray = (_BrushSize - len) / _BrushSize;
                overray = (1-pow( 1-overray, 3 )) * 0.7 * _Transparent;
              }
              fixed4 height = tex2D(_HeightMap, IN.texcoord);
              IN.texcoord += IN.viewDir.xy * ( height.r -0.5 );
              fixed4 c = SampleSpriteTexture(IN.texcoord) * IN.color;
              //c.a += overray;
              clip(c.a-0.1);
              c.rgb = lerp( c.rgb, _FadeColor*c.a, _FadeAmount );
              c.rgb = lerp( c.rgb, height, _Weight ).rgb;
              c.rgb *= _ColorScale;
              c.a *= _AlphaScale;
              return lerp( c*_Color, _BrushColor, overray );
            }
        ENDCG
        }


        Pass
        {
          Stencil {
            Ref 128
            Comp NotEqual
          }

          Blend SrcAlpha OneMinusSrcAlpha
    	  //Tags { "LightMode"="Always" }
          CGPROGRAM
            #pragma vertex SpriteVert
            #pragma fragment OutlineFrag
            #pragma multi_compile _ OUTLINE_ON

            #include "UnitySprites.cginc"

            fixed4 _OutlineColor;

            fixed4 OutlineFrag(v2f IN) : SV_Target
            {
#ifdef OUTLINE_ON
              _OutlineColor.a = 1;
#else
              _OutlineColor.a = 0;
#endif
                return _OutlineColor;
            }
        ENDCG
        }

    }
}