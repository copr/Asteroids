

class VectorUmnanaged
{
public:
	VectorUmnanaged(double x, double y)
	{
		this->x = x;
		this->y = y;
	}

	double x = 0;
	double y = 0;
};


VectorUmnanaged& operator+= (VectorUmnanaged& v1, VectorUmnanaged& v2)
{
	VectorUmnanaged v(v1.x + v2.x, v1.y + v2.y);
	return v;
}