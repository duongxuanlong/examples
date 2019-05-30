#ifndef __MESH__
#define __MESH__

#include "glm\gtc\matrix_transform.hpp";
#include "glm\glm.hpp"
//#include "GLShader.h"
#include <string>
#include <vector>

class GLShader;

struct Vertex
{
	glm::vec3 m_Position;
	glm::vec3 m_Normal;
	glm::vec2 m_TexCoord;
	glm::vec3 m_Tangent;
	glm::vec3 m_BiTangent;
};

struct Texture
{
	unsigned int m_TextureID;
	std::string m_Type;
	std::string m_Path;
};

class Mesh
{
private:
	std::vector<Vertex> m_Vertices;
	std::vector<Texture> m_Textures;
	std::vector<unsigned int> m_Indices;

	unsigned int m_VBO;
	unsigned int m_EBO;

	int m_aPos;
	int m_aNormal;
	int m_aTexCoord;
	int m_Tangent;
	int m_BiTangent;

	void SetupMesh();
public:
	Mesh(const std::vector<Vertex>& vertices, const std::vector<unsigned int>& indices, const std::vector<Texture>& textures);
	void Draw(GLShader* shader);
};

#endif