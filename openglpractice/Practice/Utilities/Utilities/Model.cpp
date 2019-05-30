#include "Model.h"
#include "GLShader.h"

#include "assimp\Importer.hpp"
#include "assimp\postprocess.h"
#include "glad\glad.h"

#include <iostream>
#include <string>

unsigned int TextureFromFile(const char* path, const std::string& directory, bool gamma);

void Model::Draw(GLShader* shader)
{
	m_LoadedTextures;
	for (int i = 0; i < m_Meshes.size(); ++i)
	{
		m_Meshes[i].Draw(shader);
	}
}

void Model::LoadModel(const std::string& path)
{
	Assimp::Importer importer;
	const aiScene* scene = importer.ReadFile(path, aiProcess_Triangulate | aiProcess_FlipUVs | aiProcess_CalcTangentSpace);

	if (!scene || scene->mFlags & AI_SCENE_FLAGS_INCOMPLETE || !scene->mRootNode)
	{
		std::cout << "Could not load file model " << path << std::endl;
		return;
	}

	m_Directory = path.substr(0, path.find_last_of('/'));

	ProcessNode(scene->mRootNode, scene);
}

void Model::ProcessNode(aiNode* node, const aiScene* scene)
{
	for (int i = 0; i < node->mNumMeshes; ++i)
	{
		aiMesh* mesh = scene->mMeshes[node->mMeshes[i]];
		m_Meshes.push_back(ProcessMesh(mesh, scene));
	}

	for (int i = 0; i < node->mNumChildren; ++i)
		ProcessNode(node->mChildren[i], scene);
}

Mesh Model::ProcessMesh(aiMesh* mesh, const aiScene* scene)
{
	std::vector<Vertex> vertices;
	std::vector<unsigned int> indices;
	std::vector<Texture> textures;

	//retrieve vertices
	for (int i = 0; i < mesh->mNumVertices; ++i)
	{
		Vertex vertex;
		vertex.m_Position.x = mesh->mVertices[i].x;
		vertex.m_Position.y = mesh->mVertices[i].y;
		vertex.m_Position.z = mesh->mVertices[i].z;

		vertex.m_Normal.x = mesh->mNormals[i].x;
		vertex.m_Normal.y = mesh->mNormals[i].y;
		vertex.m_Normal.z = mesh->mNormals[i].z;

		if (mesh->mTextureCoords[0])
		{
			glm::vec2 vec;
			vec.x = mesh->mTextureCoords[0][i].x;
			vec.y = mesh->mTextureCoords[0][i].y;
			vertex.m_TexCoord = vec;
		}
		else
			vertex.m_TexCoord = glm::vec2(0.0f, 0.0f);

		vertex.m_Tangent.x = mesh->mTangents[i].x;
		vertex.m_Tangent.y = mesh->mTangents[i].y;
		vertex.m_Tangent.z = mesh->mTangents[i].z;

		vertex.m_BiTangent.x = mesh->mBitangents[i].x;
		vertex.m_BiTangent.y = mesh->mBitangents[i].y;
		vertex.m_BiTangent.z = mesh->mBitangents[i].z;

		vertices.push_back(vertex);
	}

	//retrieve indices
	for (int i = 0; i < mesh->mNumFaces; ++i)
	{
		aiFace face = mesh->mFaces[i];
		for (int j = 0; j < face.mNumIndices; ++j)
			indices.push_back(face.mIndices[j]);
	}

	//retrieve material
	aiMaterial* material = scene->mMaterials[mesh->mMaterialIndex];
	LoadMaterialTextures(textures, material, aiTextureType_DIFFUSE, "texture_diffuse");
	LoadMaterialTextures(textures, material, aiTextureType_SPECULAR, "texture_specular");
	LoadMaterialTextures(textures, material, aiTextureType_HEIGHT, "texture_normal");
	LoadMaterialTextures(textures, material, aiTextureType_AMBIENT, "texture_ambient");

	return Mesh(vertices, indices, textures);
}

void Model::LoadMaterialTextures(std::vector<Texture>& textures, aiMaterial* material, aiTextureType type, const std::string& name)
{
	for (int i = 0; i < material->GetTextureCount(type); ++i)
	{
		aiString str;
		material->GetTexture(type, i, &str);
		std::cout << "Texture name: " << str.C_Str() << std::endl;

		/*if (aiGetMaterialTexture(material, type, 0, &str) != aiReturn_SUCCESS)
			continue;*/

		/*if (str.length == 0)
			continue;*/

		bool skip = false;
		for (int j = 0; j < m_LoadedTextures.size(); ++j)
		{
			if (std::strcmp(m_LoadedTextures[j].m_Path.c_str(), str.C_Str()) == 0)
			{
				textures.push_back(m_LoadedTextures[j]);
				skip = true;
				break;
			}
		}

		if (!skip)
		{
			Texture texture;
			texture.m_TextureID = TextureFromFile(str.C_Str(), m_Directory, m_GammaCorrection);
			texture.m_Type = name;
			texture.m_Path = str.C_Str();
			textures.push_back(texture);
			m_LoadedTextures.push_back(texture);
		}
	}
}

#ifndef STB_IMAGE_IMPLEMENTATION
#define STB_IMAGE_IMPLEMENTATION
#endif

#include "stb_image.h"

unsigned int TextureFromFile(const char* path, const std::string& directory, bool gamma)
{
	std::string filename = std::string(path);
	//filename = directory + '/' + filename;

	unsigned int id = 0;
	glGenTextures(1, &id);

	int height, width, channel;
	//stbi_set_flip_vertically_on_load(true);
	unsigned char* data = stbi_load(filename.c_str(), &width, &height, &channel, 0);

	if (data)
	{
		GLenum format;
		if (channel == 1)
			format = GL_RED;
		else if (channel == 3)
			format = GL_RGB;
		else if (channel == 4)
			format = GL_RGBA;
		else format = GL_RGB;

		glBindTexture(GL_TEXTURE_2D, id);
		glTexImage2D(GL_TEXTURE_2D, 0, format, width, height, 0, format, GL_UNSIGNED_BYTE, data);
		glGenerateMipmap(GL_TEXTURE_2D);

		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_S, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_WRAP_T, GL_REPEAT);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_LINEAR);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);

		glBindTexture(GL_TEXTURE_2D, 0);
	}
	else
	{
		std::cout << "Could not load texture: " << filename << std::endl;
	}

	return id;
}


















