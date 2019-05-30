#include "glad\glad.h"
#include "GLFW\glfw3.h"

#include "GLContext.h"
#include "GLShader.h"

#include "glm\glm.hpp"
#include "glm\gtc\matrix_transform.hpp"
#include "glm\gtc\type_ptr.hpp"

#include "Camera.h"

#define STB_IMAGE_IMPLEMENTATION
#include "stb_image.h"

#include <iostream>

GLShader* shader1, *shader2;

//GLint aPos;
glm::mat4x4 uMVP;

glm::vec3 uLightColor;
glm::vec3 uObjectColor;
glm::vec3 LightPos(1.2f, 1.0f, 2.0f);

bool UseViewForLight = false;
float CurrentTime = (float)glfwGetTime();
float WIDTH = 800.0f;
float HEIGHT = 600.0f;

GLuint VBO;
GLuint TextureID01 = 0;
GLuint TextureID02 = 0;
GLuint TextureID03 = 0;

//Normal vertices
//float CubeVertices[] = {
//	-0.5f, -0.5f, -0.5f,
//	0.5f, -0.5f, -0.5f,
//	0.5f, 0.5f, -0.5f,
//	0.5f, 0.5f, -0.5f,
//	-0.5f, 0.5f, -0.5f,
//	-0.5f, -0.5f, -0.5f,
//
//	-0.5f, -0.5f, 0.5f,
//	0.5f, -0.5f, 0.5f,
//	0.5f, 0.5f, 0.5f,
//	0.5f, 0.5f, 0.5f,
//	-0.5f, 0.5f, 0.5f,
//	-0.5f, -0.5f, 0.5f,
//
//	-0.5f, 0.5f, 0.5f,
//	-0.5f, 0.5f, -0.5f,
//	-0.5f, -0.5f, -0.5f,
//	-0.5f, -0.5f, -0.5f,
//	-0.5f, -0.5f, 0.5f,
//	-0.5f, 0.5f, 0.5f,
//
//	0.5f, 0.5f, 0.5f,
//	0.5f, 0.5f, -0.5f,
//	0.5f, -0.5f, -0.5f,
//	0.5f, -0.5f, -0.5f,
//	0.5f, -0.5f, 0.5f,
//	0.5f, 0.5f, 0.5f,
//
//	-0.5f, -0.5f, -0.5f,
//	0.5f, -0.5f, -0.5f,
//	0.5f, -0.5f, 0.5f,
//	0.5f, -0.5f, 0.5f,
//	-0.5f, -0.5f, 0.5f,
//	-0.5f, -0.5f, -0.5f,
//
//	-0.5f, 0.5f, -0.5f,
//	0.5f, 0.5f, -0.5f,
//	0.5f, 0.5f, 0.5f,
//	0.5f, 0.5f, 0.5f,
//	-0.5f, 0.5f, 0.5f,
//	-0.5f, 0.5f, -0.5f,
//};


//vertices with normal vector
//float CubeVertices[] = {
//	-0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
//	0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
//	0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
//	0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
//	-0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
//	-0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f,
//
//	-0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
//	0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
//	0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
//	0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
//	-0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
//	-0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f,
//
//	-0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f,
//	-0.5f, 0.5f, -0.5f, -1.0f, 0.0f, 0.0f,
//	-0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f,
//	-0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f,
//	-0.5f, -0.5f, 0.5f, -1.0f, 0.0f, 0.0f,
//	-0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f,
//
//	0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f,
//	0.5f, 0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
//	0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
//	0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f,
//	0.5f, -0.5f, 0.5f, 1.0f, 0.0f, 0.0f,
//	0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f,
//
//	-0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f,
//	0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f,
//	0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f,
//	0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f,
//	-0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f,
//	-0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f,
//
//	-0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
//	0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f,
//	0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f,
//	0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f,
//	-0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f,
//	-0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f
//};

