#pragma once
#include "Figure.h"
class Edge :
	public Figure
{
private:
	std::pair <Point*, Point*>* connect;
	int* weight;
public:
	Edge(Point* obj1, Point* obj2, int w = 1);
	~Edge();
	Edge(const Edge& obj);
	Edge& operator=(const Edge& obj);
	std::pair <Point*, Point*>* getPair() { return connect; }
	Figure* copy() { return new Edge(*this); }
	void OutInfo(std::ofstream& file) {
		file << "�����: " << *connect->first->getName() << "-" <<
			*connect->second->getName() << '\n';
	}
};

