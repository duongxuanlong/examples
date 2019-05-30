#version 330 core

uniform mat4 MVPMatrix;
uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;
uniform int UseViewForLight;

in vec3 aPos;
in vec3 aNormal;

out vec3 FragPos;
out vec3 Normal;

void main()
{
	gl_Position = MVPMatrix * vec4 (aPos, 1.0f);

	if (UseViewForLight == 0)
	{
		FragPos = vec3(ModelMatrix * vec4 (aPos, 1.0f));
		Normal = aNormal;
	}
	else
	{
		FragPos = vec3(ViewMatrix * ModelMatrix * vec4 (aPos, 1.0f));
		Normal = mat3 (transpose(inverse( ViewMatrix * ModelMatrix))) * aNormal;
	}

	
}