//vertices with normal, texcoord
float CubeVertices[] = {
	// positions          // normals           // texture coords
	-0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,
	0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 0.0f,
	0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
	0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 1.0f, 1.0f,
	-0.5f, 0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 1.0f,
	-0.5f, -0.5f, -0.5f, 0.0f, 0.0f, -1.0f, 0.0f, 0.0f,

	-0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,
	0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 0.0f,
	0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
	0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 1.0f, 1.0f,
	-0.5f, 0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 1.0f,
	-0.5f, -0.5f, 0.5f, 0.0f, 0.0f, 1.0f, 0.0f, 0.0f,

	-0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
	-0.5f, 0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
	-0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
	-0.5f, -0.5f, -0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
	-0.5f, -0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
	-0.5f, 0.5f, 0.5f, -1.0f, 0.0f, 0.0f, 1.0f, 0.0f,

	0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,
	0.5f, 0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 1.0f,
	0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
	0.5f, -0.5f, -0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 1.0f,
	0.5f, -0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 0.0f, 0.0f,
	0.5f, 0.5f, 0.5f, 1.0f, 0.0f, 0.0f, 1.0f, 0.0f,

	-0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,
	0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 1.0f,
	0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
	0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 1.0f, 0.0f,
	-0.5f, -0.5f, 0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 0.0f,
	-0.5f, -0.5f, -0.5f, 0.0f, -1.0f, 0.0f, 0.0f, 1.0f,

	-0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f,
	0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 1.0f,
	0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
	0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 1.0f, 0.0f,
	-0.5f, 0.5f, 0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 0.0f,
	-0.5f, 0.5f, -0.5f, 0.0f, 1.0f, 0.0f, 0.0f, 1.0f
};

//cubes position
glm::vec3 cubePositions[] = {
	glm::vec3(0.0f, 0.0f, 0.0f),
	glm::vec3(0.5f, 1.0f, -3.0f),
	glm::vec3(-1.5f, -2.2f, -2.5f),
	glm::vec3(-3.8f, -2.0f, -12.3f),
	glm::vec3(2.4f, -0.4f, -3.5f),
	glm::vec3(-1.7f, 3.0f, -7.5f),
	glm::vec3(1.3f, -2.0f, -2.5f),
	glm::vec3(1.5f, 2.0f, -2.5f),
	glm::vec3(1.5f, 0.2f, -1.5f),
	glm::vec3(-1.3f, 1.0f, -1.5f)
};

void GenerateBuffer()
{
	glGenBuffers(1, &VBO);
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	glBufferData(GL_ARRAY_BUFFER, sizeof(CubeVertices), CubeVertices, GL_STATIC_DRAW);
	glBindBuffer(GL_ARRAY_BUFFER, 0);
}

void LoadTexture(const char* filename, GLuint& id)
{
	glGenTextures(1, &id);

	stbi_set_flip_vertically_on_load(true);
	int width, height, channel;
	unsigned char* data = stbi_load(filename, &width, &height, &channel, 0);

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
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MAG_FILTER, GL_NEAREST);
		glTexParameteri(GL_TEXTURE_2D, GL_TEXTURE_MIN_FILTER, GL_NEAREST);
		
		glBindTexture(GL_TEXTURE_2D, 0);
	}
	else
	{
		std::cout << "Could not load texture" << std::endl;
	}

	stbi_image_free(data);
}

void PrepareShaders()
{
	//basic lighting, material
	//shader1 = new GLShader("_VS.glsl", "_FS.glsl");
	/*shader1 = new GLShader("_VS.glsl", "_FSMaterial.glsl");
	shader2 = new GLShader("NULL", "_FS2.glsl");
	shader2->SetVertexId(shader1->GetVertexId());
	shader2->LinkProgram();*/

	//light map
	/*shader1 = new GLShader("_VSLightMap.glsl", "_FSLightMap.glsl");
	shader2 = new GLShader("_VS.glsl", "_FS2.glsl");*/

	//Directional light
	/*shader1 = new GLShader("_VSDirectionalLight.glsl", "_FSDirectionalLight.glsl");
	shader2 = new GLShader("_VS.glsl", "_FS2.glsl");*/

	//Point light
	/*shader1 = new GLShader("_VSPointLight.glsl", "_FSPointLight.glsl");
	shader2 = new GLShader("_VS.glsl", "_FS2.glsl");*/

	//Spot light
	shader1 = new GLShader("_VSSpotLight.glsl", "_FSSpotLight.glsl");
	////shader2 = new GLShader("_VS.glsl", "_FS2.glsl");

	uLightColor = glm::vec3(1.0f, 1.0f, 1.0f);
	uObjectColor = glm::vec3(1.0f, 0.7f, 0.3f);
}

