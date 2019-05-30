#include "Camera.h"

Camera* Camera::m_Camera = NULL;

Camera* Camera::GetInstance()
{
	if (Camera::m_Camera == NULL)
	{
		Camera::m_Camera = new Camera();
		Camera::GetInstance()->m_Front = glm::vec3(0.0f, 0.0f, -1.0f);
	}

	return Camera::m_Camera;
}

void Camera::SetUpCameraParam(const glm::vec3& pos, const glm::vec3& target, const glm::vec3& up)
{
	m_Position = pos;
	m_Target = target;
	m_Up = up;
}

void Camera::SetUpProjectionParam(float fov, float width, float height, float nearPlane, float farPlane)
{
	m_FOV = fov;
	m_Width = width;
	m_Height = height;
	m_NearPlane = nearPlane;
	m_FarPlane = farPlane;
}

void Camera::SelfLookAt()
{
	glm::vec3 zAxis = glm::normalize(m_Position - m_Target);
	glm::vec3 xAxis = glm::normalize(glm::cross(zAxis, m_Up));
	glm::vec3 yAxis = glm::normalize(glm::cross(xAxis, zAxis));

	m_RotationMatrix[0][0] = xAxis.x;
	m_RotationMatrix[1][0] = xAxis.y;
	m_RotationMatrix[2][0] = xAxis.z;
	m_RotationMatrix[0][1] = yAxis.x;
	m_RotationMatrix[1][1] = yAxis.y;
	m_RotationMatrix[1][2] = yAxis.z;
	m_RotationMatrix[2][0] = zAxis.x;
	m_RotationMatrix[2][1] = zAxis.y;
	m_RotationMatrix[2][2] = zAxis.z;

	m_TranslationMatrix[3][0] = -m_Position.x;
	m_TranslationMatrix[3][1] = -m_Position.y;
	m_TranslationMatrix[3][2] = -m_Position.z;

	//Should we reverse this order
	m_ViewMatrix = m_TranslationMatrix * m_RotationMatrix;

	//m_ViewMatrix = m_RotationMatrix * m_TranslationMatrix;
}

void Camera::CalculateProjectionMatrix()
{
	m_ProjectionMatrix = glm::perspective(m_FOV, m_Width / m_Height, m_NearPlane, m_FarPlane);
}

const glm::mat4x4& Camera::GetViewMatrix()
{
	return m_ViewMatrix;
}

const glm::mat4x4& Camera::GetProjectionMatrix()
{
	return m_ProjectionMatrix;
}