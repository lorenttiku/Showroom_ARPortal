Shader "Custom/PortalWindow"
{
	Subshader
{
	ZWrite off
	ColorMask 0
	Cull off

	Stencil{
		Ref 1
		Pass replace
	}
	
	

		Pass
		{
		}
	}
}