void DrawShader(GLShader* shader, const glm::vec3 &lightcolor, const glm::vec3 &objectcolor, const glm::mat4x4 &mvp, const glm::mat4x4 &model, const glm::vec3 &lightPos)
{
	glBindBuffer(GL_ARRAY_BUFFER, VBO);
	GLint pos = glGetAttribLocation(shader->GetProgramId(), "aPos");
	if (pos != -1)
	{
		glEnableVertexAttribArray(pos);
		glVertexAttribPointer(pos, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GL_FLOAT), (void*)0);
	}

	GLint nor = glGetAttribLocation(shader->GetProgramId(), "aNormal");
	if (nor != -1)
	{
		glEnableVertexAttribArray(nor);
		glVertexAttribPointer(nor, 3, GL_FLOAT, GL_FALSE, 8 * sizeof(GL_FLOAT), (void*)(3 * sizeof(GL_FLOAT)));
	}

	GLint texcoord = glGetAttribLocation(shader->GetProgramId(), "aTexCoordinate");
	if (texcoord != -1)
	{
		glEnableVertexAttribArray(texcoord);
		glVertexAttribPointer(texcoord, 2, GL_FLOAT, GL_FALSE, 8 * sizeof(GL_FLOAT), (void*)(6 * sizeof(GL_FLOAT)));
	}
	glBindBuffer(GL_ARRAY_BUFFER, 0);

	shader->ActiveProgram();
	shader->SetInt("UseViewForLight", UseViewForLight);
	shader->SetVec3("LightColor", lightcolor);
	shader->SetVec3("ObjectColor", objectcolor);
	shader->SetVec3("LightPos", lightPos);
	shader->SetVec3("ViewPos", Camera::GetInstance()->GetCameraPos());
	shader->SetMat4x4("MVPMatrix", mvp);
	shader->SetMat4x4("ModelMatrix", model);
	shader->SetMat4x4("ViewMatrix", Camera::GetInstance()->GetViewMatrix());

	//Setup material for object color
	//shader->SetVec3("uMaterial.AmbientColor", glm::vec3(1.0f, 0.5f, 0.31f));
	//shader->SetVec3("uMaterial.DiffuseColor", glm::vec3(1.0f, 0.5f, 0.31f));
	//shader->SetVec3("uMaterial.SpecularColor", glm::vec3(0.5f, 0.5f, 0.5f));
	shader->SetFloat("uMaterial.Shininess", 32.0f);

	//Setup LightColor
	glm::vec3 light;
	light.x = sin(glfwGetTime() * 1.5f);
	light.y = sin(glfwGetTime() * 2.0f);
	light.z = sin(glfwGetTime() * 1.0f);
	glm::vec3 ambient = light * 0.3f;

	light = glm::vec3(0.8f);
	ambient = glm::vec3(0.1f);

	//light = glm::vec3(sin(glfwGetTime()) * 0.5f + 0.7f);
	glm::vec3 emit = glm::vec3(sin(glfwGetTime()) * 0.5f + 0.5f);

	shader->SetVec3("uLightColor.AmbientColor", ambient);
	shader->SetVec3("uLightColor.DiffuseColor", light);
	shader->SetVec3("uLightColor.SpecularColor", glm::vec3(1.0f));
	shader->SetVec3("uLightColor.EmitColor", emit);
	shader->SetVec3("uLightColor.LightPos", lightPos);
	shader->SetVec3("uLightColor.LightDirection", glm::vec3(0.0f, 0.0f, -1.0f));

	//Params for point light
	shader->SetFloat("uLightColor.KConstant", 1.0f);
	shader->SetFloat("uLightColor.KLinear", 0.09f);
	shader->SetFloat("uLightColor.KQuadratic", 0.32f);

	//Param for spot light
	shader->SetFloat("uLightColor.CutOff", glm::cos(glm::radians(12.5f)));
	shader->SetFloat("uLightColor.OuterCutOff", glm::cos(glm::radians(17.5f)));

	//Active Texture
	int textureSlot = 0;
	if (TextureID01 > 0)
	{
		glActiveTexture(GL_TEXTURE0 + textureSlot);
		glBindTexture(GL_TEXTURE_2D, TextureID01);
		shader->SetInt("uMaterial.DiffuseColor", textureSlot);
		textureSlot++;
	}

	if (TextureID02 > 0)
	{
		glActiveTexture(GL_TEXTURE0 + textureSlot);
		glBindTexture(GL_TEXTURE_2D, TextureID02);
		shader->SetInt("uMaterial.SpecularColor", textureSlot);
		textureSlot++;
	}

	if (TextureID03 > 0)
	{
		glActiveTexture(GL_TEXTURE0 + textureSlot);
		glBindTexture(GL_TEXTURE_2D, TextureID03);
		shader->SetInt("uMaterial.EmitColor", textureSlot);
		textureSlot++;
	}

	//glDrawArrays(GL_TRIANGLES, 0, sizeof(CubeVertices) / sizeof(GL_FLOAT));
	glDrawArrays(GL_TRIANGLES, 0, 36);
}

