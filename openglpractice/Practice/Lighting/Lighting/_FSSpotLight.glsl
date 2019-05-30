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
	vec3 LightDirection;
	float CutOff;
	float OuterCutOff;

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
	//spot light param
	vec3 lightDirection = normalize (uLightColor.LightPos - FragPos);
	float theta = dot (lightDirection, normalize(-uLightColor.LightDirection));
	float epsilon = uLightColor.CutOff - uLightColor.OuterCutOff;
	float intensity = clamp(((theta - uLightColor.OuterCutOff) / epsilon), 0.0, 1.0);

	float distance = length (uLightColor.LightPos - FragPos);
	vec4 image = texture(uMaterial.DiffuseColor, TexCoordinate);
	vec3 result;

	// sharp edge
	
	// if (theta > uLightColor.CutOff)
	// {
		// vec3 nor = normalize (Normal);
		// float diff = max (dot(nor, lightDirection), 0.0);
		// vec3 diffuse = uLightColor.DiffuseColor * diff * image.rgb;

		// vec3 ambient = uLightColor.AmbientColor * image.rgb;

		// vec4 imagespecular = texture (uMaterial.SpecularColor, TexCoordinate);
		// vec3 reflectDirection = normalize (reflect (-lightDirection, nor));
		// vec3 toEye = normalize (ViewPos - FragPos);
		// float spec = pow (max(dot(reflectDirection, toEye), 0.0), uMaterial.Shininess);
		// vec3 specular = uLightColor.SpecularColor * spec * (imagespecular.rgb); // for inverse specular
		
		// float attenuation = 1 / (uLightColor.KConstant + uLightColor.KLinear * distance + uLightColor.KQuadratic * (distance * distance));

		// result = ambient + (diffuse + specular) * attenuation;
	// }
	// else
	// {
		// result = uLightColor.AmbientColor * image.rgb;
	// }

	//smooth edge
	
	vec3 nor = normalize (Normal);
	float diff = max (dot(nor, lightDirection), 0.0);
	vec3 diffuse = uLightColor.DiffuseColor * diff * image.rgb;

	vec3 ambient = uLightColor.AmbientColor * image.rgb;

	vec4 imagespecular = texture (uMaterial.SpecularColor, TexCoordinate);
	vec3 reflectDirection = normalize (reflect (-lightDirection, nor));
	vec3 toEye = normalize (ViewPos - FragPos);
	float spec = pow (max(dot(reflectDirection, toEye), 0.0), uMaterial.Shininess);
	vec3 specular = uLightColor.SpecularColor * spec * (imagespecular.rgb); // for inverse specular

	float strength = 1 / (uLightColor.KConstant + uLightColor.KLinear * distance + uLightColor.KQuadratic * (distance * distance));

	result = ambient * strength + ((diffuse + specular) * strength * intensity);

	
	
	FragColor = vec4 (result, 1.0);
}