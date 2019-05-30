#version 330 core

out vec4 FragColor;

// Define Object Color to each component
struct Material 
{
	sampler2D DiffuseColor;
	sampler2D SpecularColor;
	float Shininess;
};

// Light color define colors
struct NewLightColor
{
	vec3 LightPos;

	vec3 AmbientColor;
	vec3 DiffuseColor;
	vec3 SpecularColor;

	float KConstant;
	float KLinear;
	float KQuadratic;
};

in vec3 FragPos;
in vec3 Normal;
in vec2 TexCoordinate;

uniform Material uMaterial;
uniform NewLightColor uLightColor;
uniform vec3 ViewPos;

void main ()
{
	vec4 image = texture(uMaterial.DiffuseColor, TexCoordinate);
	float distance = length (uLightColor.LightPos - FragPos);

	//Diffuse Color
	vec3 nor = normalize (Normal);
	vec3 lightDirection = normalize (uLightColor.LightPos - FragPos);
	float diff = max (dot(nor, lightDirection), 0.0);
	vec3 diffuse = uLightColor.DiffuseColor * diff * image.rgb;

	//Ambient Color
	vec3 ambient = uLightColor.AmbientColor * image.rgb;

	//specular color
	vec4 imagespecular = texture (uMaterial.SpecularColor, TexCoordinate);
	vec3 reflectDirection = normalize (reflect (-lightDirection, nor));
	vec3 toEye = normalize (ViewPos - FragPos);
	float spec = pow (max(dot(reflectDirection, toEye), 0.0), uMaterial.Shininess);
	//vec3 specular = uLightColor.SpecularColor * spec * imagespecular.rgb;
	vec3 specular = uLightColor.SpecularColor * spec * (imagespecular.rgb); // for inverse specular 

	//light point attenuation
	float strength = 1 / (uLightColor.KConstant + uLightColor.KLinear * distance + uLightColor.KQuadratic * (distance * distance));

	//final
	vec3 result = (ambient + diffuse + specular) * strength * 1.5f;
	FragColor = vec4 (result, 1.0);
}