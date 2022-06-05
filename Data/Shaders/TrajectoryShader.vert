#version 330

layout(location = 0) in vec3 aPosition;

uniform mat4 projMatr;
uniform vec4 transform;
uniform mat4 view;

void main()
{
	vec4 pos = vec4(aPosition, 1.0f);
	pos = pos + transform;
	gl_Position = projMatr * view *  pos;
}
