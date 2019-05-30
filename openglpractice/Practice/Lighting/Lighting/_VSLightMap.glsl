#version 330 core

uniform mat4 MVPMatrix;
uniform mat4 ModelMatrix;
uniform mat4 ViewMatrix;

in vec3 aPos;
in vec3 aNormal;
in vec2 aTexCoordinate;

out vec3 FragPos;
out vec3 Normal;
out vec2 TexCoordinate;


void main ()
{
	gl_Position = MVPMatrix * vec4 (aPos, 1.0);

	FragPos = vec3(ModelMatrix * vec4 (aPos, 1.0));
	Normal = aNormal;
	TexCoordinate = aTexCoordinate;
}