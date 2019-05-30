#version 330 core

uniform vec3 ObjectColor;
uniform vec3 LightColor;
uniform vec3 LightPos;
uniform vec3 ViewPos;

uniform int UseViewForLight;
uniform mat4 ViewMatrix;

in vec3 Normal;
in vec3 FragPos;

out vec4 FragColor;

void main()
{
	//Calculate amibent color
	float ambientStrength = 0.1f;
	vec3 ambientColor = ambientStrength * LightColor;

	//Calculate diffuse color
	vec3 dcolor = vec3(0.5, 0.5, 0.5);
	vec3 nor = normalize(Normal);
	vec3 lightPos;
	if (UseViewForLight == 0)
	{
		lightPos = LightPos;
	}
	else
	{
		lightPos = vec3(ViewMatrix * vec4(LightPos, 1.0));
	}
	vec3 direction = normalize(lightPos - FragPos);
	float diff = max (dot (nor, direction), 0.0f);
	//vec3 diffuseColor = (1 - diff) * LightColor;
	vec3 diffuseColor = diff * dcolor;

	//Calculate specular light
	float specularStrength = 1.0f;
	vec3 reflectLight = normalize (reflect (-direction, nor));
	vec3 norViewPos;
	if (UseViewForLight == 0)
		norViewPos = normalize (ViewPos - FragPos);
	else
		norViewPos = normalize (-FragPos);
	float theta = pow (max (dot (norViewPos, reflectLight), 0.0), 32);
	vec3 specularColor = specularStrength * theta * LightColor;

	//diffuseColor = vec3(0.0);

	vec3 result = (ambientColor + diffuseColor + specularColor) * ObjectColor;

	FragColor = vec4 (result, 1.0f);
}