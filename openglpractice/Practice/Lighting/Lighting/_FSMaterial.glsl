#version 330 core

// Define Object Color to each component
struct Material 
{
	vec3 AmbientColor;
	vec3 DiffuseColor;
	vec3 SpecularColor;
	float Shininess;
};

// Light color define colors
struct NewLightColor
{
	vec3 LightPos;

	vec3 AmbientColor;
	vec3 DiffuseColor;
	vec3 SpecularColor;
};

uniform vec3 ViewPos;


uniform Material uMaterial;
uniform NewLightColor uLightColor;

in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

void main()
{
	//Ambient Color
	vec3 ambient = uLightColor.AmbientColor * uMaterial.AmbientColor;

	//Diffuse Color
	vec3 nor = normalize (Normal);
	vec3 lightDirection = normalize (uLightColor.LightPos - FragPos);
	float diff = max (dot(nor, -lightDirection), 0.0);
	vec3 diffuse = uLightColor.DiffuseColor * diff * uMaterial.DiffuseColor;

	//specular color
	vec3 reflectDirection = normalize (reflect (-lightDirection, nor));
	vec3 toEye = normalize (ViewPos - FragPos);
	float spec = pow (max(dot(reflectDirection, toEye), 0.0), uMaterial.Shininess);
	vec3 specular = uLightColor.SpecularColor * (spec * uMaterial.SpecularColor);

	//final
	vec3 result = ambient + diffuse + specular;
	FragColor = vec4 (result, 1.0);
}

