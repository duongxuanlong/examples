#ifndef __GLSHADER__
#define __GLSHADER__

#include "glm\gtc\matrix_transform.hpp"

class GLShader
{
private:
	unsigned int m_VertexShader;
	unsigned int m_FragmentShader;
	unsigned int m_Program;

	void CompileShader(const char* str, unsigned int id);
	//void LinkProgram();
public:
	~GLShader();
	GLShader(const char* vertexStr, const char* fragmentStr, bool shouldLink = true);

	void ActiveProgram();
	void LinkProgram();

	unsigned int GetVertexId() { return m_VertexShader; }
	unsigned int GetFragmentId() { return m_FragmentShader; }
	unsigned int GetProgramId() { return m_Program; }
	void SetVertexId(unsigned int id) { m_VertexShader = id; }
	void SetFragmentId(unsigned int id){ m_FragmentShader = id; };

	void SetVec3(const char* uniform, const glm::vec3 &value);
	void SetMat4x4(const char* uniform, const glm::mat4x4 &matrix);
	void SetInt(const char* uniform, int value);
	void SetFloat(const char* uniform, float value);
};

#endif