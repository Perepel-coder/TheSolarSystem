#version 330

layout(location = 0) out vec4 FragColor;

in vec2 texCoord;

uniform sampler2D  textureCub;

void main()
{
	FragColor = texture(textureCub, texCoord);
}
