#ifndef __CAMERA__
#define __CAMERA__

//#include "glad\glad.h"
//#include "GLFW\glfw3.h"

#include "glm\glm.hpp"
#include "glm\gtc\matrix_transform.hpp"

class Camera
{
private:
	glm::vec3 m_Position;
	glm::vec3 m_Target;
	glm::vec3 m_Up;

	glm::vec3 m_Front;

	glm::mat4x4 m_RotationMatrix;
	glm::mat4x4 m_TranslationMatrix;
	glm::mat4x4 m_ViewMatrix;

	float m_FOV;
	float m_Width;
	float m_Height;
	float m_NearPlane;
	float m_FarPlane;
	glm::mat4x4 m_ProjectionMatrix;

	static Camera* m_Camera;

	Camera()
	{
		m_Position = glm::vec3(0.0f, 0.0f, 0.0f);
		m_Target = glm::vec3(0.0f, 0.0f, 0.0f);
		m_Up = glm::vec3(0.0f, 1.0f, 0.0f);
	}
public:
	static Camera* GetInstance();

	void SelfLookAt();

	void CalculateProjectionMatrix();

	void SetUpCameraParam(const glm::vec3& pos, const glm::vec3& target, const glm::vec3& up = glm::vec3(0.0f, 1.0f, 0.0f));

	void SetUpProjectionParam(float fov, float width, float height, float nearPlane, float farPlane);

	const glm::mat4x4& GetViewMatrix();

	const glm::mat4x4& GetProjectionMatrix();

	const glm::vec3& GetCameraPos() { return m_Position; };

	const glm::vec3& Front() { return m_Front; };

	//Camera movement
	void MoveLeft(float delta);
};

#endif