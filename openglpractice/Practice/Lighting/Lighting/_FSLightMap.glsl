#version 330 core

// Define Object Color to each component
struct Material 
{
	sampler2D DiffuseColor;
	sampler2D SpecularColor;
	sampler2D EmitColor;
	float Shininess;
};

// Light color define colors
struct NewLightColor
{
	vec3 LightPos;

	vec3 AmbientColor;
	vec3 DiffuseColor;
	vec3 SpecularColor;
	vec3 EmitColor;
};

in vec3 Normal;
in vec3 FragPos;
in vec2 TexCoordinate;

uniform vec3 ViewPos;
uniform Material uMaterial;
uniform NewLightColor uLightColor;

uniform sampler2D uDiffuse;

out vec4 FragColor;

void main()
{
	vec4 image = texture(uMaterial.DiffuseColor, TexCoordinate);
	//vec4 image = texture(uDiffuse, TexCoordinate);

	//Diffuse Color
	vec3 nor = normalize (Normal);
	vec3 lightDirection = normalize (uLightColor.LightPos - FragPos);
	float diff = max (dot(nor, -lightDirection), 0.0);
	vec3 diffuse = uLightColor.DiffuseColor * diff * image.rgb;

	//Ambient Color
	vec3 ambient = uLightColor.AmbientColor * image.rgb;

	//specular color
	vec4 imagespecular = texture (uMaterial.SpecularColor, TexCoordinate);
	vec3 reflectDirection = normalize (reflect (-lightDirection, nor));
	vec3 toEye = normalize (ViewPos - FragPos);
	float spec = pow (max(dot(reflectDirection, toEye), 0.0), uMaterial.Shininess);
	vec3 specular = uLightColor.SpecularColor * spec * imagespecular.rgb;
	//vec3 specular = uLightColor.SpecularColor * spec * (1 - imagespecular.rgb); // for inverse specular 

	//emit color
	vec3 emit;
	if (imagespecular.r == 0)
	{
		vec4 imageEmit = texture(uMaterial.EmitColor, TexCoordinate + vec2 (0.0, uLightColor.EmitColor.r));
		emit = uLightColor.EmitColor * imageEmit.rgb;
	}
	else
		emit = vec3(0.0);

	//final
	vec3 result = (ambient + diffuse + specular) + emit;

	//for debug
	//if (image.r == 0 && image.g == 0 && image.b == 0)
		//result = vec3(1.0);

	FragColor = vec4 (result, 1.0);
}

