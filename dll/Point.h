#pragma once
class Point {
private:
	std::string* index;
public:
	Point(std::string data);
	~Point();
	Point(const Point& obj);
	Point& operator=(const Point& obj);
	bool operator<(const Point& obj);
	std::string* getName() { return index; }
};


