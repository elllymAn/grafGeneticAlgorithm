#include "pch.h"
//#include "Point.h"

bool Point::operator<(const Point& obj)
{
	return *index < *obj.index;
}

Point& Point::operator=(const Point& obj)
{
	if (this != &obj)
	{
		Figure::operator=(obj);
		*index = *obj.index;
	}
	return *this;
}
Point::Point(std::string data) : Figure()
{
	std::transform(data.begin(), data.end(), data.begin(), ::toupper);
	index = new std::string(data);
}
Point::Point(const Point& obj) : Figure(obj)
{
	index = new std::string(*obj.index);
}
Point::~Point()
{
	delete index;
}
