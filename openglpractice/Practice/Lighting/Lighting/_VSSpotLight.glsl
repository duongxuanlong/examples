#version 330 core

uniform mat4 MVPMatrix;
uniform mat4 ModelMatrix;

in vec3 aPos;
in vec3 aNormal;
in vec2 aTexCoordinate;

out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoordinate;

void main()
{
	gl_Position = MVPMatrix * vec4 (aPos, 1.0f);

	Normal = aNormal;
	TexCoordinate = aTexCoordinate;
	FragPos = vec3 (ModelMatrix * vec4 (aPos, 1.0f));
}