void Draw()
{
	//Change LightPos
	LightPos.x = 1.0f + sin(glfwGetTime() * 2.0f);
	LightPos.y = sin(glfwGetTime() / 2.0f) * 1.0f;

	LightPos.x = 0.0f;
	LightPos.y = 0.0f;
	LightPos.z = 3.0f;

	//Projection matrix
	glm::mat4x4 projection = glm::perspective(glm::radians(45.0f), WIDTH / HEIGHT, 0.1f, 100.0f);

	//View matrix
	Camera::GetInstance()->SetUpCameraParam(glm::vec3(0.0f, 0.0f, 3.0f),
		glm::vec3(0.0f, 0.0f, 0.0f), glm::vec3(0.0f, 1.0f, 0.0f));
	Camera::GetInstance()->SelfLookAt();

	//Model matrix
	/*glm::mat4x4 model;
	model = glm::rotate(model, 60.0f, glm::vec3(1.0, 1.0f, 0.0f));
	model = glm::scale(model, glm::vec3(0.7f));
	glm::mat4x4 mvp1 = projection * Camera::GetInstance()->GetViewMatrix() * model;
	DrawShader(shader1, uLightColor, uObjectColor, mvp1, model, LightPos);*/

	//ten containers
	for (int i = 0; i < 10; ++i)
	{
		/*if (i == 0)
			continue;*/
		glm::mat4x4 model;
		model = glm::translate(model, cubePositions[i]);
		float angle = 20.0f * (i + 1);
		model = glm::rotate(model, glm::radians(angle), glm::vec3(1.0f, 1.0f, 0.0f));
		//model = glm::scale(model, glm::vec3(0.7f));

		glm::mat4x4 mvp1 = projection * Camera::GetInstance()->GetViewMatrix() * model;

		DrawShader(shader1, uLightColor, uObjectColor, mvp1, model, LightPos);
	}

	//Model2 matrix
	if (shader2 != NULL)
	{
		glm::mat4x4 model2;
		model2 = glm::translate(model2, LightPos);
		//model2 = glm::rotate(model2, (float)glfwGetTime(), glm::vec3(0.5f, 0.1f, 0.0f));
		//model2 = glm::rotate(model2, CurrentTime, glm::vec3(0.5f, 0.1f, 0.0f));
		model2 = glm::scale(model2, glm::vec3(0.2f));

		glm::mat4x4 mvp2 = projection * Camera::GetInstance()->GetViewMatrix() * model2;

		DrawShader(shader2, uLightColor, uObjectColor, mvp2, model2, LightPos);
	}
}

int main(int count, char** argvs)
{
	GLContext::GetInstance()->CreateContextWindow(WIDTH, HEIGHT, "Lighting");
	GLContext::GetInstance()->MakeViewport(0, 0, WIDTH, HEIGHT);

	GenerateBuffer();
	PrepareShaders();

	LoadTexture("container2.png", TextureID01);
	LoadTexture("container2_specular.png", TextureID02);
	LoadTexture("matrix.jpg", TextureID03);

	glEnable(GL_DEPTH_TEST);
	while (GLContext::GetInstance()->ShouldTerminateContext())
	{
		glfwPollEvents();
		GLContext::GetInstance()->HandleExit();

		glClearColor(0.1f, 0.1f, 0.1f, 0.8f);
		glClear(GL_COLOR_BUFFER_BIT | GL_DEPTH_BUFFER_BIT);

		Draw();

		GLContext::GetInstance()->SwapBuffers();
	}

	return 0;
}