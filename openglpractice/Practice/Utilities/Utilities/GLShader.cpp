#include "glad\glad.h"
#include "GLFW\glfw3.h"


#include "GLShader.h"

#include <iostream>
#include <fstream>
#include <sstream>

GLShader::GLShader(const char* vertexStr, const char* fragmentStr, bool shouldLink)
{
	if (vertexStr != NULL)
	{
		std::cout << "VertexShader file: " << vertexStr << std::endl;
		m_VertexShader = glCreateShader(GL_VERTEX_SHADER);
		std::fstream vertexStream;
		vertexStream.open(vertexStr);
		std::stringstream vstr;
		vstr << vertexStream.rdbuf();
		CompileShader(vstr.str().c_str(), m_VertexShader);
	}
	
	if (fragmentStr != NULL)
	{
		std::cout << "FragmentShader file: " << fragmentStr << std::endl;
		m_FragmentShader = glCreateShader(GL_FRAGMENT_SHADER);
		std::fstream fragmentStream;
		fragmentStream.open(fragmentStr);
		std::stringstream fstr;
		fstr << fragmentStream.rdbuf();
		CompileShader(fstr.str().c_str(), m_FragmentShader);
	}

	m_Program = glCreateProgram();
	if (shouldLink)
		LinkProgram();
}

GLShader::~GLShader()
{
	glDetachShader(m_Program, m_VertexShader);
	glDetachShader(m_Program, m_FragmentShader);
	
	glDeleteShader(m_VertexShader);
	glDeleteShader(m_FragmentShader);
	glDeleteProgram(m_Program);
}

void GLShader::CompileShader(const char* str, unsigned int id)
{
	glShaderSource(id, 1, &str, NULL);
	glCompileShader(id);

	int success;
	char infoLog[512];

	glGetShaderiv(id, GL_COMPILE_STATUS, &success);
	if (!success)
	{
		glGetShaderInfoLog(id, 512, NULL, infoLog);
		std::cout << "Shader compile error with log: " << infoLog << std::endl;
	}
}

void GLShader::LinkProgram()
{
	glAttachShader(m_Program, m_VertexShader);
	glAttachShader(m_Program, m_FragmentShader);
	glLinkProgram(m_Program);

	int success;
	char infoLog[512];
	glGetProgramiv(m_Program, GL_LINK_STATUS, &success);
	if (!success)
	{
		glGetProgramInfoLog(m_Program, 512, NULL, infoLog);
		std::cout << "Link program error with log: " << infoLog << std::endl;
	}
}

void GLShader::ActiveProgram()
{
	if (m_Program == 0)
		return;

	glUseProgram(m_Program);
}

void GLShader::SetVec3(const char* uniform, const glm::vec3 &value)
{
	GLint id = glGetUniformLocation(m_Program, uniform);
	if (id != -1)
	{
		glUniform3fv(id, 1, &value[0]);
	}
}

void GLShader::SetMat4x4(const char* uniform, const glm::mat4x4 &matrix)
{
	GLint id = glGetUniformLocation(m_Program, uniform);
	if (id != -1)
	{
		glUniformMatrix4fv(id, 1, GL_FALSE, &matrix[0][0]);
	}
}

void GLShader::SetInt(const char* uniform, int value)
{
	GLint id = glGetUniformLocation(m_Program, uniform);
	if (id != -1)
	{
		glUniform1i(id, value);
	}
}

void GLShader::SetFloat(const char* uniform, float value)
{
	GLint id = glGetUniformLocation(m_Program, uniform);
	if (id != -1)
	{
		glUniform1f(id, value);
	}
}