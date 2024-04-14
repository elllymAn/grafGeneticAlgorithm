#pragma once
#include "Figure.h"

class Point : public Figure {
private:
	std::string* index;
public:
	Point(std::string data);
	~Point();
	Point(const Point& obj);
	Point& operator=(const Point& obj);
	bool operator<(const Point& obj);
	std::string* getName() { return index; }
	Figure* copy() { return new Point(*this); }
	void OutInfo(std::ofstream& file) override {
		file << "Вершина: "
			<< *index << "\n";
	}
};


