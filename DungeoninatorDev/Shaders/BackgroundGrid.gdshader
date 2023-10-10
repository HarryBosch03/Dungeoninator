shader_type canvas_item;

void vertex()
{
	UV = (MODEL_MATRIX * vec4(VERTEX, 0.0, 1.0)).xy / 16.0;
}
