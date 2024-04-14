#include "pch.h"
#include "Edge.h"

Edge::Edge(Point* obj1, Point* obj2, int w) : Figure()
{
	connect =
		new std::pair<Point*, Point*>;
	connect->first = new Point(*obj1);
	connect->second = new Point(*obj2);
	this->weight = new int(w);
}

Edge::Edge(const Edge& obj) : Figure(obj)
{
	connect =
		new std::pair<Point*, Point*>;
	connect->first = new Point(*obj.connect->first);
	connect->second = new Point(*obj.connect->second);
	weight =
		new int(*obj.weight);
}

Edge& Edge::operator=(const Edge& obj)
{
	if (this != &obj)
	{
		Figure::operator=(obj);
		delete connect->first;
		delete connect->second;
		connect->first =
			new Point(*obj.connect->first);
		connect->second =
			new Point(*obj.connect->second);
		*weight = *obj.weight;
	}
	return *this;
}

Edge::~Edge()
{
	delete connect->first;
	delete connect->second;
	delete connect;
	delete weight;
}