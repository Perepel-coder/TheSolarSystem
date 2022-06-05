#version 330

layout(location = 0) out vec4 outColor;

in vec2 texCoord;

uniform sampler2D texturePlanet;

void main()
{
	outColor = texture(texturePlanet, texCoord);
}