#version 330

layout(location = 0) in vec3 aPosition;
layout(location = 1) in vec2 aTexCoord;

uniform mat4 rotation;
uniform	mat4 projMatr;
uniform	vec4 moving;
uniform	mat4 view;

out vec2 texCoord;

void main()
{
	texCoord = aTexCoord;
	vec4 pos = vec4(aPosition, 1.0f);
	pos = pos * rotation;
	pos = pos + moving;
	gl_Position = projMatr * view *  pos;

}