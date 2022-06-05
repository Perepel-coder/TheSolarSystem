#version 330

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

out vec2 texCoord;

uniform mat4 projMatr;

void main()
{
	texCoord = aTexCoord;

	vec3 pos = (aPosition + vec3(-0.5, -0.5, -1))*10000;
	gl_Position = projMatr * vec4(pos, 1.0f);
}
